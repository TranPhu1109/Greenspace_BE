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

public class RefundOrderCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }
    public class CommandHandler : IRequestHandler<RefundOrderCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IClaimsService claimsService;
        public CommandHandler(IUnitOfWork unitOfWork, IClaimsService claimsService)
        {
            this.unitOfWork = unitOfWork;
            this.claimsService = claimsService;

        }
        public async Task<bool> Handle(RefundOrderCommand request, CancellationToken cancellationToken)
        {

            if (request.OrderId == Guid.Empty)
            {
                throw new Exception("ServiceOrderId is required.");
            }
            var bill = await unitOfWork.BillRepository.FirstOrDefaultAsync(x => x.OrderId == request.OrderId);
            if (bill == null)
            {
                throw new Exception($"No Bill found for ServiceOrderId: {request.OrderId}");
            }

            var userWallet = await unitOfWork.WalletRepository.GetByIdAsync(bill.UsersWalletId);

            if (userWallet == null)
            {
                throw new Exception("User's wallet not found in Bill.");
            }
            
            // số tiền hoàn lại
            var refundAmount = bill.Price;

            // Cộng tiền vào ví
            userWallet.Amount += refundAmount;

            // Ghi log giao dịch
            var walletLog = new WalletLog
            {
                Amount = refundAmount,
                Source = $"Refund 30% for ServiceOrder {request.OrderId}",
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
    