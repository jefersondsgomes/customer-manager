using System;
using System.Net;
using System.Threading.Tasks;
using ClientManager.Model.Common;
using ClientManager.Model.Result;
using ClientManager.Repository.Interfaces;
using ClientManager.Service;
using ClientManager.Service.Interfaces;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace ClientManager.Tests
{
    public class ClientUnitTest
    {
        private readonly IMongoRepository<Client> _repository;
        private readonly IClientService _service;

        public ClientUnitTest()
        {
            _repository = Substitute.For<IMongoRepository<Client>>();
            _service = new ClientService(_repository);
        }

        [Fact]
        public async Task TestCreateShouldReturnSuccessWhenExecutionIsOk()
        {
            var expected = new Result<Client>(new Client(), HttpStatusCode.Created);

            _repository.CreateAsync(Arg.Any<Client>()).Returns(new Client());
            var result = await _service.Create(new Client());

            Assert.Equal(expected.StatusCode, result.StatusCode);
        }

        [Fact]
        public async Task TestCreateShouldReturnErrorResultWhenClientIsNull()
        {
            var expected = new Result<Client>(null, HttpStatusCode.BadRequest, new ArgumentNullException("Client cannot be null!"));

            var result = await _service.Create(null);

            Assert.Equal(expected.StatusCode, result.StatusCode);
            Assert.Equal(expected.Error.Message, result.Error.Message);
        }

        [Fact]
        public async Task TestCreateShouldReturnErrorResultWhenCannotCreate()
        {            
            var expected = new Result<Client>(new Client(), HttpStatusCode.InternalServerError, new Exception("could not create the client on database!"));

            _repository.CreateAsync(Arg.Any<Client>()).Throws(new Exception());
            var result = await _service.Create(new Client());

            Assert.Equal(expected.StatusCode, result.StatusCode);
            Assert.Equal(expected.Error.Message, result.Error.Message);
        }

        [Fact]
        public async Task TestDeleteSholdReturnFalseWhenIdIsNullOrEmpty()
        {
            string id = null;
            var expected = new Result<bool>(false, HttpStatusCode.BadRequest, new ArgumentNullException("Parameter ID cannot be null!"));

            var result = await _service.Delete(id);

            Assert.Equal(expected.Value, result.Value);
            Assert.Equal(expected.StatusCode, result.StatusCode);
            Assert.Equal(expected.Error.Message, result.Error.Message);
        }

        [Fact]
        public async Task TestDeleteSholdReturnTrueWhenIdIsOk()
        {
            var expected = new Result<bool>(true, HttpStatusCode.NoContent);

            _repository.RemoveAsync(Arg.Any<string>()).Returns(Task.CompletedTask);
            var result = await _service.Delete(Guid.NewGuid().ToString());

            Assert.Equal(expected.Value, result.Value);
            Assert.Equal(expected.StatusCode, result.StatusCode);
        }

        [Fact]
        public async Task TestDeleteSholdReturnFalseResultWhenExecutionIsNotOk()
        {                        
            var expected = new Result<bool>(false, HttpStatusCode.InternalServerError, 
                new Exception("could not delete the client on database!"));

            _repository.RemoveAsync(Arg.Any<string>()).Throws(new Exception());
            var result = await _service.Delete(Guid.NewGuid().ToString());

            Assert.Equal(expected.Value, result.Value);
            Assert.Equal(expected.StatusCode, result.StatusCode);
            Assert.Equal(expected.Error.Message, result.Error.Message);
        }
    }
}