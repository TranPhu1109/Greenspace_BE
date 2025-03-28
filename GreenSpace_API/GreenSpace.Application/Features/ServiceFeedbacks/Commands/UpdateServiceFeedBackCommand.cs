using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.ProductFeedback;
using GreenSpace.Application.ViewModels.ServiceFeedbacks;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ServiceFeedbacks.Commands
{
    public class UpdateServiceFeedBackCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public ServiceFeedbackUpdateModel UpdateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<UpdateServiceFeedBackCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
                RuleFor(x => x.UpdateModel.Reply).NotNull().NotEmpty().WithMessage("Reply must not null or empty");
            }
        }

        public class CommandHandler : IRequestHandler<UpdateServiceFeedBackCommand, bool>
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

            public async Task<bool> Handle(UpdateServiceFeedBackCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Update feedback:\n");
                var feedback = await _unitOfWork.ServiceFeedbackRepositoy.GetByIdAsync(request.Id);
                if (feedback is null) throw new NotFoundException($"feedback: with Id-{request.Id} is not exist!");
                _mapper.Map(request.UpdateModel, feedback);
                _unitOfWork.ServiceFeedbackRepositoy.Update(feedback);
                var result = await _unitOfWork.SaveChangesAsync();
                return result;
            }
        }
    }
}
