using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GreenSpace.WebAPI.Controllers
{
    public class OrderProductController : BaseController
    {
        private readonly IMediator _mediator;
        public OrderProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrderFromCart([FromBody] CreateOrderFromCartCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
