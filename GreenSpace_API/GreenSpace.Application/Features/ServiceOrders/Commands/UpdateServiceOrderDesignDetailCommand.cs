using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.ServiceOrder;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ServiceOrders.Commands
{
    public class UpdateServiceOrderDesignDetailCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public ServiceOrderUpdateDesignDetailModel UpdateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<UpdateServiceOrderDesignPriceCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

            }
        }

        public class CommandHandler : IRequestHandler<UpdateServiceOrderDesignDetailCommand, bool>
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

            public async Task<bool> Handle(UpdateServiceOrderDesignDetailCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Update DesignPrice serviceOrder:\n");

                var servicerOrder = await _unitOfWork.ServiceOrderRepository.GetByIdAsync(request.Id,p=> p.Image);
                if (servicerOrder is null) throw new NotFoundException($"servicerOrder with Id-{request.Id} does not exist!");

                //  cập nhật ảnh 
                if (request.UpdateModel.Image is not null)
                {
                    servicerOrder.Image.ImageUrl = !string.IsNullOrEmpty(request.UpdateModel.Image.ImageUrl) ? request.UpdateModel.Image.ImageUrl : servicerOrder.Image.ImageUrl;
                    servicerOrder.Image.Image2 = !string.IsNullOrEmpty(request.UpdateModel.Image.Image2) ? request.UpdateModel.Image.Image2 : servicerOrder.Image.Image2;
                    servicerOrder.Image.Image3 = !string.IsNullOrEmpty(request.UpdateModel.Image.Image3) ? request.UpdateModel.Image.Image3 : servicerOrder.Image.Image3;

                }
                _mapper.Map(request.UpdateModel, servicerOrder);
                _unitOfWork.ServiceOrderRepository.Update(servicerOrder);

                var result = await _unitOfWork.SaveChangesAsync();
                return result;
            }

        }
    }
}
