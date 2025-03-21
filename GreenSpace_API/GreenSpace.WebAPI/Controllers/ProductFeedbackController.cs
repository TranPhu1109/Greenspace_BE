using GreenSpace.Application.Features.Categories.Commands;
using GreenSpace.Application.Features.Categories.Queries;
using GreenSpace.Application.Features.ProductFeedbacks.Commands;
using GreenSpace.Application.Features.ProductFeedbacks.Queries;
using GreenSpace.Application.Features.Products.Queries;
using GreenSpace.Application.ViewModels.Category;
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
    public class ProductFeedbackController : ControllerBase
    {
        public readonly IMediator _mediator;

        public ProductFeedbackController(IMediator mediator)
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
            => Ok(await _mediator.Send(new GetAllProductFeedbackQuery { PageNumber = pageNumber, PageSize = pageSize }));



        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetFeedbackProductByIdQuery { Id = id }));


        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}/User")]
        public async Task<IActionResult> GetProductsFeedBackByUserId([FromRoute] Guid id,
                                                                [FromQuery] int pageNumber = 0,
                                                                [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetFeedbackProductByUserIdQuery { UserId = id, PageNumber = pageNumber, PageSize = pageSize }));


        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetProductsFeedBackByProductId([FromRoute] Guid id,
                                                                [FromQuery] int pageNumber = 0,
                                                                [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetFeedbackProductByProductIdQuery { ProductId = id, PageNumber = pageNumber, PageSize = pageSize }));

        #endregion

        #region Commands
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductFeedbackCreateModel model)
        {
            var result = await _mediator.Send(new CreateProductFeedBackCommand { CreateModel = model });
            if (result is null)
            {
                return BadRequest("Create Fail!");
            }
            return CreatedAtAction(nameof(GetById), new { Id = result.Id }, result);
        }

        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] ProductFeedbackUpdateModel model)
        {
            if (id != model.Id) return BadRequest("Id is not match!");
            var result = await _mediator.Send(new UpdateProductFeedBackCommand { UpdateModel = model });
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
            var result = await _mediator.Send(new DeleteProductFeedBackCommand { Id = id });
            if (!result)
            {
                return BadRequest("Delete Fail!");
            }
            return Ok("Delete successfully");
        }
        #endregion
    }
}

