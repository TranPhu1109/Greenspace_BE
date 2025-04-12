using AutoMapper;
using FluentValidation;
using GreenSpace.Application.Services;
using GreenSpace.Application.ViewModels.Contracts;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;

namespace GreenSpace.Application.Features.Contracts.Commands
{
    public class CreateContractCommand : IRequest<ContractViewModel>
    {
        public ContractCreateModel CreateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<CreateContractCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.CreateModel.Name).NotNull().NotEmpty().WithMessage("User name must not be empty");
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
                _logger.LogInformation("Generating PDF for contract with user: {UserName}", request.CreateModel.Name);
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
                contract.Name = request.CreateModel.Name;
                contract.Phone = request.CreateModel.Phone;
                contract.Email = request.CreateModel.Email;
                contract.Address = request.CreateModel.Address;
                contract.Description = pdfUrl;

                await _unitOfWork.ContractRepository.AddAsync(contract);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Contract created successfully with ID: {ContractId}", contract.Id);

                return _mapper.Map<ContractViewModel>(contract);
            }

            public async Task<byte[]> GeneratePdf(ContractCreateModel contract)
            {
                if (contract == null)
                    throw new Exception("Contract does not exist");

                // Tải ảnh chữ ký từ Cloudinary
                string imageUrl = "https://res.cloudinary.com/dyn6t5fdh/image/upload/v1743350140/u7obnw76tjwmoexzwmkk.jpg";
                byte[] signatureImage = await new HttpClient().GetByteArrayAsync(imageUrl);

                // Tạo model cho PDF
                var model = new ContractModel
                {
                    Name = contract.Name,
                    Address = contract.Address,
                    Email = contract.Email,
                    Phone = contract.Phone,
                    DesignPrice = contract.DesignPrice ?? 0,
                    ServiceOrderId = contract.ServiceOrderId,
                    SignatureImageBytes = signatureImage
                };

                // Render PDF bằng QuestPDF
                var document = new ContractDocument(model);
                using var stream = new MemoryStream();
                document.GeneratePdf(stream);
                return stream.ToArray();
            }
        }

        public class ContractModel
        {
            public string Name { get; set; } = default!;
            public string Address { get; set; } = default!;
            public string Email { get; set; } = default!;
            public string Phone { get; set; } = default!;
            public decimal DesignPrice { get; set; }
            public Guid ServiceOrderId { get; set; } = default!;
            public byte[] SignatureImageBytes { get; set; } = default!;
        }

        public class ContractDocument : IDocument
        {
            private readonly ContractModel contract;

            public ContractDocument(ContractModel contract)
            {
                this.contract = contract;
            }

            public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

            public void Compose(IDocumentContainer container)
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Column(col =>
                    {
                        col.Item().AlignCenter().Text("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM").Bold();
                        col.Item().AlignCenter().Text("Độc lập - Tự do - Hạnh phúc").Italic().Underline();
                        col.Item().AlignRight().Text($"TPHCM, ngày {DateTime.Now:dd 'tháng' MM 'năm' yyyy}");
                        col.Item().AlignCenter().Text("HỢP ĐỒNG DỊCH VỤ THIẾT KẾ").FontSize(18).Bold();
                        col.Item().AlignCenter().Text($"DỊCH VỤ THIẾT KẾ: {contract.ServiceOrderId}").FontSize(14);
                    });

                    page.Content().Column(col =>
                    {
                        col.Item().Text("BÊN A: BÊN CUNG CẤP DỊCH VỤ").Bold();
                        col.Item().Text("Tên: Công ty GreenSpaces");
                        col.Item().Text("Địa chỉ: Lô E2a-7, Đường D1, Khu CNC, TP. Thủ Đức, TP.HCM");
                        col.Item().Text("Người đại diện: Đoàn Minh Quang");
                        col.Item().Text("Chức vụ: Giám đốc");

                        col.Item().PaddingTop(10).Text("BÊN B: KHÁCH HÀNG").Bold();
                        col.Item().Text($"Họ và tên: {contract.Name}");
                        col.Item().Text($"Địa chỉ: {contract.Address}");
                        col.Item().Text($"Email: {contract.Email}");
                        col.Item().Text($"Số điện thoại: {contract.Phone}");

                        col.Item().PaddingTop(10).Text("Nội dung hợp đồng").Bold();
                        col.Item().Text("Bên B được sửa tối đa 2 lần ở giai đoạn phát thảo.");
                        col.Item().Text("Bên B được sửa tối đa 3 lần ở giai đoạn thiết kế không gian.");
                        col.Item().Text($"Tổng tiền thiết kế: {contract.DesignPrice:N0} VNĐ.");
                        col.Item().Text($"Đặt cọc 50%: {contract.DesignPrice / 2:N0} VNĐ.");
                        col.Item().Text($"Hoàn trả nếu ngưng giữa chừng: {((contract.DesignPrice / 2) * 0.3m):N0} VNĐ.");
                        col.Item().Text($"Nếu ngưng khi đã hoàn tất thiết kế: phải trả đủ {contract.DesignPrice / 2:N0} VNĐ.");

                        col.Item().PaddingTop(10).Text("ĐIỀU KHOẢN CHUNG").Bold();
                        col.Item().Text("Hai bên cam kết thực hiện đúng các điều khoản của hợp đồng.");
                        col.Item().Text("Hợp đồng có hiệu lực kể từ ngày ký kết.");

                        col.Item().PaddingTop(20).Text("ĐẠI DIỆN CÁC BÊN").Bold();
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().AlignCenter().Text("BÊN B\n(Khách hàng)").Bold();
                            row.RelativeItem().AlignCenter().Text("BÊN A\n(Công ty)").Bold();
                        });

                        col.Item().PaddingTop(20).Row(row =>
                        {
                            row.RelativeItem(); // cột bên trái trống

                            row.RelativeItem().AlignCenter().Element(container =>
                                                container.Image(contract.SignatureImageBytes)
                                                .FitWidth()
                                                );
                        });
                    });

                    page.Footer().AlignCenter().Text("GreenSpaces - Hợp đồng được tạo tự động");
                });
            }
        }
    }
}
