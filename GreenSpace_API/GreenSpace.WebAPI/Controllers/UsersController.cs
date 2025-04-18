﻿using GreenSpace.Application.Features.User.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using MediatR;
using GreenSpace.Application.Features.User.Commands;
using GreenSpace.Application.ViewModels.Users;
using GreenSpace.Application.Features.Blogs.Queries;

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
        /// Lấy hết tất cả User bi ban
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("Banned")]
        public async Task<IActionResult> GetBannedAccount([FromQuery] Dictionary<string, string> filter,
            [FromQuery] int pageNumber = -1)
        => Ok(await _mediator.Send(new GetAllBanUserQuery
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
        /// Lấy tất cả designer
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("Designer")]
        public async Task<IActionResult> GetDesigner([FromQuery] int pageNumber = 0,
                                     [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetAllDesignerQuery { PageNumber = pageNumber, PageSize = pageSize }));

        /// <summary>
        /// Lấy tất cả contructor
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("Contructor")]
        public async Task<IActionResult> GetContructor([FromQuery] int pageNumber = 0,
                                     [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetAllContructorQuery { PageNumber = pageNumber, PageSize = pageSize }));


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
        /// <summary>
        /// Đăng ký tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
        {
            var result = await _mediator.Send(new RegisterUserCommand { Model = model });
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

        /// <summary>
        /// Cập nhật thông tin user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserUpdateModel model)
        {
            var result = await _mediator.Send(new UpdateUserCommand { Model = model, Id = id });
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
        
        /// <summary>
        /// Xoá User theo Id - Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteUserCommand { Id = id });
            return NoContent();
        }

        /// <summary>
        ///  User theo Id - Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("Unban{id}")]
        public async Task<IActionResult> Unban(Guid id)
        {
            await _mediator.Send(new UnbanUserCommand { Id = id });
            return NoContent();
        }
    }
}
