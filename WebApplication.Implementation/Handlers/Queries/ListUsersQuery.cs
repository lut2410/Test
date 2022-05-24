using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Application.Interfaces;
using WebApplication.Application.Interfaces.Models.Dto;
using WebApplication.Core.Common.Models;
using WebApplication.Core.Users.Queries;

namespace WebApplication.Implementation.Handlers.Queries
{
    public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, PaginatedDto<IEnumerable<UserDto>>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public ListUsersQueryHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        /// <inheritdoc />
        public async Task<PaginatedDto<IEnumerable<UserDto>>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
        {
            var usersTask = _userService.GetPaginatedAsync(request.PageNumber, request.ItemsPerPage, cancellationToken);
            var countTask = _userService.CountAsync(cancellationToken);
            var maxCurrentRequestCount = request.PageNumber * request.ItemsPerPage;

            await Task.WhenAll(usersTask, countTask);
            //if (user is default(User)) throw new NotFoundException($"The user '{request.Id}' could not be found.");

            var userDtos = usersTask.Result.Select(u => _mapper.Map<UserDto>(u));
            var result = new PaginatedDto<IEnumerable<UserDto>>
            {
                Data = userDtos,
                HasNextPage = countTask.Result > maxCurrentRequestCount
            };
            return result;
        }
    }
}
