using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.WorkTasks.Commands
{
    public class DeleteWorkTaskCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public class CommmandValidation : AbstractValidator<DeleteWorkTaskCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

            }
        }
        public class CommandHandler : IRequestHandler<DeleteWorkTaskCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            public CommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteWorkTaskCommand request, CancellationToken cancellationToken)
            {
                var task = await _unitOfWork.WorkTaskRepository.GetByIdAsync(request.Id);
                if (task is null) throw new NotFoundException($"Category with Id-{request.Id} is not exist!");
                _unitOfWork.WorkTaskRepository.SoftRemove(task);
                return await _unitOfWork.SaveChangesAsync();
            }
        }

    }
}
