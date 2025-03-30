using GreenSpace.Application.IntergrationServices.Models;
using GreenSpace.Application.Utilities;
using GreenSpace.Domain.Entities;
using GreenSpace.Domain.Enum;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GreenSpace.Application.Features.UserWallet.Commands;

public class VnPayResponseCommand : IRequest<bool>
{
    //public VnPayResponseModel Model { get; set; } = new();
    public string ReturnUrl { get; set; } = string.Empty;
    public class CommandHandler : IRequestHandler<VnPayResponseCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly AppSettings appSettings;
        public CommandHandler(IUnitOfWork unitOfWork,
                AppSettings appSettings)
        {
            this.appSettings = appSettings;
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(VnPayResponseCommand request, CancellationToken cancellationToken)
        {

            const string toolService = nameof(VnPayResponseCommand);

            // Parse URL để lấy các tham số
            Uri uri = new Uri(request.ReturnUrl);
            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            // Lấy userId từ URL
            if (!Guid.TryParse(queryParams["userId"], out Guid userId))
            {
                return false;
            }

            // Lấy chữ ký từ response
            string vnp_SecureHash = queryParams["vnp_SecureHash"];
            if (string.IsNullOrEmpty(vnp_SecureHash))
                return false;

            // Tạo VnPayLibrary và thêm các tham số VNPay (không bao gồm userId và vnp_SecureHash)
            var vnPayLibrary = new VnPayLibrary();
            foreach (string key in queryParams.AllKeys)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_") &&
                    key != "vnp_SecureHash" && key != "vnp_SecureHashType")
                {
                    vnPayLibrary.AddResponseData(key, queryParams[key]);
                }
            }

            // Kiểm tra chữ ký
            if (!vnPayLibrary.ValidateSignature(vnp_SecureHash, appSettings.VnPay.Vnp_HashSecret))
            {
                return false;
            }

            // Kiểm tra trạng thái giao dịch
            if (queryParams["vnp_TransactionStatus"] != "00")
            {
                return false;
            }

            // Tìm ví của người dùng
            var wallet = await unitOfWork.WalletRepository.FirstOrDefaultAsync(x => x.UserId == userId);
            if (wallet is null)
                throw new ArgumentException($"Source: {toolService}_Wallet is null");

            // Kiểm tra giao dịch đã tồn tại
            var isExisted = await unitOfWork.WalletLogRepository.FirstOrDefaultAsync(x =>
                x.TransactionNo == queryParams["vnp_TransactionNo"] &&
                x.TxnRef == queryParams["vnp_TxnRef"]);

            if (isExisted is not null)
            {
                return false;
            }

            // Tạo log giao dịch
            var walletLog = new WalletLog()
            {
                Amount = long.Parse(queryParams["vnp_Amount"] ?? "0") / 100,
                TxnRef = queryParams["vnp_TxnRef"],
                TransactionNo = queryParams["vnp_TransactionNo"],
                Source = "VNPay",
                Type = nameof(WalletLogTypeEnum.Deposit),
                WalletId = wallet.Id,
                CreationDate = DateTime.ParseExact(queryParams["vnp_PayDate"], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)
            };

            // Cập nhật ví và lưu log
            wallet.Amount += walletLog.Amount;
            unitOfWork.WalletRepository.Update(wallet);
            await unitOfWork.WalletLogRepository.AddAsync(walletLog);

            return await unitOfWork.SaveChangesAsync();
        }


    }
        
}
