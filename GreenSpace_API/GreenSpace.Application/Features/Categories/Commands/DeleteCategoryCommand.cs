using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Categories.Commands
{
    public class DeleteCategoryCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public class CommmandValidation : AbstractValidator<DeleteCategoryCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

            }
        }
        public class CommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            public CommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
            {
                var cate = await _unitOfWork.CategoryRepository.GetByIdAsync(request.Id);
                if (cate is null) throw new NotFoundException($"Category with Id-{request.Id} is not exist!");
                _unitOfWork.CategoryRepository.SoftRemove(cate);
                return await _unitOfWork.SaveChangesAsync();
            }
        }

    }
}
