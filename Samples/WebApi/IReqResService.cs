using System.Threading.Tasks;
using Refit;
using Shiny.WebApi;
using Shiny.WebApi.Caching;
using Shiny.WebApi.Policing;
using Shiny.WebApi.Tracing;

[assembly:Policy("TransientHttpError")]
namespace Samples.WebApi
{
    [WebApi("https://reqres.in/"), Cache, Trace]
    public interface IReqResService
    {
        [Get("/api/users")]
        Task<UserList> GetUsersAsync();
    }
}
