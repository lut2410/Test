using FluentValidation;
using MediatR;
using System.Collections.Generic;
using WebApplication.Application.Interfaces.Models.Dto;
using WebApplication.Core.Common.Models;

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
                RuleFor(x => x.PageNumber)
                   .GreaterThan(0);
            }
        }
    }
}
