using GreenSpace.Application.Features.Carts.Commands;
using GreenSpace.Application.Features.Carts.Queries;
using GreenSpace.Application.ViewModels.MongoDbs.Carts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenSpace.WebAPI.Controllers;
[Route("api/[controller]")]
public class CartsController : ControllerBase
{
    private readonly IMediator mediator;
    public CartsController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetCartByUser()
    {
        var result = await mediator.Send(new GetCartByUserQuery());
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> CreateCart([FromBody] CartCreateModel model)
    {
        var result = await mediator.Send(new CreateCartCommand { model = model });
        return Ok(result);
    }
    [HttpDelete, Authorize]
    public async Task<IActionResult> DeleteCart(DeleteCartCommand request)
        => Ok(await mediator.Send(request));

    [HttpPut, Authorize]
    public async Task<IActionResult> UpdateCart([FromBody] UpdateCartCommand request)
    {
        return Ok(await mediator.Send(request));
    }
    [HttpPut("remove-item"), Authorize]
    public async Task<IActionResult> RemoveItem([FromBody] DeleteItemFromCartCommand request)
    {
        return Ok(await mediator.Send(request));
    }
}
