using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.RecordDesigns.Commands
{
    public class DeleteRecordDesignCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public class CommmandValidation : AbstractValidator<DeleteRecordDesignCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

            }
        }
        public class CommandHandler : IRequestHandler<DeleteRecordDesignCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            public CommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteRecordDesignCommand request, CancellationToken cancellationToken)
            {
                var record = await _unitOfWork.RecordDesignRepository.GetByIdAsync(request.Id);
                if (record is null) throw new NotFoundException($"record with Id-{request.Id} is not exist!");
                _unitOfWork.RecordDesignRepository.SoftRemove(record);
                return await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
