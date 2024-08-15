using ReactiveUI;
using System;

namespace eynia.ViewModels
{
    // Instead of implementing "INotifyPropertyChanged" on our own we use "ReactiveObject" as
    // our base class. Read more about it here: https://www.reactiveui.net
    public class ViewModelBase : ReactiveObject
    {
        private string? _Name; // This is our backing field for Name

        public string? Name
        {
            get
            {
                return _Name;
            }
            set
            {
                // We can use "RaiseAndSetIfChanged" to check if the value changed and automatically notify the UI
                this.RaiseAndSetIfChanged(ref _Name, value);
            }
        }
    }
}