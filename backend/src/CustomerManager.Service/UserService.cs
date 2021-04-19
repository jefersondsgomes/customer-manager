using CustomerManager.Models.Entities;
using CustomerManager.Models.Results;
using CustomerManager.Repositories.Interfaces;
using CustomerManager.Services.Interfaces;
using MongoDB.Driver;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CustomerManager.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoRepository<User> _userRepository;

        public UserService(IMongoRepository<User> repository)
        {
            _userRepository = repository;
        }

        public async Task<Result<bool>> AuthenticateAsync(User user)
        {
            if (user == null)
                return new Result<bool>(false, HttpStatusCode.BadRequest,
                    new ArgumentNullException("user cannot be null!"));

            try
            {
                var filter = Builders<User>.Filter.Where(u => u.UserName == user.UserName && u.Password == user.Password);

                var userRepository = await _userRepository.FindAsync(filter);
                if (userRepository == null)
                    return new Result<bool>(false, HttpStatusCode.NotFound, new Exception("invalid credentials!"));

                return new Result<bool>(true, HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return new Result<bool>(false, HttpStatusCode.InternalServerError,
                    new Exception($"could not authenticate user: {e.Message}"));
            }
        }

        public async Task<Result<User>> CreateAsync(User user)
        {
            if (user == null)
                return new Result<User>(null, HttpStatusCode.BadRequest, new ArgumentNullException("user cannot be null!"));

            try
            {
                var builder = Builders<User>.Filter;
                var filter = builder.Eq(u => u.UserName, user.UserName) & builder.Eq(u => u.Password, user.Password);
                var dbUser = await _userRepository.FindAsync(filter);

                if (dbUser != null)
                    return new Result<User>(dbUser, HttpStatusCode.Conflict, new Exception("user already exists!"));

                var result = await _userRepository.CreateAsync(user);
                return new Result<User>(result, HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                return new Result<User>(null, HttpStatusCode.InternalServerError,
                    new Exception($"could not create user: {e.Message}"));
            }
        }

        public async Task<Result<User>> GetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new Result<User>(null, HttpStatusCode.BadRequest, new ArgumentException("id can't be null or empty!"));

            try
            {
                var user = await _userRepository.FindAsync(id);
                if (user == null)
                    return new Result<User>(null, HttpStatusCode.NotFound, new Exception("user was not found!"));

                return new Result<User>(user, HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return new Result<User>(null, HttpStatusCode.InternalServerError,
                    new Exception($"could not find user: {e.Message}"));
            }
        }
    }
}