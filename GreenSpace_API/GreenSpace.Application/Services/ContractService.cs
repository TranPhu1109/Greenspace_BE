//using PdfSharpCore.Pdf;
//using PdfSharpCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using HtmlRendererCore.PdfSharp;
//using GreenSpace.Application.Repositories;
//using GreenSpace.Domain.Entities;
//using GreenSpace.Application.ViewModels.Contracts;
//using GreenSpace.Application.ViewModels.DesignIdea;
//using AutoMapper;
//using GreenSpace.Application.Features.Blogs.Queries;
//using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
//using GreenSpace.Application.ViewModels.Blogs;

//namespace GreenSpace.Application.Services
//{
//    public class ContractService
//    {

//        private readonly IUnitOfWork _unitOfWork;
//        private readonly CloudinaryService _cloudinaryService;
//        private readonly IMapper _mapper;

//        public ContractService(IUnitOfWork unitOfWork, CloudinaryService cloudinaryService, IMapper mapper)
//        {
//            _unitOfWork = unitOfWork;
//            _cloudinaryService = cloudinaryService;
//            _mapper = mapper;
//        }
//        public async Task<byte[]> GeneratePdf(ContractCreateModel contract)
//        {
//            try
//            {
//                if (contract == null)
//                {
//                    throw new Exception("Contract does not exist");
//                }

//                using (var document = new PdfDocument())
//                {
//                    string htmlContent = "<style> h1, h2, h3 { text-align: center; } </style>";
//                    htmlContent += "<h2>CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</h2>";
//                    htmlContent += "<h3><u>Độc lập - Tự do - Hạnh phúc</u></h3>";
//                    string formattedDate = DateTime.Now.ToString("dd 'tháng' MM 'năm' yyyy");
//                    htmlContent += $"<p style='text-align: right; margin-right: 10px;'>TPHCM, ngày {formattedDate}</p>";

//                    htmlContent += "<h1>HỢP ĐỒNG DỊCH VỤ THIẾT KẾ</h1>";
//                    htmlContent += $"<h2>DỊCH VỤ THIẾT KẾ:{contract.ServiceOrderId}</h2>";
//                    htmlContent += "<p><b>BÊN A:</b> BÊN CUNG CẤP DỊCH VỤ</p>";
//                    htmlContent += "<p>Tên: Công ty GreenSpaces</p>";
//                    htmlContent += "<p>Địa chỉ: Lô E2a-7, Đường D1 Khu Công nghệ cao, P. Long Thạnh Mỹ, TP. Thủ Đức, TP. Hồ Chí Minh</p>";
//                    htmlContent += "<p>Người đại diện: Đoàn Minh Quang</p>";
//                    htmlContent += "<p>Chức vụ: Giám đốc</p>";

//                    htmlContent += "<p><b>BÊN B:</b> KHÁCH HÀNG</p>";
//                    htmlContent += $"<p>Họ và tên: {contract.UserName}</p>";
//                    htmlContent += $"<p>Địa chỉ: {contract.Address}</p>";
//                    htmlContent += $"<p>Email: {contract.Email}</p>";
//                    htmlContent += $"<p>Số điện thoại: {contract.Phone}</p>";

//                    htmlContent += "<h3><strong>Nội dung hợp đồng</strong></h3>";
//                    htmlContent += "<h3><strong>Điều khoản đặt dịch vụ thiết kế.</strong></h3>";
//                    htmlContent += "<p>Bên đặt dịch được phép yêu cầu sửa đổi tối đa 2 lần trong giai đoạn phát thảo ý tưởng thiết kế .</p>";
//                    htmlContent += "<p>Bên đặt dịch được phép yêu cầu sửa đổi tối đa 3 lần trong giai đoạn  thiết kế không gian xanh .</p>";
//                    htmlContent += $"<p>Tổng số tiền thiết kế cho dịch vụ là :{contract.DesignPrice:N0} VNĐ.</p>";
//                    htmlContent += "<p>Trong quá trình đặt dịch vụ Bên B phải đặt cọc cho bên A 50% tiền thiết kế để triển khai dịch vụ thiết kế  .</p>";
//                    htmlContent += $"<p>Tổng số tiền đặt cọc thiết kế cho dịch vụ là :{contract.DesignPrice/2:N0} VNĐ.</p>";
//                    htmlContent += "<p>Trong quá trình thiết kế chưa hoàn thành Bên B muốn ngưng không làm nữa Bên A sẽ hoàn trả 30 trên số tiền Bên B đã đặt cọc .</p>";
//                    htmlContent += $"<p>Tổng số tiền Bên A hoàn trả là dựa trên số tiền cọc của Bên B là :{((contract.DesignPrice / 2) * 0.3):N0} VNĐ.</p>";
//                    htmlContent += $"<p>Trong quá trình thiết kế đã  hoàn thành Bên B muốn ngưng không làm nữa thì phải thanh toán toàn bộ số tiền thiết kế còn lại {contract.DesignPrice / 2:N0} VNĐ.</p>";

//                    htmlContent += "<h3><strong>Điều khoản chung</strong></h3>";
//                    htmlContent += "<p>Hai bên cam kết thực hiện đúng các điều khoản của hợp đồng.</p>";
//                    htmlContent += "<p>Hợp đồng có hiệu lực kể từ ngày ký kết.</p>";

//                    PdfGenerator.AddPdfPages(document, htmlContent, PageSize.A4);

//                    using (var ms = new MemoryStream())
//                    {
//                        document.Save(ms);
//                        return ms.ToArray(); // Trả về PDF dưới dạng byte[]
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }

//        public async Task<ContractViewModel> SaveContractAsync(ContractCreateModel model)
//        {
//            // 1. Generate PDF từ model
//            var pdfBytes = await GeneratePdf(model);

//            // 2. Upload lên Cloudinary
//            string pdfUrl = await _cloudinaryService.UploadPdfAsync(pdfBytes, $"contract_{model.UserId}.pdf");
//            var contract = _mapper.Map<Contract>(model);
//            contract.Id = Guid.NewGuid();
//            contract.UserId = model.UserId;
//            contract.Description = pdfUrl;
             

//            await _unitOfWork.ContractRepository.AddAsync(contract);
//            await _unitOfWork.SaveChangesAsync();
//            return _mapper.Map<ContractViewModel>(contract);
//        }
//        public async Task<ContractViewModel> GetContractByIdAsync(Guid id)
//        {
//            var contract = await _unitOfWork.ContractRepository.GetByIdAsync(id, x => x.User);
//            if (contract is null) throw new NotFoundException($"Contract with ID-{id} is not exist!");
//            var result = _mapper.Map<ContractViewModel>(contract);
//            return result;
//        }

//    }
//}
