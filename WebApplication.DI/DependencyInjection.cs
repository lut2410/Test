using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebApplication.Infrastructure.Contexts;
using WebApplication.Core.Common.Behaviours;
using WebApplication.Core.Common.CustomProblemDetails;
using WebApplication.Core.Common.Exceptions;
using WebApplication.Application.Interfaces;
using WebApplication.Implementation;
using WebApplication.Implementation.Handlers.Commands;
using WebApplication.Implementation.Handlers.Queries;
using WebApplication.Application.Interfaces.Mappings;
using WebApplication.Application.Interfaces.Models.Commands;
using WebApplication.Application.Interfaces.Models.Queries;

namespace WebApplication.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.AddMediatR(new[] { typeof(CreateUserCommandHandler).Assembly, typeof(FindUsersQueryHandler).Assembly }, cfg => cfg.AsScoped());
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddInfrastructureServices();
            AssemblyScanner.FindValidatorsInAssemblies(new[] { typeof(CreateUserCommand.Validator).Assembly, typeof(FindUsersQuery.Validator).Assembly })
                .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddProblemDetails(
                options =>
                {
                    options.IncludeExceptionDetails = (context, ex) => env.IsDevelopment();
                    options.Map<ValidationException>(ex => new BadRequestProblemDetails(ex));
                    options.Map<InvalidOperationException>(ex => new BadRequestProblemDetails(ex));
                    options.Map<ArgumentOutOfRangeException>(ex => new BadRequestProblemDetails(ex));
                    options.Map<NotFoundException>(ex => new NotFoundProblemDetails(ex));
                    options.Map<Exception>(ex => new UnhandledExceptionProblemDetails(ex));
                }
            );
            services.AddDbContext<InMemoryContext>();

            services.AddScoped<IUserService, UserService>();

            return services;
        }
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddDbContext<InMemoryContext>();

            return services;
        }
    }
}
