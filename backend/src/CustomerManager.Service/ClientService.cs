using CustomerManager.Model.Common;
using CustomerManager.Model.Result;
using CustomerManager.Repository.Interfaces;
using CustomerManager.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CustomerManager.Service
{
    public class ClientService : IClientService
    {
        private readonly IMongoRepository<Customer> _clientRepository;

        public ClientService(IMongoRepository<Customer> repository)
        {
            _clientRepository = repository;
        }

        public async Task<Result<Customer>> CreateAsync(Customer client)
        {
            if (client == null)
                return new Result<Customer>(null, HttpStatusCode.BadRequest,
                    new ArgumentNullException("client cannot be null!"));

            try
            {
                var created = await _clientRepository.CreateAsync(client);
                return new Result<Customer>(created, HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                return new Result<Customer>(client, HttpStatusCode.InternalServerError,
                    new Exception($"could not create the client on database: {e.Message}"));
            }
        }

        public async Task<Result<Customer>> GetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new Result<Customer>(null, HttpStatusCode.BadRequest,
                    new ArgumentNullException("parameter Id cannot be null or empty!"));

            try
            {
                var client = await _clientRepository.FindAsync(id);
                if (client == null)
                    return new Result<Customer>(null, HttpStatusCode.NotFound, new Exception("client not found!"));

                return new Result<Customer>(client, HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return new Result<Customer>(null, HttpStatusCode.InternalServerError,
                    new Exception($"could not get the client from database: {e.Message}"));
            }
        }

        public async Task<Result<ICollection<Customer>>> GetAllAsync()
        {
            try
            {
                var clients = await _clientRepository.FindAsync();
                if (!clients.Any())
                    return new Result<ICollection<Customer>>(clients, HttpStatusCode.NoContent,
                        new Exception("there are no clients!"));

                return new Result<ICollection<Customer>>(clients, HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return new Result<ICollection<Customer>>(null, HttpStatusCode.InternalServerError,
                    new Exception($"could not list the clients from database: {e.Message}"));
            }
        }

        public async Task<Result<Customer>> UpdateAsync(string id, Customer client)
        {
            if (string.IsNullOrEmpty(id))
                return new Result<Customer>(client, HttpStatusCode.BadRequest,
                    new ArgumentNullException("id cannot be null or empty!"));

            if (client == null)
                return new Result<Customer>(client, HttpStatusCode.BadRequest,
                    new ArgumentNullException("client cannot be null!"));

            try
            {
                var clientRepository = await _clientRepository.FindAsync(id);
                if (clientRepository == null)
                    return new Result<Customer>(client, HttpStatusCode.NotFound,
                        new Exception("client to be updated was not found!"));

                await _clientRepository.ReplaceAsync(id, client);
                return new Result<Customer>(client, HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return new Result<Customer>(client, HttpStatusCode.InternalServerError,
                    new Exception($"could not be update the client: {e.Message}"));
            }
        }

        public async Task<Result<bool>> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new Result<bool>(false, HttpStatusCode.BadRequest,
                    new ArgumentNullException("id cannot be null or empty!"));

            try
            {
                await _clientRepository.RemoveAsync(id);
                return new Result<bool>(true, HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return new Result<bool>(false,
                    HttpStatusCode.InternalServerError,
                        new Exception($"could not delete the client on database: {e.Message}"));
            }
        }
    }
}