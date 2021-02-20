using CustomerManager.Model.Common;
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
    public class UserUnitTest
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly IUserService _userService;

        public UserUnitTest()
        {
            _userRepository = Substitute.For<IMongoRepository<User>>();
            _userService = new UserService(_userRepository);
        }

        #region VALIDATE

        [Fact]
        public async Task TestValidateShouldReturnFalseWhenUserIsNull()
        {
            var result = await _userService.Validate(null);
            Assert.False(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentNullException>(result.Error);
            Assert.Contains("user cannot be null!", result.Error.Message);
        }

        [Fact]
        public async Task TestValidateShouldReturnFalseWhenExecutionFail()
        {
            _userRepository.FindAsync(Arg.Any<FilterDefinition<User>>()).Throws(new Exception());
            var result = await _userService.Validate(Mock.User.Failed);
            Assert.False(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("could not validate user:", result.Error.Message);
        }

        [Fact]
        public async Task TestValidateShouldReturnFalseWhenUserNotFound()
        {
            _userRepository.FindAsync(Arg.Any<FilterDefinition<User>>()).Returns(Task.FromResult(Mock.User.Null));
            var result = await _userService.Validate(Mock.User.Invalid);
            Assert.False(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("invalid user!", result.Error.Message);
        }

        [Fact]
        public async Task TestValidateUserShouldReturnTrueWhenOk()
        {
            _userRepository.FindAsync(Arg.Any<FilterDefinition<User>>()).Returns(Task.FromResult(Mock.User.Success));
            var result = await _userService.Validate(Mock.User.Success);
            Assert.True(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Error);
        }

        #endregion        
    }
}
