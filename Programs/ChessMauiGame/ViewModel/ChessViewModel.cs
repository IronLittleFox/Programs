﻿using CommunityToolkit.Maui.Core;
using ChessMauiGame.Model;
using System.Windows.Input;
using UtilsMaui.Interfaces;
using UtilsMaui.Utils;
using System.Collections.ObjectModel;
using ChessMauiGame.Model.ChessPieces;

namespace ChessMauiGame.ViewModel
{
    public class ChessViewModel : BindableObject, IGameViewModel
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

        private int _rowCount = 8;
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

        private ICommand squareCommand;
        public ICommand SquareCommand
        {
            get
            {
                if (squareCommand == null)
                    squareCommand = new Command<BoardSquare>(
                        async chooseBoardSquare =>
                        {
                            if (isEndGame)
                                return;

                            //jeśli kliknięto na pole które nie jest pionkiem które może sie ruszyć
                            if (!currentPlayerBoardSquaresToMove.Any(x => x.boardSquare == chooseBoardSquare))
                                return;
                            //jeśli kliknięto na pole które nie znajduje się na liście pól możliwych do ruchu
                            if (!currentPlayerBoardSquaresToMove.Any(x => x.listOfBoardSquareMove.Any(y => y == chooseBoardSquare && y.IsPossibleMove)))
                                return;

                            //kliknięto na pole z pionkiem który może się ruszyć
                            if (currentPlayerBoardSquaresToMove.Any(x => x.boardSquare == chooseBoardSquare))
                            {
                                //odznaczenie wcześniejszego pionka
                                if (selectedPawnOnBoardSquare != null)
                                {
                                    currentPlayerBoardSquaresToMove
                                    .Where(x => x.boardSquare == selectedPawnOnBoardSquare)
                                    .ForAll(y => y.listOfBoardSquareMove.ForAll(z => z.IsPossibleMove = false));
                                }

                                //zapamiętanie aktualnego pionka
                                selectedPawnOnBoardSquare = chooseBoardSquare;

                                //pokazanie ruchów aktualnego pionka
                                currentPlayerBoardSquaresToMove
                                    .Where(x => x.boardSquare == selectedPawnOnBoardSquare)
                                    .ForAll(y => y.listOfBoardSquareMove.ForAll(z => z.IsPossibleMove = true));

                                return;
                            }

                            //kliknieto na pole na które może być ruch
                            if (selectedPawnOnBoardSquare != null
                                && currentPlayerBoardSquaresToMove
                                    .Where(x => x.boardSquare == selectedPawnOnBoardSquare)
                                    .Any(x => x.listOfBoardSquareMove.Any(z => z == chooseBoardSquare)))
                            {
                                //wybrane pole jest zajęte
                                if (chooseBoardSquare.ChessPiece != falseChessPiece)
                                {
                                    //pozbywamy się pionka przeciwnika
                                    gamePlayers.ForAll(gp => gp.ChessPieces.Remove(chooseBoardSquare.ChessPiece));
                                }

                                //odznaczamy możliwe ruchy
                                currentPlayerBoardSquaresToMove
                                    .Where(x => x.boardSquare == selectedPawnOnBoardSquare)
                                    .ForAll(x => x.listOfBoardSquareMove.ForAll(z => z.IsPossibleMove = false));

                                //wykonujemy ruch wybranego pionka
                                selectedPawnOnBoardSquare.ChessPiece.IsFirstMove = false;
                                currentPlayerBoardSquaresToMove.ForAll(x => x.boardSquare.IsChessPieceMustMove = false);
                                chooseBoardSquare.ChessPiece = selectedPawnOnBoardSquare.ChessPiece;
                                selectedPawnOnBoardSquare.ChessPiece = falseChessPiece;

                                //jeśli wybranym pionkiem jest Pawn i dotarliśmy do lini końcowej
                                if (chooseBoardSquare.ChessPiece is Pawn
                                    && endOfPlayerRow[CurrentGamePlayer] == chooseBoardSquare.RowIndex)
                                {
                                    //wyświetlamy komunikat o wyborze nowego pionka
                                    var result = await popupService.ShowPopupAsync<ChessPawnPomotionPopupViewModel>(
                                    onPresenting: vm =>
                                    {
                                        vm.PawnColor = CurrentGamePlayer.PlayerColor;
                                    });

                                    //podmieniamy tego pionka na wybranego
                                    if (result is Type)
                                    {
                                        var newPiece = Activator.CreateInstance(result as Type, CurrentGamePlayer.PlayerColor) as ChessPiece;
                                        CurrentGamePlayer.ChessPieces.Remove(chooseBoardSquare.ChessPiece);
                                        chooseBoardSquare.ChessPiece = newPiece;
                                        CurrentGamePlayer.ChessPieces.Add(newPiece);
                                    }
                                }

                                //pobieramy ponownie listę możliwych ruchów aby sprawdzić potencjalną wygraną
                                currentPlayerBoardSquaresToMove = FindAllPawnToMove(board, CurrentGamePlayer);

                                //jeśli na liście możliwych ruchów bijących znajduje się król wcześniejszego gracza
                                if (currentPlayerBoardSquaresToMove.Any(x => x.listOfBoardSquareMove.Any(y => y.ChessPiece is King)))
                                {
                                    GamePlayer prevPlayer = CurrentGamePlayer;
                                    CurrentGamePlayer = gamePlayers.GetNext();

                                    currentPlayerBoardSquaresToMove = GetPossibleKingDefenseMoves(prevPlayer);

                                    //sprawdzamy czy jest możliwość obrony
                                    if (currentPlayerBoardSquaresToMove.Count > 0)
                                    {
                                        await popupService.ShowPopupAsync<ChessMessagePopupViewModel>(
                                        onPresenting: vm =>
                                        {
                                            vm.Message = "SZACH";
                                        });
                                    }
                                    else
                                    {
                                        await popupService.ShowPopupAsync<ChessMessagePopupViewModel>(
                                        onPresenting: vm =>
                                        {
                                            vm.Message = "SZACH MAT";
                                        });
                                        isEndGame = true;
                                        return;
                                    }

                                    currentPlayerBoardSquaresToMove.ForAll(x => x.boardSquare.IsChessPieceMustMove = true);
                                    selectedPawnOnBoardSquare = null;
                                    return;
                                }
                            }

                            CurrentGamePlayer = gamePlayers.GetNext();

                            currentPlayerBoardSquaresToMove = FindAllPawnToMove(board, CurrentGamePlayer);

                            //jeśli lista ruchów jest pusta
                            if (currentPlayerBoardSquaresToMove.Count == 0)
                            {
                                await popupService.ShowPopupAsync<ChessMessagePopupViewModel>(
                                    onPresenting: vm =>
                                    {
                                        vm.Message = "PAT";
                                    });
                                isEndGame = true;
                                return;
                            }

                            currentPlayerBoardSquaresToMove.ForAll(x => x.boardSquare.IsChessPieceMustMove = true);
                            selectedPawnOnBoardSquare = null;
                        });
                return squareCommand;
            }
        }

