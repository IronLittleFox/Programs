using CheckersWpfGame.Model;
using GameUtils.Interfaces;
using GameUtils.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq.Dynamic.Core;

namespace CheckersWpfGame.ViewModel
{
    public static class Extensions
    {
        public static IEnumerable<IGrouping<bool, T>> Split<T>(
            this IEnumerable<T> source,
            Func<T, bool> predicate)
        {
            return source.GroupBy(predicate);
        }
    }

    internal class CheckersViewModel : ViewObserver, IGameViewModel
    {
        private bool runGame = true;
        public bool RunGame
        {
            get { return runGame; }
            set
            {
                runGame = value;
                OnPropertyChanged(nameof(RunGame));
            }
        }

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

        private ObservableCollection<PlayingField>? _listOfField = null;
        public ObservableCollection<PlayingField> ListOfPlayingField
        {
            get { return _listOfField; }
            set
            {
                _listOfField = value;
                OnPropertyChanged();
            }
        }

        private Player? currentPlayer = null;
        public Player CurrentPlayer
        {
            get { return currentPlayer; }
            set
            {
                currentPlayer = value;
                OnPropertyChanged(nameof(CurrentPlayer));
            }
        }

        #region GameScore
        private bool showGameScore = false;
        public bool ShowGameScore
        {
            get { return showGameScore; }
            set
            {
                showGameScore = value;
                OnPropertyChanged(nameof(ShowGameScore));
            }
        }

        private string showMessageScore = "";
        public string ShowMessageScore
        {
            get { return showMessageScore; }
            set
            {
                showMessageScore = value;
                OnPropertyChanged(nameof(ShowMessageScore));
            }
        }

        private ICommand? closeGameScoreCommand = null;
        public ICommand CloseGameScoreCommand
        {
            get
            {
                if (closeGameScoreCommand == null)
                    closeGameScoreCommand = new RelayCommand<object>(
                        o =>
                        {
                            ShowGameScore = false;
                        }
                        );
                return closeGameScoreCommand;
            }
        }
        #endregion

        private ICommand? newGameCommand = null;
        public ICommand NewGameCommand
        {
            get
            {
                if (newGameCommand == null)
                    newGameCommand = new RelayCommand<object>(
                        o =>
                        {
                            NewGame();
                        }
                        );
                return newGameCommand;
            }
        }

