using AutoMapper;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Core.Users.Common.Models;
using WebApplication.Infrastructure.Entities;
using WebApplication.Infrastructure.Interfaces;

namespace WebApplication.Core.Users.Commands
{
    public class UpdateUserCommand : IRequest<UserDto>
    {
        public int Id { get; set; }
        public string GivenNames { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;

        public class Validator : AbstractValidator<UpdateUserCommand>
        {
            public Validator()
            {
                // TODO: Create validation rules for UpdateUserCommand so that all properties are required.
                // If you are feeling ambitious, also create a validation rule that ensures the user exists in the database.
            }
        }

        public class Handler : IRequestHandler<UpdateUserCommand, UserDto>
        {
            private readonly IUserService _userService;
            private readonly IMapper _mapper;

            public Handler(IUserService userService, IMapper mapper)
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
}
