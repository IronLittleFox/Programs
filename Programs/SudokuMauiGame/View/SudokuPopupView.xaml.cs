using CommunityToolkit.Maui.Views;
using SudokuMauiGame.ViewModel;

namespace SudokuMauiGame.View;

public partial class SudokuPopupView : Popup
{
	public SudokuPopupView(SudokuPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}