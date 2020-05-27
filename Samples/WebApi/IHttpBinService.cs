using System.Net.Http;
using System.Threading.Tasks;
using Refit;
using Shiny.WebApi;
using Shiny.WebApi.Tracing;

namespace Samples.WebApi
{
    [WebApi("https://httpbin.org/"), Trace]
    public interface IHttpBinService
    {
        [Get("/bearer")]
        [Headers("Authorization: Bearer")]
        Task<HttpResponseMessage> AuthBearerAsync();
    }
}
