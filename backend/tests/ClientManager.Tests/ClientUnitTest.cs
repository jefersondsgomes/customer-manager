using ClientManager.Model.Common;
using ClientManager.Repository.Interfaces;
using ClientManager.Service;
using ClientManager.Service.Interfaces;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ClientManager.Test
{
    public class ClientUnitTest
    {
        private readonly IMongoRepository<Client> _clientRepository;
        private readonly IClientService _clientService;

        public ClientUnitTest()
        {
            _clientRepository = Substitute.For<IMongoRepository<Client>>();
            _clientService = new ClientService(_clientRepository);

            Setup();
        }

        private void Setup()
        {
            _clientRepository.CreateAsync(Mock.Client.Failed).Throws(new Exception());
            _clientRepository.CreateAsync(Mock.Client.Success).Returns(Task.FromResult(Mock.Client.Success));

            _clientRepository.FindAsync("123").Throws(new Exception());
            _clientRepository.FindAsync("456").Returns(Task.FromResult<Client>(null));
            _clientRepository.FindAsync("789").Returns(Task.FromResult(Mock.Client.Success));

            _clientRepository.ReplaceAsync("789", Mock.Client.Failed).Throws(new Exception());
            _clientRepository.ReplaceAsync("789", Mock.Client.Success).Returns(Task.FromResult(Mock.Client.Success));

            _clientRepository.RemoveAsync("456").Throws(new Exception());
            _clientRepository.RemoveAsync("789").Returns(Task.FromResult(Mock.Client.Success));
        }

        #region CREATE

        [Fact]
        public async Task TestCreateShouldReturnErrorResultWhenClientIsNull()
        {
            var result = await _clientService.CreateAsync(Mock.Client.Null);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentNullException>(result.Error);
            Assert.Contains("client cannot be null!", result.Error.Message);
        }

        [Fact]
        public async Task TestCreateShouldReturnErrorResultWhenCannotCreate()
        {
            var result = await _clientService.CreateAsync(Mock.Client.Failed);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("could not create the client on database:", result.Error.Message);
        }

        [Fact]
        public async Task TestCreateShouldReturnSuccessWhenExecutionIsOk()
        {
            var result = await _clientService.CreateAsync(Mock.Client.Success);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.Null(result.Error);
        }

        #endregion

        #region READ

        [Fact]
        public async Task TestGetShouldReturnErrorResultWhenIdParameterIsNull()
        {
            var result = await _clientService.GetAsync(null);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentNullException>(result.Error);
            Assert.Contains("parameter Id cannot be null or empty!", result.Error.Message);
        }

        [Fact]
        public async Task TestGetShouldReturnErrorResultWhenClientNotFound()
        {
            var result = await _clientService.GetAsync("456");
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("client not found!", result.Error.Message);
        }

        [Fact]
        public async Task TestGetShouldReturnErrorResultWhenExecutionIsNotOk()
        {
            var result = await _clientService.GetAsync("123");
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("could not get the client from database:", result.Error.Message);
        }

        [Fact]
        public async Task TestGetShouldReturnSuccessResultWhenClientExists()
        {
            var result = await _clientService.GetAsync("789");
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Error);
        }

        #endregion

        #region READ ALL

        [Fact]
        public async Task TestGetAllShouldReturnErrorResultWhenRepositoryFail()
        {
            _clientRepository.FindAsync().Throws(new Exception());
            var result = await _clientService.GetAllAsync();
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("could not list the clients from database:", result.Error.Message);
        }

        [Fact]
        public async Task TestGetAllShouldReturnErrorResultWhenwhenThereAreNoRecords()
        {
            _clientRepository.FindAsync().Returns(Task.FromResult<IList<Client>>(new List<Client>()));
            var result = await _clientService.GetAllAsync();
            Assert.NotNull(result.Value);
            Assert.False(result.Value.Any());
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("there are no clients!", result.Error.Message);
        }

        [Fact]
        public async Task TestGetAllShouldReturnSuccessResultWhenRunsSuccessfully()
        {
            _clientRepository.FindAsync().Returns(Task.FromResult<IList<Client>>(new List<Client>() { new Client() }));
            var result = await _clientService.GetAllAsync();
            Assert.NotNull(result.Value);
            Assert.True(result.Value.Any());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Error);
        }

        #endregion

        #region UPDATE

        [Fact]
        public async Task TestUpdateShouldReturnErrorResultWhenIdNull()
        {
            var result = await _clientService.UpdateAsync(null, Mock.Client.Success);
            Assert.Equal(Mock.Client.Success, result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentNullException>(result.Error);
            Assert.Contains("id cannot be null or empty!", result.Error.Message);
        }

        [Fact]
        public async Task TestUpdateShouldReturnErrorResultWhenClientIsNull()
        {
            var result = await _clientService.UpdateAsync("123", Mock.Client.Null);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentNullException>(result.Error);
            Assert.Contains("client cannot be null!", result.Error.Message);
        }

        [Fact]
        public async Task TestUpdateShouldReturnErrorResultWhenClientDoesNotExistsInDatabase()
        {
            var result = await _clientService.UpdateAsync("456", Mock.Client.Success);
            Assert.NotNull(result.Error);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("client to be updated was not found!", result.Error.Message);
        }

        [Fact]
        public async Task TestUpdateShouldReturnErrorResultWhenExceptionsWasThrowedInExecution()
        {
            var result = await _clientService.UpdateAsync("789", Mock.Client.Failed);
            Assert.Equal(Mock.Client.Failed, result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("could not be update the client:", result.Error.Message);
        }

        [Fact]
        public async Task TestUpdateShouldReturnSuccessResultWhenExecutionIsOk()
        {
            var result = await _clientService.UpdateAsync("789", Mock.Client.Success);
            Assert.Equal(Mock.Client.Success, result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            Assert.Null(result.Error);
        }

        #endregion

        # region DELETE

        [Fact]
        public async Task TestDeleteSholdReturnFalseResultWhenIdIsNull()
        {
            var result = await _clientService.DeleteAsync(null);
            Assert.False(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentNullException>(result.Error);
            Assert.Contains("id cannot be null or empty!", result.Error.Message);
        }

        [Fact]
        public async Task TestDeleteSholdReturnFalseResultWhenExecutionIsNotOk()
        {
            var result = await _clientService.DeleteAsync("456");
            Assert.False(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("could not delete the client on database:", result.Error.Message);
        }

        [Fact]
        public async Task TestDeleteSholdReturnTrueResulteWhenExecutionIsOk()
        {
            var result = await _clientService.DeleteAsync("789");
            Assert.True(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            Assert.Null(result.Error);
        }

        #endregion
    }
}