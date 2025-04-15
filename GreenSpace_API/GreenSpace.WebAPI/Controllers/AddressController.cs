using GreenSpace.Application.Features.Address.Command;
using GreenSpace.Application.Features.Address.Queries;
using GreenSpace.Application.Features.WorkTasks.Queries;
using GreenSpace.Application.ViewModels.Address;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : BaseController
    {
        public readonly IMediator _mediator;
        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Lấy địa chỉ theo ID.
        /// </summary>
        /// <param name="id">ID của địa chỉ.</param>
        /// <returns>Đối tượng AddressViewModel.</returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAddressById(Guid id)
        {
            var query = new GetAddressByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Lấy danh sách địa chỉ theo UserId.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        /// <returns>Danh sách AddressViewModel.</returns>
        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetAddressesByUserId(Guid userId)
        {
            var query = new GetAddressByUserIdQuery { UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Tạo mới một địa chỉ.
        /// </summary>
        /// <param name="createModel">Thông tin địa chỉ cần tạo.</param>
        /// <returns>Đối tượng AddressViewModel của địa chỉ vừa tạo.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressModel createModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new CreateAddressCommand { CreateModel = createModel };
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAddressById), new { id = result.Id }, result);
        }
    }
}

