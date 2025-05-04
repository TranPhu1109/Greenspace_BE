using AutoMapper;
using FluentValidation;
using GreenSpace.Application.ViewModels.Blogs;
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
    public class CreateExternalProductCommand : IRequest<ExternalProductsViewModel>
    {
        public ExternalProductsCreateModel CreateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<CreateExternalProductCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.CreateModel.Name).NotNull().NotEmpty().WithMessage("Name must not be null or empty");

                RuleFor(x => x.CreateModel.Quantity).NotNull().WithMessage("Quantity must not be empty");

                RuleFor(x => x.CreateModel.Description).NotNull().NotEmpty().WithMessage("Description must not be null or empty");

            }
        }
        public class CommandHandler : IRequestHandler<CreateExternalProductCommand, ExternalProductsViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;

            public CommandHandler(IUnitOfWork unitOfWork,
                    IMapper mapper,
                    ILogger<CommandHandler> logger,
                    AppSettings appSettings)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
                _appSettings = appSettings;
            }


            public async Task<ExternalProductsViewModel> Handle(CreateExternalProductCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Creating ExternalProducts: {Name}", request.CreateModel.Name);

                // Tạo mới 
                var externalProducts = _mapper.Map<ExternalProducts>(request.CreateModel);
                externalProducts.Id = Guid.NewGuid();
                await _unitOfWork.ExternalProductsRepository.AddAsync(externalProducts);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("externalProducts created successfully with ID: {externalProductsId}", externalProducts.Id);


                var ViewModel = _mapper.Map<ExternalProductsViewModel>(externalProducts);


                return ViewModel;
            }


        }
    }
}
