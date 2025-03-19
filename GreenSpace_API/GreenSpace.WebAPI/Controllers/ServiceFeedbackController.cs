using GreenSpace.Application.Features.ProductFeedbacks.Commands;
using GreenSpace.Application.Features.ProductFeedbacks.Queries;
using GreenSpace.Application.Features.ServiceFeedbacks.Commands;
using GreenSpace.Application.Features.ServiceFeedbacks.Queries;
using GreenSpace.Application.ViewModels.ProductFeedback;
using GreenSpace.Application.ViewModels.ServiceFeedbacks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceFeedbackController : ControllerBase
    {
        public readonly IMediator _mediator;

        public ServiceFeedbackController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Queries
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> Get(
                                            [FromQuery] int pageNumber = 0,
                                            [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetAllServiceFeedbackQuery { }));



        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetServiceFeedbackByIdQuery { Id = id }));


        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}/User")]
        public async Task<IActionResult> GetServiceFeedBackByUserId([FromRoute] Guid id,
                                                                [FromQuery] int pageNumber = 0,
                                                                [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetServiceFeedbackByUserIdQuery { UserId = id, PageNumber = pageNumber, PageSize = pageSize }));


        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetServicesFeedBackByDesignId([FromRoute] Guid id,
                                                                [FromQuery] int pageNumber = 0,
                                                                [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetServiceFeedbackByDesignIdQuery { DesignId = id, PageNumber = pageNumber, PageSize = pageSize }));

        #endregion

        #region Commands
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ServiceFeedbackCreateModel model)
        {
            var result = await _mediator.Send(new CreateServiceFeedBackCommand { CreateModel = model });
            if (result is null)
            {
                return BadRequest("Create Fail!");
            }
            return CreatedAtAction(nameof(GetById), new { Id = result.Id }, result);
        }



        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteServiceFeedBackCommand { Id = id });
            if (!result)
            {
                return BadRequest("Delete Fail!");
            }
            return Ok("Delete successfully");
        }
        #endregion
    }
}
