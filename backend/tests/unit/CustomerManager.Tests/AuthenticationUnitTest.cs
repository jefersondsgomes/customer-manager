using CustomerManager.Model.Common;
using CustomerManager.Model.Helper;
using CustomerManager.Repository.Interfaces;
using CustomerManager.Service;
using CustomerManager.Service.Interfaces;
using MongoDB.Driver;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CustomerManager.Test
{
    public class AuthenticationUnitTest
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly AppSettings _appSettings;

        public AuthenticationUnitTest()
        {
            _userRepository = Substitute.For<IMongoRepository<User>>();
            _appSettings = Substitute.For<AppSettings>();
            _authenticationService = new AuthenticationService(_userRepository, _appSettings);
        }

        [Fact]
        public async Task TestAuthenticateShouldReturnExceptionResultWhenAuthenticationRequestIsNull()
        {
            var result = await _authenticationService.AuthenticateAsync(Mock.AuthenticationRequest.Null);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentNullException>(result.Error);
        }

        [Fact]
        public async Task TestAuthenticateShouldReturnExceptionResultWhenUserNameIsNull()
        {
            var result = await _authenticationService.AuthenticateAsync(Mock.AuthenticationRequest.NullUserName);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentException>(result.Error);
        }

        [Fact]
        public async Task TestAuthenticateShouldReturnExceptionResultWhenPasswordIsNull()
        {
            var result = await _authenticationService.AuthenticateAsync(Mock.AuthenticationRequest.NullPassword);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentException>(result.Error);
        }

        [Fact]
        public async Task TestAuthenticateShouldReturnExceptionResultWhenRepositoryFail()
        {
            _userRepository.FindAsync(Arg.Any<FilterDefinition<User>>()).Throws(new Exception());
            var result = await _authenticationService.AuthenticateAsync(Mock.AuthenticationRequest.Error);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public async Task TestAuthenticateShouldReturnExceptionResultWhenUserIsInvalid()
        {
            _userRepository.FindAsync(Arg.Any<FilterDefinition<User>>()).Returns(Task.FromResult<User>(null));
            var result = await _authenticationService.AuthenticateAsync(Mock.AuthenticationRequest.Ok);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public async Task TestAuthenticateShouldReturnOk()
        {
            _userRepository.FindAsync(Arg.Any<FilterDefinition<User>>()).Returns(Task.FromResult(new User()));
            var result = await _authenticationService.AuthenticateAsync(Mock.AuthenticationRequest.Ok);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Error);
        }
    }
}
