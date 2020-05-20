using System.Threading.Tasks;
using Refit;
using Shiny.WebApi.Caching;

namespace Samples.WebApi
{
    public interface IWebApiService
    {
        [Get("/api/users")]
        [Cache]
        Task<UserList> GetUsersAsync();
    }
}
