using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Application.Interfaces;
using WebApplication.Application.Interfaces.Models.Commands;
using WebApplication.Application.Interfaces.Models.Dto;
using WebApplication.Infrastructure.Entities;

namespace WebApplication.Implementation.Handlers.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
        {
            private readonly IUserService _userService;
            private readonly IMapper _mapper;

            public UpdateUserCommandHandler(IUserService userService, IMapper mapper)
            {
                _userService = userService;
                _mapper = mapper;
            }
            /// <inheritdoc />
            public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var user = new User
                {
                    Id = request.Id,
                    GivenNames = request.GivenNames,
                    LastName = request.LastName,
                    ContactDetail = new ContactDetail
                    {
                        EmailAddress = request.EmailAddress,
                        MobileNumber = request.MobileNumber
                    }
                };

                User addedUser = await _userService.UpdateAsync(user, cancellationToken);
                UserDto result = _mapper.Map<UserDto>(addedUser);

                return result;
            }
    }
}
