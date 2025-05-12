using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.SignalR;
using GreenSpace.Application.ViewModels.Blogs;
using GreenSpace.Application.ViewModels.Complaints;
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

namespace GreenSpace.Application.Features.Complaints.Commands
{
    public class UpdateComplaintCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public ComplaintUpdateModel UpdateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<UpdateComplaintCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.UpdateModel.Status).NotNull().NotEmpty().WithMessage("Status must not be null or empty");

                RuleFor(x => x.UpdateModel.ComplaintType).NotNull().WithMessage("Type must not be empty");


            }
        }
        public class CommandHandler : IRequestHandler<UpdateComplaintCommand, bool>
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

            public async Task<bool> Handle(UpdateComplaintCommand request, CancellationToken cancellationToken)
            {

                var complaint = await _unitOfWork.ComplaintRepository.GetByIdAsync(request.Id, p => p.Image ,p => p.User,p => p.ComplaintDetails);
                if (complaint is null) throw new NotFoundException($"Complain with Id {request.Id} does not exist!");
                if (!Enum.IsDefined(typeof(ComplaintStatusEnum), request.UpdateModel.Status))
                {
                    throw new InvalidOperationException($"Invalid status value: {request.UpdateModel.Status}");
                }
                _mapper.Map(request.UpdateModel, complaint);
                complaint.ComplaintType = ((ComplaintTypeEnum)request.UpdateModel.ComplaintType).ToString();

                if (request.UpdateModel.Status == 3 && complaint.ComplaintType == ComplaintTypeEnum.ProductReturn.ToString())
                {
                    foreach (var detail in complaint.ComplaintDetails)
                    {
                        if (!detail.IsCheck)
                        {
                            continue;
                        }

                        var product = await _unitOfWork.ProductRepository.GetByIdAsync(detail.ProductId);
                        if (product == null)
                        {
                            throw new NotFoundException($"Product with Id-{detail.ProductId} not found!");
                        }

                        if (product.Stock < 0)
                        {
                            throw new InvalidOperationException($"Not enough stock for Product Id {product.Id}");
                        }
                        if (product.Stock < detail.Quantity)
                        {
                            throw new InvalidOperationException($"Not enough stock for Product Id {product.Id}. Available: {product.Stock}, Required: {detail.Quantity}");
                        }

                        product.Stock -= detail.Quantity;
                        _unitOfWork.ProductRepository.Update(product);
                    }
                }
                _unitOfWork.ComplaintRepository.Update(complaint);
       
                var result = await _unitOfWork.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("messageReceived", "UpdateComplaint", $"{request.Id}");
                return result;
            }
        }

    }
}
