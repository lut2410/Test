using AutoMapper;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Core.Common.Models;
using WebApplication.Core.Users.Common.Models;
using WebApplication.Infrastructure.Interfaces;

namespace WebApplication.Core.Users.Queries
{
    public class ListUsersQuery : IRequest<PaginatedDto<IEnumerable<UserDto>>>
    {
        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; } = 10;

        public class Validator : AbstractValidator<ListUsersQuery>
        {
            public Validator()
            {
                // TODO: Create a validation rule so that PageNumber is always greater than 0
            }
        }

        public class Handler : IRequestHandler<ListUsersQuery, PaginatedDto<IEnumerable<UserDto>>>
        {
            private readonly IUserService _userService;
            private readonly IMapper _mapper;
            public Handler(IUserService userService, IMapper mapper)
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
}