        private ICommand? boardFieldCommand = null;
        public ICommand BoardFieldCommand
        {
            get
            {
                if (boardFieldCommand == null)
                    boardFieldCommand = new RelayCommand<PlayingField>(
                        playingField =>
                        {
                            if (isEndGame)
                                return;

                            //kliknieto możliwy ruch
                            //wykonanie ruchu, odzanaczenie starych możliwości, i jeśli zbito pionek gracza to sprawdzenie
                            //czy gracz ten ma kontynuować czy zmiana gracza
                            if (selectedField != null
                            && currentPlayerPawnsToMove[selectedField.PlayerPawn].Any(diagonal => diagonal.Any(pf => pf == playingField)))
                            {
                                ChangeDiagonalColor(currentPlayerPawnsToMove[selectedField.PlayerPawn], selectedField.FieldColor);
                                selectedField.PlayerPawn.PawnColor = CurrentPlayer.PlayerColor;

                                bool? isOponentOnMoveDiagonal = currentPlayerPawnsToMove[selectedField.PlayerPawn]
                                .FirstOrDefault(diagonal => diagonal.Contains(playingField))?.Any(pf => pf.PlayerPawn != falsePawn);

                                //zbito pionek przeciwnika to go usuwamy
                                if (isOponentOnMoveDiagonal != null && isOponentOnMoveDiagonal.Value)
                                {
                                    PlayingField playingFieldWidthOponentPawn = currentPlayerPawnsToMove[selectedField.PlayerPawn]
                                 .First(diagonal => diagonal.Contains(playingField)).First(pf => pf.PlayerPawn != falsePawn);

                                    _players
                                    .First(player => player.Pawns.Contains(playingFieldWidthOponentPawn.PlayerPawn))
                                    ?.Pawns
                                    ?.Remove(playingFieldWidthOponentPawn.PlayerPawn);

                                    playingFieldWidthOponentPawn.PlayerPawn = falsePawn;
                                }

                                playingField.PlayerPawn = selectedField.PlayerPawn;
                                selectedField.PlayerPawn = falsePawn;
                                ChangePawnMustMoveColor("Transparent");

                                selectedField = null;

                                if (!_players.All(p => p.Pawns.Count > 0))
                                {
                                    isEndGame = true;
                                    ShowGameScore = true;
                                    ShowMessageScore = "Koniec gry.";
                                    return;
                                }

                                if (isOponentOnMoveDiagonal != null && isOponentOnMoveDiagonal.Value)
                                {
                                    //sprawdzamy czy pionek na nowym miejscu ma kolejne bicie
                                    Dictionary<Pawn, List<List<PlayingField>>> pawnToMove = GetCurrentPlayerPawnsToMove(playingField);
                                    if (CheckIsAnyCapturingOnDiagonals(pawnToMove))
                                    {
                                        currentPlayerPawnsToMove = pawnToMove;
                                        ChangePawnMustMoveColor("aqua");
                                        return;
                                    }

                                }

                                currentPlayerNumber = (currentPlayerNumber + 1) % _players.Count;
                                CurrentPlayer = _players[currentPlayerNumber];

                                currentPlayerPawnsToMove = GetCurrentPlayerPawnsToMove();
                                ChangePawnMustMoveColor("aqua");
                                return;

                            }

                            //kliknięto pionek aktualnego gracza ktory nie był wcześniej wybrany
                            //odzanaczenie starych możliwośći i zaznaczenie nowych
                            if (selectedField != playingField
                                && currentPlayerPawnsToMove.ContainsKey(playingField.PlayerPawn))
                            {
                                if (selectedField != null)
                                {
                                    ChangeDiagonalColor(currentPlayerPawnsToMove[selectedField.PlayerPawn], selectedField.FieldColor);
                                    selectedField.PlayerPawn.PawnColor = CurrentPlayer.PlayerColor;
                                }

                                selectedField = playingField;
                                selectedField.PlayerPawn.PawnColor = selectColorPawn;

                                ChangeDiagonalColor(currentPlayerPawnsToMove[selectedField.PlayerPawn], possibleMovmentColorField);

                            }
                        }
                        );
                return boardFieldCommand;
            }
        }

        private List<Player> _players = new List<Player>();
        private int currentPlayerNumber = 0;
        private bool isEndGame = false;
        private string whiteColorField = "white";
        private string darkColorField = "brown";
        private string possibleMovmentColorField = "gold";
        private string selectColorPawn = "black";
        private Pawn falsePawn = new Pawn();

        private PlayingField? selectedField = null;

        private Dictionary<Pawn, List<List<PlayingField>>> currentPlayerPawnsToMove = new Dictionary<Pawn, List<List<PlayingField>>>();


        public CheckersViewModel()
        {
            _players.Add(new Player() { PlayerColor = "Green" });
            _players.Add(new Player() { PlayerColor = "Blue" });
            currentPlayerNumber = 0;
            CurrentPlayer = _players[currentPlayerNumber];
            NewGame();
        }

