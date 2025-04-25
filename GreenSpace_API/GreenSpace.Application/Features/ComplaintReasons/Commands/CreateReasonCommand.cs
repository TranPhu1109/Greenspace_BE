using AutoMapper;
using FluentValidation;
using GreenSpace.Application.ViewModels.Banner;
using GreenSpace.Application.ViewModels.ComplaintReason;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ComplaintReasons.Commands
{
    public class CreateReasonCommand : IRequest<ComplaintReasonViewModel>
    {
        public ComplaintReasonCreateModel CreateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<CreateReasonCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.CreateModel.Reason).NotNull().NotEmpty().WithMessage("Reason must not be null or empty");

            }
        }
        public class CommandHandler : IRequestHandler<CreateReasonCommand, ComplaintReasonViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;
            private readonly IMediator _mediator;

            public CommandHandler(IUnitOfWork unitOfWork,
                    IMapper mapper,
                    ILogger<CommandHandler> logger,
                    AppSettings appSettings,
                    IMediator mediator)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
                _appSettings = appSettings;
                _mediator = mediator;
            }


            public async Task<ComplaintReasonViewModel> Handle(CreateReasonCommand request, CancellationToken cancellationToken)
            {
                var existReason = await _unitOfWork.ComplaintReasonRepository.FirstOrDefaultAsync(p => p.Reason.ToLower() == request.CreateModel.Reason.ToLower());

                if (existReason != null)
                {

                    throw new InvalidOperationException($"Complaint with reason '{request.CreateModel.Reason}' already exists.");
                }

                var reason = _mapper.Map<ComplaintReason>(request.CreateModel);
                reason.Id = Guid.NewGuid();

                await _unitOfWork.ComplaintReasonRepository.AddAsync(reason);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Reason created successfully with ID: {ReasonID}", reason.Id);


                var ViewModel = _mapper.Map<ComplaintReasonViewModel>(reason);


                return ViewModel;
            }


        }
    }
}
