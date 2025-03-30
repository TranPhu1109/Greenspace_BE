using GreenSpace.Application.Features.Blogs.Commands;
using GreenSpace.Application.Features.Blogs.Queries;
using GreenSpace.Application.Features.Contracts.Commands;
using GreenSpace.Application.Features.Contracts.Queries;
using GreenSpace.Application.Features.Products.Queries;
using GreenSpace.Application.Services;
using GreenSpace.Application.ViewModels.Blogs;
using GreenSpace.Application.ViewModels.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ContractController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContractController(IMediator mediator)
        {
            _mediator = mediator;
        }


        #region Queries



        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetContractByIdQuery { Id = id }));

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 0,
                                     [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetAllContractQuery { PageNumber = pageNumber, PageSize = pageSize }));


        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}/Users")]
        public async Task<IActionResult> GetContractByUserId([FromRoute] Guid id,
                                                                [FromQuery] int pageNumber = 0,
                                                                [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetContractByUserIdQuery { UserId = id, PageNumber = pageNumber, PageSize = pageSize }));

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}/ServiceOrder")]
        public async Task<IActionResult> GetContractByServiceOrderId([FromRoute] Guid id,
                                                             [FromQuery] int pageNumber = 0,
                                                             [FromQuery] int pageSize = 10)
     => Ok(await _mediator.Send(new GetContractByServiceOrderIdQuery { ServiceOrderId = id, PageNumber = pageNumber, PageSize = pageSize }));


        #endregion

        #region Commands

        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContractCreateModel model)
        {
            var result = await _mediator.Send(new CreateContractCommand { CreateModel = model });
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
        public async Task<IActionResult> Update(Guid id, [FromBody] ContractUpdateModel model)
        {

            var result = await _mediator.Send(new UpdateContractSignatureCommand { ContractId = id, UpdateModel = model });
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return Ok("Update Successfully!");
        }

        #endregion
    }
}

