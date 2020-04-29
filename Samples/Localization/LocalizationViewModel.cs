using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny;
using Shiny.Localization;

namespace Samples.Localization
{
    public class LocalizationViewModel : ViewModel
    {
        readonly ILocalizationManager localizationManager;

        public LocalizationViewModel(ILocalizationManager localizationManager)
        {
            this.localizationManager = localizationManager;
            Cultures = new ObservableCollection<CultureInfo>(localizationManager.AvailableCultures);
            SelectedCulture = localizationManager.CurrentCulture;

            this.WhenAnyValue(x => x.SelectedCulture)
                .SubscribeAsync(culture => this.localizationManager.InitializeAsync(culture))
                .DisposedBy(this.DestroyWith);

            this.localizationManager.WhenLocalizationStatusChanged()  
                .SubOnMainThread(LocalizationStatusChanged)
                .DisposeWith(this.DestroyWith);
        }

        private void LocalizationStatusChanged(LocalizationState state)
        {
            if (state > LocalizationState.Initializing)
                this.RaisePropertyChanged("Item");
        }

        public string this[string key] => this.localizationManager.GetText(key);

        [Reactive] public ObservableCollection<CultureInfo> Cultures { get; set; }

        [Reactive] public CultureInfo SelectedCulture { get; set; }
    }
}
