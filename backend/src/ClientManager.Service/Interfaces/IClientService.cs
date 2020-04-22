using System.Collections.Generic;
using System.Threading.Tasks;
using ClientManager.Model.Common;
using ClientManager.Model.Result;

namespace ClientManager.Service.Interfaces
{
    public interface IClientService
    {
         Task<Result<Client>> Get(string id);
         Task<List<Result<Client>>> Get();         
         Task<Result<Client>> Create(Client client);
         Task<Result<Client>> Update(string id, Client client);
         Task<Result<bool>> Delete(string id);
    }
}