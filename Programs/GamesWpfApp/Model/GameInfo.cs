
using GameUtils.Interfaces;
using GameUtils.Utils;

namespace GamesWpfApp.Model
{
    class GameInfo : ViewObserver
    {
        private string nameOfGame;
        public string NameOfGame
        {
            get { return nameOfGame; }
            set
            {
                nameOfGame = value;
                OnPropertyChanged(nameof(NameOfGame));
            }
        }

        private IGameViewModel gameViewModel;
        public IGameViewModel GameViewModel
        {
            get { return gameViewModel; }
            set
            {
                gameViewModel = value;
                OnPropertyChanged(nameof(GameViewModel));
            }
        }
    }
}
