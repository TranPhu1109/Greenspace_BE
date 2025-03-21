using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.RecordSketchs.Commands
{
    public class DeleteRecordSketchCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public class CommmandValidation : AbstractValidator<DeleteRecordSketchCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

            }
        }
        public class CommandHandler : IRequestHandler<DeleteRecordSketchCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            public CommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteRecordSketchCommand request, CancellationToken cancellationToken)
            {
                var recordSketch = await _unitOfWork.RecordSketchRepository.GetByIdAsync(request.Id);
                if (recordSketch is null) throw new NotFoundException($"recordSketch with Id-{request.Id} is not exist!");
                _unitOfWork.RecordSketchRepository.SoftRemove(recordSketch);
                return await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
