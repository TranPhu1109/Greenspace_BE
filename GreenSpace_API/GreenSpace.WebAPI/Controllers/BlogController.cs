using GreenSpace.Application.Features.Blogs.Commands;
using GreenSpace.Application.Features.Blogs.Queries;
using GreenSpace.Application.Features.Products.Commands;
using GreenSpace.Application.Features.Products.Queries;
using GreenSpace.Application.ViewModels.Blogs;
using GreenSpace.Application.ViewModels.Products;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlogController(IMediator mediator)
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
        => Ok(await _mediator.Send(new GetAllBlogQuery { PageNumber = pageNumber, PageSize = pageSize }));



        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetBlogByIdQuery { Id = id }));


    
        #endregion

        #region Commands

        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BlogCreateModel model)
        {
            var result = await _mediator.Send(new CreateBlogCommand { CreateModel = model });
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
        public async Task<IActionResult> Update(Guid id, [FromBody] BlogCreateModel model)
        {

            var result = await _mediator.Send(new UpdateBlogCommand { Id = id, UpdateModel = model });
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
            var result = await _mediator.Send(new DeleteblogCommand { Id = id });
            if (!result)
            {
                return BadRequest("Delete Fail!");
            }
            return Ok("Delete Successfully!");
        }
        #endregion
    }
}
