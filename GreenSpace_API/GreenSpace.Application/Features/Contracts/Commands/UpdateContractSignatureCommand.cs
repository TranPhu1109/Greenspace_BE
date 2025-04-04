using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Services;
using GreenSpace.Application.ViewModels.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using System.Text;

namespace GreenSpace.Application.Features.Contracts.Commands
{
    public class UpdateContractSignatureCommand : IRequest<bool>
    {
        public Guid ContractId { get; set; }
        public ContractUpdateModel UpdateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<UpdateContractSignatureCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.UpdateModel.SignatureBase64)
                    .NotNull().NotEmpty().WithMessage("Signature must not be null or empty");
            }
        }

        public class CommandHandler : IRequestHandler<UpdateContractSignatureCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly CloudinaryService _cloudinaryService;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(IUnitOfWork unitOfWork, IMapper mapper,
                CloudinaryService cloudinaryService, ILogger<CommandHandler> logger)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _cloudinaryService = cloudinaryService;
                _logger = logger;
            }

            public async Task<bool> Handle(UpdateContractSignatureCommand request, CancellationToken cancellationToken)
            {
                var contract = await _unitOfWork.ContractRepository.GetByIdAsync(request.ContractId);
                if (contract is null || string.IsNullOrEmpty(contract.Description))
                {
                    _logger.LogError("Contract {ContractId} does not exist or has no PDF file.", request.ContractId);
                    throw new NotFoundException($"Contract with ID {request.ContractId} does not exist or has no PDF file!");
                }
                // tải pdf từ cloudinary
                byte[] pdfBytes = await _cloudinaryService.DownloadPdfAsync(contract.Description);
                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    _logger.LogError("Failed to download PDF for ContractId: {ContractId}", request.ContractId);
                    throw new Exception("Failed to download contract PDF.");
                }

                byte[] updatedPdfBytes;

                if (!string.IsNullOrEmpty(request.UpdateModel.SignatureUrl))
                {
                    // Image URL (Chữ ký dưới dạng URL ảnh)
                    updatedPdfBytes = AddSignatureImageToPdf(pdfBytes, request.UpdateModel.SignatureUrl);
                }
                else if (!string.IsNullOrEmpty(request.UpdateModel.SignatureBase64))
                {
                    // Base64 text (Chữ ký dưới dạng văn bản Base64)
                    updatedPdfBytes = AddSignatureTextToPdf(pdfBytes, request.UpdateModel.SignatureBase64);
                }
                else
                {
                  
                    throw new ArgumentException("Chữ ký (Base64 hoặc URL) phải được cung cấp.");
                }
                string? newPdfUrl = await _cloudinaryService.UploadPdfAsync(updatedPdfBytes, $"contract_signed_{request.ContractId}.pdf");
                if (string.IsNullOrEmpty(newPdfUrl))
                {
                    _logger.LogError("Failed to upload signed contract PDF for ContractId: {ContractId}", request.ContractId);
                    throw new Exception("Failed to upload signed contract PDF.");
                }

                contract.Description = newPdfUrl;
                _unitOfWork.ContractRepository.Update(contract);
                return await _unitOfWork.SaveChangesAsync();
            }

            private byte[] AddSignatureTextToPdf(byte[] pdfBytes, string signatureText)
            {
                using (var ms = new MemoryStream(pdfBytes))
                using (var output = new MemoryStream())
                {
                    var document = PdfReader.Open(ms, PdfDocumentOpenMode.Modify);
                    var gfx = XGraphics.FromPdfPage(document.Pages[document.Pages.Count - 1]);
                    //XFont font = new XFont("Arial", 10, XFontStyle.Bold);

                    // Kiểm tra nếu là chuỗi Base64 thì giải mã
                    string decodedText;
                    try
                    {
                        byte[] bytes = Convert.FromBase64String(signatureText);
                        decodedText = Encoding.UTF8.GetString(bytes);
                    }
                    catch (FormatException)
                    {
                        decodedText = signatureText;
                    }

                    // Vẽ chữ ký lên PDF
                    //gfx.DrawString(decodedText, XBrushes.Black, new XPoint(100, 400));

                    document.Save(output);
                    return output.ToArray();
                }
            }

            //private byte[] AddSignatureImageToPdf(byte[] pdfBytes, string base64Signature)
            //{
            //    if (string.IsNullOrWhiteSpace(base64Signature))
            //    {
            //        throw new ArgumentException("Base64 image data is null or empty.");
            //    }

            //    try
            //    {
            //        // Giải mã Base64 thành byte[]
            //        byte[] signatureBytes = Convert.FromBase64String(base64Signature);


            //        string fileName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "signature.jpg");


            //        // Lưu ảnh vào file, sẽ ghi đè nếu file đã tồn tại
            //        File.WriteAllBytes(fileName, signatureBytes);

            //        // Bắt đầu tạo và vẽ lên PDF
            //        using (var pdfStream = new MemoryStream(pdfBytes))
            //        using (var output = new MemoryStream())
            //        {
            //            var document = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Modify);
            //            var page = document.Pages[document.Pages.Count - 1];
            //            var gfx = XGraphics.FromPdfPage(page);

            //            // Đọc ảnh từ file cố định và vẽ lên PDF
            //            XImage xImage = XImage.FromFile(fileName);
            //            gfx.DrawImage(xImage, 100, 450, 150, 50); // Vị trí và kích thước chữ ký

            //            // Lưu PDF mới vào output stream
            //            document.Save(output);
            //            return output.ToArray();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new InvalidOperationException("Lỗi khi xử lý ảnh chữ ký: " + ex.Message);
            //    }
            //}
            private byte[] AddSignatureImageToPdf(byte[] pdfBytes, string imageUrl)
            {
                if (string.IsNullOrWhiteSpace(imageUrl))
                {
                    throw new ArgumentException("Image URL is null or empty.");
                }

                try
                {
                    // Tải ảnh từ URL
                    using (var webClient = new System.Net.WebClient())
                    {
                        byte[] signatureBytes = webClient.DownloadData(imageUrl); // Tải dữ liệu ảnh từ URL

                       
                        string tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "signature.jpg");
                        File.WriteAllBytes(tempFilePath, signatureBytes); // Lưu vào thư mục

                        // Bắt đầu tạo và vẽ lên PDF
                        using (var pdfStream = new MemoryStream(pdfBytes))
                        using (var output = new MemoryStream())
                        {
                            var document = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Modify);
                            var page = document.Pages[document.Pages.Count - 1];
                            var gfx = XGraphics.FromPdfPage(page);

                            // Đọc ảnh từ file đã tải
                            XImage xImage = XImage.FromFile(tempFilePath);
                            gfx.DrawImage(xImage, 100, 350, 150, 50); 

                            // Lưu PDF mới vào output stream
                            document.Save(output);
                            return output.ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Lỗi khi xử lý ảnh chữ ký từ URL: " + ex.Message);
                }
            }










        }
    }
}
