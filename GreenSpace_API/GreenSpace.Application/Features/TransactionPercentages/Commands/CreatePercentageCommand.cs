using AutoMapper;
using FluentValidation;
using GreenSpace.Application.ViewModels.Banner;
using GreenSpace.Application.ViewModels.TransactionPercentage;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GreenSpace.Application.Features.TransactionPercentages.Commands
{
    public class CreatePercentageCommand : IRequest<TransactionPercentageViewModel>
    {
        public TransactionPercentageCreateModel CreateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<CreatePercentageCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.CreateModel.DepositPercentage)
                     .GreaterThanOrEqualTo(30.0m)
                     .WithMessage("Deposit must be at least 30%")
                     .LessThanOrEqualTo(80.0m)
                     .WithMessage("Deposit must not exceed 80%");

                RuleFor(x => x.CreateModel.RefundPercentage)
                    .GreaterThanOrEqualTo(10.0m)
                    .WithMessage("Refund must be at least 10%")
                    .LessThanOrEqualTo(50.0m)
                    .WithMessage("Refund must not exceed 50%");


            }
        }
        public class CommandHandler : IRequestHandler<CreatePercentageCommand, TransactionPercentageViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;
            private readonly IMediator _mediator;
            public CommandHandler(IUnitOfWork unitOfWork,
                    IMapper mapper,
                    ILogger<CommandHandler> logger,
                    AppSettings appSettings,
                    IMediator mediator)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
                _appSettings = appSettings;
                _mediator = mediator;
            }


            public async Task<TransactionPercentageViewModel> Handle(CreatePercentageCommand request, CancellationToken cancellationToken)
            {
                var exist = await _unitOfWork.TransactionPercentageRepository.GetAllAsync();
                if (exist.Count > 0) throw new Exception("There are TransactionPercentage in the database exsit !");

                var tage = _mapper.Map<TransactionPercentage>(request.CreateModel);
                tage.Id = Guid.NewGuid();

                await _unitOfWork.TransactionPercentageRepository.AddAsync(tage);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("TransactionPercentage created successfully with ID: {BannerId}", tage.Id);


                var ViewModel = _mapper.Map<TransactionPercentageViewModel>(tage);


                return ViewModel;
            }


        }
    }
}
