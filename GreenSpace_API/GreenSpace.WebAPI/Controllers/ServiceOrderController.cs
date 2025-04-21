using GreenSpace.Application.Features.Categories.Commands;
using GreenSpace.Application.Features.Products.Commands;
using GreenSpace.Application.Features.Products.Queries;
using GreenSpace.Application.Features.ServiceOrders.Commands;
using GreenSpace.Application.Features.ServiceOrders.Queries;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.Products;
using GreenSpace.Application.ViewModels.ServiceOrder;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceOrderController : BaseController
    {
        public readonly IMediator _mediator;

        public ServiceOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #region Queries
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("NoIdea")]
        public async Task<IActionResult> GetNoUsingIdea([FromQuery] int pageNumber = 0,
                                             [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetAllServiceOrderNoUsingIdeaQuery{ PageNumber = pageNumber, PageSize = pageSize }));

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("UsingIdea")]
        public async Task<IActionResult> GetUsingIdea([FromQuery] int pageNumber = 0,
                                       [FromQuery] int pageSize = 10)
  => Ok(await _mediator.Send(new GetAllServiceOrderUsingIdeaQuery { PageNumber = pageNumber, PageSize = pageSize }));


        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("status")]
        public async Task<IActionResult> GetStatusConsulting([FromQuery] int pageNumber = 0,
                                                             [FromQuery] int pageSize = 10)
=> Ok(await _mediator.Send(new GetAllServiceOrderStatusConsultingQuery { PageNumber = pageNumber, PageSize = pageSize }));


        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("accountant")]
        public async Task<IActionResult> GetStatusDetermingPrice([FromQuery] int pageNumber = 0,
                                                     [FromQuery] int pageSize = 10)
=> Ok(await _mediator.Send(new GetAllServiceOrderStatusDetermingPriceQuery { PageNumber = pageNumber, PageSize = pageSize }));


        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("MaterialPrice")]
        public async Task<IActionResult> GetStatusDetermingMaterialPrice([FromQuery] int pageNumber = 0,
                                                [FromQuery] int pageSize = 10)
=> Ok(await _mediator.Send(new GetAllServiceOrderStatusMaterialPriceQuery { PageNumber = pageNumber, PageSize = pageSize }));



        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetServiceOrderByIdQuery { Id = id }));

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("UserID-NoUsingIdea/{id}")]
        public async Task<IActionResult> GetNoUsingIdeaByUserID([FromRoute] Guid id,
                                                                [FromQuery] int pageNumber = 0,
                                                                [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetAllServiceOrderNoUsingIdeaByUserIdQuery { UserId = id, PageNumber = pageNumber, PageSize = pageSize }));



        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("UserID-UsingIdea/{id}")]
        public async Task<IActionResult> GetUsingIdeaByUserID([FromRoute] Guid id,
                                                               [FromQuery] int pageNumber = 0,
                                                               [FromQuery] int pageSize = 10)
  => Ok(await _mediator.Send(new GetAllServiceOrderUsingIdeaByUserIdQuery { UserId = id, PageNumber = pageNumber, PageSize = pageSize }));




        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("search-NoUsing")]
        public async Task<IActionResult> GetNoUsingIdeaByFilter([FromQuery] int pageNumber = 0,
                                                                [FromQuery] int pageSize = 10,
                                                                [FromQuery] string? phone = null,
                                                                [FromQuery] string? username = null,
                                                                [FromQuery] int? status = null)
        => Ok(await _mediator.Send(new GetServiceOrderNoUsingIdeatByFillterQuery { PageNumber = pageNumber, PageSize = pageSize, Phone = phone, Username = username, Status = status }));
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("search-Using")]
        public async Task<IActionResult> GetUsingIdeaByFilter([FromQuery] int pageNumber = 0,
                                                             [FromQuery] int pageSize = 10,
                                                             [FromQuery] string? phone = null,
                                                             [FromQuery] string? username = null,
                                                             [FromQuery] int? status = null)
     => Ok(await _mediator.Send(new GetServiceOrderUsingIdeatByFillterQuery { PageNumber = pageNumber, PageSize = pageSize, Phone = phone, Username = username, Status = status }));
        #endregion

        #region Commands
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceOrderCreateModel model)
        {
            var result = await _mediator.Send(new CreateServiceOrderCommand { CreateModel = model });
            if (result is null)
            {
                return BadRequest("Create Fail!");
            }
            return CreatedAtAction(nameof(GetById), new { Id = result.Id }, new { Message = " created Successfully", Data = result });
        }

        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("NoUsing")]
        public async Task<IActionResult> CreateNoUsingIdea([FromBody] ServiceOrderNoUsingCreateModel model)
        {
            var result = await _mediator.Send(new CreateServiceOrderNoIdeaCommand { CreateModel = model });
            if (result is null)
            {
                return BadRequest("Create Fail!");
            }
            return CreatedAtAction(nameof(GetById), new { Id = result.Id }, new { Message = " created Successfully", Data = result });
        }


        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("Customer/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ServiceOrderUpdateModel model)
        {
       
            var result = await _mediator.Send(new UpdateServiceOrderForCustomerCommand { Id = id, UpdateModel = model });
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return Ok("Update Successfully!");
        }

        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] ServiceOrderUpdateStatusModel model)
        {

            var result = await _mediator.Send(new UpdateServiceOrderStatusCommand { Id = id, UpdateModel = model });
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return Ok("Update Successfully!");
        }

        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("DesignPrice/{id}")]
        public async Task<IActionResult> UpdateDesignPrice(Guid id, [FromBody] ServiceOrderUpdateDesignPriceModel model)
        {

            var result = await _mediator.Send(new UpdateServiceOrderDesignPriceCommand { Id = id, UpdateModel = model });
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return Ok("Update Successfully!");
        }

        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("Deposit/{id}")]
        public async Task<IActionResult> UpdateDeposit(Guid id, [FromBody] ServiceOrderUpdateDepositModel model)
        {

            var result = await _mediator.Send(new UpdateServiceOrderForManagerCommand { Id = id, UpdateModel = model });
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return Ok("Update Successfully!");
        }


        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("DesignDetail/{id}")]
        public async Task<IActionResult> UpdateDesignDetail(Guid id, [FromBody] ServiceOrderUpdateDesignDetailModel model)
        {

            var result = await _mediator.Send(new UpdateServiceOrderDesignDetailCommand { Id = id, UpdateModel = model });
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return Ok("Update Successfully!");
        }

        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("Contructor/{id}")]
        public async Task<IActionResult> UpdateContructor(Guid id, [FromBody] ServiceOrderUpdateContructorModel model)
        {

            var result = await _mediator.Send(new UpdateServiceOrderForContructorCommand { Id = id, UpdateModel = model });
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return Ok("Update Successfully!");
        }

        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateForCustomer(Guid id, [FromBody] ServiceOrderUpdateModel model)
        {
          
            var result = await _mediator.Send(new UpdateServiceOrderCommand { Id = id, UpdateModel = model });
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return Ok("Update Successfully!");
        }
        #endregion
    }
}
