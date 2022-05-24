using AutoMapper;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Application.Interfaces;
using WebApplication.Application.Interfaces.Models.Commands;
using WebApplication.Application.Interfaces.Models.Dto;
using WebApplication.Infrastructure.Entities;

namespace WebApplication.Implementation.Handlers.Commands
{
        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
        {
            private readonly IUserService _userService;
            private readonly IMapper _mapper;

            public CreateUserCommandHandler(IUserService userService, IMapper mapper)
            {
                _userService = userService;
                _mapper = mapper;
            }

            /// <inheritdoc />
            public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                User user = new User
                {
                    GivenNames = request.GivenNames,
                    LastName = request.LastName,
                    ContactDetail = new ContactDetail
                    {
                        EmailAddress = request.EmailAddress,
                        MobileNumber = request.MobileNumber
                    }
                };

                User addedUser = await _userService.AddAsync(user, cancellationToken);
                UserDto result = _mapper.Map<UserDto>(addedUser);

                return result;
            }
        }
}
