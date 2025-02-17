using PokerPerfect.Maui.ViewModels.Chipsets;

namespace PokerPerfect.Maui.Views_MVVM.Chipsets;

[QueryProperty(nameof(ChipsetId), "ChipsetId")]

public partial class EditChipsetPage_MVVM : ContentPage
{
  private readonly ChipsetViewModel chipsetViewModel;

  public EditChipsetPage_MVVM(ChipsetViewModel chipsetViewModel)
  {
    InitializeComponent();

    BindingContext = this.chipsetViewModel = chipsetViewModel;
  }

  public string ChipsetId
  {
    set
    {
      if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out int chipsetId))
        LoadChipset(chipsetId);
    }
  }

  private async void LoadChipset(int chipsetId)
  {
    await chipsetViewModel.LoadChipset(chipsetId);
  }
}