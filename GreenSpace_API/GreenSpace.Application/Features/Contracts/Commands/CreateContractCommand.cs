using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Contracts;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PdfSharpCore.Pdf;
using PdfSharpCore;
using HtmlRendererCore.PdfSharp;
using GreenSpace.Application.Services;
using PdfSharpCore.Drawing;


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

                    using (var document = new PdfDocument())
                    {
                        string htmlContent = "<style> h1, h2, h3 { text-align: center; } </style>";
                        htmlContent += "<h2>CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</h2>";
                        htmlContent += "<h3><u>Độc lập - Tự do - Hạnh phúc</u></h3>";
                        string formattedDate = DateTime.Now.ToString("dd 'tháng' MM 'năm' yyyy");
                        htmlContent += $"<p style='text-align: right; margin-right: 10px;'>TPHCM, ngày {formattedDate}</p>";

                        htmlContent += "<h1>HỢP ĐỒNG DỊCH VỤ THIẾT KẾ</h1>";
                        htmlContent += $"<h2>DỊCH VỤ THIẾT KẾ:{contract.ServiceOrderId}</h2>";
                        htmlContent += "<p><b>BÊN A:</b> BÊN CUNG CẤP DỊCH VỤ</p>";
                        htmlContent += "<p>Tên: Công ty GreenSpaces</p>";
                        htmlContent += "<p>Địa chỉ: Lô E2a-7, Đường D1 Khu Công nghệ cao, P. Long Thạnh Mỹ, TP. Thủ Đức, TP. Hồ Chí Minh</p>";
                        htmlContent += "<p>Người đại diện: Đoàn Minh Quang</p>";
                        htmlContent += "<p>Chức vụ: Giám đốc</p>";

                        htmlContent += "<p><b>BÊN B:</b> KHÁCH HÀNG</p>";
                        htmlContent += $"<p>Họ và tên: {contract.UserName}</p>";
                        htmlContent += $"<p>Địa chỉ: {contract.Address}</p>";
                        htmlContent += $"<p>Email: {contract.Email}</p>";
                        htmlContent += $"<p>Số điện thoại: {contract.Phone}</p>";

                        htmlContent += "<h3><strong>Nội dung hợp đồng</strong></h3>";
                        htmlContent += "<h3><strong>Điều khoản đặt dịch vụ thiết kế.</strong></h3>";
                        htmlContent += "<p>Bên đặt dịch được phép yêu cầu sửa đổi tối đa 2 lần trong giai đoạn phát thảo ý tưởng thiết kế .</p>";
                        htmlContent += "<p>Bên đặt dịch được phép yêu cầu sửa đổi tối đa 3 lần trong giai đoạn  thiết kế không gian xanh .</p>";
                        htmlContent += $"<p>Tổng số tiền thiết kế cho dịch vụ là :{contract.DesignPrice:N0} VNĐ.</p>";
                        htmlContent += "<p>Trong quá trình đặt dịch vụ Bên B phải đặt cọc cho bên A 50% tiền thiết kế để triển khai dịch vụ thiết kế  .</p>";
                        htmlContent += $"<p>Tổng số tiền đặt cọc thiết kế cho dịch vụ là :{contract.DesignPrice / 2:N0} VNĐ.</p>";
                        htmlContent += "<p>Trong quá trình thiết kế chưa hoàn thành Bên B muốn ngưng không làm nữa Bên A sẽ hoàn trả 30 trên số tiền Bên B đã đặt cọc .</p>";
                        htmlContent += $"<p>Tổng số tiền Bên A hoàn trả là dựa trên số tiền cọc của Bên B là :{((contract.DesignPrice / 2) * 0.3):N0} VNĐ.</p>";
                        htmlContent += $"<p>Trong quá trình thiết kế đã  hoàn thành Bên B muốn ngưng không làm nữa thì phải thanh toán toàn bộ số tiền thiết kế còn lại {contract.DesignPrice / 2:N0} VNĐ.</p>";

                        htmlContent += "<h3><strong>Điều khoản chung</strong></h3>";
                        htmlContent += "<p>Hai bên cam kết thực hiện đúng các điều khoản của hợp đồng.</p>";
                        htmlContent += "<p>Hợp đồng có hiệu lực kể từ ngày ký kết.</p>";

                        // Thêm khu vực ký tên với hình ảnh chữ ký mặc định cho Bên A
                        htmlContent += "<h3><strong>ĐẠI DIỆN CÁC BÊN</strong></h3>";
                        htmlContent += "<table style='width:100%; border-top: 1px solid black; border-collapse: collapse;'>";
                        htmlContent += "<tr>";
                        htmlContent += "<td style='width: 50%; text-align: center; padding-top: 50px;'><b>BÊN B</b><br><u>Chữ ký và họ tên</u></td>";
                        htmlContent += "<td style='width: 50%; text-align: center; padding-top: 50px;'><b>BÊN A</b></td>";
                        htmlContent += "</tr>";
                        htmlContent += "</table>";



                        PdfGenerator.AddPdfPages(document, htmlContent, PageSize.A4);

                        // vẽ ảnh kí 
                        //string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "sig_n.jpg");
                        string imageUrl = "https://res.cloudinary.com/dyn6t5fdh/image/upload/v1743350140/u7obnw76tjwmoexzwmkk.jpg";
                        string tempFile = Path.GetTempFileName() + ".jpg";

                        // Tải ảnh về
                        await File.WriteAllBytesAsync(tempFile, await new HttpClient().GetByteArrayAsync(imageUrl));

                        // Vẽ lên PDF
                        PdfPage page = document.Pages[^1];
                        XGraphics gfx = XGraphics.FromPdfPage(page);
                        gfx.DrawImage(XImage.FromFile(tempFile), 390, 330, 150, 50);

                        
                        File.Delete(tempFile);

                        using (var ms = new MemoryStream())
                        {
                            document.Save(ms);
                            return ms.ToArray(); // Trả về PDF dưới dạng byte[]
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
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
