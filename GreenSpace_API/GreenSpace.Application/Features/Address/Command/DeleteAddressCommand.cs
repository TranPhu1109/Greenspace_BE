using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Address.Command
{
    public class DeleteAddressCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public class CommmandValidation : AbstractValidator<DeleteAddressCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

            }
        }

        public class CommandHandler : IRequestHandler<DeleteAddressCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
            {
                var address = await _unitOfWork.AddressRepository.GetByIdAsync(request.Id);
                if (address is null) throw new NotFoundException($"Address with Id-{request.Id} is not exist!");
                _unitOfWork.AddressRepository.SoftRemove(address);
                return await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
