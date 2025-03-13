using GreenSpace.Application.Features.Products.Commands;
using GreenSpace.Application.ViewModels.Products;
using GreenSpace.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateModel model)
        {
            var result = await _mediator.Send(new CreateProductCommand { CreateModel = model });
            if (result is null)
            {
                return BadRequest("Create Fail!");
            }
            return Ok(result);
        }

        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] ProductUpdateModel model)
        {
            if (id != model.Id) return BadRequest("Id is not match!");
            var result = await _mediator.Send(new UpdateProductCommand { UpdateModel = model });
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return NoContent();
        }
    }

}
