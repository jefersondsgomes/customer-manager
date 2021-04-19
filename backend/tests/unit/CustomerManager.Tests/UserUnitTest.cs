using CustomerManager.Models.Entities;
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
    public class UserUnitTest
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly IUserService _userService;

        public UserUnitTest()
        {
            _userRepository = Substitute.For<IMongoRepository<User>>();
            _userService = new UserService(_userRepository);

            Setup();
        }

        private void Setup()
        {
            _userRepository.CreateAsync(Mocks.User.Failed).Throws(new Exception());
            _userRepository.CreateAsync(Mocks.User.Success).Returns(Task.FromResult(Mocks.User.Success));
            _userRepository.FindAsync("0").Throws(new Exception());
            _userRepository.FindAsync("1").Returns(Task.FromResult<User>(null));
            _userRepository.FindAsync("2").Returns(Task.FromResult(new User()));
        }

        [Fact]
        public async Task TestValidateShouldReturnFalseWhenUserIsNull()
        {
            var result = await _userService.AuthenticateAsync(null);
            Assert.False(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentNullException>(result.Error);
            Assert.Contains("user cannot be null!", result.Error.Message);
        }

        [Fact]
        public async Task TestAuthenticateShouldReturnFalseWhenExecutionFail()
        {
            _userRepository.FindAsync(Arg.Any<FilterDefinition<User>>()).Throws(new Exception());
            var result = await _userService.AuthenticateAsync(Mocks.User.Failed);
            Assert.False(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("could not authenticate user:", result.Error.Message);
        }

        [Fact]
        public async Task TestAuthenticateShouldReturnFalseWhenUserNotFound()
        {
            _userRepository.FindAsync(Arg.Any<FilterDefinition<User>>()).Returns(Task.FromResult(Mocks.User.Null));
            var result = await _userService.AuthenticateAsync(Mocks.User.Invalid);
            Assert.False(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("invalid credentials!", result.Error.Message);
        }

        [Fact]
        public async Task TestAuthenticateUserShouldReturnTrueWhenOk()
        {
            _userRepository.FindAsync(Arg.Any<FilterDefinition<User>>()).Returns(Task.FromResult(Mocks.User.Success));
            var result = await _userService.AuthenticateAsync(Mocks.User.Success);
            Assert.True(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task TestCreateUserShouldReturnExceptionWhenUserIsNull()
        {
            var result = await _userService.CreateAsync(Mocks.User.Null);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentNullException>(result.Error);
            Assert.Contains("user cannot be null!", result.Error.Message);
        }

        [Fact]
        public async Task TestCreateUserShouldReturnExceptionWhenRepositoryFail()
        {
            var result = await _userService.CreateAsync(Mocks.User.Failed);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("could not create user:", result.Error.Message);
        }

        [Fact]
        public async Task TesteCreateUserShouldReturnExceptionResultWhenUserAlreadyExists()
        {
            _userRepository.FindAsync(Arg.Any<FilterDefinition<User>>()).Returns(Mocks.User.Exists);
            var result = await _userService.CreateAsync(Mocks.User.Exists);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("user already exists!", result.Error.Message);
        }

        [Fact]
        public async Task TestCreateUserShouldReturnOk()
        {
            _userRepository.FindAsync(Arg.Any<FilterDefinition<User>>()).Returns(Task.FromResult<User>(null));
            var result = await _userService.CreateAsync(Mocks.User.Success);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task TestGetUserShouldReturnExceptionResultWhenIdIsNullOrEmpty()
        {
            var result = await _userService.GetAsync(string.Empty);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentException>(result.Error);
        }

        [Fact]
        public async Task TestGetUserShouldReturnExceptionResultWhenRepositoryFail()
        {
            var result = await _userService.GetAsync("0");
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public async Task TestGetUserShouldReturnExceptionResultWhenUserWasNotFound()
        {
            var result = await _userService.GetAsync("1");
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public async Task TestGetUserShouldReturnOk()
        {
            var result = await _userService.GetAsync("2");
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Error);
        }
    }
}