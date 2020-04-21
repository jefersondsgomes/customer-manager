using System;
using System.Net;
using System.Threading.Tasks;
using ClientManager.Model.Result;
using ClientManager.Model.User;
using ClientManager.Repository.Interfaces;
using ClientManager.Service;
using ClientManager.Service.Interfaces;
using MongoDB.Driver;
using NSubstitute;
using Xunit;

namespace ClientManager.Tests
{
    public class UserUnitTest
    {
        private readonly IMongoRepository<User> _repository;
        private readonly IUserService _service;

        public UserUnitTest()
        {
            _repository = Substitute.For<IMongoRepository<User>>(); 
            _service = new UserService(_repository);
        }

        #region Setup
        private static User NullUser() => null;
        private static User InvalidUser() => new User() {Login = "Any", Password = "Any"};
        private static User ValidUser() => new User() {Login = "Ok", Password = "Ok"};
        #endregion

        [Fact]
        public async Task TestValidateUserShouldReturnFalseWhenUserIsNull()
        {
            User user = NullUser();
            var expected = new Result<bool>(false, HttpStatusCode.NotFound, new ArgumentNullException("User cannot be null!"));

            var result = await _service.ValidadeUser(user);

            Assert.Equal(expected.Value, result.Value);
            Assert.Equal(expected.StatusCode, result.StatusCode);
            Assert.Equal(expected.Error.Message, result.Error.Message);
        }

        [Fact]
        public async Task TestValidateUserShouldReturnFalseWhenDoesNotExists()
        {
            User user = InvalidUser();            
            var expected = new Result<bool>(false, HttpStatusCode.NotFound, new Exception("Invalid User or Password!"));
            
            _repository.FindAsync(Arg.Any<FilterDefinition<User>>()).Returns<User>(NullUser());
            var result = await _service.ValidadeUser(user);

            Assert.Equal(expected.Value, result.Value);
            Assert.Equal(expected.StatusCode, result.StatusCode);
            Assert.Equal(expected.Error.Message, result.Error.Message);
        }

        [Fact]
        public async Task TestValidateUserShouldReturnTrueWhenUserExists()
        {
            User user = ValidUser();                   
            var expected = new Result<bool>(true, HttpStatusCode.OK);

            _repository.FindAsync(Arg.Any<FilterDefinition<User>>()).Returns<User>(user);
            var result = await _service.ValidadeUser(user);

            Assert.Equal(expected.Value, result.Value);
            Assert.Equal(expected.StatusCode, result.StatusCode);
        }
    }
}
