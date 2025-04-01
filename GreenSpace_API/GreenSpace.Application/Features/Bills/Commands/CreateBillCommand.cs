using AutoMapper;
using FluentValidation;
using GreenSpace.Application.ViewModels.Bills;
using GreenSpace.Domain.Entities;
using GreenSpace.Domain.Enum;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GreenSpace.Application.Features.Bills.Commands;

public class CreateBillCommand : IRequest<BillViewModel>
{
    public CreateBillRequestModel CreateModel { get; set; } = default!;

    public class CommandValidation : AbstractValidator<CreateBillCommand>
    {
        public CommandValidation()
        {
            RuleFor(x => x.CreateModel.WalletId).NotEmpty().WithMessage("WalletId must not be empty");

            RuleFor(x => x.CreateModel.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero");

            RuleFor(x => x.CreateModel.Description).NotNull().NotEmpty().WithMessage("Description must not be null or empty");

            RuleFor(x => x.CreateModel).Must(x => x.OrderId.HasValue || x.ServiceOrderId.HasValue)
                .WithMessage("Either OrderId or ServiceOrderId must be provided");
        }
    }

    public class CommandHandler : IRequestHandler<CreateBillCommand, BillViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CommandHandler> _logger;
        private readonly AppSettings _appSettings;

        public CommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CommandHandler> logger,
            AppSettings appSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _appSettings = appSettings;
        }

        public async Task<BillViewModel> Handle(CreateBillCommand request, CancellationToken cancellationToken)
        {

            // Kiểm tra ví tồn tại
            var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(request.CreateModel.WalletId);
            if (wallet == null)
            {
                throw new ApplicationException($"Wallet with ID {request.CreateModel.WalletId} not found");
            }

            // Kiểm tra số dư ví
            if (wallet.Amount < request.CreateModel.Amount)
            {
                throw new ApplicationException("Tiền trong ví của bạn không đủ thực hiện giao dịch này");
            }

            // Trừ tiền từ ví
            wallet.Amount -= request.CreateModel.Amount;
            _unitOfWork.WalletRepository.Update(wallet);

            // Tạo WalletLog
            var walletLog = new WalletLog
            {
                Id = Guid.NewGuid(),
                WalletId = wallet.Id,
                Amount = -request.CreateModel.Amount, 
                Status = WalletLogStatusEnum.Sucess.ToString(),
                Type = WalletLogTypeEnum.Pay.ToString(),
                CreationDate = DateTime.UtcNow,
        
            };
            await _unitOfWork.WalletLogRepository.AddAsync(walletLog);

            // Tạo mới Bill
            var bill = _mapper.Map<Bill>(request.CreateModel);
            bill.Id = Guid.NewGuid();
            bill.CreationDate = DateTime.UtcNow;

            await _unitOfWork.BillRepository.AddAsync(bill);
            await _unitOfWork.SaveChangesAsync();

            var billViewModel = _mapper.Map<BillViewModel>(bill);

            return billViewModel;
        }
    }
}