        private IPopupService popupService;
        private CircularObservableCollection<GamePlayer> gamePlayers;
        private Dictionary<GamePlayer, int> endOfPlayerRow;
        private bool isEndGame = false;
        private string whiteColorField = "White";
        private string darkColorField = "#FFC5C5C5";
        private ChessPiece falseChessPiece = new Pawn("empty", 0);
        private List<Type> listOfChessPiecesFirstRow;
        private List<(BoardSquare boardSquare, List<BoardSquare> listOfBoardSquareMove)> currentPlayerBoardSquaresToMove;
        private BoardSquare? selectedPawnOnBoardSquare = null;

        public ChessViewModel(IPopupService popupService)
        {
            this.popupService = popupService;

            board = new ObservableCollection<BoardSquare>();
            Board = board;

            gamePlayers = new CircularObservableCollection<GamePlayer>()
            {
                new GamePlayer() {PlayerColor = "white", ChessPieces = new List<ChessPiece>() },
                new GamePlayer() {PlayerColor = "black", ChessPieces = new List<ChessPiece>() }
            };
            endOfPlayerRow = new Dictionary<GamePlayer, int>() { { gamePlayers[0], RowCount - 1 }, { gamePlayers[1], 0 } };

            listOfChessPiecesFirstRow =
            [
                typeof(Rook),
                typeof(Knight),
                typeof(Bishop),
                typeof(Queen),
                typeof(King),
                typeof(Bishop),
                typeof(Knight),
                typeof(Rook),
            ];

            NewGame();
        }

