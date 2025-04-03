using GreenSpace.Application.ViewModels.OrderProducts;
using GreenSpace.Application.Features.OrderProduct.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using GreenSpace.Application.Features.OrderProduct.Queries;
using GreenSpace.Application.Features.ServiceOrders.Commands;
using GreenSpace.Application.ViewModels.ServiceOrder;

namespace GreenSpace.WebAPI.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class OrderProductController : BaseController
    {

        private readonly IMediator _mediator;
        public OrderProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        ///     #region Queries
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 0,
                                             [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetAllOrderProductQuery { PageNumber = pageNumber, PageSize = pageSize }));



        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetOrderProductByIdQuery { Id = id }));


        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("User")]
        public async Task<IActionResult> GetUserOrders([FromQuery] int pageNumber = 0,
                                                       [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetOrderProductByUserIdQuery { PageNumber = pageNumber, PageSize = pageSize }));





        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> CreateOrderFromCart([FromBody] CreateOrderProductModel model)
        {
            var result = await _mediator.Send(new CreateOrderFromCartCommand { CreateModel = model} );
            if (result is null)
            {
                return BadRequest("Create Fail!");
            }
            return Ok("create successful");
        }

        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("Buy-Now")]
        public async Task<IActionResult> BuyNow([FromBody] CreateOrderModel model)
        {
            var result = await _mediator.Send(new BuyNowCommand { Model = model });
            if (result is null)
            {
                return BadRequest("Create Fail!");
            }
            return Ok();
        }


        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] OrderUpdateStatusModel model)
        {

            var result = await _mediator.Send(new UpdateOrderStatusCommand { Id = id, UpdateModel = model });
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return Ok("Update Successfully!");
        }
    }
}
