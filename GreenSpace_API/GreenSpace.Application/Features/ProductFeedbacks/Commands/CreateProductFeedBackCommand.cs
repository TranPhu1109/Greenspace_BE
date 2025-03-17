using AutoMapper;
using FluentValidation;
using GreenSpace.Application.Features.Categories.Commands;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.ProductFeedback;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ProductFeedbacks.Commands
{
    public class CreateProductFeedBackCommand : IRequest<ProductFeedbackViewModel>
    {
        public ProductFeedbackCreateModel CreateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<CreateProductFeedBackCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.CreateModel.Description).NotNull().NotEmpty().WithMessage("Description must not null or empty");
                RuleFor(x => x.CreateModel.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");
                RuleFor(x => x.CreateModel.UserId).NotEmpty().WithMessage("UserId is required");
                RuleFor(x => x.CreateModel.ProductId).NotEmpty().WithMessage("ProductId is required");
            }
        }
        public class CommandHandler : IRequestHandler<CreateProductFeedBackCommand, ProductFeedbackViewModel>
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

            public async Task<ProductFeedbackViewModel> Handle(CreateProductFeedBackCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Create productfeedback:\n");
                var productFeedback = _mapper.Map<ProductFeedback>(request.CreateModel);
                productFeedback.Id = Guid.NewGuid();
                await _unitOfWork.ProductFeedbackRepository.AddAsync(productFeedback);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<ProductFeedbackViewModel>(productFeedback);
            }
        }
    }
}
