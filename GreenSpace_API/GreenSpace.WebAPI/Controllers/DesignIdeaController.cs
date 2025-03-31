using GreenSpace.Application.Features.DesignIdeas.Commands;
using GreenSpace.Application.Features.DesignIdeas.Queries;
using GreenSpace.Application.Features.Products.Commands;
using GreenSpace.Application.Features.Products.Queries;
using GreenSpace.Application.ViewModels.DesignIdea;
using GreenSpace.Application.ViewModels.Products;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignIdeaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DesignIdeaController(IMediator mediator)
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
        => Ok(await _mediator.Send(new GetAllDesignIdeaQuery { PageNumber = pageNumber, PageSize = pageSize }));



        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetDesignIdeaByQuery { Id = id }));


        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("search")]
        public async Task<IActionResult> GetByFilter([FromQuery] int pageNumber = 0,
                                                             [FromQuery] int pageSize = 10,
                                                             [FromQuery] string? category = null,
                                                             [FromQuery] string? name = null,
                                                             [FromQuery] decimal? minPrice = null,
                                                             [FromQuery] decimal? maxPrice = null)
        => Ok(await _mediator.Send(new GetDesignIdeaByFillterQuery { PageNumber = pageNumber, PageSize = pageSize, Category = category, Name = name, MinPrice = minPrice, MaxPrice = maxPrice }));
        #endregion

        #region Commands

        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DesignIdeaCreateModel model)
        {
            var result = await _mediator.Send(new CreateDesignIdeaCommand { CreateModel = model });
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
        public async Task<IActionResult> Update(Guid id, [FromBody] DesignIdeaUpdateModel model)
        {
           
            var result = await _mediator.Send(new UpdateDesignIdeaCommand { Id = id, UpdateModel = model });
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return Ok("Update Successfully!");
        }
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteDesignIdeaCommand { Id = id });
            if (!result)
            {
                return BadRequest("Delete Fail!");
            }
            return Ok("Delete Successfully!");
        }
        #endregion
    }
}
