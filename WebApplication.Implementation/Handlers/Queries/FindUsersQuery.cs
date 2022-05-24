using AutoMapper;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Application.Interfaces;
using WebApplication.Application.Interfaces.Models.Dto;
using WebApplication.Application.Interfaces.Models.Queries;
using WebApplication.Infrastructure.Entities;

namespace WebApplication.Implementation.Handlers.Queries
{
    public class FindUsersQueryHandler : IRequestHandler<FindUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public FindUsersQueryHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserDto>> Handle(FindUsersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<User> users = await _userService.FindAsync(request.GivenNames, request.LastName, cancellationToken);
            return users.Select(user => _mapper.Map<UserDto>(user));
        }
    }
}
