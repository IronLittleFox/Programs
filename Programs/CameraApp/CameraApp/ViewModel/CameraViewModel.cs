using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CameraApp.ViewModel
{
    class CameraViewModel: BindableObject
    {
        private bool isTortchActive;

        public bool IsTortchActive
        {
            get { return isTortchActive; }
            set 
            {
                isTortchActive = value;
                OnPropertyChanged();
            }
        }

        private ICommand checkTortchActiveCommand;
        public ICommand CheckTortchActiveCommand
        {
            get 
            {
                if (checkTortchActiveCommand == null)
                    checkTortchActiveCommand = new Command(async () =>
                    {
                        ICameraService cameraService = DependencyService.Get<ICameraService>();
                        IsTortchActive = await cameraService.GetTorchStatusAsync();

                    });
                return checkTortchActiveCommand; 
            }
        }

        private ICommand tortchOnCommand;
        public ICommand TortchOnCommand
        {
            get
            {
                if (tortchOnCommand == null)
                    tortchOnCommand = new Command(async () =>
                    {
                        await Flashlight.TurnOnAsync();
                    });
                return tortchOnCommand;
            }
        }


    }
}
