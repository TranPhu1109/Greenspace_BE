using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ServiceFeedbacks.Commands
{
    public class DeleteServiceFeedBackCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public class CommmandValidation : AbstractValidator<DeleteServiceFeedBackCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

            }
        }
        public class CommandHandler : IRequestHandler<DeleteServiceFeedBackCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            public CommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteServiceFeedBackCommand request, CancellationToken cancellationToken)
            {
                var feedback = await _unitOfWork.ServiceFeedbackRepositoy.GetByIdAsync(request.Id);
                if (feedback is null) throw new NotFoundException($"ProdutFeedback with Id-{request.Id} is not exist!");
                _unitOfWork.ServiceFeedbackRepositoy.SoftRemove(feedback);
                return await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
