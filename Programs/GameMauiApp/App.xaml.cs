using GameMauiApp.Pages;

namespace GameMauiApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //MainPage = new GamePage();
            MainPage = ServiceHelper.Services.GetService(typeof(GamePage)) as Page;
            
        }
    }
}
