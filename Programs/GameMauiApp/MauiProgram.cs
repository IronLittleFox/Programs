using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using MinesweeperMauiGame.View;
using TicTacToeMauiGame.View;
using TicTacToeMauiGame.ViewModel;
using MinesweeperMauiGame.ViewModel;
using MemoryMauiGame.View;
using MemoryMauiGame.ViewModel;
using GameMauiApp.Pages;
using GameMauiApp.ViewModel;
using ConnectFourMauiGame.View;
using ConnectFourMauiGame.ViewModel;
using CheckersMauiGame.View;
using CheckersMauiGame.ViewModel;
using SudokuMauiGame.View;
using SudokuMauiGame.ViewModel;
using GoMauiGame.View;
using GoMauiGame.ViewModel;
using ImportantDatesMauiGame.View;
using ImportantDatesMauiGame.ViewModel;
using CalculatorMauiGame.View;
using CalculatorMauiGame.ViewModel;
using ChessMauiGame.View;
using ChessMauiGame.ViewModel;
using Microsoft.Maui.LifecycleEvents;
using SlidingPuzzleMauiGame.View;
using SlidingPuzzleMauiGame.ViewModel;
using Create2048MauiGame.ViewModel;
using Create2048MauiGame.View;

namespace GameMauiApp
{
    public static class ServiceHelper
    {
        public static IServiceProvider Services { get; private set; }

        public static void Initialize(IServiceProvider serviceProvider) =>
            Services = serviceProvider;

        public static T GetService<T>() => Services.GetService<T>();
    }

    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .Services.AddTransientPopup<TicTacToePopupPage, TicTacToePopupViewModel>()
                .AddTransientPopup<MinesweeperPopupView, MinesweeperPopupViewModel>()
                .AddTransientPopup<ConnectFourPopupView, ConnectFourPopupViewModel>()
                .AddTransientPopup<CheckersPopupView, CheckersPopupViewModel>()
                .AddTransientPopup<SudokuPopupView, SudokuPopupViewModel>()
                .AddTransientPopup<ChessPawnPomotionPopupView, ChessPawnPomotionPopupViewModel>()
                .AddTransientPopup<ChessMessagePopupView, ChessMessagePopupViewModel>()
                .AddTransientPopup<SlidingPuzzlePopupView, SlidingPuzzlePopupViewModel>()
                .AddTransient<GamePage>()
                .AddTransient<GameViewModel>()
                .AddTransient<TicTacToeViewModel>()
                .AddTransient<TicTacToeView>()
                .AddTransient<MinesweeperView>()
                .AddTransient<MinesweeperViewModel>()
                .AddTransient<MemoryView>()
                .AddTransient<MemoryViewModel>()
                .AddTransient<ConnectFourView>()
                .AddTransient<ConnectFourViewModel>()
                .AddTransient<CheckersView>()
                .AddTransient<CheckersViewModel>()
                .AddTransient<SudokuView>()
                .AddTransient<SudokuViewModel>()
                .AddTransient<GoView>()
                .AddTransient<GoViewModel>()
                .AddTransient<ImportantDatesView>()
                .AddTransient<ImportantDatesViewModel>()
                .AddTransient<MainCalculatorView>()
                .AddTransient<MainCalculatorViewModel>()
                .AddTransient<ChessView>()
                .AddTransient<ChessViewModel>()
                .AddTransient<SlidingPuzzleView>()
                .AddTransient<SlidingPuzzleViewModel>()
                .AddTransient<Create2048View>()
                .AddTransient<Create2048ViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

#if WINDOWS
            builder.ConfigureLifecycleEvents(events =>
            {
                // Make sure to add "using Microsoft.Maui.LifecycleEvents;" in the top of the file 
                events.AddWindows(windowsLifecycleBuilder =>
                {
                    windowsLifecycleBuilder.OnWindowCreated(window =>
                    {
                        window.ExtendsContentIntoTitleBar = false;
                        var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                        switch (appWindow.Presenter)
                        {
                            case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                                overlappedPresenter.SetBorderAndTitleBar(false, false);
                                overlappedPresenter.Maximize();
                                break;
                        }
                    });
                });
            });
#endif

            //return builder.Build();
            var app = builder.Build();

            //we must initialize our service helper before using it
            ServiceHelper.Initialize(app.Services);


            return app;
        }
    }
}
