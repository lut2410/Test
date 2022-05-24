using FluentValidation;
using MediatR;
using WebApplication.Application.Interfaces.Models.Dto;

namespace WebApplication.Application.Interfaces.Models.Commands
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string GivenNames { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;

        public class Validator : AbstractValidator<CreateUserCommand>
        {
            public Validator()
            {
                RuleFor(x => x.GivenNames)
                    .NotEmpty();

                RuleFor(x => x.LastName)
                    .NotEmpty();

                RuleFor(x => x.EmailAddress)
                    .NotEmpty();

                RuleFor(x => x.MobileNumber)
                    .NotEmpty();
            }
        }
    }
}
