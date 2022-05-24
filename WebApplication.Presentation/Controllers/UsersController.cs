using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Application.Interfaces.Models.Commands;
using WebApplication.Application.Interfaces.Models.Dto;
using WebApplication.Application.Interfaces.Models.Queries;
using WebApplication.Core.Common.Models;
using WebApplication.Core.Users.Queries;

namespace WebApplication.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserAsync(
            [FromQuery] GetUserQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("Find")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> FindUsersAsync(
            [FromQuery] FindUsersQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }


        [HttpGet]
        [Route("List")]
        [ProducesResponseType(typeof(PaginatedDto<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListUsersAsync(
            [FromQuery] ListUsersQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateUserAsync(
            [FromBody] CreateUserCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUserAsync(
            [FromBody] UpdateUserCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUserAsync(
             int id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteUserCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
