using GreenSpace.Application.Features.Blogs.Commands;
using GreenSpace.Application.Features.Blogs.Queries;
using GreenSpace.Application.Features.Complaints.Commands;
using GreenSpace.Application.Features.Complaints.Queries;
using GreenSpace.Application.Features.ServiceOrders.Queries;
using GreenSpace.Application.Features.WorkTasks.Queries;
using GreenSpace.Application.ViewModels.Blogs;
using GreenSpace.Application.ViewModels.Complaints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ComplaintController(IMediator mediator)
        {
            _mediator = mediator;
        }


        #region Queries
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 0,
                                             [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetAllComplaintQuery { PageNumber = pageNumber, PageSize = pageSize }));

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}/Users")]
        public async Task<IActionResult> GetComplaintByUserId([FromRoute] Guid id)
=> Ok(await _mediator.Send(new GetComplaintByUserIdQuery { UserId = id }));

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetComplaintByIdQuery { Id = id }));

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("Refund")]
        public async Task<IActionResult> GetRefund([FromQuery] int pageNumber = 0,
                                     [FromQuery] int pageSize = 10)
=> Ok(await _mediator.Send(new GetAllComplainRefundQuery { PageNumber = pageNumber, PageSize = pageSize }));

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("ProductReturn")]
        public async Task<IActionResult> GetProductReturn([FromQuery] int pageNumber = 0,
                                       [FromQuery] int pageSize = 10)
  => Ok(await _mediator.Send(new GetAllComplainProductReturnQuery { PageNumber = pageNumber, PageSize = pageSize }));

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("StatusRefund")]
        public async Task<IActionResult> GetStatusRefund([FromQuery] int pageNumber = 0,
                               [FromQuery] int pageSize = 10)
=> Ok(await _mediator.Send(new GetAllComplainStatusRefundQuery { PageNumber = pageNumber, PageSize = pageSize }));

        #endregion

        #region Commands

        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ComplaintCreateModel model)
        {
            var result = await _mediator.Send(new CreateComplaintCommand { CreateModel = model });
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
        public async Task<IActionResult> Update(Guid id, [FromBody] ComplaintUpdateModel model)
        {

            var result = await _mediator.Send(new UpdateComplaintCommand { Id = id, UpdateModel = model });
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return Ok("Update Successfully!");
        }

        #endregion
    }
}
