using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchiMauiGame.ViewModel
{
    public class AchiPopupViewModel : BindableObject
    {
        private string message = "";
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        private string imageSymbol = "";
        public string ImageSymbol
        {
            get { return imageSymbol; }
            set
            {
                imageSymbol = value;
                OnPropertyChanged();
            }
        }
    }
}
