using AutoMapper;
using Azure.Core;
using CloudinaryDotNet.Core;
using FluentValidation;
using GreenSpace.Application.Services;
using GreenSpace.Application.Services.Interfaces;
using GreenSpace.Application.ViewModels.Contracts;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using static GreenSpace.Application.Features.Contracts.Commands.CreateContractCommand;

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
                RuleFor(x => x.UpdateModel.SignatureUrl)
                    .NotNull().NotEmpty().WithMessage("Signature URL must not be null or empty");
            }
        }

        public class CommandHandler : IRequestHandler<UpdateContractSignatureCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly CloudinaryService _cloudinaryService;
            private readonly ILogger<CommandHandler> _logger;
            private readonly IClaimsService claimsService;

            public CommandHandler(IUnitOfWork unitOfWork, IMapper mapper,
                CloudinaryService cloudinaryService, ILogger<CommandHandler> logger, IClaimsService claimsService)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _cloudinaryService = cloudinaryService;
                _logger = logger;
                this.claimsService = claimsService;
            }

            public async Task<bool> Handle(UpdateContractSignatureCommand request, CancellationToken cancellationToken)
            {
                var contract = await _unitOfWork.ContractRepository.GetByIdAsync(request.ContractId);

                if (contract == null || string.IsNullOrEmpty(contract.Description))
                {
                    _logger.LogError("Contract {ContractId} does not exist or has no PDF file.", request.ContractId);
                    throw new Exception($"Contract with ID {request.ContractId} does not exist or has no PDF file!");
                }

                var order = await _unitOfWork.ServiceOrderRepository.GetByIdAsync(contract.ServiceOrderId);

                if (order == null)
                    throw new Exception($"ServiceOrderID not found with ID {contract.ServiceOrderId}");
                var selectedPharse = await _unitOfWork.RecordSketchRepository.FirstOrDefaultAsync(x => x.ServiceOrderId == contract.ServiceOrderId && x.isSelected == true,x => x.Image);

                //image phác thảo

                var httpClient = new HttpClient();

                byte[] Image1 = await new HttpClient().GetByteArrayAsync(selectedPharse.Image.ImageUrl);
                byte[] Image2 = await new HttpClient().GetByteArrayAsync(selectedPharse.Image.Image2);
                byte[] Image3 = await new HttpClient().GetByteArrayAsync(selectedPharse.Image.Image3);

                var model = new ContractModel
                {
                    Name = contract.Name,
                    Address = contract.Address,
                    Email = contract.Email,
                    Phone = contract.Phone,
                    DesignPrice = order.DesignPrice ?? 0,
                    ServiceOrderId = contract.ServiceOrderId,
                    DepositPercentage = contract.DepositPercentage,
                    RefundPercentage = contract.RefundPercentage,
                    ModificatedBy = claimsService.GetCurrentUser,
                    Pharse = selectedPharse.phase,
                    SketchImage1 = Image1,
                    SketchImage2 = Image2,
                    SketchImage3 = Image3
                };

               
                if (string.IsNullOrEmpty(request.UpdateModel.SignatureUrl))
                {
                    throw new Exception("Signature URL cannot be null or empty.");
                }

                var imageBytes = await new HttpClient().GetByteArrayAsync(request.UpdateModel.SignatureUrl);
                var signedBytes = await GeneratePdfWithSignatureAsync(model, imageBytes);

                // Upload  PDF to Cloudinary
                string? newPdfUrl = await _cloudinaryService.UploadPdfAsync(signedBytes, $"contract_signed_{request.ContractId}.pdf");

                if (string.IsNullOrEmpty(newPdfUrl))
                {
                   
                    throw new Exception("Failed to upload signed contract PDF.");
                }

                // Update the contract description with the new PDF URL
                contract.Description = newPdfUrl;
                _unitOfWork.ContractRepository.Update(contract);
                return await _unitOfWork.SaveChangesAsync();
            }

            public class ContractModel
            {
                public string Name { get; set; } = default!;
                public string Address { get; set; } = default!;
                public string Email { get; set; } = default!;
                public string Phone { get; set; } = default!;
                public decimal DesignPrice { get; set; }
                public Guid ServiceOrderId { get; set; } = default!;
                public decimal DepositPercentage { get; set; }
                public decimal RefundPercentage { get; set; }
                public int Pharse { get; set; }
                public byte[]? SketchImage1 { get; set; }
                public byte[]? SketchImage2 { get; set; }
                public byte[]? SketchImage3 { get; set; }
                public Guid? ModificatedBy { get; set; } = default!;
            }

            private async Task<byte[]> GeneratePdfWithSignatureAsync(ContractModel contract, byte[] signatureBytes)
            {
                using var stream = new MemoryStream();
                QuestPDF.Settings.License = LicenseType.Community;

               
                var imageUrl = "https://res.cloudinary.com/dyn6t5fdh/image/upload/v1743350140/u7obnw76tjwmoexzwmkk.jpg";
                var imageData = await new HttpClient().GetByteArrayAsync(imageUrl);

             

                QuestPDF.Fluent.Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                        page.DefaultTextStyle(x => x.FontSize(14));

                        page.Header().ShowOnce().Column(col =>
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

                            decimal deposit = contract.DesignPrice * contract.DepositPercentage / 100;
                            decimal refund = deposit * contract.RefundPercentage / 100;
                            decimal remaining = contract.DesignPrice - deposit;

                            col.Item().PaddingTop(10).Text("Nội dung hợp đồng").Bold();
                            col.Item().Text("Bên B được sửa tối đa 2 lần ở giai đoạn phát thảo.");
                            col.Item().Text("Bên B được sửa tối đa 3 lần ở giai đoạn thiết kế không gian.");
                            col.Item().Text($"Tổng tiền thiết kế: {contract.DesignPrice:N0} VNĐ.");
                            col.Item().Text($"Đặt cọc {contract.DepositPercentage:0}% tiền thiết kế: {deposit:N0} VNĐ.");
                            col.Item().Text($"Hoàn trả {contract.RefundPercentage:0}% tiền đặt cọc nếu ngưng giữa chừng giai đoạn thiết kế: {refund:N0} VNĐ.");
                            col.Item().Text($"Nếu ngưng khi đã hoàn tất thiết kế chi tiết: phải trả đủ phần còn lại {remaining:N0} VNĐ.");
                            col.Item().Text($"Bảng phác thảo được chọn: {contract.Pharse}.");
                            col.Item().PaddingTop(10).Text("Ảnh phác thảo").Bold();

                            col.Item().Row(row =>
                            {
                                row.RelativeItem().Image(contract.SketchImage1).FitWidth();
                                row.RelativeItem().Image(contract.SketchImage2).FitWidth();
                                row.RelativeItem().Image(contract.SketchImage3).FitWidth();
                            });

                            col.Item().PaddingTop(10).Text("ĐIỀU KHOẢN CHUNG").Bold();
                            col.Item().Text("Hai bên cam kết thực hiện đúng các điều khoản của hợp đồng.");
                            col.Item().Text($"Bảng phác thảo được chọn: {contract.Pharse}.");
                            col.Item().Text("Lưu ý hợp đồng chưa bao gồm các chi phí vật liệu.");
                            col.Item().Text("Hợp đồng có hiệu lực kể từ ngày ký kết.");


                            
                            col.Item().PaddingTop(20).Text("ĐẠI DIỆN CÁC BÊN").Bold();
                            col.Item().Row(row =>
                            {
                                row.RelativeItem().AlignCenter().Text("BÊN B\n(Khách hàng)").Bold();
                                row.RelativeItem().AlignCenter().Text("BÊN A\n(Công ty)").Bold();
                            });

                            col.Item().PaddingTop(20).Row(row =>
                            {
                                // Chữ ký người dùng (từ dữ liệu truyền vào)
                                row.RelativeItem().AlignCenter().Column(colLeft =>
                                {
                                    colLeft.Item().Image(signatureBytes).FitWidth(); 
                                    colLeft.Item().Text(contract.Name).Italic().FontSize(14); 
                                });

                                // Chữ ký cố định (hình ảnh cố định từ URL)
                                row.RelativeItem().AlignCenter().PaddingLeft(10).Image(imageData).FitWidth();
                                
                            });
                        });

                        page.Footer().AlignCenter().Text($"GreenSpaces - Hợp đồng đã được ký  TPHCM, ngày {DateTime.Now:dd 'tháng' MM 'năm' yyyy}");
                    });
                }).GeneratePdf(stream);

                return stream.ToArray();
            }
        }
    }
}