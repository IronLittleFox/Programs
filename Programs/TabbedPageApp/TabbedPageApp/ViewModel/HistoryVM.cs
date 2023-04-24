using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TabbedPageApp.ViewModel
{
    class HistoryVM : BindableObject, INotifyPropertyChanged
    {

        private ICommand goArchCommand;

        public ICommand GoArchCommand
        {
            get
            {
                if (goArchCommand == null)
                    goArchCommand = new Command(() =>
                    Parent.Navigation.PushAsync(new ArchHistoryPage()));
                return goArchCommand;
            }

        }




        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanges([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public static BindableProperty ParentProperty = BindableProperty.Create("Parent", typeof(ContentPage), typeof(HistoryVM), null, BindingMode.OneWay);
        public ContentPage Parent
        {
            get => (ContentPage)GetValue(ParentProperty);
            set => SetValue(ParentProperty, value);
        }
    }
}
