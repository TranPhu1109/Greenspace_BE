using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.ExternalProduct;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ExternalProduct.Commands
{
    public class UpdateExternalProductPriceCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public ExternalProductsUpdatePriceModel UpdateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<UpdateExternalProductPriceCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.UpdateModel.Price).GreaterThan(0).WithMessage("Price must be greater than zero");
            }
        }
        public class CommandHandler : IRequestHandler<UpdateExternalProductPriceCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;



            public CommandHandler(IUnitOfWork unitOfWork,
                IMapper mapper, ILogger<CommandHandler> logger,
                AppSettings appSettings)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
                _appSettings = appSettings;

            }

            public async Task<bool> Handle(UpdateExternalProductPriceCommand request, CancellationToken cancellationToken)
            {

                var external = await _unitOfWork.ExternalProductsRepository.GetByIdAsync(request.Id);
                if (external is null) throw new NotFoundException($"externalProduct with Id {request.Id} does not exist!");
                external.TotalPrice = request.UpdateModel.Price * external.Quantity;
                _mapper.Map(request.UpdateModel, external);
                _unitOfWork.ExternalProductsRepository.Update(external);
                return await _unitOfWork.SaveChangesAsync();
            }
        }

    }
}
