using GreenSpace.Application.Features.User.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using MediatR;

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
    }
}
