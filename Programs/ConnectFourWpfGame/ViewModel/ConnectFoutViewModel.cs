﻿using ConnectFourWpfGame.Model;
using GameUtils.Interfaces;
using GameUtils.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConnectFourWpfGame.ViewModel
{
    public class ConnectFoutViewModel : ViewObserver, IGameViewModel
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

        private int _columnCount = 6;
        public int ColumnCount
        {
            get { return _columnCount; }
            set
            {
                _columnCount = value;
                OnPropertyChanged();
            }
        }

        private int _rowCount = 7;
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

        private int currentPlayerNumber = 0;

        private Player currentPlayer;
        public Player CurrentPlayer
        {
            get { return currentPlayer; }
            set
            {
                currentPlayer = value;
                OnPropertyChanged(nameof(CurrentPlayer));
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

                            PlayingField? firstFreePlayingFieldInRow
                            = ListOfPlayingField.Where(pf => pf.ColumnIndex == playingField.ColumnIndex
                                                             && pf.FieldColor == emptyColorField)
                            .OrderByDescending(pf => pf.RowIndex).FirstOrDefault();
                            if (firstFreePlayingFieldInRow != null)
                            {
                                firstFreePlayingFieldInRow.FieldColor = CurrentPlayer.PlayerColor;
                                if (CheckWeen(firstFreePlayingFieldInRow))
                                {
                                    isEndGame = true;
                                    ShowGameScore = true;
                                    ShowMessageScore = "Koniec gry.\nWygrana.";
                                    return;
                                }
                                if (CheckDraw())
                                {
                                    isEndGame = true;
                                    ShowGameScore = true;
                                    ShowMessageScore = "Koniec gry.\nRemis.";
                                    return;
                                }
                                currentPlayerNumber = (currentPlayerNumber + 1) % _players.Count;
                                CurrentPlayer = _players[currentPlayerNumber];
                            }
                        }
                        );
                return boardFieldCommand;
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

        private List<Player> _players;
        private string emptyColorField = "white";
        private bool isEndGame = false;
        private int winLineLength = 4;

        public ConnectFoutViewModel()
        {
            NewGame();

            _players = new List<Player>();
            _players.Add(new Player() { PlayerColor = "Red" });
            _players.Add(new Player() { PlayerColor = "Blue" });
            CurrentPlayer = _players.First();
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
                        FieldColor = emptyColorField,
                        BoardFieldCommand = BoardFieldCommand,
                    });
                }
            isEndGame = false;
        }

        private bool CheckDraw()
        {
            return !ListOfPlayingField.Any(pf => pf.FieldColor == emptyColorField);
        }

        private bool CheckWeen(PlayingField firstFreePlayingFieldInRow)
        {
            var horizontalLine = ListOfPlayingField.Where(pf => pf.RowIndex == firstFreePlayingFieldInRow.RowIndex
                                                              && pf.ColumnIndex > firstFreePlayingFieldInRow.ColumnIndex - winLineLength
                                                              && pf.ColumnIndex < firstFreePlayingFieldInRow.ColumnIndex + winLineLength
                                                              && pf.FieldColor == CurrentPlayer.PlayerColor
            ).OrderBy(pf => pf.ColumnIndex).ToList();

            int currentLineLength = 1;
            for (int i = 1; i < horizontalLine.Count; i++)
            {
                if (horizontalLine[i].ColumnIndex == horizontalLine[i - 1].ColumnIndex + 1)
                    currentLineLength++;
                else
                    currentLineLength = 1;
                if (currentLineLength == winLineLength)
                    return true;
            }

            var verticalLine = ListOfPlayingField.Where(pf => pf.ColumnIndex == firstFreePlayingFieldInRow.ColumnIndex
                                                              && pf.RowIndex > firstFreePlayingFieldInRow.RowIndex - winLineLength
                                                              && pf.RowIndex < firstFreePlayingFieldInRow.RowIndex + winLineLength
                                                              && pf.FieldColor == CurrentPlayer.PlayerColor
            ).OrderBy(pf => pf.RowIndex).ToList();

            currentLineLength = 1;
            for (int i = 1; i < verticalLine.Count; i++)
            {
                if (verticalLine[i].RowIndex == verticalLine[i - 1].RowIndex + 1)
                    currentLineLength++;
                else
                    currentLineLength = 1;
                if (currentLineLength == winLineLength)
                    return true;
            }

            List<PlayingField> firstDiagonalLine = new List<PlayingField>();

            for (int row = firstFreePlayingFieldInRow.RowIndex - winLineLength + 1, column = firstFreePlayingFieldInRow.ColumnIndex - winLineLength + 1; row < firstFreePlayingFieldInRow.RowIndex + winLineLength; row++, column++)
            {
                if (row < 0 || row >= RowCount || column < 0 || column >= ColumnCount)
                    continue;
                PlayingField? playerField = ListOfPlayingField.FirstOrDefault(pf => pf.RowIndex == row && pf.ColumnIndex == column);
                if (playerField != null && playerField.FieldColor == CurrentPlayer.PlayerColor)
                    firstDiagonalLine.Add(playerField);
            }
            firstDiagonalLine = firstDiagonalLine.OrderBy(pf => pf.RowIndex).ToList();

            currentLineLength = 1;
            for (int i = 1; i < firstDiagonalLine.Count; i++)
            {
                if (firstDiagonalLine[i].RowIndex == firstDiagonalLine[i - 1].RowIndex + 1)
                    currentLineLength++;
                else
                    currentLineLength = 1;
                if (currentLineLength == winLineLength)
                    return true;
            }

            List<PlayingField> secondDiagonalLine = new List<PlayingField>();

            for (int row = firstFreePlayingFieldInRow.RowIndex - winLineLength + 1, column = firstFreePlayingFieldInRow.ColumnIndex + winLineLength - 1; row < firstFreePlayingFieldInRow.RowIndex + winLineLength; row++, column--)
            {
                if (row < 0
                    || row >= RowCount
                    || column < 0
                    || column >= ColumnCount)
                    continue;
                PlayingField? playerField = ListOfPlayingField.FirstOrDefault(pf => pf.RowIndex == row && pf.ColumnIndex == column);
                if (playerField != null && playerField.FieldColor == CurrentPlayer.PlayerColor)
                    secondDiagonalLine.Add(playerField);
            }

            secondDiagonalLine = secondDiagonalLine.OrderBy(pf => pf.RowIndex).ToList();

            currentLineLength = 1;
            for (int i = 1; i < secondDiagonalLine.Count; i++)
            {
                if (secondDiagonalLine[i].RowIndex == secondDiagonalLine[i - 1].RowIndex + 1)
                    currentLineLength++;
                else
                    currentLineLength = 1;
                if (currentLineLength == winLineLength)
                    return true;
            }

            return false;
        }
    }
}