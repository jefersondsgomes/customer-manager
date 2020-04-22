using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ClientManager.Model.Common;
using ClientManager.Model.Result;
using ClientManager.Repository.Interfaces;
using ClientManager.Service.Interfaces;

namespace ClientManager.Service
{
    public class ClientService : IClientService
    {
        private readonly IMongoRepository<Client> _repository;

        public ClientService(IMongoRepository<Client> repository)
        {
            _repository = repository;
        }

        public async Task<Result<Client>> Create(Client client)
        {
            if (client == null)
                return new Result<Client>(null, HttpStatusCode.BadRequest,
                new ArgumentNullException("Client cannot be null!"));

            try
            {
                var created = await _repository.CreateAsync(client);
                return new Result<Client>(created, HttpStatusCode.Created);
            }
            catch (Exception)
            {
                return new Result<Client>(client, HttpStatusCode.InternalServerError,
                new Exception($"could not create the client on database!"));
            }
        }

        public async Task<Result<bool>> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return new Result<bool>(false, HttpStatusCode.BadRequest,
                new ArgumentNullException("Parameter ID cannot be null!"));

            try
            {
                await _repository.RemoveAsync(id);
            }
            catch (Exception)
            {
                return new Result<bool>(false,
                    HttpStatusCode.InternalServerError,
                    new Exception("could not delete the client on database!"));
            }

            return new Result<bool>(true, HttpStatusCode.NoContent);
        }

        public async Task<Result<Client>> Get(string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Result<Client>>> Get()
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result<Client>> Update(string id, Client client)
        {
            throw new System.NotImplementedException();
        }
    }
}