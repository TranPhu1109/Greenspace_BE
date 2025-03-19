using AutoMapper;
using FluentValidation;
using GreenSpace.Application.ViewModels.ProductFeedback;
using GreenSpace.Application.ViewModels.ServiceFeedbacks;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ServiceFeedbacks.Commands
{
    public class CreateServiceFeedBackCommand : IRequest<ServiceFeedbackViewModel>
    {
        public ServiceFeedbackCreateModel CreateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<CreateServiceFeedBackCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.CreateModel.Description).NotNull().NotEmpty().WithMessage("Description must not null or empty");
                RuleFor(x => x.CreateModel.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");
                RuleFor(x => x.CreateModel.UserId).NotEmpty().WithMessage("UserId is required");
                RuleFor(x => x.CreateModel.DesignIdeaId).NotEmpty().WithMessage("DesignIdId is required");
            }
        }
        public class CommandHandler : IRequestHandler<CreateServiceFeedBackCommand, ServiceFeedbackViewModel>
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

            public async Task<ServiceFeedbackViewModel> Handle(CreateServiceFeedBackCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Create Servicefeedback:\n");
                var serviceFeedback = _mapper.Map<ServiceFeedback>(request.CreateModel);
                serviceFeedback.Id = Guid.NewGuid();
                await _unitOfWork.ServiceFeedbackRepositoy.AddAsync(serviceFeedback);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<ServiceFeedbackViewModel>(serviceFeedback);
            }
        }
    }
}
