using System.Threading.Tasks;
using Refit;
using Shiny.WebApi;
using Shiny.WebApi.Caching;

namespace Samples.WebApi
{
    [WebApi("https://reqres.in/"), Cache, Trace]
    public interface IWebApiService
    {
        [Get("/api/users")]
        Task<UserList> GetUsersAsync();
    }
}
