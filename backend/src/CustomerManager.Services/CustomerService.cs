using CustomerManager.Models.Entities;
using CustomerManager.Models.Results;
using CustomerManager.Repositories.Interfaces;
using CustomerManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CustomerManager.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMongoRepository<Customer> _customerRepository;

        public CustomerService(IMongoRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<Customer>> CreateAsync(Customer customer)
        {
            if (customer == null)
                return new Result<Customer>(null, HttpStatusCode.BadRequest,
                    new ArgumentNullException("customer cannot be null!"));

            try
            {
                var created = await _customerRepository.CreateAsync(customer);
                return new Result<Customer>(created, HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                return new Result<Customer>(customer, HttpStatusCode.InternalServerError,
                    new Exception($"could not create the customer on database: {e.Message}"));
            }
        }

        public async Task<Result<Customer>> GetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new Result<Customer>(null, HttpStatusCode.BadRequest,
                    new ArgumentNullException("parameter Id cannot be null or empty!"));

            try
            {
                var customer = await _customerRepository.FindAsync(id);
                if (customer == null)
                    return new Result<Customer>(null, HttpStatusCode.NotFound, new Exception("customer not found!"));

                return new Result<Customer>(customer, HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return new Result<Customer>(null, HttpStatusCode.InternalServerError,
                    new Exception($"could not get the customer from database: {e.Message}"));
            }
        }

        public async Task<Result<ICollection<Customer>>> GetAllAsync()
        {
            try
            {
                var customers = await _customerRepository.FindAsync();
                if (!customers.Any())
                    return new Result<ICollection<Customer>>(customers, HttpStatusCode.NoContent,
                        new Exception("there are no customers!"));

                return new Result<ICollection<Customer>>(customers, HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return new Result<ICollection<Customer>>(null, HttpStatusCode.InternalServerError,
                    new Exception($"could not list the customers from database: {e.Message}"));
            }
        }

        public async Task<Result<Customer>> UpdateAsync(string id, Customer customer)
        {
            if (string.IsNullOrEmpty(id))
                return new Result<Customer>(customer, HttpStatusCode.BadRequest,
                    new ArgumentNullException("id cannot be null or empty!"));

            if (customer == null)
                return new Result<Customer>(customer, HttpStatusCode.BadRequest,
                    new ArgumentNullException("customer cannot be null!"));

            try
            {
                var customerRepository = await _customerRepository.FindAsync(id);
                if (customerRepository == null)
                    return new Result<Customer>(customer, HttpStatusCode.NotFound,
                        new Exception("customer to be updated was not found!"));

                await _customerRepository.ReplaceAsync(id, customer);
                return new Result<Customer>(customer, HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return new Result<Customer>(customer, HttpStatusCode.InternalServerError,
                    new Exception($"could not be update the customer: {e.Message}"));
            }
        }

        public async Task<Result<bool>> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new Result<bool>(false, HttpStatusCode.BadRequest,
                    new ArgumentNullException("id cannot be null or empty!"));

            try
            {
                await _customerRepository.RemoveAsync(id);
                return new Result<bool>(true, HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return new Result<bool>(false,
                    HttpStatusCode.InternalServerError,
                        new Exception($"could not delete the customer on database: {e.Message}"));
            }
        }
    }
}