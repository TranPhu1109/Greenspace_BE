using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.SignalR;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.ServiceOrder;
using GreenSpace.Domain.Entities;
using GreenSpace.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ServiceOrders.Commands
{
    public class UpdateServiceOrderCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public ServiceOrderUpdateModel UpdateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<UpdateServiceOrderCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class CommandHandler : IRequestHandler<UpdateServiceOrderCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;
            private readonly IHubContext<SignalrHub> _hubContext;
            public CommandHandler(IUnitOfWork unitOfWork,
                    ILogger<CommandHandler> logger,
                    IMapper mapper,
                    AppSettings appSettings,
                   IHubContext<SignalrHub> hubContext)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _mapper = mapper;
                _appSettings = appSettings;
                _hubContext = hubContext;
            }

            public async Task<bool> Handle(UpdateServiceOrderCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Update ServiceOrder:\n");
                var serviceOrder = await _unitOfWork.ServiceOrderRepository.GetByIdAsync(request.Id,p => p.Image,p => p.ServiceOrderDetails);
                if (serviceOrder is null) throw new NotFoundException($"ServiceOrder with Id-{request.Id} is not exist!");
                var design = serviceOrder.DesignIdeaId.HasValue ?
                           await _unitOfWork.DesignIdeaRepository.GetByIdAsync(serviceOrder.DesignIdeaId.Value) : null;
               
                if (!Enum.IsDefined(typeof(ServiceOrderStatus), request.UpdateModel.Status))
                {
                    throw new InvalidOperationException($"Invalid status value: {request.UpdateModel.Status}");
                }
                //if (design != null && request.UpdateModel.DesignPrice > (double)design.DesignPrice * 1.3 && serviceOrder.ServiceType == ServiceTypeEnum.UsingDesignIdea.ToString())
                //{
                //    request.UpdateModel.Status = (int)ServiceOrderStatus.Warning;
                //}

                // Cập nhật ảnh
                if (request.UpdateModel.Image is not null)
                {
                    //update record
                    if (request.UpdateModel.Status == (int)ServiceOrderStatus.ReDeterminingDesignPrice)
                    {
                        var records = await _unitOfWork.RecordSketchRepository.WhereAsync(rs => rs.ServiceOrderId == serviceOrder.Id ,x =>x.Image);
                        var late = records.OrderByDescending(rs => rs.phase).FirstOrDefault();

                        if (late != null)
                        {
                            late.Image.ImageUrl = request.UpdateModel.Image.ImageUrl;
                            late.Image.Image2 = request.UpdateModel.Image.Image2;
                            late.Image.Image3 = request.UpdateModel.Image.Image3;

                            _unitOfWork.RecordSketchRepository.Update(late);
                        }
                    }


                    // tọa ảnh record steck 
                    if (request.UpdateModel.Status == (int)ServiceOrderStatus.ConsultingAndSketching || request.UpdateModel.Status == (int)ServiceOrderStatus.ReConsultingAndSketching)
                    {
                        // Đếm số lượng RecordSketch hiện có của ServiceOrder này
                        var existRecord = await _unitOfWork.RecordSketchRepository.WhereAsync(rs => rs.ServiceOrderId == serviceOrder.Id);
                        if (existRecord.Count > 3)
                        {
                            throw new NotFoundException($"Sorry, you have reached the maximum number of edits!");
                        }
                        if (existRecord.Count <= 3) 
                        {
                            // Nếu chưa có bản ghi thiết kế nào thì lưu ảnh cũ vào RecordSketch 
                            if (existRecord.Count == 0)
                            {
                                var oldImage = new Image
                                {
                                    Id = Guid.NewGuid(),
                                    ImageUrl = serviceOrder.Image.ImageUrl,
                                    Image2 = serviceOrder.Image.Image2,
                                    Image3 = serviceOrder.Image.Image3
                                };

                                await _unitOfWork.ImageRepository.AddAsync(oldImage);

                                var recordsSketch = new RecordSketch
                                {
                                    Id = Guid.NewGuid(),
                                    ServiceOrderId = serviceOrder.Id,
                                    ImageId = oldImage.Id,
                                    phase = 0,
                                    isSelected = false
                                };

                                await _unitOfWork.RecordSketchRepository.AddAsync(recordsSketch);
                                existRecord = await _unitOfWork.RecordSketchRepository.WhereAsync(rs => rs.ServiceOrderId == serviceOrder.Id);
                            }
                            int maxPhase = existRecord.Any() ? existRecord.Max(rs => rs.phase) : 0;
                            // Tạo ảnh record mới từ ảnh  service order
                            var newImage = new Image
                            {
                                Id = Guid.NewGuid(),
                                ImageUrl = request.UpdateModel.Image.ImageUrl,
                                Image2 = request.UpdateModel.Image.Image2,
                                Image3 = request.UpdateModel.Image.Image3
                            };

                            await _unitOfWork.ImageRepository.AddAsync(newImage);

                            var recordSketch = new RecordSketch
                            {
                                Id = Guid.NewGuid(),
                                ServiceOrderId = serviceOrder.Id,
                                ImageId = newImage.Id,
                                phase = maxPhase + 1,
                                isSelected = false
                            };

                            await _unitOfWork.RecordSketchRepository.AddAsync(recordSketch);
                        }
                    }
                    // tọa ảnh record design 
                    if (request.UpdateModel.Status == (int)ServiceOrderStatus.AssignToDesigner || request.UpdateModel.Status == (int)ServiceOrderStatus.ReDesign)
                    {
                        var existDesignRecords = await _unitOfWork.RecordDesignRepository.WhereAsync(rd => rd.ServiceOrderId == serviceOrder.Id);
                        if (existDesignRecords.Count > 3)
                        {
                            throw new NotFoundException($"Sorry, you have reached the maximum number of edits!");
                        }

                        if (existDesignRecords.Count <= 3)
                        {

                            // lưu ảnh mới vào RecordDesign
                            var newDesignImage = new Image
                            {
                                Id = Guid.NewGuid(),
                                ImageUrl = request.UpdateModel.Image.ImageUrl,
                                Image2 = request.UpdateModel.Image.Image2,
                                Image3 = request.UpdateModel.Image.Image3
                            };

                            await _unitOfWork.ImageRepository.AddAsync(newDesignImage);

                            var recordDesign = new RecordDesign
                            {
                                Id = Guid.NewGuid(),
                                ServiceOrderId = serviceOrder.Id,
                                ImageId = newDesignImage.Id,
                                phase = existDesignRecords.Count + 1,
                                isSelected = false
                            };

                            await _unitOfWork.RecordDesignRepository.AddAsync(recordDesign);
                        }
                    }
                    serviceOrder.Image.ImageUrl = !string.IsNullOrEmpty(request.UpdateModel.Image.ImageUrl) ? request.UpdateModel.Image.ImageUrl : serviceOrder.Image.ImageUrl;
                    serviceOrder.Image.Image2 = !string.IsNullOrEmpty(request.UpdateModel.Image.Image2) ? request.UpdateModel.Image.Image2 : serviceOrder.Image.Image2;
                    serviceOrder.Image.Image3 = !string.IsNullOrEmpty(request.UpdateModel.Image.Image3) ? request.UpdateModel.Image.Image3 : serviceOrder.Image.Image3;
                }

                // Nếu request có danh sách ServiceOrderDetails
                if (request.UpdateModel.ServiceOrderDetails != null && request.UpdateModel.ServiceOrderDetails.Any())
                {
                    // dsach mới
                    var requestDetails = request.UpdateModel.ServiceOrderDetails;
                    //danh sách cũ
                    var existingDetails = serviceOrder.ServiceOrderDetails.ToList();

                    // Thêm hoặc cập nhật ServiceOrderDetails
                    foreach (var detail in requestDetails)
                    {
                        var product = await _unitOfWork.ProductRepository.GetByIdAsync(detail.ProductId);
                        if (product == null)
                            throw new NotFoundException($"Product with Id {detail.ProductId} does not exist!");

                        var existingDetail = existingDetails.FirstOrDefault(d => d.ProductId == detail.ProductId);

                        if (existingDetail != null)
                        {
                            // Nếu đã tồn tại, cập nhật
                            existingDetail.Quantity = detail.Quantity;
                            existingDetail.Price = product.Price;
                            existingDetail.TotalPrice = product.Price * detail.Quantity;
                        }
                        else
                        {
                            // Nếu chưa tồn tại, thêm mới
                            var newDetail = new ServiceOrderDetail
                            {
                                Id = Guid.NewGuid(),
                                ServiceOrderId = serviceOrder.Id,
                                ProductId = detail.ProductId,
                                Quantity = detail.Quantity,
                                Price = product.Price,
                                TotalPrice = product.Price * detail.Quantity
                            };
                            await _unitOfWork.ServiceOrderDetailRepository.AddAsync(newDetail);
                            existingDetails.Add(newDetail);
                        }
                    }

                    // Xóa ServiceOrderDetails không còn trong request
                    var requestProductIds = requestDetails.Select(d => d.ProductId).ToHashSet();
                    var detailsToRemove = existingDetails.Where(d => !requestProductIds.Contains(d.ProductId)).ToList();

                    foreach (var d in detailsToRemove)
                    {
                        serviceOrder.ServiceOrderDetails.Remove(d);
                        await _unitOfWork.ServiceOrderDetailRepository.RemoveServiceOrderDetail(d);
                    }
                }
                serviceOrder.MaterialPrice = serviceOrder.ServiceOrderDetails.Sum(d => d.TotalPrice);

                _mapper.Map(request.UpdateModel, serviceOrder);
                serviceOrder.ServiceType = ((ServiceTypeEnum)request.UpdateModel.ServiceType).ToString();
                _unitOfWork.ServiceOrderRepository.Update(serviceOrder);
                var result = await _unitOfWork.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("messageReceived", "UpdateOrderService", $"{request.Id}");
                return result;
            }
        }
    }
}
