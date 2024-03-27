namespace Create2048MauiGame.View;

using Create2048MauiGame.Enums;
using Create2048MauiGame.ViewModel;
using UtilsMaui.Interfaces;

public partial class Create2048View : ContentView, IDisposableGameView
{
    private double deltaX = 0;
    private double deltaY = 0;

    public Create2048View(Create2048ViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
        Label l = new Label();
        //l.Padding
    }

    public void Dispose()
    {
        IGameViewModel? gameViewModel = BindingContext as IGameViewModel;
        if (gameViewModel != null)
        {
            gameViewModel.Dispose();
        }
    }

    private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
    { 
        if (BindingContext is Create2048ViewModel vm)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                case GestureStatus.Canceled:
                    deltaX = 0;
                    deltaY = 0;
                    break;
                case GestureStatus.Running:
                    deltaX = e.TotalX;
                    deltaY = e.TotalY;
                    break;
                case GestureStatus.Completed:
                    Move move;
                    if (Math.Abs(deltaX) > Math.Abs(deltaY))
                        if (deltaX > 0)
                            move = Move.Right;
                        else
                            move = Move.Left;
                    else if (deltaY > 0)
                        move = Move.Down;
                    else
                        move = Move.Up;

                    vm.MovmentCommand.Execute(move);
                    deltaX = 0;
                    deltaY = 0;
                    break;
            }
        }
    }
}