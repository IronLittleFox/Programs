﻿using AchiMauiGame.View;
using CalculatorMauiGame.View;
using CheckersMauiGame.View;
using ChessMauiGame.View;
using ConnectFourMauiGame.View;
using Create2048MauiGame.View;
using GameMauiApp.Model;
using GoMauiGame.View;
using ImportantDatesMauiGame.View;
using MemoryMauiGame.View;
using Microsoft.Maui.Controls;
using MinesweeperMauiGame.View;
using SlidingPuzzleMauiGame.View;
using SudokuMauiGame.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeMauiGame.View;
using UtilsMaui.Interfaces;

namespace GameMauiApp.ViewModel
{
    public class GameViewModel : BindableObject, IGameViewModel
    {
        public ObservableCollection<GameInfo> ListOfGame { get; set; }

        private GameInfo selectedGame;
        public GameInfo SelectedGame
        {
            get { return selectedGame; }
            set
            {
                selectedGame = value;
                SelectedGameView?.Dispose();
                SelectedGameView = ServiceHelper.Services.GetService(selectedGame.GameType) as IDisposableGameView;
                OnPropertyChanged();
            }
        }

        private IDisposableGameView selectedGameView;
        public IDisposableGameView SelectedGameView
        {
            get { return selectedGameView; }
            set
            {
                selectedGameView = value;
                OnPropertyChanged();
            }
        }

        public GameViewModel()
        {
            ListOfGame = new ObservableCollection<GameInfo>();

            ListOfGame.Add(new GameInfo() { NameOfGame = "Ważne daty", GameType = typeof(ImportantDatesView) });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Kalkulator", GameType = typeof(MainCalculatorView) });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Kółko i krzyżyk", GameType = typeof(TicTacToeView) });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Saper", GameType = typeof(MinesweeperView) });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Memory", GameType = typeof(MemoryView) });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Połącz czwórki", GameType = typeof(ConnectFourView) });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Warcaby", GameType = typeof(CheckersView) });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Sudoku", GameType=typeof(SudokuView) });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Go", GameType = typeof(GoView) });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Szachy", GameType = typeof(ChessView) });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Przesuwane puzle", GameType = typeof(SlidingPuzzleView) });
            ListOfGame.Add(new GameInfo() { NameOfGame = "2048", GameType = typeof(Create2048View) });
            ListOfGame.Add(new GameInfo() { NameOfGame = "Achi", GameType = typeof(AchiView) });

            SelectedGame = ListOfGame.FirstOrDefault();
            //SelectedGame = ListOfGame.LastOrDefault();
        }

        public void Dispose()
        {
            SelectedGameView?.Dispose();
        }
    }
}
