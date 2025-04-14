using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Banner;
using GreenSpace.Application.ViewModels.Document;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Documents.Commands
{
    public class UpdatePolicyCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DocumentCreateModel UpdateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<UpdatePolicyCommand>
        {
            public CommandValidation()
            {


            }
        }
        public class CommandHandler : IRequestHandler<UpdatePolicyCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;



            public CommandHandler(IUnitOfWork unitOfWork,
                IMapper mapper, ILogger<CommandHandler> logger,
                AppSettings appSettings)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
                _appSettings = appSettings;

            }

            public async Task<bool> Handle(UpdatePolicyCommand request, CancellationToken cancellationToken)
            {

                var policy = await _unitOfWork.DocumentRepository.GetByIdAsync(request.Id);
                if (policy is null) throw new NotFoundException($"blog with Id {request.Id} does not exist!");

                _mapper.Map(request.UpdateModel, policy);
                _unitOfWork.DocumentRepository.Update(policy);
                return await _unitOfWork.SaveChangesAsync();
            }
        }

    }
}
