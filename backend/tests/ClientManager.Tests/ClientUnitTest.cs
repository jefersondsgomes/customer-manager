using System;
using System.Collections.Generic;
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


        #region CREATE
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
        #endregion

        #region READ
        
        [Fact]
        public async Task TestGetShouldReturnErrorResultWhenIdParameterIsNull()
        {
            var expected = new Result<Client>(null, HttpStatusCode.BadRequest, 
                new ArgumentNullException("Parameter ID cannot be null or empty!"));

            var result = await _service.Get(null);

            Assert.Equal(expected.StatusCode, result.StatusCode);
            Assert.Equal(expected.Error.Message, result.Error.Message);            
        }

        [Fact]
        public async Task TestGetShouldReturnErrorResultWhenExecutionIsNotOk()
        {
            var expected = new Result<Client>(null, 
                HttpStatusCode.InternalServerError, new Exception("An error occurred while trying to get the client!"));

            _repository.FindAsync(Arg.Any<string>()).Throws(new Exception());
            var result = await _service.Get(Guid.NewGuid().ToString());

            Assert.Equal(expected.StatusCode, result.StatusCode);
            Assert.Equal(expected.Error.Message, result.Error.Message);            
        }

        [Fact]
        public async Task TestGetShouldReturnErrorResultWhenClientNotFound()
        {
            var expected = new Result<Client>(null, 
                HttpStatusCode.NotFound, new Exception("Client not found!"));

            _repository.FindAsync(Arg.Any<string>()).Returns((Client)null);
            var result = await _service.Get(Guid.NewGuid().ToString());

            Assert.Equal(expected.StatusCode, result.StatusCode);
            Assert.Equal(expected.Error.Message, result.Error.Message);            
        }

        [Fact]
        public async Task TestGetShouldReturnSuccessResultWhenClientExists()
        {
            var client = new Client();
            var expected = new Result<Client>(client, HttpStatusCode.OK);

            _repository.FindAsync(Arg.Any<string>()).Returns(client);
            var result = await _service.Get(Guid.NewGuid().ToString());

            Assert.Equal(expected.StatusCode, result.StatusCode);
            Assert.Equal(expected.Value, result.Value);
        }

        #endregion
        
        #region UPDATE

        [Fact]
        public async Task TestUpdateShouldReturnErrorResultWhenIdIsNull()
        {
            var expected = new Result<Client>(new Client(), HttpStatusCode.BadRequest, new ArgumentNullException("Parameter ID cannot be null or empty!"));

            var result = await _service.Update(null, new Client());

            Assert.Equal(expected.StatusCode, result.StatusCode);
            Assert.Equal(expected.Error.Message, result.Error.Message);
        }

        [Fact]
        public async Task TestUpdateShouldReturnErrorResultWhenClientIsNull()
        {
            var expected = new Result<Client>(null, HttpStatusCode.BadRequest, new ArgumentNullException("Client cannot be null!"));

            var result = await _service.Update(Guid.NewGuid().ToString(), null);

            Assert.Equal(expected.StatusCode, result.StatusCode);
            Assert.Equal(expected.Error.Message, result.Error.Message);
        }

        [Fact]
        public async Task TestUpdateShouldReturnErrorResultWhenClientDoesNotExistsInDatabase()
        {
            Client nullClient = null;
            var expected = new Result<Client>(null, HttpStatusCode.BadRequest, new Exception("The client to be updated does not exists in database!"));

            _repository.FindAsync(Arg.Any<string>()).Returns(nullClient);
            var result = await _service.Update(Guid.NewGuid().ToString(), new Client());

            Assert.Equal(expected.StatusCode, result.StatusCode);
            Assert.Equal(expected.Error.Message, result.Error.Message);
        }

        [Fact]
        public async Task TestUpdateShouldReturnErrorResultWhenExceptionsWasThrowedInExecution()
        {
            var expected = new Result<Client>(new Client(), HttpStatusCode.InternalServerError, new Exception("Could not be update the client!"));
            
            _repository.FindAsync(Arg.Any<string>()).Returns(new Client());
            _repository.ReplaceAsync(Arg.Any<string>(), Arg.Any<Client>()).Throws(new Exception());
            
            var result = await _service.Update(Guid.NewGuid().ToString(), new Client());

            Assert.Equal(expected.StatusCode, result.StatusCode);
            Assert.Equal(expected.Error.Message, result.Error.Message);
        }

        [Fact]
        public async Task TestUpdateShouldReturnSuccessResultWhenExecutionIsOk()
        {
            var expected = new Result<Client>(new Client(), HttpStatusCode.OK);
            
            _repository.FindAsync(Arg.Any<string>()).Returns(new Client());
            _repository.ReplaceAsync(Arg.Any<string>(), Arg.Any<Client>()).Returns(new Client());
            
            var result = await _service.Update(Guid.NewGuid().ToString(), new Client());

            Assert.Equal(expected.StatusCode, result.StatusCode);
        }


        #endregion

        # region DELETE
        [Fact]
        public async Task TestDeleteSholdReturnFalseResultWhenIdIsNull()
        {
            string id = null;
            var expected = new Result<bool>(false, HttpStatusCode.BadRequest, new ArgumentNullException("Parameter ID cannot be null!"));

            var result = await _service.Delete(id);

            Assert.Equal(expected.Value, result.Value);
            Assert.Equal(expected.StatusCode, result.StatusCode);
            Assert.Equal(expected.Error.Message, result.Error.Message);
        }

        [Fact]
        public async Task TestDeleteSholdReturnTrueResulteWhenExecutionIsOk()
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
        #endregion
    }
}