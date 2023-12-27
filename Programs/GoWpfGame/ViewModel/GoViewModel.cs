using GameUtils.Interfaces;
using GameUtils.Utils;
using GoWpfGame.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace GoWpfGame.ViewModel
{
    public class GoViewModel : ViewObserver, IGameViewModel
    {
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

        private string showMessageScore;
        public string ShowMessageScore
        {
            get { return showMessageScore; }
            set
            {
                showMessageScore = value;
                OnPropertyChanged(nameof(ShowMessageScore));
            }
        }

        private ICommand closeGameScoreCommand;
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

        private int _columnCount;
        public int ColumnCount
        {
            get { return _columnCount; }
            set
            {
                _columnCount = value;
                OnPropertyChanged();
            }
        }

        private int _rowCount;
        public int RowCount
        {
            get { return _rowCount; }
            set
            {
                _rowCount = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<GoField> goBoard;
        public ObservableCollection<GoField> GoBoard
        {
            get { return goBoard; }
            set
            {
                goBoard = value;
                OnPropertyChanged(nameof(goBoard));
            }
        }


        private ObservableDictionary<string,int> playersPoints;
        public ObservableDictionary<string,int> PlayersPoints
        {
            get { return playersPoints; }
            set
            {
                playersPoints = value;
                OnPropertyChanged(nameof(PlayersPoints));
            }
        }


        private string currentPlayerColor;
        public string CurrentPlayerColor
        {
            get { return currentPlayerColor; }
            set
            {
                currentPlayerColor = value;
                OnPropertyChanged(nameof(CurrentPlayerColor));
            }
        }

        private ICommand newGameCommand;
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


        private ICommand boardFieldCommand;
        public ICommand BoardFieldCommand
        {
            get
            {
                if (boardFieldCommand == null)
                    boardFieldCommand = new RelayCommand<GoField>(
                        gf =>
                        {
                            if (gf.Sign != " ")
                                return;

                            gf.Sign = currentPlayerSign;
                            gf.ColorPawn = playersColor[currentPlayerSign];

                            List<GoField> surroundedFields = GetSurroundedFields(opponentPlayerSign);
                            surroundedFields.ForEach(gf =>
                            {
                                gf.Sign = " ";
                                gf.ColorPawn = "Transparent";
                            });
                            PlayersPoints[playersColor[opponentPlayerSign]] += surroundedFields.Count;

                            surroundedFields = GetSurroundedFields(currentPlayerSign);
                            surroundedFields.ForEach(gf =>
                            {
                                gf.Sign = " ";
                                gf.ColorPawn = "Transparent";
                            });
                            PlayersPoints[CurrentPlayerColor] += surroundedFields.Count;

                            ChangePlayer();
                        }
                        );
                return boardFieldCommand;
            }
        }

        private CircularObservableCollection<string> playersSign;
        private Dictionary<string, string> playersColor;
        private string currentPlayerSign;
        private string opponentPlayerSign;

        public GoViewModel()
        {
            ColumnCount = 19;
            RowCount = 19;
            GoBoard = new ObservableCollection<GoField>();

            playersSign = new CircularObservableCollection<string>() { "B", "C" };
            playersColor = new Dictionary<string, string>
            {
                { "B", "white" },
                { "C", "black" }
            };

            NewGame();
        }

        private void NewGame()
        {
            for (int col = 0; col < ColumnCount; col++)
                for (int row = 0; row < RowCount; row++)
                    GoBoard.Add(new GoField() { ColIndex = col, RowIndex = row, Sign = " ", ColorPawn = "Transparent", BoardFieldCommand = BoardFieldCommand });

            PlayersPoints = new ObservableDictionary<string, int>();
            foreach (var playerColor in playersColor)
            {
                PlayersPoints.Add(playerColor.Value, 0);
            }

            currentPlayerSign = playersSign.GetNext();
            ChangePlayer();
        }

        private void ChangePlayer()
        {
            opponentPlayerSign = currentPlayerSign;
            currentPlayerSign = playersSign.GetNext();
            CurrentPlayerColor = playersColor[currentPlayerSign];
        }

        private List<GoField> GetSurroundedFields(string playerSign)
        {
            List<GoField> listFieldNotSurrounded = new List<GoField>();
            foreach (GoField goField in GoBoard)
            {
                if (goField.Sign == playerSign
                    && IsNotSurrounded(goField))
                {
                    listFieldNotSurrounded.Add(goField);
                }
            }

            List<GoField> cloneListFieldNotSurrounded = new List<GoField>(listFieldNotSurrounded);
            foreach (GoField goField in cloneListFieldNotSurrounded)
            {
                FindAllNeighbors(listFieldNotSurrounded, goField);
            }

            List<GoField> allPawn = GoBoard.Where(gf => gf.Sign == playerSign).ToList();

            List<GoField> surroundedFields = allPawn.Except(listFieldNotSurrounded).ToList();

            return surroundedFields;
        }

        private bool IsNotSurrounded(GoField goField)
        {
            int[] dRow = { -1, 0, 1, 0 };
            int[] dCol = { 0, 1, 0, -1 };

            for (int i = 0; i < dRow.Length; i++)
            {
                GoField? findField = GoBoard.FirstOrDefault(gf => gf.RowIndex == goField.RowIndex + dRow[i] && gf.ColIndex == goField.ColIndex + dCol[i]);
                if (findField != null && findField.Sign == " ")
                    return true;
            }
            return false;
        }

        private void FindAllNeighbors(List<GoField> listFieldNotSurrounded, GoField goField)
        {
            int[] dRow = { -1, 0, 1, 0 };
            int[] dCol = { 0, 1, 0, -1 };

            for (int i = 0; i < dRow.Length; i++)
            {
                GoField? findField = GoBoard.FirstOrDefault(gf => gf.RowIndex == goField.RowIndex + dRow[i] && gf.ColIndex == goField.ColIndex + dCol[i]);
                if (findField != null
                    && findField.Sign == goField.Sign
                    && !listFieldNotSurrounded.Contains(findField))
                {
                    listFieldNotSurrounded.Add(findField);
                    FindAllNeighbors(listFieldNotSurrounded, findField);
                }

            }
        }
    }
}
