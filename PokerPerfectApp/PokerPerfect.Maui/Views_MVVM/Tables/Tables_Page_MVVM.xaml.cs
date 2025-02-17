using PokerPerfect.Maui.ViewModels.Tables;

namespace PokerPerfect.Maui.Views_MVVM.Tables;

public partial class Tables_Page_MVVM : ContentPage
{
  private readonly TablesViewModel tablesViewModel;

  public Tables_Page_MVVM(TablesViewModel tablesViewModel)
  {
    InitializeComponent();

    BindingContext = this.tablesViewModel = tablesViewModel;
  }

  protected async override void OnAppearing()
  {
    base.OnAppearing();

	await tablesViewModel.LoadTables();
  }
}