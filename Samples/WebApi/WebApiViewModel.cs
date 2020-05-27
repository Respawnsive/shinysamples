using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny;
using Shiny.WebApi;
using Xamarin.Forms;

namespace Samples.WebApi
{
    public class WebApiViewModel : ViewModel
    {
        readonly IWebApi<IReqResService> reqResService;
        readonly IWebApi<IHttpBinService> httpBinService;

        public WebApiViewModel(IWebApi<IReqResService> reqResService, IWebApi<IHttpBinService> httpBinService)
        {
            this.reqResService = reqResService;
            this.httpBinService = httpBinService;
            this.AuthCommand = ReactiveCommand.CreateFromTask(AuthAsync);
        }

        [Reactive] public ObservableCollection<User>? Users { get; set; }

        public ICommand AuthCommand { get; }

        private async Task GetUsersAsync()
        {
            try
            {
                var userList = await this.reqResService.ExecuteAsync(x => x.GetUsersAsync());
                if (!userList.Data.IsEmpty())
                    Users = new ObservableCollection<User>(userList.Data);
            }
            catch (Exception ex)
            {

            }
        }

        private async Task AuthAsync()
        {
            try
            {
                var response = await this.httpBinService.ExecuteAsync(api => api.AuthBearerAsync());
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            Device.InvokeOnMainThreadAsync(GetUsersAsync);
        }
    }
}
