using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.UsersWallets;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.UserWallet.Queries;

public class GetUserWalletByIdQuery : IRequest<WalletViewModel>
{
    public Guid Id { get; set; } = default!;

    public class QueryValidation : AbstractValidator<GetUserWalletByIdQuery>
    {
        public QueryValidation()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }
    }
    public class QueryHandler : IRequestHandler<GetUserWalletByIdQuery, WalletViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<WalletViewModel> Handle(GetUserWalletByIdQuery request, CancellationToken cancellationToken)
        {
            
            var wallet = await _unitOfWork.WalletRepository.FirstOrDefaultAsync(x => x.Id == request.Id, x => x.Bills);
            if (wallet is null) throw new BadRequestException($"WalletId-{request.Id} is not exist!");
            // await _cacheService.SetAsync<Wallet>(CacheKey.WALLET + wallet.Id, wallet);
            return _mapper.Map<WalletViewModel>(wallet);
        }
    }
}
