using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMauiApp.Model
{
    public class GameInfo : BindableObject
    {
        private string nameOfGame;
        public string NameOfGame
        {
            get { return nameOfGame; }
            set
            {
                nameOfGame = value;
                OnPropertyChanged();
            }
        }

        private Type gameType;
        public Type GameType
        {
            get { return gameType; }
            set
            {
                gameType = value;
                OnPropertyChanged();
            }
        }
    }
}
