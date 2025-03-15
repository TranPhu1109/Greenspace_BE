using GreenSpace.Application.Features.User.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using MediatR;
using GreenSpace.Application.Features.User.Commands;
using GreenSpace.Application.ViewModels.Users;

namespace GreenSpace.WebAPI.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Lấy hết tất cả User
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Dictionary<string, string> filter,
            [FromQuery] int pageNumber = -1)
        => Ok(await _mediator.Send(new GetAllUserQuery
        {
            Filter = filter,
            PageNumber = pageNumber
        }));

        /// <summary>
        /// Lấy User theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        => Ok(await _mediator.Send(new GetUserByIdQuery{ Id = id }));

        /// <summary>
        /// Tạo mới một user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateModel model)
        {
            var result = await _mediator.Send(new CreateUserCommand { Model = model });
            if (result is not null)
            {
                return CreatedAtAction(
                    actionName: nameof(GetById),
                    routeValues: new { id = result.Id },
                    value: result
                );
            }
            else return BadRequest();
        }
    }
}
