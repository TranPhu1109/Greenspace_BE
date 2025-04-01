using GreenSpace.Application.ViewModels.OrderProducts;
using GreenSpace.Application.Features.OrderProduct.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductController : BaseController
    {

        private readonly IMediator _mediator;
        public OrderProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// 
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
            return Ok();
        }
    }
}
