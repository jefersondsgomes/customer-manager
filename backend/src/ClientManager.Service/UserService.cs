using System;
using System.Net;
using System.Threading.Tasks;
using ClientManager.Model.Result;
using ClientManager.Model.User;
using ClientManager.Repository.Interfaces;
using ClientManager.Service.Interfaces;
using MongoDB.Driver;

namespace ClientManager.Service
{
    public class UserService : IUserService
    {
        private readonly IMongoRepository<User> _repository;

        public UserService(IMongoRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> ValidadeUser(User user)
        {
            if (user == null)
                return new Result<bool>(false,
                HttpStatusCode.NotFound,
                new ArgumentNullException("User cannot be null!"));

            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(x => x.Login, user.Login),
                Builders<User>.Filter.Eq(x => x.Password, user.Password));

            return await _repository.FindAsync(filter) != null ?
                new Result<bool>(true, HttpStatusCode.OK) :
                new Result<bool>(false, HttpStatusCode.NotFound, new Exception("Invalid User or Password!"));
        }
    }
}