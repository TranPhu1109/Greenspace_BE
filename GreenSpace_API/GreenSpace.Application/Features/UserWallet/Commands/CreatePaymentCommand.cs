using GreenSpace.Application.Features.User.Commands;
using GreenSpace.Application.IntergrationServices.Models;
using GreenSpace.Application.ViewModels.Bills;
using GreenSpace.Application.ViewModels.Users;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.UserWallet.Commands
{
    public class CreatePaymentCommand : IRequest<BillViewModel>
    {
        public CreateBillRequestModel Model { get; set; } = default!;

        public class CommandHandler : IRequestHandler<CreatePaymentCommand, BillViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            public CommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<BillViewModel> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
            {
                var wallet = await _unitOfWork.WalletRepository.FirstOrDefaultAsync(x => x.Id == request.Model.WalletId);
                if (wallet == null) throw new Exception("Ví không tồn tại");

                // 2. Kiểm tra số dư
                if (wallet.Amount < request.Model.Amount) throw new Exception("Số dư không đủ");

                // 3. Trừ tiền
                wallet.Amount -= request.Model.Amount;

                // 5. Tạo Bill
                var bill = new Bill
                {
                    Id = Guid.NewGuid(),
                    OrderId = request.Model.OrderId,
                    ServiceOrderId = request.Model.ServiceOrderId,
                    Price = request.Model.Amount,
                    Description = request.Model.Description,
                    UsersWalletId = request.Model.WalletId
                };
                await _unitOfWork.BillRepository.AddAsync(bill);

                await _unitOfWork.SaveChangesAsync();

                return new BillViewModel
                {
                    Amount = bill.Price,
                    WalletId = bill.UsersWalletId,
                    OrderId = bill.OrderId,
                    ServiceOrderId = bill.ServiceOrderId,
                    Description = bill.Description
                };
            }
        }
    }
}
