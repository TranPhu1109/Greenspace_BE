using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ExternalProduct.Commands
{
    public class DeleteExternalProductCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public class CommmandValidation : AbstractValidator<DeleteExternalProductCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

            }
        }

        public class CommandHandler : IRequestHandler<DeleteExternalProductCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteExternalProductCommand request, CancellationToken cancellationToken)
            {
                var external = await _unitOfWork.ExternalProductsRepository.GetByIdAsync(request.Id);
                if (external is null) throw new NotFoundException($"ExternalProduct with Id-{request.Id} is not exist!");
                _unitOfWork.ExternalProductsRepository.SoftRemove(external);
                return await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
