using ClientManager.Model.Common;
using ClientManager.Model.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientManager.Service.Interfaces
{
    public interface IClientService
    {
        Task<Result<Client>> GetAsync(string id);
        Task<Result<ICollection<Client>>> GetAllAsync();
        Task<Result<Client>> CreateAsync(Client client);
        Task<Result<Client>> UpdateAsync(string id, Client client);
        Task<Result<bool>> DeleteAsync(string id);
    }
}