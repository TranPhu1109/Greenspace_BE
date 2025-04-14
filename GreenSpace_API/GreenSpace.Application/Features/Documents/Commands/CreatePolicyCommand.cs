using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Banner;
using GreenSpace.Application.ViewModels.Document;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using QuestPDF.Drawing.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Documents.Commands
{
    public class CreatePolicyCommand : IRequest<DocumentViewModel>
    {
        public DocumentCreateModel CreateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<CreatePolicyCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.CreateModel.Document1).NotNull().NotEmpty().WithMessage("Name must not be null or empty");

            }
        }
        public class CommandHandler : IRequestHandler<CreatePolicyCommand, DocumentViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;

            public CommandHandler(IUnitOfWork unitOfWork,
                    IMapper mapper,
                    ILogger<CommandHandler> logger,
                    AppSettings appSettings)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
                _appSettings = appSettings;
            }


            public async Task<DocumentViewModel> Handle(CreatePolicyCommand request, CancellationToken cancellationToken)
            {
                var documents = await _unitOfWork.DocumentRepository.GetAllAsync();
                if (documents.Count >0) throw new Exception("There are policy in the database exsit !");

                var policy = _mapper.Map<Document>(request.CreateModel);
                policy.Id = Guid.NewGuid();

                await _unitOfWork.DocumentRepository.AddAsync(policy);
                await _unitOfWork.SaveChangesAsync();



                var ViewModel = _mapper.Map<DocumentViewModel>(policy);


                return ViewModel;
            }


        }
    }
}
