using GreenSpace.Application.Features.Banner.Commands;
using GreenSpace.Application.Features.Banner.Queries;
using GreenSpace.Application.Features.Blogs.Commands;
using GreenSpace.Application.Features.Blogs.Queries;
using GreenSpace.Application.Features.Dashboard.Queries;
using GreenSpace.Application.ViewModels.Banner;
using GreenSpace.Application.ViewModels.Blogs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class WebManageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WebManageController(IMediator mediator)
        {
            _mediator = mediator;
        }


        #region Queries
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("banner")]
        public async Task<IActionResult> Get()
     => Ok(await _mediator.Send(new GetAllBannerQuery()));



        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("Banner/{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetBannerByIdQuery { Id = id }));

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("Logo")]
        public async Task<IActionResult> GetLogo()
    => Ok(await _mediator.Send(new GetLogoQuery()));



        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("Logo/{id}")]
        public async Task<IActionResult> GetByLogoId([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetLogoByIdQuery { Id = id }));

        #endregion

        #region Commands

        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("banner")]
        public async Task<IActionResult> Create([FromBody] BannerCreateModel model)
        {
            var result = await _mediator.Send(new CreateBannerCommand { CreateModel = model });
            if (result is null)
            {
                return BadRequest("Create Fail!");
            }
            return CreatedAtAction(nameof(GetById), new { Id = result.Id }, new { Message = " created Successfully", Data = result });
        }



        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("Banner/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BannerCreateModel model)
        {

            var result = await _mediator.Send(new UpdateBannerCommand { Id = id, UpdateModel = model });
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return Ok("Update Successfully!");
        }
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpDelete("Banner/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeletebannerCommand { Id = id });
            if (!result)
            {
                return BadRequest("Delete Fail!");
            }
            return Ok("Delete Successfully!");
        }

        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("Logo")]
        public async Task<IActionResult> CreateLogo([FromBody] LogoCreateModel model)
        {
            var result = await _mediator.Send(new CreateLogoCommand { CreateModel = model });
            if (result is null)
            {
                return BadRequest("Create Fail!");
            }
            return CreatedAtAction(nameof(GetByLogoId), new { Id = result.Id }, new { Message = " created Successfully", Data = result });
        }

        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("Logo/{id}")]
        public async Task<IActionResult> UpdateLogo(Guid id, [FromBody] LogoCreateModel model)
        {

            var result = await _mediator.Send(new UpdateLogoCommand { Id = id, UpdateModel = model });
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return Ok("Update Successfully!");
        }






        #endregion
    }
}
