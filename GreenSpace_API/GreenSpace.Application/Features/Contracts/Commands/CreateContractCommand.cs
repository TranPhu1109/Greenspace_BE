using AutoMapper;
using FluentValidation;
using GreenSpace.Application.Services;
using GreenSpace.Application.ViewModels.Contracts;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf;
using PdfDocument = Syncfusion.Pdf.PdfDocument;
using System.IO;


namespace GreenSpace.Application.Features.Contracts.Commands
{
    public class CreateContractCommand : IRequest<ContractViewModel>
    {
        public ContractCreateModel CreateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<CreateContractCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.CreateModel.UserName).NotNull().NotEmpty().WithMessage("User name must not be empty");
                RuleFor(x => x.CreateModel.Email).NotNull().NotEmpty().EmailAddress().WithMessage("Invalid email");
                RuleFor(x => x.CreateModel.Phone).NotNull().NotEmpty().WithMessage("Phone must not be empty");
                RuleFor(x => x.CreateModel.DesignPrice).GreaterThan(0).WithMessage("Price must be greater than zero");
            }
        }

        public class CommandHandler : IRequestHandler<CreateContractCommand, ContractViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly CloudinaryService _cloudinaryService;
            private readonly IMapper _mapper;
            private readonly ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;

            public CommandHandler(IUnitOfWork unitOfWork, CloudinaryService cloudinaryService, IMapper mapper, ILogger<CommandHandler> logger, AppSettings appSettings)
            {
                _unitOfWork = unitOfWork;
                _cloudinaryService = cloudinaryService;
                _mapper = mapper;
                _logger = logger;
                _appSettings = appSettings;
            }

            public async Task<ContractViewModel> Handle(CreateContractCommand request, CancellationToken cancellationToken)
            {

                _logger.LogInformation("Generating PDF for contract with user: {UserName}", request.CreateModel.UserName);
                var user = await _unitOfWork.UserRepository.GetByIdAsync(request.CreateModel.UserId);
                if (user == null)
                {
                    throw new Exception("User does not exist.");
                }
                // 1. Tạo PDF từ model
                byte[] pdfBytes = await GeneratePdf(request.CreateModel);
                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    throw new Exception("Failed to generate contract PDF.");
                }


                // 2. Upload PDF lên Cloudinary
                string? pdfUrl = await _cloudinaryService.UploadPdfAsync(pdfBytes, $"contract_{request.CreateModel.UserId}.pdf");


                // 3. Tạo entity Contract
                var contract = _mapper.Map<Contract>(request.CreateModel);
                contract.Id = Guid.NewGuid();
                contract.UserId = request.CreateModel.UserId;
                contract.ServiceOrderId = request.CreateModel.ServiceOrderId;
                contract.Description = pdfUrl;


                await _unitOfWork.ContractRepository.AddAsync(contract);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Contract created successfully with ID: {ContractId}", contract.Id);

                return _mapper.Map<ContractViewModel>(contract);
            }

            public async Task<byte[]> GeneratePdf(ContractCreateModel contract)
            {
                try
                {
                    if (contract == null)
                    {
                        throw new Exception("Contract does not exist");
                    }

                    // Tạo HTML content
                    string htmlContent = "<style> h1, h2, h3 { text-align: center; } </style>";
                    htmlContent += "<h2>CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</h2>";
                    htmlContent += "<h3><u>Độc lập - Tự do - Hạnh phúc</u></h3>";
                    string formattedDate = DateTime.Now.ToString("dd 'tháng' MM 'năm' yyyy");
                    htmlContent += $"<p style='text-align: right; margin-right: 10px;'>TPHCM, ngày {formattedDate}</p>";

                    // Thêm nội dung hợp đồng vào htmlContent
                    // ...

                    // Thêm khu vực ký tên
                    htmlContent += "<h3><strong>ĐẠI DIỆN CÁC BÊN</strong></h3>";
                    htmlContent += "<table style='width:100%; border-top: 1px solid black; border-collapse: collapse;'>";
                    htmlContent += "<tr>";
                    htmlContent += "<td style='width: 50%; text-align: center; padding-top: 50px;'><b>BÊN B</b><br><u>Chữ ký và họ tên</u></td>";
                    htmlContent += "<td style='width: 50%; text-align: center; padding-top: 50px;'><b>BÊN A</b></td>";
                    htmlContent += "</tr>";
                    htmlContent += "</table>";

                    // Tạo converter
                    HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();

                    // Cấu hình converter đúng cách
                    BlinkConverterSettings settings = new BlinkConverterSettings();
                    // Thêm các tham số để chạy không cần sandbox trên Linux
                    settings.CommandLineArguments.Add("--no-sandbox");
                    settings.CommandLineArguments.Add("--disable-setuid-sandbox");
                    htmlConverter.ConverterSettings = settings;

                    // Chuyển đổi HTML sang PDF
                    PdfDocument document = htmlConverter.Convert(htmlContent, "");

                    // Thêm chữ ký
                    string imageUrl = "https://res.cloudinary.com/dyn6t5fdh/image/upload/v1743350140/u7obnw76tjwmoexzwmkk.jpg";
                    byte[] imageBytes = await new HttpClient().GetByteArrayAsync(imageUrl);

                    PdfPage lastPage = document.Pages[document.Pages.Count - 1];
                    PdfGraphics graphics = lastPage.Graphics;
                    PdfBitmap image = new PdfBitmap(new MemoryStream(imageBytes));
                    graphics.DrawImage(image, 390, 330, 150, 50);

                    // Lưu PDF vào MemoryStream
                    MemoryStream stream = new MemoryStream();
                    document.Save(stream);

                    // Đóng document để giải phóng tài nguyên
                    document.Close(true);


                    return stream.ToArray();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi tạo PDF: {ex.Message}", ex);
                }
            }

            private string ConvertImageToBase64(string relativePath)
            {
                try
                {
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

                    if (!File.Exists(fullPath))
                    {
                        throw new Exception($"Không tìm thấy file ảnh: {fullPath}");
                    }

                    byte[] imageBytes = File.ReadAllBytes(fullPath);
                    return Convert.ToBase64String(imageBytes);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi chuyển ảnh sang Base64: {ex.Message}");
                }
            }


        }
    }
}
