using GreenSpace.Application.Features.Categories.Commands;
using GreenSpace.Application.Features.Categories.Queries;
using GreenSpace.Application.Features.DesignIdeas.Queries;
using GreenSpace.Application.Features.DesignIdeasCategories.Commands;
using GreenSpace.Application.Features.DesignIdeasCategories.Queries;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.DesignIdeasCategory;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignCategoriesController : BaseController
    {
        public readonly IMediator _mediator;

        public DesignCategoriesController(IMediator mediator)
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
        => Ok(await _mediator.Send(new GetAllDesignCategoryQuery { }));



        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetDesignCategoryByIdQuery { Id = id }));


        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}/DesignIdea")]
        public async Task<IActionResult> GetDesignsByCategoryId([FromRoute] Guid id,
                                                                [FromQuery] int pageNumber = 0,
                                                                [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetDesignByCategoryIdQuery { CategoryId = id, PageNumber = pageNumber, PageSize = pageSize }));
        #endregion

        #region Commands
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DesignIdeasCategoryCreateModel model)
        {
            var result = await _mediator.Send(new CreateDesignCategoryCommand { CreateModel = model });
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
        public async Task<IActionResult> Update(Guid id, [FromBody] DesignIdeasCategoryUpdateModel model)
        {
            if (id != model.Id) return BadRequest("Id is not match!");
            var result = await _mediator.Send(new UpdateDesignCategoryCommand { UpdateModel = model });
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
            var result = await _mediator.Send(new DeleteDesignCategoryCommand { Id = id });
            if (!result)
            {
                return BadRequest("Delete Fail!");
            }
            return Ok("Delete successfully");
        }
        #endregion
    }
}
