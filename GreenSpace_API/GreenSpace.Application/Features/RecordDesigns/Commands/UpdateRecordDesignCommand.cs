using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.RecordDesign;
using GreenSpace.Application.ViewModels.RecordSketch;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.RecordDesigns.Commands
{
    public class UpdateRecordDesignCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public RecordDesignUpdateModel UpdateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<UpdateRecordDesignCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class CommandHandler : IRequestHandler<UpdateRecordDesignCommand, bool>
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

            public async Task<bool> Handle(UpdateRecordDesignCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Update select recordDesign:\n");
                var record = await _unitOfWork.RecordDesignRepository.GetByIdAsync(request.Id);
                if (record is null) throw new NotFoundException($" recordSketch with Id-{request.Id} is not exist!");
                _mapper.Map(request.UpdateModel, record);
                _unitOfWork.RecordDesignRepository.Update(record);
                var result = await _unitOfWork.SaveChangesAsync();
                return result;
            }
        }
    }
}
