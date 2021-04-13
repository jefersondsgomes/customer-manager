using CustomerManager.Model.Common;
using CustomerManager.Repository.Interfaces;
using CustomerManager.Service;
using CustomerManager.Service.Interfaces;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CustomerManager.Test
{
    public class CustomerUnitTest
    {
        private readonly IMongoRepository<Customer> _customerRepository;
        private readonly ICustomerService _customerService;

        public CustomerUnitTest()
        {
            _customerRepository = Substitute.For<IMongoRepository<Customer>>();
            _customerService = new CustomerService(_customerRepository);

            Setup();
        }

        private void Setup()
        {
            _customerRepository.CreateAsync(Mock.Customer.Failed).Throws(new Exception());
            _customerRepository.CreateAsync(Mock.Customer.Success).Returns(Task.FromResult(Mock.Customer.Success));

            _customerRepository.FindAsync("123").Throws(new Exception());
            _customerRepository.FindAsync("456").Returns(Task.FromResult<Customer>(null));
            _customerRepository.FindAsync("789").Returns(Task.FromResult(Mock.Customer.Success));

            _customerRepository.ReplaceAsync("789", Mock.Customer.Failed).Throws(new Exception());
            _customerRepository.ReplaceAsync("789", Mock.Customer.Success).Returns(Task.FromResult(Mock.Customer.Success));

            _customerRepository.RemoveAsync("456").Throws(new Exception());
            _customerRepository.RemoveAsync("789").Returns(Task.FromResult(Mock.Customer.Success));
        }

        [Fact]
        public async Task TestCreateShouldReturnErrorResultWhenCustomerIsNull()
        {
            var result = await _customerService.CreateAsync(Mock.Customer.Null);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentNullException>(result.Error);
            Assert.Contains("customer cannot be null!", result.Error.Message);
        }

        [Fact]
        public async Task TestCreateShouldReturnErrorResultWhenCannotCreate()
        {
            var result = await _customerService.CreateAsync(Mock.Customer.Failed);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("could not create the customer on database:", result.Error.Message);
        }

        [Fact]
        public async Task TestCreateShouldReturnSuccessWhenExecutionIsOk()
        {
            var result = await _customerService.CreateAsync(Mock.Customer.Success);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task TestGetShouldReturnErrorResultWhenIdParameterIsNull()
        {
            var result = await _customerService.GetAsync(null);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentNullException>(result.Error);
            Assert.Contains("parameter Id cannot be null or empty!", result.Error.Message);
        }

        [Fact]
        public async Task TestGetShouldReturnErrorResultWhenCustomerNotFound()
        {
            var result = await _customerService.GetAsync("456");
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("customer not found!", result.Error.Message);
        }

        [Fact]
        public async Task TestGetShouldReturnErrorResultWhenExecutionIsNotOk()
        {
            var result = await _customerService.GetAsync("123");
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("could not get the customer from database:", result.Error.Message);
        }

        [Fact]
        public async Task TestGetShouldReturnSuccessResultWhenCustomerExists()
        {
            var result = await _customerService.GetAsync("789");
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task TestGetAllShouldReturnErrorResultWhenRepositoryFail()
        {
            _customerRepository.FindAsync().Throws(new Exception());
            var result = await _customerService.GetAllAsync();
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("could not list the customers from database:", result.Error.Message);
        }

        [Fact]
        public async Task TestGetAllShouldReturnErrorResultWhenwhenThereAreNoRecords()
        {
            _customerRepository.FindAsync().Returns(Task.FromResult<IList<Customer>>(new List<Customer>()));
            var result = await _customerService.GetAllAsync();
            Assert.NotNull(result.Value);
            Assert.False(result.Value.Any());
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("there are no customers!", result.Error.Message);
        }

        [Fact]
        public async Task TestGetAllShouldReturnSuccessResultWhenRunsSuccessfully()
        {
            _customerRepository.FindAsync().Returns(Task.FromResult<IList<Customer>>(new List<Customer>() { new Customer() }));
            var result = await _customerService.GetAllAsync();
            Assert.NotNull(result.Value);
            Assert.True(result.Value.Any());
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task TestUpdateShouldReturnErrorResultWhenIdNull()
        {
            var result = await _customerService.UpdateAsync(null, Mock.Customer.Success);
            Assert.Equal(Mock.Customer.Success, result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentNullException>(result.Error);
            Assert.Contains("id cannot be null or empty!", result.Error.Message);
        }

        [Fact]
        public async Task TestUpdateShouldReturnErrorResultWhenCustomerIsNull()
        {
            var result = await _customerService.UpdateAsync("123", Mock.Customer.Null);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentNullException>(result.Error);
            Assert.Contains("customer cannot be null!", result.Error.Message);
        }

        [Fact]
        public async Task TestUpdateShouldReturnErrorResultWhenCustomerDoesNotExistsInDatabase()
        {
            var result = await _customerService.UpdateAsync("456", Mock.Customer.Success);
            Assert.NotNull(result.Error);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("customer to be updated was not found!", result.Error.Message);
        }

        [Fact]
        public async Task TestUpdateShouldReturnErrorResultWhenExceptionsWasThrowedInExecution()
        {
            var result = await _customerService.UpdateAsync("789", Mock.Customer.Failed);
            Assert.Equal(Mock.Customer.Failed, result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("could not be update the customer:", result.Error.Message);
        }

        [Fact]
        public async Task TestUpdateShouldReturnSuccessResultWhenExecutionIsOk()
        {
            var result = await _customerService.UpdateAsync("789", Mock.Customer.Success);
            Assert.Equal(Mock.Customer.Success, result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task TestDeleteSholdReturnFalseResultWhenIdIsNull()
        {
            var result = await _customerService.DeleteAsync(null);
            Assert.False(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.IsType<ArgumentNullException>(result.Error);
            Assert.Contains("id cannot be null or empty!", result.Error.Message);
        }

        [Fact]
        public async Task TestDeleteSholdReturnFalseResultWhenExecutionIsNotOk()
        {
            var result = await _customerService.DeleteAsync("456");
            Assert.False(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Contains("could not delete the customer on database:", result.Error.Message);
        }

        [Fact]
        public async Task TestDeleteSholdReturnTrueResulteWhenExecutionIsOk()
        {
            var result = await _customerService.DeleteAsync("789");
            Assert.True(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            Assert.Null(result.Error);
        }
    }
}