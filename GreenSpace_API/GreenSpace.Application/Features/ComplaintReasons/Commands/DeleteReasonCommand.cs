using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ComplaintReasons.Commands
{
    public class DeleteReasonCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public class CommmandValidation : AbstractValidator<DeleteReasonCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

            }
        }

        public class CommandHandler : IRequestHandler<DeleteReasonCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteReasonCommand request, CancellationToken cancellationToken)
            {
                var reason = await _unitOfWork.ComplaintReasonRepository.GetByIdAsync(request.Id);
                if (reason is null) throw new NotFoundException($"ComplaintReason with Id-{request.Id} is not exist!");
                _unitOfWork.ComplaintReasonRepository.SoftRemove(reason);
                return await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
