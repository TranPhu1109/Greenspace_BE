using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Documents.Commands
{
    public class DeletePolicyCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public class CommmandValidation : AbstractValidator<DeletePolicyCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

            }
        }

        public class CommandHandler : IRequestHandler<DeletePolicyCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeletePolicyCommand request, CancellationToken cancellationToken)
            {
                var policy = await _unitOfWork.DocumentRepository.GetByIdAsync(request.Id);
                if (policy is null) throw new NotFoundException($"Policy with Id-{request.Id} is not exist!");
                _unitOfWork.DocumentRepository.SoftRemove(policy);
                return await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
