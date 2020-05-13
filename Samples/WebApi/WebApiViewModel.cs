using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using Shiny;
using Xamarin.Forms;

namespace Samples.WebApi
{
    public class WebApiViewModel : ViewModel
    {
        readonly IWebApiService webApiService;

        public WebApiViewModel(IWebApiService webApiService)
        {
            this.webApiService = webApiService;
        }

        [Reactive] public ObservableCollection<User>? Users { get; set; }

        private async Task GetUsersAsync()
        {
            var userList = await this.webApiService.GetUsersAsync(0);
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
