using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GameMauiApp.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            UnhandledException += (sender, error) =>
            {
                try
                {
                    string filePath = "errors.txt";
                    using (StreamWriter writer = File.AppendText(filePath))
                    {
                        writer.WriteLine("Wykryto nowy błąd o godzinie " + DateTime.Now);
                        writer.WriteLine(error.Message);
                        writer.WriteLine(error.Exception.StackTrace);
                        writer.WriteLine(Environment.NewLine);
                    }

                }
                catch (Exception ex)
                {
                    
                }
            };
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }

}
