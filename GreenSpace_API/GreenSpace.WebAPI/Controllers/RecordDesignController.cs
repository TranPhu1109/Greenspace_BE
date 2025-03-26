using GreenSpace.Application.Features.Categories.Commands;
using GreenSpace.Application.Features.Categories.Queries;
using GreenSpace.Application.Features.Products.Queries;
using GreenSpace.Application.Features.RecordDesigns.Commands;
using GreenSpace.Application.Features.RecordDesigns.Queries;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.RecordDesign;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordDesignController : BaseController
    {
        public readonly IMediator _mediator;

        public RecordDesignController(IMediator mediator)
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
        => Ok(await _mediator.Send(new GetRecordDesignByServiceOrderIdQuery { ServiceOrderId = id, PageNumber = pageNumber, PageSize = pageSize }));
        #endregion

        #region Commands


        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RecordDesignUpdateModel model)
        {
           
            var result = await _mediator.Send(new UpdateRecordDesignCommand { Id = id, UpdateModel = model });
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
            var result = await _mediator.Send(new DeleteRecordDesignCommand { Id = id });
            if (!result)
            {
                return BadRequest("Delete Fail!");
            }
            return Ok("Delete successfully");
        }
        #endregion
    }
}
