using CustomerManager.Models.Entities;
using CustomerManager.Models.Helpers.Interfaces;
using CustomerManager.Repositories.Interfaces;
using CustomerManager.Services;
using CustomerManager.Services.Interfaces;
using MongoDB.Driver;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CustomerManager.Tests
{
    public class AuthenticationUnitTest
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly ISettings _settings;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationUnitTest()
        {
            _userRepository = Substitute.For<IMongoRepository<User>>();
            _settings = Substitute.For<ISettings>();
            _authenticationService = new AuthenticationService(_userRepository, _settings);

            Setup();
        }

        private void Setup()
        {
            _settings.Secret = "supersecret123456789";
        }

        [Fact]
        public async Task TestAuthenticateShouldReturnExceptionResultWhenAuthenticationRequestIsNull()
        {
            var result = await _authenticationService.AuthenticateAsync(Mocks.AuthenticationRequest.Null);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentNullException>(result.Error);
        }

        [Fact]
        public async Task TestAuthenticateShouldReturnExceptionResultWhenUserNameIsNull()
        {
            var result = await _authenticationService.AuthenticateAsync(Mocks.AuthenticationRequest.NullUserName);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentException>(result.Error);
        }

        [Fact]
        public async Task TestAuthenticateShouldReturnExceptionResultWhenPasswordIsNull()
        {
            var result = await _authenticationService.AuthenticateAsync(Mocks.AuthenticationRequest.NullPassword);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentException>(result.Error);
        }

        [Fact]
        public async Task TestAuthenticateShouldReturnExceptionResultWhenRepositoryFail()
        {
            _userRepository.FindAsync(Arg.Any<FilterDefinition<User>>()).Throws(new Exception());
            var result = await _authenticationService.AuthenticateAsync(Mocks.AuthenticationRequest.Error);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public async Task TestAuthenticateShouldReturnExceptionResultWhenUserIsInvalid()
        {
            _userRepository.FindAsync(Arg.Any<FilterDefinition<User>>()).Returns(Task.FromResult<User>(null));
            var result = await _authenticationService.AuthenticateAsync(Mocks.AuthenticationRequest.Ok);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public async Task TestAuthenticateShouldReturnOk()
        {
            _userRepository.FindAsync(Arg.Any<FilterDefinition<User>>()).Returns(Task.FromResult(new User() { Id = "707606698121b6ad3fed413c" }));
            var result = await _authenticationService.AuthenticateAsync(Mocks.AuthenticationRequest.Ok);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Error);
        }
    }
}