        private void NewGame()
        {
            CurrentGamePlayer = gamePlayers.First();
            gamePlayers.SetCurrent(CurrentGamePlayer);

            isEndGame = false;
            Board.Clear();
            for (int row = 0; row < RowCount; row++)
                for (int col = 0; col < ColumnCount; col++)
                {
                    Board.Add(new BoardSquare()
                    {
                        ColumnIndex = col,
                        RowIndex = row,
                        SquareColor = ((col + row % 2) % 2) == 0 ? whiteColorField : darkColorField,
                        SquareCommand = SquareCommand,
                        ChessPiece = falseChessPiece,
                        IsChessPieceMustMove = false,
                        IsPossibleMove = false
                    });
                }

            gamePlayers.ForAll(gp => gp.ChessPieces.Clear());


            board.Where(bs => bs.RowIndex == 1).ForAll(bs =>
            {
                bs.ChessPiece = new Pawn("white", 1);
                gamePlayers[0].ChessPieces.Add(bs.ChessPiece);
            });
            board.Where(bs => bs.RowIndex == 0).ForAll(bs =>
            {
                bs.ChessPiece = Activator.CreateInstance(listOfChessPiecesFirstRow[bs.ColumnIndex], gamePlayers[0].PlayerColor) as ChessPiece;
                gamePlayers[0].ChessPieces.Add(bs.ChessPiece);
            });


            board.Where(bs => bs.RowIndex == RowCount - 2).ForAll(bs =>
            {
                bs.ChessPiece = new Pawn("black", -1);
                gamePlayers[1].ChessPieces.Add(bs.ChessPiece);
            });
            board.Where(bs => bs.RowIndex == RowCount - 1).ForAll(bs =>
            {
                bs.ChessPiece = Activator.CreateInstance(listOfChessPiecesFirstRow[bs.ColumnIndex], gamePlayers[1].PlayerColor) as ChessPiece;
                gamePlayers[1].ChessPieces.Add(bs.ChessPiece);
            });

            currentPlayerBoardSquaresToMove = FindAllPawnToMove(board, CurrentGamePlayer);
            currentPlayerBoardSquaresToMove.ForAll(x => x.boardSquare.IsChessPieceMustMove = true);
            selectedPawnOnBoardSquare = null;
        }

        private List<(BoardSquare boardSquare, List<BoardSquare> listOfBoardSquareMove)> FindAllPawnToMove(ObservableCollection<BoardSquare> boardToCheck, GamePlayer gamePlayer)
        {
            List<(BoardSquare boardSquare, List<BoardSquare> listOfBoardSquareMove)> boardSquaresToMove = new();

            gamePlayer.ChessPieces.ForAll(cp =>
            {
                BoardSquare? boardSquare = boardToCheck.FirstOrDefault(bs => bs.ChessPiece == cp);
                if (boardSquare == null)
                {
                    return;
                }
                List<BoardSquare> listOfMoves = cp.GetListOfMoves(boardToCheck, boardSquare, falseChessPiece.Color);
                if (listOfMoves.Count != 0)
                    boardSquaresToMove.Add((boardSquare, listOfMoves));
            });

            return boardSquaresToMove;
        }

