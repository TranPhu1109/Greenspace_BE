using GreenSpace.Application.Features.RecordDesigns.Commands;
using GreenSpace.Application.Features.RecordDesigns.Queries;
using GreenSpace.Application.Features.RecordSketchs.Commands;
using GreenSpace.Application.Features.RecordSketchs.Queries;
using GreenSpace.Application.ViewModels.RecordDesign;
using GreenSpace.Application.ViewModels.RecordSketch;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordSketchController : BaseController
    {
        public readonly IMediator _mediator;

        public RecordSketchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Queries

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}/orderService")]
        public async Task<IActionResult> GetRecordDesignByOrderServiceId([FromRoute] Guid id,
                                                                         [FromQuery] int pageNumber = 0,
                                                                         [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetRecordSketchByServiceOrderIdQuery { ServiceOrderId = id, PageNumber = pageNumber, PageSize = pageSize }));
        #endregion

        #region Commands


        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RecordSketchUpdateModel model)
        {
           
            var result = await _mediator.Send(new UpdateRecordSketchCommand { Id = id, UpdateModel = model });
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return Ok("Update Successfully");
        }

        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteRecordSketchCommand { Id = id });
            if (!result)
            {
                return BadRequest("Delete Fail!");
            }
            return Ok("Delete successfully");
        }
        #endregion
    }
}
