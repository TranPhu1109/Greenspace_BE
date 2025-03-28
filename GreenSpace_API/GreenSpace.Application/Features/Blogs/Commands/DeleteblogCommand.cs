using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Blogs.Commands
{
    public class DeleteblogCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public class CommmandValidation : AbstractValidator<DeleteblogCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

            }
        }

        public class CommandHandler : IRequestHandler<DeleteblogCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteblogCommand request, CancellationToken cancellationToken)
            {
                var blog = await _unitOfWork.BlogRepository.GetByIdAsync(request.Id);
                if (blog is null) throw new NotFoundException($"Blog with Id-{request.Id} is not exist!");
                _unitOfWork.BlogRepository.SoftRemove(blog);
                return await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
