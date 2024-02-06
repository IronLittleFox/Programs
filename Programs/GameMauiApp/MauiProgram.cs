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
                .Services.AddTransientPopup< TicTacToePopupPage, TicTacToePopupViewModel>()
                .AddTransientPopup<MinesweeperPopupView, MinesweeperPopupViewModel>()
                .AddTransientPopup<ConnectFourPopupView, ConnectFourPopupViewModel>()
                .AddTransient<GamePage>()
                .AddTransient<GameViewModel>()
                .AddTransient<TicTacToeViewModel>()
                .AddTransient<TicTacToeView>()
                .AddTransient<MinesweeperView>()
                .AddTransient<MinesweeperViewModel>()
                .AddTransient<MemoryView>()
                .AddTransient<MemoryViewModel>()
                .AddTransient<ConnectFourView>()
                .AddTransient<ConnectFourViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            //return builder.Build();
            var app = builder.Build();

            //we must initialize our service helper before using it
            ServiceHelper.Initialize(app.Services);

            return app;
        }
    }
}
