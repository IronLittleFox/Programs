using CommunityToolkit.Maui.Views;
using MinesweeperMauiGame.ViewModel;

namespace MinesweeperMauiGame.View;

public partial class MinesweeperPopupView : Popup
{
	public MinesweeperPopupView(MinesweeperPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}