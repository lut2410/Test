using AutoMapper;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Application.Interfaces.Models.Dto;
using WebApplication.Infrastructure.Entities;

namespace WebApplication.Application.Interfaces.Models.Queries
{
    public class FindUsersQuery : IRequest<IEnumerable<UserDto>>
    {
        public string? GivenNames { get; set; }
        public string? LastName { get; set; }

        public class Validator : AbstractValidator<FindUsersQuery>
        {
            public Validator()
            {
                RuleFor(x => x.GivenNames)
                    .NotEmpty()
                    .When(x => string.IsNullOrWhiteSpace(x.LastName));

                RuleFor(x => x.LastName)
                    .NotEmpty()
                    .When(x => string.IsNullOrWhiteSpace(x.GivenNames));
            }
        }
    }
}
