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
                                if (boardSquare.GamePawn.Color == currentGamePlayer.PlayerColor)
                                {
                                    BoardSquare? emptyPlace = board.Where(bs => bs.ColumnIndex >= boardSquare.ColumnIndex - 1
                                                        && bs.ColumnIndex <= boardSquare.ColumnIndex + 1
                                                        && bs.RowIndex >= boardSquare.RowIndex - 1
                                                        && bs.RowIndex <= boardSquare.RowIndex + 1
                                                        && bs.GamePawn == falsePawn).FirstOrDefault();

                                    if (emptyPlace == null)
                                        return;

                                    emptyPlace.GamePawn = boardSquare.GamePawn;
                                    boardSquare.GamePawn = falsePawn;

                                    if (CheckWin())
                                    {
                                        isEndGame = true;
                                        popupService.ShowPopupAsync<AchiPopupViewModel>(
                                            onPresenting: vm =>
                                            {
                                                vm.Message = "Koniec gry.\nWygrywa:\n";
                                                vm.ImageSymbol = CurrentGamePlayer.PlayerColor;
                                            });
                                        return;
                                    }

                                    CurrentGamePlayer = gamePlayers.GetNext();
                                }



                                /*if (chooseBoardSquare == null
                                    && boardSquare.GamePawn.Color != currentGamePlayer.PlayerColor)
                                    return;

                                if (chooseBoardSquare == null
                                    || boardSquare.GamePawn.Color == currentGamePlayer.PlayerColor)
                                {
                                    if (chooseBoardSquare != null)
                                        chooseBoardSquare.IsChoose = false;
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
                                    chooseBoardSquare.IsChoose = false;
                                    boardSquare.GamePawn = chooseBoardSquare.GamePawn;
                                    chooseBoardSquare.GamePawn = falsePawn;
                                    chooseBoardSquare = null;

                                    if (CheckWin())
                                    {
                                        isEndGame = true;
                                        popupService.ShowPopupAsync<AchiPopupViewModel>(
                                            onPresenting: vm =>
                                            {
                                                vm.Message = "Koniec gry.\nWygrywa:\n";
                                                vm.ImageSymbol = CurrentGamePlayer.PlayerColor;
                                            });
                                        return;
                                    }

                                    CurrentGamePlayer = gamePlayers.GetNext();
                                }
                                */
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

        private const int MAX_PAWNS_FOR_PLAYERS = 4;
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

        private bool CheckWin()
        {
            foreach (var columbGroup in Board.GroupBy(x => x.ColumnIndex))
            {
                if (CheckLineWin(columbGroup.ToList()))
                    return true;
            }

            foreach (var rowGroup in Board.GroupBy(x => x.RowIndex))
            {
                if (CheckLineWin(rowGroup.ToList()))
                    return true;
            }

            if (CheckLineWin(Board.Where(x => x.RowIndex == x.ColumnIndex).ToList()))
                return true;

            if (CheckLineWin(Board.Where(
                x => x.ColumnIndex == Math.Sqrt(Board.Count) - 1 - x.RowIndex).ToList()))
                return true;

            return false;
        }

        private bool CheckLineWin(List<BoardSquare> line)
        {
            return line.All(playingField => playingField.GamePawn.Color == CurrentGamePlayer.PlayerColor);
        }

        public void Dispose()
        {
            
        }
    }
}