        private List<(BoardSquare boardSquare, List<BoardSquare> listOfBoardSquareMove)> GetPossibleKingDefenseMoves(GamePlayer prevPlayer)
        {
            List<(BoardSquare boardSquare, List<BoardSquare> listOfBoardSquareMove)> values = new List<(BoardSquare boardSquare, List<BoardSquare> listOfBoardSquareMove)>();

            ObservableCollection<BoardSquare> copyOfBoard = new ObservableCollection<BoardSquare>();
            foreach (BoardSquare bs in board)
            {
                copyOfBoard.Add(new BoardSquare()
                {
                    ColumnIndex = bs.ColumnIndex,
                    RowIndex = bs.RowIndex,
                    ChessPiece = bs.ChessPiece,
                });
            }

            ChessPiece? kingToDedfense = CurrentGamePlayer.ChessPieces.FirstOrDefault(cp => cp is King);

            List<(BoardSquare boardSquare, List<BoardSquare> listOfBoardSquareMove)> listOfMovesToCheck = FindAllPawnToMove(copyOfBoard, CurrentGamePlayer);

            foreach (var movesToCheck in listOfMovesToCheck)
            {
                //dla każdego znalezionego pionka którym można się ruszyć sprawdzam jego ruchy czy bronią króla
                List<BoardSquare> listOfBoardSquareMove = new();
                foreach (var bs in movesToCheck.listOfBoardSquareMove)
                {
                    ChessPiece copyChessPiece = bs.ChessPiece;
                    bool isBeating = copyChessPiece != falseChessPiece;
                    if (isBeating)
                    {
                        prevPlayer.ChessPieces.Remove(copyChessPiece);
                    }
                    //wykonaj ruch
                    bs.ChessPiece = movesToCheck.boardSquare.ChessPiece;
                    movesToCheck.boardSquare.ChessPiece = falseChessPiece;

                    //pobierz listę możliwych ruchów gracza przeciwnego
                    var listOfMovesPrevPlayer = FindAllPawnToMove(copyOfBoard, prevPlayer);

                    //jesli na liście nie znajduje się król aktualnego gracza to dodaj ten ruch do możliwych ruchów
                    if (!listOfMovesPrevPlayer.Any(x => x.listOfBoardSquareMove.Any(y => y.ChessPiece == kingToDedfense)))
                    {
                        listOfBoardSquareMove.Add(bs);
                    }

                    //wycofaj wykonany ruch
                    movesToCheck.boardSquare.ChessPiece = bs.ChessPiece;
                    bs.ChessPiece = copyChessPiece;
                    if (isBeating)
                    {
                        prevPlayer.ChessPieces.Add(copyChessPiece);
                    }
                }
                //jeśli lista nie jest pusta z ruchami to dodaj ten pione z listą ruchów do możliwej obrony
                if (listOfBoardSquareMove.Count > 0)
                    values.Add((movesToCheck.boardSquare, listOfBoardSquareMove));
            }

            List<(BoardSquare boardSquare, List<BoardSquare> listOfBoardSquareMove)> returns = new List<(BoardSquare boardSquare, List<BoardSquare> listOfBoardSquareMove)>();

            values.ForAll(x =>
            {
                BoardSquare boardSquare = board.First(bs => bs.RowIndex == x.boardSquare.RowIndex && bs.ColumnIndex == x.boardSquare.ColumnIndex);
                List<BoardSquare> listOfBoardSquareMove = board.Where(bs => x.listOfBoardSquareMove.Any(y => bs.RowIndex == y.RowIndex && bs.ColumnIndex == y.ColumnIndex)).ToList();
                returns.Add((boardSquare, listOfBoardSquareMove));
            });

            return returns;
        }

        public void Dispose()
        {
        }
    }
}
