using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Bills;
using GreenSpace.Application.ViewModels.UsersWallets;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GreenSpace.Application.Features.UserWallet.Queries
{
    public class GetUserWalletByUserIdQuery : IRequest<WalletViewModel>
    {
        public Guid UserId { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetUserWalletByUserIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.UserId).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }
        public class QueryHandler : IRequestHandler<GetUserWalletByUserIdQuery, WalletViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<QueryHandler> _logger;

            public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper,  ILogger<QueryHandler> logger)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
            }
            public async Task<WalletViewModel> Handle(GetUserWalletByUserIdQuery request, CancellationToken cancellationToken)
            {

                var wallet = await _unitOfWork.WalletRepository.FirstOrDefaultAsync(x => x.UserId == request.UserId);
                if (wallet is null) throw new BadRequestException($"User-{request.UserId} is not exist any Wallet!");

                var result = _mapper.Map<WalletViewModel>(wallet);
                result.Bills = await GetBills(wallet.Id);
                return result;
            }
            private async Task<List<BillViewModel>> GetBills(Guid walletId)
            {
                var transaction = await _unitOfWork.BillRepository.WhereAsync(x => x.UsersWalletId == walletId, x => x.Payment!, x => x.Payment!.Orders);
                return _mapper.Map<List<BillViewModel>>(transaction);
            }
        }
    }
}
