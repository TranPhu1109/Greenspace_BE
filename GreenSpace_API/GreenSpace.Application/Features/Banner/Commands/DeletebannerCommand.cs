using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Banner.Commands
{
    public class DeletebannerCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public class CommmandValidation : AbstractValidator<DeletebannerCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

            }
        }

        public class CommandHandler : IRequestHandler<DeletebannerCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeletebannerCommand request, CancellationToken cancellationToken)
            {
                var banner = await _unitOfWork.WebManagerRepository.GetByIdAsync(request.Id);
                if (banner is null) throw new NotFoundException($"Blog with Id-{request.Id} is not exist!");
                _unitOfWork.WebManagerRepository.SoftRemove(banner);
                return await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
