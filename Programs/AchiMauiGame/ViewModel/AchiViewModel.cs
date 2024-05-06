using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilsMaui.Utils;
using AchiMauiGame.Model;
using UtilsMaui.Interfaces;
using System.Windows.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using AchiMauiGame.Enums;

namespace AchiMauiGame.ViewModel
{
    public class AchiViewModel : BindableObject, IGameViewModel
    {
        private int _columnCount = 8;
        public int ColumnCount
        {
            get { return _columnCount; }
            set
            {
                _columnCount = value;
                OnPropertyChanged();
            }
        }

        private int _rowCount = 10;
        public int RowCount
        {
            get { return _rowCount; }
            set
            {
                _rowCount = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<BoardSquare> board;
        public ObservableCollection<BoardSquare> Board
        {
            get { return board; }
            set
            {
                board = value;
                OnPropertyChanged(nameof(Board));
            }
        }

        private GamePlayer currentGamePlayer;
        public GamePlayer CurrentGamePlayer
        {
            get { return currentGamePlayer; }
            set
            {
                currentGamePlayer = value;
                OnPropertyChanged(nameof(CurrentGamePlayer));
            }
        }

        private ICommand? squareCommand;
        public ICommand SquareCommand
        {
            get
            {
                if (squareCommand == null)
                    squareCommand = new Command<BoardSquare>(
                        boardSquare =>
                        {
                            if (isEndGame)
                                return;

                            if (gamePhase == GamePhase.FIRST)
                            {
                                if (boardSquare.GamePawn != falsePawn)
                                    return;

                                Pawn pawn = new Pawn(currentGamePlayer.PlayerColor);
                                boardSquare.GamePawn = pawn;
                                CurrentGamePlayer.Pawns.Add(pawn);

                                CurrentGamePlayer = gamePlayers.GetNext();

                                if (gamePlayers.All(gp => gp.Pawns.Count == MAX_PAWNS_FOR_PLAYERS))
                                {
                                    gamePhase = GamePhase.SECOND;
                                }
                            }
                            else
                            {
                                if (chooseBoardSquare == null
                                    && boardSquare.GamePawn.Color != currentGamePlayer.PlayerColor)
                                    return;

                                if (chooseBoardSquare == null
                                    || boardSquare.GamePawn.Color == currentGamePlayer.PlayerColor)
                                {
                                    chooseBoardSquare = boardSquare;
                                    boardSquare.IsChoose = true;
                                    return;
                                }

                                if (chooseBoardSquare != null
                                    && boardSquare.GamePawn == falsePawn
                                    && board.Where(bs => bs.ColumnIndex >= chooseBoardSquare.ColumnIndex - 1
                                                        && bs.ColumnIndex <= chooseBoardSquare.ColumnIndex + 1
                                                        && bs.RowIndex >= chooseBoardSquare.RowIndex - 1
                                                        && bs.RowIndex <= chooseBoardSquare.RowIndex + 1)
                                            .Any(bs => bs == boardSquare))
                                {
                                    boardSquare.GamePawn = chooseBoardSquare.GamePawn;
                                    chooseBoardSquare.GamePawn = falsePawn;
                                    chooseBoardSquare = null;
                                    CurrentGamePlayer = gamePlayers.GetNext();
                                }

                            }
                        }
                        );
                return squareCommand;
            }
        }

        private ICommand? newGameCommand = null;
        public ICommand NewGameCommand
        {
            get
            {
                if (newGameCommand == null)
                    newGameCommand = new Command<object>(
                        o =>
                        {
                            NewGame();
                        }
                        );
                return newGameCommand;
            }
        }

        private bool isEndGame = false;
        private CircularObservableCollection<GamePlayer> gamePlayers;
        private string whiteColorField = "white";
        private Pawn falsePawn = new Pawn("empty");
        private IPopupService popupService;

        private const int MAX_PAWNS_FOR_PLAYERS = 3;
        private GamePhase gamePhase;
        private BoardSquare? chooseBoardSquare = null;

        public AchiViewModel(IPopupService popupService)
        {
            this.popupService = popupService;

            board = new ObservableCollection<BoardSquare>();
            Board = board;

            gamePlayers = new CircularObservableCollection<GamePlayer>()
            {
                new GamePlayer() {PlayerColor = "white", Pawns = new List<Pawn>() },
                new GamePlayer() {PlayerColor = "black", Pawns = new List<Pawn>() }
            };

            CurrentGamePlayer = currentGamePlayer = gamePlayers.First();

            NewGame();
        }

        private void NewGame()
        {
            RowCount = 3;
            ColumnCount = 3;

            isEndGame = false;
            chooseBoardSquare = null;
            Board.Clear();
            for (int row = 0; row < RowCount; row++)
                for (int col = 0; col < ColumnCount; col++)
                {
                    Board.Add(new BoardSquare()
                    {
                        ColumnIndex = col,
                        RowIndex = row,
                        SquareColor = whiteColorField,
                        SquareCommand = SquareCommand,
                        GamePawn = falsePawn,
                        IsChoose = false
                    });
                }

            gamePhase = GamePhase.FIRST;
            gamePlayers.ForAll(gp => gp.Pawns.Clear());
        }


        public void Dispose()
        {
            
        }
    }
}
