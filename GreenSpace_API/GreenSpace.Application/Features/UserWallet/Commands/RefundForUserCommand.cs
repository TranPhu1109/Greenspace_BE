using GreenSpace.Application.Services.Interfaces;
using GreenSpace.Application.ViewModels.UsersWallets;
using GreenSpace.Domain.Entities;
using GreenSpace.Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.UserWallet.Commands;

public class RefundForUserCommand : IRequest<bool>
{
    public Guid ServiceOrderId { get; set; }
    public class CommandHandler : IRequestHandler<RefundForUserCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IClaimsService claimsService;
        public CommandHandler(IUnitOfWork unitOfWork, IClaimsService claimsService)
        {
            this.unitOfWork = unitOfWork;
            this.claimsService = claimsService;

        }
        public async Task<bool> Handle(RefundForUserCommand request, CancellationToken cancellationToken)
        {

            if (request.ServiceOrderId == Guid.Empty)
            {
                throw new Exception("ServiceOrderId is required.");
            }
            var bill = await unitOfWork.BillRepository.FirstOrDefaultAsync(x => x.ServiceOrderId == request.ServiceOrderId);
            if (bill == null)
            {
                throw new Exception($"No Bill found for ServiceOrderId: {request.ServiceOrderId}");
            }

            var userWallet = await unitOfWork.WalletRepository.GetByIdAsync(bill.UsersWalletId);

            if (userWallet == null)
            {
                throw new Exception("User's wallet not found in Bill.");
            }
            
            // Tính 30% số tiền hoàn lại
            var refundAmount = bill.Price * 0.3m;

            // Cộng tiền vào ví
            userWallet.Amount += refundAmount;

            // Ghi log giao dịch
            var walletLog = new WalletLog
            {
                Amount = refundAmount,
                Source = $"Refund 30% for ServiceOrder {request.ServiceOrderId}",
                TxnRef = DateTime.Now.Ticks.ToString(),
                Type = nameof(WalletLogTypeEnum.Refund),
                WalletId = userWallet.Id
            };

            unitOfWork.WalletRepository.Update(userWallet);
            await unitOfWork.WalletLogRepository.AddAsync(walletLog);
            return await unitOfWork.SaveChangesAsync();
        }
    }

}
    