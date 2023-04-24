using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShellPageApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShellApp : Shell
    {
        private ICommand logOutCommand;

        public ICommand LogOutCommand
        {
            get 
            {
                if (logOutCommand == null)
                    logOutCommand = new Command(() => {
                        DisplayAlert("Alert", "You have been logout", "OK");
                    });
                return logOutCommand; 
            }
            set { logOutCommand = value; }
        }


        public ShellApp()
        {
            InitializeComponent();
        }
    }
}