using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Controllers;
using WebApplication.Core.Common.Behaviours;
using WebApplication.Core.Users.Common.Models;
using WebApplication.Middlewares;
using Xunit;

namespace WebApplication.IntegrationTests
{
    public class LoggingTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly Mock<ILogger<ExceptionHandlerMiddleware>> _mockLoggerExceptionHandlerMiddleware;
        private readonly Mock<ILogger<LoggingBehavior<IRequest<UserDto>, UserDto>>> _mockLoggerBehavior;
        private readonly Mock<IMediator> _mediator;

        private readonly UsersController _usersController;
        private readonly Mock<IRequest<UserDto>> _mockRequestUpdateUserResultDTO;
        private readonly Mock<RequestHandlerDelegate<UserDto>> _mockRequestHandlerDelegateUpdateUserResultDTO;

        public LoggingTests()
        {
            _mockLoggerExceptionHandlerMiddleware = new Mock<ILogger<ExceptionHandlerMiddleware>>();
            _mockLoggerBehavior = new Mock<ILogger<LoggingBehavior<IRequest<UserDto>, UserDto>>>();
            _mediator = new Mock<IMediator>();
            _usersController = new UsersController(_mediator.Object);
            _mockRequestUpdateUserResultDTO = new Mock<IRequest<UserDto>>();
            _mockRequestHandlerDelegateUpdateUserResultDTO = new Mock<RequestHandlerDelegate<UserDto>>();
        }

        // TEST NAME - Logging in ExceptionHandlerMiddleware
        // TEST DESCRIPTION - Get user should succeed
        [Fact]
        public async Task CheckLoggerWorksInExceptionHandlerMiddleware()
        {
            //arrange
            var exception = new ArgumentNullException();
            RequestDelegate mockNextMiddleware = (HttpContext) =>
            {
                return Task.FromException(exception);
            };
            var httpContext = new DefaultHttpContext();

            var exceptionHandlingMiddleware = new ExceptionHandlerMiddleware(mockNextMiddleware, _mockLoggerExceptionHandlerMiddleware.Object);

            //act
            await exceptionHandlingMiddleware.InvokeAsync(httpContext);

            //// Assert
            Assert.Equal(LogLevel.Error, _mockLoggerExceptionHandlerMiddleware.Invocations[0].Arguments[0]);
        }

        // TEST NAME - Logging in LoggingBehavior
        // TEST DESCRIPTION - Invalid user id should return bad request
        [Fact]
        public async Task CheckLoggerWorksInLoggingBehavior()
        {
            //arrange
            var mockUpdateUserResultDTO = new UserDto { UserId = 1 };
            var loggingBehavior = new LoggingBehavior<IRequest<UserDto>, UserDto>(_mockLoggerBehavior.Object);

            _mockRequestHandlerDelegateUpdateUserResultDTO.Setup(m => m.Invoke())
                .ReturnsAsync(mockUpdateUserResultDTO);

            //act
            await loggingBehavior.Handle(_mockRequestUpdateUserResultDTO.Object, new CancellationToken(), _mockRequestHandlerDelegateUpdateUserResultDTO.Object);

            //// Assert
            //Start
            Assert.Equal(LogLevel.Information, _mockLoggerBehavior.Invocations[0].Arguments[0]);
            //End
            Assert.Equal(LogLevel.Information, _mockLoggerBehavior.Invocations[1].Arguments[0]);
        }
    }
}
