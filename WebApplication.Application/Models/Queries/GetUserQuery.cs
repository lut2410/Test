using FluentValidation;
using MediatR;
using WebApplication.Application.Interfaces.Models.Dto;

namespace WebApplication.Application.Interfaces.Models.Queries

{
    public class GetUserQuery : IRequest<UserDto>
    {
        public int Id { get; set; }

        public class Validator : AbstractValidator<GetUserQuery>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0);
            }
        }
    }
}
