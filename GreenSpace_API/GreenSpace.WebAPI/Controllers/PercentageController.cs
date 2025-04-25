using GreenSpace.Application.Features.ComplaintReasons.Commands;
using GreenSpace.Application.Features.ComplaintReasons.Queries;
using GreenSpace.Application.Features.TransactionPercentages.Commands;
using GreenSpace.Application.Features.TransactionPercentages.Queries;
using GreenSpace.Application.Features.WorkTasks.Commands;
using GreenSpace.Application.ViewModels.ComplaintReason;
using GreenSpace.Application.ViewModels.TransactionPercentage;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PercentageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PercentageController(IMediator mediator)
        {
            _mediator = mediator;
        }


        #region Queries
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> Get()
        => Ok(await _mediator.Send(new GetPercentageQuery { }));



        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetPercentageByidQuery { Id = id }));



        #endregion

        #region Commands

        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TransactionPercentageCreateModel model)
        {
            var result = await _mediator.Send(new CreatePercentageCommand { CreateModel = model });
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
        public async Task<IActionResult> Update(Guid id, [FromBody] TransactionPercentageCreateModel model)
        {

            var result = await _mediator.Send(new UpdatePercentageCommand { Id = id, UpdateModel = model });
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return Ok("Update Successfully!");
        }

        #endregion
    }
}
