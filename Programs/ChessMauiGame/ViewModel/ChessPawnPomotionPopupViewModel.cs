using ChessMauiGame.Model.ChessPieces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChessMauiGame.ViewModel
{
    public class InfoAboutPiece
    {
        public string Color { get; set; } = "";
        public string Type { get; set; } = "";
        public Type PieceType { get; set; } = typeof(ChessPiece);
        public ICommand? CloseCommand { get; set; } = null;
    }

    public class ChessPawnPomotionPopupViewModel : BindableObject
    {
        public delegate Task CloseHandler<T>(T result);
        public event CloseHandler<Type>? OnClose;

        private string pawnColor = "";
        public string PawnColor
        {
            get { return pawnColor; }
            set
            {
                pawnColor = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PieceNames));
            }
        }

        public ObservableCollection<InfoAboutPiece> PieceNames
        {
            get
            {
                return new ObservableCollection<InfoAboutPiece>()
                {
                    new InfoAboutPiece(){ Type = "rook",Color = PawnColor, CloseCommand = CloseCommand, PieceType = typeof(Rook) } ,
                    new InfoAboutPiece(){ Type = "knight",Color = PawnColor, CloseCommand = CloseCommand, PieceType = typeof(Knight) },
                    new InfoAboutPiece(){ Type = "bishop",Color = PawnColor, CloseCommand = CloseCommand, PieceType = typeof(Bishop) },
                    new InfoAboutPiece(){ Type = "queen",Color = PawnColor, CloseCommand = CloseCommand, PieceType = typeof(Queen) }
                };
            }
        }

        private ICommand? closeCommand = null;
        public ICommand CloseCommand
        {
            get
            {
                if (closeCommand == null)
                    closeCommand = new Command<InfoAboutPiece>(
                        o =>
                        {
                            if (OnClose != null)
                            {
                                OnClose(o.PieceType);
                            }
                        }
                        );
                return closeCommand;
            }
        }
    }
}
