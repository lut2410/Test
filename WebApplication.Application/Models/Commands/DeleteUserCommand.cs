using FluentValidation;
using MediatR;
using WebApplication.Application.Interfaces.Models.Dto;

namespace WebApplication.Application.Interfaces.Models.Commands
{
    public class DeleteUserCommand : IRequest<UserDto>
    {
        public int Id { get; set; }
        public class Validator : AbstractValidator<DeleteUserCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0);
            }
        }
    }
}
