using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.ServiceOrder;
using GreenSpace.Domain.Entities;
using GreenSpace.Domain.Enum;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ServiceOrders.Commands
{
    public class UpdateServiceOrderForCustomerCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public ServiceOrderUpdateModel UpdateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<UpdateServiceOrderForCustomerCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class CommandHandler : IRequestHandler<UpdateServiceOrderForCustomerCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;
            public CommandHandler(IUnitOfWork unitOfWork,
                    ILogger<CommandHandler> logger,
                    IMapper mapper,
                    AppSettings appSettings)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _mapper = mapper;
                _appSettings = appSettings;
            }

            public async Task<bool> Handle(UpdateServiceOrderForCustomerCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Update ServiceOrder:\n");
                var serviceOrder = await _unitOfWork.ServiceOrderRepository.GetByIdAsync(request.Id, p => p.Image, p => p.ServiceOrderDetails);
                if (serviceOrder is null) throw new NotFoundException($"ServiceOrder with Id-{request.Id} is not exist!");
                var design = serviceOrder.DesignIdeaId.HasValue ?
                           await _unitOfWork.DesignIdeaRepository.GetByIdAsync(serviceOrder.DesignIdeaId.Value) : null;
             
                if (!Enum.IsDefined(typeof(ServiceOrderStatus), request.UpdateModel.Status))
                {
                    throw new InvalidOperationException($"Invalid status value: {request.UpdateModel.Status}");
                }
                if (design != null && request.UpdateModel.DesignPrice > (double)design.DesignPrice * 1.3 && serviceOrder.ServiceType == ServiceTypeEnum.UsingDesignIdea.ToString())
                {
                    request.UpdateModel.Status = (int)ServiceOrderStatus.Warning;
                }

                // Cập nhật ảnh
                if (request.UpdateModel.Image is not null)
                {
                
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
                return result;
            }
        }
    }
}
