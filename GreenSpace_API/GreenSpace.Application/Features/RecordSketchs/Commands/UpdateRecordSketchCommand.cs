using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.RecordSketch;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.RecordSketchs.Commands
{
    public class UpdateRecordSketchCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public RecordSketchUpdateModel UpdateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<UpdateRecordSketchCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class CommandHandler : IRequestHandler<UpdateRecordSketchCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;
            public CommandHandler(IUnitOfWork unitOfWork,
                    ILogger<CommandHandler> logger,
                    IMapper mapper,
                    AppSettings appSettings)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _mapper = mapper;
                _appSettings = appSettings;
            }

            public async Task<bool> Handle(UpdateRecordSketchCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Update select recordSketch:\n");
                var recordSketch = await _unitOfWork.RecordSketchRepository.GetByIdAsync(request.Id);
                if (recordSketch is null) throw new NotFoundException($" recordSketch with Id-{request.Id} is not exist!");
                _mapper.Map(request.UpdateModel, recordSketch);
                _unitOfWork.RecordSketchRepository.Update(recordSketch);
                var result = await _unitOfWork.SaveChangesAsync();
                return result;
            }
        }
    }
}
