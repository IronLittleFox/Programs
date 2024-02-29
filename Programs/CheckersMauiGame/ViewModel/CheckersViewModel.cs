using CheckersMauiGame.Enums;
using CheckersMauiGame.Model;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UtilsMaui.Interfaces;
using UtilsMaui.Utils;

namespace CheckersMauiGame.ViewModel
{
    public class CheckersViewModel : BindableObject, IGameViewModel
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

                            //kliknięto na pionek aktualnego gracza który może się ruszyć
                            if (boardSquaresToMove.Any(x => x.boardSquare == boardSquare))
                            {
                                //odznaczamy możliwe ruchy
                                boardSquaresToMove.Where(x => x.boardSquare == chooseBoardSquare)?.ForAll(x => x.diagonals.ForAll(y => y.diagonal.ForAll(z => z.IsPossibleMove = false)));

                                //zaznaczamy nowe możliwe ruchy
                                boardSquaresToMove.Where(x => x.boardSquare == boardSquare)?.ForAll(x => x.diagonals.ForAll(y => y.diagonal.ForAll(z => { if (z.CheckerPiece == falseCheckerPiece) z.IsPossibleMove = true; })));

                                chooseBoardSquare = boardSquare;
                                return;
                            }

                            //kliknięto na pole na które można się poruszyć
                            if (chooseBoardSquare != null
                                && boardSquaresToMove.Where(x => x.boardSquare == chooseBoardSquare)
                                     .Any(x => x.diagonals.Any(y => y.diagonal.Contains(boardSquare)))
                                && boardSquare.CheckerPiece == falseCheckerPiece)
                            {
                                //odznaczamy możliwe ruchy
                                boardSquaresToMove.Where(x => x.boardSquare == chooseBoardSquare)?.ForAll(x => x.diagonals.ForAll(y => y.diagonal.ForAll(z => z.IsPossibleMove = false)));
                                boardSquaresToMove.ForAll(x => x.boardSquare.IsCheckerPieceMustMove = false);

                                CheckerPiece? opponentCapturing = boardSquaresToMove
                                .Where(x => x.boardSquare == chooseBoardSquare)
                                .First()
                                .diagonals
                                .First(x => x.diagonal.Contains(boardSquare))
                                .diagonal
                                .FirstOrDefault(x => x.CheckerPiece != falseCheckerPiece)
                                ?.CheckerPiece;


                                boardSquare.CheckerPiece = chooseBoardSquare.CheckerPiece;
                                chooseBoardSquare.CheckerPiece = falseCheckerPiece;

                                if (opponentCapturing != null)
                                {
                                    var findBoardSquare = board.First(bs => bs.CheckerPiece == opponentCapturing);
                                    findBoardSquare.CheckerPiece = falseCheckerPiece;
                                    gamePlayers.First(gp => gp.CheckerPieces.Contains(opponentCapturing)).CheckerPieces.Remove(opponentCapturing);

                                    //znalezienie możliwej kontynuacji ruchu
                                    List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> diagonals = FindDiagonals(boardSquare);
                                    if (diagonals.Any(y => y.diagonal.Any(z => z.CheckerPiece != falseCheckerPiece)))
                                    {
                                        boardSquaresToMove.Clear();
                                        boardSquaresToMove.Add((boardSquare, diagonals));

                                        //boardSquaresToMove.ForEach(x => x.diagonals = x.diagonals.Where(y => y.diagonal.Any(z => z.CheckerPiece != falseCheckerPiece)).ToList());
                                        boardSquaresToMove = RemoveDiagonalsWithoutCapturing(boardSquaresToMove);

                                        boardSquaresToMove.RemoveAll(x => x.diagonals.Count == 0);

                                        boardSquaresToMove.ForAll(x => x.boardSquare.IsCheckerPieceMustMove = true);
                                        chooseBoardSquare = boardSquare;

                                        squareCommand?.Execute(boardSquare);

                                        return;
                                    }
                                }
                                if (endOfPlayerRow[currentGamePlayer] == boardSquare.RowIndex)
                                {
                                    CurrentGamePlayer.CheckerPieces.Remove(boardSquare.CheckerPiece);
                                    boardSquare.CheckerPiece = new CheckerKing(CurrentGamePlayer.PlayerColor, RowCount, listOfPlayerKingDirections[CurrentGamePlayer]);
                                    CurrentGamePlayer.CheckerPieces.Add(boardSquare.CheckerPiece);
                                }

                                if (gamePlayers.Where(gp => gp.PlayerColor != CurrentGamePlayer.PlayerColor).First().CheckerPieces.Count == 0)
                                {
                                    isEndGame = true;
                                    popupService.ShowPopupAsync<CheckersPopupViewModel>(
                                    onPresenting: vm =>
                                    {
                                        vm.Message = "Koniec gry.\nWygrana.\n";
                                        //vm.ImageSymbol = currentPlayer.Name;
                                    });
                                    return;
                                }

                                CurrentGamePlayer = gamePlayers.GetNext();
                                boardSquaresToMove = FindAllPawnToMove();
                                boardSquaresToMove.ForAll(x => x.boardSquare.IsCheckerPieceMustMove = true);

                                chooseBoardSquare = null;
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

        private CircularObservableCollection<GamePlayer> gamePlayers;
        private Dictionary<GamePlayer, int> endOfPlayerRow;
        private Dictionary<GamePlayer, List<(TypeOfDirection typeOfDirection, int col, int row)>> listOfPlayerPawnDirections;
        private Dictionary<GamePlayer, List<(TypeOfDirection typeOfDirection, int col, int row)>> listOfPlayerKingDirections;
        private CheckerPiece falseCheckerPiece = new CheckerPawn("empty", new List<(TypeOfDirection typeOfDirection, int col, int row)>());
        private string whiteColorField = "white";
        private string darkColorField = "#FFC5C5C5";
        private List<(BoardSquare boardSquare, List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> diagonals)> boardSquaresToMove = new();
        private BoardSquare? chooseBoardSquare;
        private bool isEndGame = false;
        private IPopupService popupService;

        public CheckersViewModel(IPopupService popupService)
        {
            this.popupService = popupService;

            board = new ObservableCollection<BoardSquare>();
            Board = board;

            gamePlayers = new CircularObservableCollection<GamePlayer>()
            {
                new GamePlayer() {PlayerColor = "white", CheckerPieces = new List<CheckerPiece>() },
                new GamePlayer() {PlayerColor = "black", CheckerPieces = new List<CheckerPiece>() }
            };
            endOfPlayerRow = new Dictionary<GamePlayer, int>() { { gamePlayers[0], RowCount - 1 }, { gamePlayers[1], 0 } };
            listOfPlayerPawnDirections = new Dictionary<GamePlayer, List<(TypeOfDirection typeOfDirection, int col, int row)>>()
            {
                { gamePlayers[0], new List<(TypeOfDirection typeOfDirection, int col, int row)>
                                    {
                                        (TypeOfDirection.Move, -1, 1), (TypeOfDirection.Move, 1, 1),
                                        (TypeOfDirection.Capturing, -1, -1), (TypeOfDirection.Capturing, 1, -1)
                                    }},
                {gamePlayers[1], new List<(TypeOfDirection typeOfDirection, int col, int row)>
                                    {
                                        (TypeOfDirection.Move, - 1, -1), (TypeOfDirection.Move, 1, -1),
                                        (TypeOfDirection.Capturing, -1, 1), (TypeOfDirection.Capturing, 1, 1)
                                    }}
            };
            listOfPlayerKingDirections = new Dictionary<GamePlayer, List<(TypeOfDirection typeOfDirection, int col, int row)>>()
            {
                { gamePlayers[0], new List<(TypeOfDirection typeOfDirection, int col, int row)>
                                    {
                                        (TypeOfDirection.Move, -1, 1), (TypeOfDirection.Move, 1, 1),
                                        (TypeOfDirection.Move, -1, -1), (TypeOfDirection.Move, 1, -1)
                                    }},
                {gamePlayers[1], new List<(TypeOfDirection typeOfDirection, int col, int row)>
                                    {
                                        (TypeOfDirection.Move, - 1, -1), (TypeOfDirection.Move, 1, -1),
                                        (TypeOfDirection.Move, -1, 1), (TypeOfDirection.Move, 1, 1)
                                    }}
            };

            CurrentGamePlayer = currentGamePlayer = gamePlayers.First();

            NewGame();
        }

        private void NewGame()
        {
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
                        CheckerPiece = falseCheckerPiece,
                        IsCheckerPieceMustMove = false,
                        IsPossibleMove = false
                    });
                }

            Board.Where(bs => bs.SquareColor == darkColorField
                                                && (bs.RowIndex == 0
                                                    || bs.RowIndex == 1
                                                    || bs.RowIndex == 2
                                                    || bs.RowIndex == 3))
                .ForAll(bs =>
                {
                    CheckerPiece checkerPiece = new CheckerPawn(gamePlayers[0].PlayerColor, listOfPlayerPawnDirections[gamePlayers[0]]);
                    gamePlayers[0].CheckerPieces.Add(checkerPiece);
                    bs.CheckerPiece = checkerPiece;
                });

            Board.Where(bs => bs.SquareColor == darkColorField
                                                && (bs.RowIndex == RowCount - 1
                                                    || bs.RowIndex == RowCount - 2
                                                    || bs.RowIndex == RowCount - 3
                                                    || bs.RowIndex == RowCount - 4))
                .ForAll(bs =>
                {
                    CheckerPiece checkerPiece = new CheckerPawn(gamePlayers[1].PlayerColor, listOfPlayerPawnDirections[gamePlayers[1]]);

                    gamePlayers[1].CheckerPieces.Add(checkerPiece);
                    bs.CheckerPiece = checkerPiece;
                });

            boardSquaresToMove = FindAllPawnToMove();
            boardSquaresToMove.ForAll(x => x.boardSquare.IsCheckerPieceMustMove = true);
            chooseBoardSquare = null;
        }

        private List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> FindDiagonals(BoardSquare boardSquare)
        {
            List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> listOfDiagonals = new List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)>();

            List<BoardSquare> diagonal;
            foreach (var directions in boardSquare.CheckerPiece.Directions)
            {
                diagonal = FindDiagonal(boardSquare.ColumnIndex + directions.col, boardSquare.RowIndex + directions.row,
                                       boardSquare.ColumnIndex + boardSquare.CheckerPiece.Distance * directions.col,
                                       boardSquare.RowIndex + boardSquare.CheckerPiece.Distance * directions.row,
                                       directions.col, directions.row);
                if (diagonal.Count > 0)
                    listOfDiagonals.Add((directions.typeOfDirection, diagonal));
            }
            return listOfDiagonals;
        }

        private List<BoardSquare> FindDiagonal(int columnStart, int rowStart, int columnEnd, int rowEnd, int sx, int sy)
        {
            int currColumn = columnStart;
            int currRow = rowStart;

            List<BoardSquare> squaresOnDiagonal = new List<BoardSquare>();

            int dx = Math.Abs(columnEnd - currColumn);
            int dy = Math.Abs(rowEnd - currRow);
            bool oneMoreField = false;

            while (true)
            {
                BoardSquare? boardSquare = Board
                    .FirstOrDefault(bs => bs.ColumnIndex == currColumn
                                        && bs.RowIndex == currRow);

                if (boardSquare == null
                    || boardSquare.CheckerPiece.Color == CurrentGamePlayer.PlayerColor)
                    break;

                if (boardSquare.CheckerPiece == falseCheckerPiece)
                    squaresOnDiagonal.Add(boardSquare);

                if (oneMoreField)
                    break;

                //jest to przeciwnik to dodajemy jeszcze jedno pole za nim
                if (boardSquare.CheckerPiece != falseCheckerPiece)
                {
                    squaresOnDiagonal.Add(boardSquare);
                    oneMoreField = true;
                }

                if (currColumn == columnEnd
                    && currRow == rowEnd
                    && !oneMoreField)
                {
                    break;
                }
                currColumn += sx;
                currRow += sy;
            }

            //ostatnim na liście jest pionek przeciwnika to trzeba go usunąć.
            BoardSquare? latstField = squaresOnDiagonal.LastOrDefault();
            if (latstField != null && latstField.CheckerPiece != falseCheckerPiece)
                squaresOnDiagonal.Remove(latstField);

            //jesli na liście znajduje się pionek przeciwnika to bierzemy wszystko od tego pionka do końca
            if (squaresOnDiagonal.Any(bs => bs.CheckerPiece != falseCheckerPiece))
                squaresOnDiagonal = squaresOnDiagonal.SkipWhile(bs => bs.CheckerPiece == falseCheckerPiece).ToList();

            return squaresOnDiagonal;
        }

        private List<(BoardSquare boardSquare, List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> diagonals)> FindAllPawnToMove()
        {
            List<(BoardSquare boardSquare, List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> diagonals)> boardSquaresToMove = new List<(BoardSquare boardSquare, List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> diagonals)>();

            List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> diagonals;
            foreach (BoardSquare boardSquare in Board.Where(bs => bs.CheckerPiece.Color == CurrentGamePlayer.PlayerColor))
            {
                diagonals = FindDiagonals(boardSquare);

                if (diagonals.Count > 0)
                    boardSquaresToMove.Add((boardSquare, diagonals));
            }

            if (boardSquaresToMove.Any(x => x.diagonals.Any(y => y.diagonal.Any(z => z.CheckerPiece != falseCheckerPiece))))
            {
                boardSquaresToMove = RemoveDiagonalsWithoutCapturing(boardSquaresToMove);
            }
            else
                boardSquaresToMove = RemoveDiagonals(TypeOfDirection.Capturing, boardSquaresToMove);


            boardSquaresToMove.RemoveAll(x => x.diagonals.Count == 0);

            return boardSquaresToMove;
        }

        private List<(BoardSquare boardSquare, List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> diagonals)> RemoveDiagonalsWithoutCapturing(List<(BoardSquare boardSquare, List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> diagonals)> boardSquaresToMove)
        {
            List<(BoardSquare boardSquare, List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> diagonals)> boardSuaresToMoveCopy = new List<(BoardSquare boardSquare, List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> diagonals)>();
            List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> diagonals;
            foreach (var tupple in boardSquaresToMove)
            {
                diagonals = tupple.diagonals.Where(y => y.diagonal.Any(z => z.CheckerPiece != falseCheckerPiece)).ToList();
                boardSuaresToMoveCopy.Add((tupple.boardSquare, diagonals));
            }
            return boardSuaresToMoveCopy;
        }

        private List<(BoardSquare boardSquare, List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> diagonals)> RemoveDiagonals(TypeOfDirection typeOfDirection, List<(BoardSquare boardSquare, List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> diagonals)> boardSquaresToMove)
        {
            List<(BoardSquare boardSquare, List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> diagonals)> boardSuaresToMoveCopy = new List<(BoardSquare boardSquare, List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> diagonals)>();
            List<(TypeOfDirection typeOfDirection, List<BoardSquare> diagonal)> diagonals;
            foreach (var tupple in boardSquaresToMove)
            {
                diagonals = tupple.diagonals.Where(y => y.typeOfDirection != typeOfDirection).ToList();
                boardSuaresToMoveCopy.Add((tupple.boardSquare, diagonals));
            }
            return boardSuaresToMoveCopy;
        }

        public void Dispose()
        {
        }
    }
}
