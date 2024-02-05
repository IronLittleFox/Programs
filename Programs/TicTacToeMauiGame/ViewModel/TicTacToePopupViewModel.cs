using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeMauiGame.ViewModel
{
    public class TicTacToePopupViewModel : BindableObject
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


        public TicTacToePopupViewModel()
        {
            
        }
    }
}
