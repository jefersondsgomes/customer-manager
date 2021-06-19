using CustomerManager.Models.Results;
using CustomerManager.Models.Transients;
using System.Threading.Tasks;

namespace CustomerManager.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<AuthenticateResponse>> AuthenticateAsync(AuthenticateRequest authenticateRequest);
    }
}
