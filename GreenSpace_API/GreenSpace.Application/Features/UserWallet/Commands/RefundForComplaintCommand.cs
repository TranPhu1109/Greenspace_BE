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

public class RefundForComplaintCommand : IRequest<bool>
{
    public Guid ComplaintId { get; set; }
    public class CommandHandler : IRequestHandler<RefundForComplaintCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IClaimsService claimsService;
        public CommandHandler(IUnitOfWork unitOfWork, IClaimsService claimsService)
        {
            this.unitOfWork = unitOfWork;
            this.claimsService = claimsService;

        }
        public async Task<bool> Handle(RefundForComplaintCommand request, CancellationToken cancellationToken)
        {

            var complaint = await unitOfWork.ComplaintRepository.GetByIdAsync(request.ComplaintId, x => x.ComplaintDetails);
            if (complaint == null)
            {
                throw new Exception("Complaint not found.");
            }
            var user = await unitOfWork.UserRepository.GetByIdAsync(complaint.UserId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            var userWallet = await unitOfWork.WalletRepository.GetByIdAsync(user.WalletId);

            if (userWallet == null)
            {
                throw new Exception("User's wallet not found in Bill.");
            }
            
            // Tính 30% số tiền hoàn lại
            var listcomplaintDetail = complaint.ComplaintDetails.Where(x => x.IsCheck == true).ToList();
            var sum = listcomplaintDetail.Sum(x => x.TotalPrice);

            // Cộng tiền vào ví
            userWallet.Amount += sum;

            // Ghi log giao dịch
            var walletLog = new WalletLog
            {
                Amount = sum,
                Source = $"Hoàn tiền cho đơn phản ánh {request.ComplaintId}",
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
    