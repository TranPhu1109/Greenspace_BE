using GreenSpace.Application.Features.Categories.Commands;
using GreenSpace.Application.Features.Products.Commands;
using GreenSpace.Application.Features.Products.Queries;
using GreenSpace.Application.Features.ServiceOrders.Commands;
using GreenSpace.Application.Features.ServiceOrders.Queries;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.Products;
using GreenSpace.Application.ViewModels.ServiceOrder;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceOrderController : BaseController
    {
        public readonly IMediator _mediator;

        public ServiceOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #region Queries
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("NoIdea")]
        public async Task<IActionResult> GetNoUsingIdea([FromQuery] int pageNumber = 0,
                                             [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetAllServiceOrderNoUsingIdeaQuery{ PageNumber = pageNumber, PageSize = pageSize }));

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("UsingIdea")]
        public async Task<IActionResult> GetUsingIdea([FromQuery] int pageNumber = 0,
                                       [FromQuery] int pageSize = 10)
  => Ok(await _mediator.Send(new GetAllServiceOrderUsingIdeaQuery { PageNumber = pageNumber, PageSize = pageSize }));



        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetServiceOrderByIdQuery { Id = id }));

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("UserID-NoUsingIdea/{id}")]
        public async Task<IActionResult> GetNoUsingIdeaByUserID([FromRoute] Guid id,
                                                                [FromQuery] int pageNumber = 0,
                                                                [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetAllServiceOrderNoUsingIdeaByUserIdQuery { UserId = id, PageNumber = pageNumber, PageSize = pageSize }));



        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("UserID-UsingIdea/{id}")]
        public async Task<IActionResult> GetUsingIdeaByUserID([FromRoute] Guid id,
                                                               [FromQuery] int pageNumber = 0,
                                                               [FromQuery] int pageSize = 10)
  => Ok(await _mediator.Send(new GetAllServiceOrderUsingIdeaByUserIdQuery { UserId = id, PageNumber = pageNumber, PageSize = pageSize }));




        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("search-NoUsing")]
        public async Task<IActionResult> GetNoUsingIdeaByFilter([FromQuery] int pageNumber = 0,
                                                                [FromQuery] int pageSize = 10,
                                                                [FromQuery] string? phone = null,
                                                                [FromQuery] string? username = null,
                                                                [FromQuery] int? status = null)
        => Ok(await _mediator.Send(new GetServiceOrderNoUsingIdeatByFillterQuery { PageNumber = pageNumber, PageSize = pageSize, Phone = phone, Username = username, Status = status }));
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("search-Using")]
        public async Task<IActionResult> GetUsingIdeaByFilter([FromQuery] int pageNumber = 0,
                                                             [FromQuery] int pageSize = 10,
                                                             [FromQuery] string? phone = null,
                                                             [FromQuery] string? username = null,
                                                             [FromQuery] int? status = null)
     => Ok(await _mediator.Send(new GetServiceOrderUsingIdeatByFillterQuery { PageNumber = pageNumber, PageSize = pageSize, Phone = phone, Username = username, Status = status }));
        #endregion

        #region Commands
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ServiceOrderCreateModel model)
        {
            var result = await _mediator.Send(new CreateServiceOrderCommand { CreateModel = model });
            if (result is null)
            {
                return BadRequest("Create Fail!");
            }
            return CreatedAtAction(nameof(GetById), new { Id = result.Id }, new { Message = " created Successfully", Data = result });
        }

        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("NoUsing")]
        public async Task<IActionResult> CreateNoUsingIdea([FromForm] ServiceOrderNoUsingCreateModel model)
        {
            var result = await _mediator.Send(new CreateServiceOrderNoIdeaCommand { CreateModel = model });
            if (result is null)
            {
                return BadRequest("Create Fail!");
            }
            return CreatedAtAction(nameof(GetById), new { Id = result.Id }, new { Message = " created Successfully", Data = result });
        }


        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ServiceOrderUpdateModel model)
        {
            if (id != model.Id) return BadRequest("Id is not match!");
            var result = await _mediator.Send(new UpdateServiceOrderCommand { UpdateModel = model });
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return Ok("Update Successfully!");
        }
        #endregion
    }
}
