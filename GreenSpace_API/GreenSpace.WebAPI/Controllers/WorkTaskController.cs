﻿using GreenSpace.Application.Features.Categories.Commands;
using GreenSpace.Application.Features.Categories.Queries;
using GreenSpace.Application.Features.Products.Queries;
using GreenSpace.Application.Features.WorkTasks.Commands;
using GreenSpace.Application.Features.WorkTasks.Queries;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.WorkTasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkTaskController : BaseController
    {
        public readonly IMediator _mediator;

        public WorkTaskController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Tất cả task
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        #region Queries
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> Get(
                                            [FromQuery] int pageNumber = 0,
                                            [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetAllWorkTaskQuery { }));

        /// <summary>
        /// Task của Designer
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("Designer")]
        public async Task<IActionResult> GetAllDesignerTask(
                                            [FromQuery] int pageNumber = 0,
                                            [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetAllDesignTaskQuery { }));

        /// <summary>
        /// Task của Contructor
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("Contructor")]
        public async Task<IActionResult> GetAllContructorTask(
                                            [FromQuery] int pageNumber = 0,
                                            [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetAllContructTaskQuery { }));


        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetWorkTaskByIdQuery { Id = id }));


        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}/Users")]
        public async Task<IActionResult> GetWorkTaskByUserId([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetWorkTaskByUserIdQuery { UserId = id}));
        #endregion

        #region Commands
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("Design")]
        public async Task<IActionResult> Create([FromBody] WorkTaskCreateModel model)
        {
            var result = await _mediator.Send(new CreateWorkTasksCommand { CreateModel = model });
            if (result is null)
            {
                return BadRequest("Create Fail!");
            }
            return CreatedAtAction(nameof(GetById), new { Id = result.Id }, new { Message = " created Successfully", Data = result });
        }

        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("Contruction")]
        public async Task<IActionResult> CreateContructTask([FromBody] WorkTaskCreateModel model)
        {
            var result = await _mediator.Send(new CreateContructTaskCommand { CreateModel = model });
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
        public async Task<IActionResult> Update(Guid id, [FromBody] WorkTaskUpdateModel model)
        {
        
            var result = await _mediator.Send(new UpdateWorkTasksCommand { Id = id, UpdateModel = model });
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
            var result = await _mediator.Send(new DeleteWorkTaskCommand { Id = id });
            if (!result)
            {
                return BadRequest("Delete Fail!");
            }
            return Ok("Delete successfully");
        }
        #endregion
    }
}
