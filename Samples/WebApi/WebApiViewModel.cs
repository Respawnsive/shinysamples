using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using Shiny;
using Shiny.WebApi;
using Xamarin.Forms;

namespace Samples.WebApi
{
    public class WebApiViewModel : ViewModel
    {
        readonly IWebApi<IWebApiService> webApiService;

        public WebApiViewModel(IWebApi<IWebApiService> webApiService)
        {
            this.webApiService = webApiService;
        }

        [Reactive] public ObservableCollection<User>? Users { get; set; }

        private async Task GetUsersAsync()
        {
            var userList = await this.webApiService.ExecuteAsync(x => x.GetUsersAsync());
            if(!userList.Data.IsEmpty())
                Users = new ObservableCollection<User>(userList.Data);
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            Device.InvokeOnMainThreadAsync(GetUsersAsync);
        }
    }
}