        private void NewGame()
        {
            ListOfPlayingField = new ObservableCollection<PlayingField>();
            for (int row = 0; row < RowCount; row++)
                for (int col = 0; col < ColumnCount; col++)
                {
                    ListOfPlayingField.Add(new PlayingField()
                    {
                        ColumnIndex = col,
                        RowIndex = row,
                        FieldColor = ((col + row % 2) % 2) == 0 ? whiteColorField : darkColorField,
                        SpareColor = ((col + row % 2) % 2) == 0 ? whiteColorField : darkColorField,
                        BoardFieldCommand = BoardFieldCommand,
                        PlayerPawn = falsePawn
                    });
                }
            isEndGame = false;

            _players[0].Pawns = new List<Pawn>();
            ListOfPlayingField.Where(pf => pf.FieldColor == darkColorField
                                                                && (pf.RowIndex == 0
                                                                    || pf.RowIndex == 1
                                                                    || pf.RowIndex == 2
                                                                    || pf.RowIndex == 3))
                .ForAll(pf =>
                {
                    Pawn pawn = new Pawn() 
                    {
                        PawnColor = _players[0].PlayerColor,
                        Distance = 1,
                        DirectionsOfMovements = new List<(int col, int row)> { (-1, 1), (1, 1)},
                        DirectionsOfCapturing = new List<(int col, int row)> { (-1, -1), (1, -1)}
                    };
                    _players[0].Pawns.Add(pawn);
                    pf.PlayerPawn = pawn;

                });

            _players[1].Pawns = new List<Pawn>();
            ListOfPlayingField.Where(pf => pf.FieldColor == darkColorField
                                                                && (pf.RowIndex == RowCount - 1
                                                                    || pf.RowIndex == RowCount - 2
                                                                    || pf.RowIndex == RowCount - 3
                                                                    || pf.RowIndex == RowCount - 4))
                .ForAll(pf =>
                {
                    Pawn pawn = new Pawn() 
                    { 
                        PawnColor = _players[1].PlayerColor, 
                        Distance = 1,
                        DirectionsOfMovements = new List<(int row, int col)> { (-1, -1), (1, -1) },
                        DirectionsOfCapturing = new List<(int col, int row)> { (-1, 1), (1, 1) }
                    };
                    _players[1].Pawns.Add(pawn);
                    pf.PlayerPawn = pawn;

                });

            ChangePawnMustMoveColor("Transparent");
            currentPlayerPawnsToMove = GetCurrentPlayerPawnsToMove();
            ChangePawnMustMoveColor("aqua");
        }

        private bool CheckIsAnyCapturingOnDiagonals(Dictionary<Pawn, List<List<PlayingField>>> pawnsToMove)
        {
            return pawnsToMove.Any(kwp => kwp.Value.Any(diagonal => diagonal.Any(pf => pf.PlayerPawn != falsePawn)));
        }

        private void DeletePawnsToMoveWithoutCapturing(Dictionary<Pawn, List<List<PlayingField>>> pawnsToMove)
        {
            foreach (var kwp in pawnsToMove)
            {
                kwp.Value.RemoveAll(diagonal => diagonal.All(pf => pf.PlayerPawn == falsePawn));
                foreach (var diagonal in kwp.Value)
                {
                    var toRemoveField = diagonal.TakeWhile(pf => pf.PlayerPawn == falsePawn).ToList();
                    foreach (var item in toRemoveField)
                    {
                        diagonal.Remove(item);
                    }
                }
            }
            var toRemowe = pawnsToMove.Where(kwp => kwp.Value.Count == 0);
            foreach (var kwp in toRemowe)
            {
                pawnsToMove.Remove(kwp.Key);
            }
        }

        private void ChangePawnMustMoveColor(string color)
        {
            foreach (var kwp in currentPlayerPawnsToMove)
            {
                kwp.Key.PawnMustMoveColor = color;
            }
        }

        private void ChangeDiagonalColor(List<List<PlayingField>> diagonalsPlayingFields, string color)
        {
            foreach (var diagonal in diagonalsPlayingFields)
            {
                foreach (var plaingField in diagonal)
                {
                    if (plaingField.PlayerPawn == falsePawn)
                        plaingField.FieldColor = color;
                }
            }
        }

        private Dictionary<Pawn, List<List<PlayingField>>> GetCurrentPlayerPawnsToMove()
        {
            Dictionary<Pawn, List<List<PlayingField>>> pawnsToMove = new Dictionary<Pawn, List<List<PlayingField>>>();

            foreach (var pawn in CurrentPlayer.Pawns)
            {
                PlayingField? playingField = ListOfPlayingField.FirstOrDefault(pf => pf.PlayerPawn == pawn);
                if (playingField != null)
                {
                    List<List<PlayingField>> diagonals = GetCollectionOfDiagonal(playingField);
                    if (diagonals.Count > 0)
                        pawnsToMove.Add(pawn, diagonals);
                }
            }

            if (CheckIsAnyCapturingOnDiagonals(pawnsToMove))
                DeletePawnsToMoveWithoutCapturing(pawnsToMove);

            return pawnsToMove;
        }

