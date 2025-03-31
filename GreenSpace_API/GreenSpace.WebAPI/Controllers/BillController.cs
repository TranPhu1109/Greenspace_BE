using GreenSpace.Application.Features.Bills.Commands;
using GreenSpace.Application.ViewModels.Bills;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BillController : ControllerBase
{
    private readonly IMediator _mediator;

    public BillController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBillRequestModel model)
    {
        return Ok(await _mediator.Send(new CreateBillCommand { CreateModel = model }));
    }
}
