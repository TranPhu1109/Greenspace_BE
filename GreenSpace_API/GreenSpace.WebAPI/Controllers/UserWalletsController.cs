using GreenSpace.Application.Features.User.Queries;
using GreenSpace.Application.Features.UserWallet.Commands;
using GreenSpace.Application.Features.UserWallet.Queries;
using GreenSpace.Application.IntergrationServices.Models;
using GreenSpace.Application.ViewModels.UsersWallets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers;

public class WalletsController : BaseController
{
    private readonly IMediator mediator;
    public WalletsController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    [HttpGet("test")]
    public IActionResult Test()
    {
        var html = System.IO.File.ReadAllText(@"./wwwroot/payment-sucess.html");
        return base.Content(html, "text/html");
    }
    /// <summary>
    /// Hoàn lại 30% tiền đặt cọc cho đơn dịch vụ.
    /// </summary>
    /// <param name="id">ServiceOrderId</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("refund")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> RefundForUser([FromQuery] Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("ServiceOrderId is required.");
        }

        var result = await mediator.Send(new RefundForUserCommand { ServiceOrderId = id });

        if (!result)
            return StatusCode((int)HttpStatusCode.InternalServerError, "Refund failed.");

        return Ok("Refund successful.");
    }

    /// <summary>
    /// Hoàn tiền
    /// </summary>
    /// <param name="id">ServiceOrderId</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("refund-complaint")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> RefundForComplaint([FromQuery] Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("ComplaintId is required.");
        }

        var result = await mediator.Send(new RefundForComplaintCommand { ComplaintId = id });

        if (!result)
            return StatusCode((int)HttpStatusCode.InternalServerError, "Refund failed.");

        return Ok("Refund successful.");
    }
    /// <summary>
    /// Hoàn lại tiền  cho đơn hàng.
    /// </summary>
    /// <param name="id">ServiceOrderId</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("refund-order")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> RefundOrder([FromQuery] Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("OrderId is required.");
        }

        var result = await mediator.Send(new RefundOrderCommand { OrderId = id });

        if (!result)
            return StatusCode((int)HttpStatusCode.InternalServerError, "Refund failed.");

        return Ok("Refund successful.");
    }


    [HttpGet("vn-pay/response")]
    public async Task<IActionResult> VNPayResponse([FromQuery] string returnUrl)
    {
        var result = await mediator.Send(new VnPayResponseCommand { ReturnUrl = returnUrl });

        if (result)
        {
            return Ok("Nạp tiền vào ví thành công");
        }
        else
        {
            return BadRequest("Nạp tiền vào ví thất bại");
        }
    }

    [Authorize]
    [HttpPost("vn-pay")]
    public async Task<IActionResult> VNPayCallBack([FromBody] decimal amount, bool isMobile)
    {
        var result = await mediator.Send(new RequestVNPayCommand { Amount = amount, IsMobile = isMobile });
        return Ok(result);
    }

    //[Authorize]
    //[HttpPost("vn-pay/refund")]
    //public async Task<IActionResult> VNPayCallBackRefund([FromBody] string TxnRef)
    //{
    //    var result = await mediator.Send(new RequestRefundVNPayCommand { TxnRef = TxnRef });
    //    return Ok(result);
    //}
    #region Queries

    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWalletById([FromRoute] Guid id)
    => Ok(await mediator.Send(new GetUserWalletByIdQuery { Id = id }));

    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet("User{id}")]
    public async Task<IActionResult> GetWalletByUserId([FromRoute] Guid id)
    => Ok(await mediator.Send(new GetUserWalletByUserIdQuery { UserId = id }));
    #endregion
}

