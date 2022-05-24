using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Application.Interfaces;
using WebApplication.Application.Interfaces.Models.Commands;
using WebApplication.Application.Interfaces.Models.Dto;
using WebApplication.Core.Common.Exceptions;
using WebApplication.Infrastructure.Entities;

namespace WebApplication.Implementation.Handlers.Commands
{
        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserDto>
        {
            private readonly IUserService _userService;
            private readonly IMapper _mapper;

            public DeleteUserCommandHandler(IUserService userService, IMapper mapper)
            {
                _userService = userService;
                _mapper = mapper;
            }

            /// <inheritdoc />
            public async Task<UserDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                User? deletedUser = await _userService.DeleteAsync(request.Id, cancellationToken);

                if (deletedUser is default(User)) throw new NotFoundException($"The user '{request.Id}' could not be found.");

                return _mapper.Map<UserDto>(deletedUser);
            }
    }
}
