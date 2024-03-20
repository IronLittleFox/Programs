using CommunityToolkit.Maui.Views;
using SlidingPuzzleMauiGame.ViewModel;

namespace SlidingPuzzleMauiGame.View;

public partial class SlidingPuzzlePopupView : Popup
{
    public SlidingPuzzlePopupView(SlidingPuzzlePopupViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }
}