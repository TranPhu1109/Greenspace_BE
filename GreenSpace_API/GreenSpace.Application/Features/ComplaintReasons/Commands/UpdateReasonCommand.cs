using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Banner;
using GreenSpace.Application.ViewModels.ComplaintReason;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ComplaintReasons.Commands
{
    public class UpdateReasonCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public ComplaintReasonCreateModel UpdateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<UpdateReasonCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.UpdateModel.Reason).NotNull().NotEmpty().WithMessage("Reason must not be null or empty");

            }
        }
        public class CommandHandler : IRequestHandler<UpdateReasonCommand, bool>
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

            public async Task<bool> Handle(UpdateReasonCommand request, CancellationToken cancellationToken)
            {

                var reason = await _unitOfWork.ComplaintReasonRepository.GetByIdAsync(request.Id);
                if (reason is null) throw new NotFoundException($"reason with Id {request.Id} does not exist!");

                _mapper.Map(request.UpdateModel, reason);
                _unitOfWork.ComplaintReasonRepository.Update(reason);
                return await _unitOfWork.SaveChangesAsync();
            }
        }

    }
}
