using AutoMapper;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Application.Interfaces;
using WebApplication.Application.Interfaces.Models.Dto;
using WebApplication.Application.Interfaces.Models.Queries;
using WebApplication.Core.Common.Exceptions;
using WebApplication.Infrastructure.Entities;

namespace WebApplication.Implementation.Handlers.Queries
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            User? user = await _userService.GetAsync(request.Id, cancellationToken);

            if (user is default(User)) throw new NotFoundException($"The user '{request.Id}' could not be found.");

            UserDto result = _mapper.Map<UserDto>(user);

            return result;
        }
    }
}
