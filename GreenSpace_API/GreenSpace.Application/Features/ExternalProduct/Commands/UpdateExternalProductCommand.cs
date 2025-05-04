using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Blogs;
using GreenSpace.Application.ViewModels.ExternalProduct;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ExternalProduct.Commands
{
    public class UpdateExternalProductCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public ExternalProductsUpdateModel UpdateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<UpdateExternalProductCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.UpdateModel.Name).NotNull().NotEmpty().WithMessage("Name must not be null or empty");

                RuleFor(x => x.UpdateModel.Quantity).NotNull().WithMessage("Quantity must not be empty");

                RuleFor(x => x.UpdateModel.Description).NotNull().NotEmpty().WithMessage("Description must not be null or empty");

            }
        }
        public class CommandHandler : IRequestHandler<UpdateExternalProductCommand, bool>
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

            public async Task<bool> Handle(UpdateExternalProductCommand request, CancellationToken cancellationToken)
            {

                var external = await _unitOfWork.ExternalProductsRepository.GetByIdAsync(request.Id);
                if (external is null) throw new NotFoundException($"externalProduct with Id {request.Id} does not exist!");
                _mapper.Map(request.UpdateModel, external);
                _unitOfWork.ExternalProductsRepository.Update(external);
                return await _unitOfWork.SaveChangesAsync();
            }
        }

    }
}
