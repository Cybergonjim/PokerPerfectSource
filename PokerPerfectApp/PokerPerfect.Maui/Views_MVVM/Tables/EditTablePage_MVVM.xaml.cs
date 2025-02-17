using PokerPerfect.Maui.ViewModels.Tables;

namespace PokerPerfect.Maui.Views_MVVM.Tables;

[QueryProperty(nameof(TableId), "TableId")]

public partial class EditTablePage_MVVM : ContentPage
{
  private readonly TableViewModel tableViewModel;

  public EditTablePage_MVVM(TableViewModel tableViewModel)
  {
    InitializeComponent();

    BindingContext = this.tableViewModel = tableViewModel;
  }

  public string TableId
  {
    set
    {
      if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out int tableId))
        LoadTable(tableId);
    }
  }

  private async void LoadTable(int tableId)
  {
    await tableViewModel.LoadTable(tableId);
  }
}