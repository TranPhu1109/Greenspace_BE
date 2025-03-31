using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Services;
using GreenSpace.Application.ViewModels.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GreenSpace.Domain.Entities;
using PdfSharpCore.Pdf.IO;
using Image = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Formats;
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

                if (request.UpdateModel.SignatureBase64.StartsWith("data:image"))
                {
                    // Xử lý chữ ký dạng ảnh
                    byte[] signatureBytes = Convert.FromBase64String(request.UpdateModel.SignatureBase64.Split(',')[1]);
                    updatedPdfBytes = AddSignatureImageToPdf(pdfBytes, signatureBytes);
                }
                else
                {
                    // Xử lý chữ ký dạng văn bản
                    updatedPdfBytes = AddSignatureTextToPdf(pdfBytes, request.UpdateModel.SignatureBase64);
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
                    XFont font = new XFont("Arial", 10, XFontStyle.Bold);

                    // Kiểm tra nếu là chuỗi Base64 thì giải mã
                    string decodedText;
                    try
                    {
                        byte[] bytes = Convert.FromBase64String(signatureText);
                        decodedText = Encoding.UTF8.GetString(bytes);
                    }
                    catch (FormatException)
                    {
                        decodedText = signatureText; // Nếu không phải Base64, giữ nguyên chuỗi gốc
                    }

                    // Vẽ chữ ký lên PDF
                    gfx.DrawString(decodedText, font, XBrushes.Black, new XPoint(100, 450));

                    document.Save(output);
                    return output.ToArray();
                }
            }


            private byte[] AddSignatureImageToPdf(byte[] pdfBytes, byte[] signatureBytes)
            {
                using (var ms = new MemoryStream(pdfBytes))
                using (var output = new MemoryStream())
                {
                    var document = PdfReader.Open(ms, PdfDocumentOpenMode.Modify);
                    var gfx = XGraphics.FromPdfPage(document.Pages[document.Pages.Count - 1]);
                    using (var image = Image.Load(signatureBytes))
                    {
                        using (var imgStream = new MemoryStream())
                        {
                            image.Save(imgStream, new JpegEncoder());
                            XImage xImage = XImage.FromStream(() => new MemoryStream(imgStream.ToArray()));
                            gfx.DrawImage(xImage, 420, 750, 150, 50);
                        }
                    }
                    document.Save(output);
                    return output.ToArray();
                }
            }
        }
    }
}
