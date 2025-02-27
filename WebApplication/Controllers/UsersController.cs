﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Core.Common.Models;
using WebApplication.Core.Users.Commands;
using WebApplication.Core.Users.Common.Models;
using WebApplication.Core.Users.Queries;
using WebApplication.Filters;

namespace WebApplication.Controllers
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

        // TODO: create a route (at /Find) that can retrieve a list of matching users using the `FindUsersQuery`
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


        // TODO: create a route (at /List) that can retrieve a paginated list of users using the `ListUsersQuery`
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

        // TODO: create a route that can create a user using the `CreateUserCommand`
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateUserAsync(
            [FromBody] CreateUserCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        // TODO: create a route that can update an existing user using the `UpdateUserCommand`
        [HttpPut]
        //[Route("Update")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUserAsync(
            [FromBody] UpdateUserCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        // TODO: create a route that can delete an existing user using the `DeleteUserCommand`
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