        private Dictionary<Pawn, List<List<PlayingField>>> GetCurrentPlayerPawnsToMove(PlayingField playingField)
        {
            Dictionary<Pawn, List<List<PlayingField>>> pawnsToMove = new Dictionary<Pawn, List<List<PlayingField>>>();

            List<List<PlayingField>> diagonals = GetCollectionOfDiagonal(playingField);
            if (diagonals.Count > 0)
                pawnsToMove.Add(playingField.PlayerPawn, diagonals);


            if (CheckIsAnyCapturingOnDiagonals(pawnsToMove))
                DeletePawnsToMoveWithoutCapturing(pawnsToMove);

            return pawnsToMove;
        }

        private List<List<PlayingField>> GetCollectionOfDiagonal(PlayingField playingField)
        {
            List<List<PlayingField>> diagonals = new List<List<PlayingField>>();

            List<PlayingField> diagonal = GetDiagonal(playingField.ColumnIndex - 1, playingField.RowIndex - 1,
                                                      playingField.ColumnIndex - playingField.PlayerPawn.Distance, playingField.RowIndex - playingField.PlayerPawn.Distance,
                                                      -1, -1);
            if (diagonal.Count > 0)
                diagonals.Add(diagonal);

            diagonal = GetDiagonal(playingField.ColumnIndex + 1, playingField.RowIndex - 1,
                                   playingField.ColumnIndex + playingField.PlayerPawn.Distance, playingField.RowIndex - playingField.PlayerPawn.Distance,
                                   1, -1);
            if (diagonal.Count > 0)
                diagonals.Add(diagonal);

            diagonal = GetDiagonal(playingField.ColumnIndex - 1, playingField.RowIndex + 1,
                                   playingField.ColumnIndex - playingField.PlayerPawn.Distance, playingField.RowIndex + playingField.PlayerPawn.Distance,
                                   -1, 1);
            if (diagonal.Count > 0)
                diagonals.Add(diagonal);

            diagonal = GetDiagonal(playingField.ColumnIndex + 1, playingField.RowIndex + 1,
                                      playingField.ColumnIndex + playingField.PlayerPawn.Distance, playingField.RowIndex + playingField.PlayerPawn.Distance,
                                      1, 1);
            if (diagonal.Count > 0)
                diagonals.Add(diagonal);

            return diagonals;
        }

        private List<PlayingField> GetDiagonal(int columnStart, int rowStart, int columnEnd, int rowEnd, int sx, int sy)
        {
            int currColumn = columnStart;
            int currRow = rowStart;

            List<PlayingField> fieldsOnDiagonal = new List<PlayingField>();

            int dx = Math.Abs(columnEnd - currColumn);
            int dy = Math.Abs(rowEnd - currRow);
            bool oneMoreField = false;

            while (true)
            {
                PlayingField? playingField = ListOfPlayingField
                    .FirstOrDefault(pf => pf.ColumnIndex == currColumn
                                        && pf.RowIndex == currRow);

                if (playingField == null
                    || playingField.PlayerPawn.PawnColor == CurrentPlayer.PlayerColor)
                    break;

                if (playingField.PlayerPawn == falsePawn)
                    fieldsOnDiagonal.Add(playingField);

                if (oneMoreField)
                    break;

                //jest to przeciwnik to dodajemy jeszcze jedno pole za nim
                if (playingField.PlayerPawn != falsePawn)
                {
                    fieldsOnDiagonal.Add(playingField);
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
            PlayingField? latstField = fieldsOnDiagonal.LastOrDefault();
            if (latstField != null && latstField.PlayerPawn != falsePawn)
                fieldsOnDiagonal.Remove(latstField);

            return fieldsOnDiagonal;
        }
    }
}
