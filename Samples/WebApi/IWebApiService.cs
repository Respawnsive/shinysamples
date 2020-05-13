using System.Threading.Tasks;
using Refit;

namespace Samples.WebApi
{
    public interface IWebApiService
    {
        [Get("/api/users?page={page}")]
        Task<UserList> GetUsersAsync(int page);
    }
}
