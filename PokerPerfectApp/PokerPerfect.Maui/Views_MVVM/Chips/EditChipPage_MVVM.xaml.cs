using PokerPerfect.Maui.ViewModels.Chips;

namespace PokerPerfect.Maui.Views_MVVM.Chips;

[QueryProperty(nameof(ChipId), "ChipId")]

public partial class EditChipPage_MVVM : ContentPage
{
  private readonly ChipViewModel chipViewModel;

  public EditChipPage_MVVM(ChipViewModel chipViewModel)
  {
    InitializeComponent();

    BindingContext = this.chipViewModel = chipViewModel;
  }

  public string ChipId
  {
    set
    {
      if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out int chipId))
        LoadChip(chipId);
    }
  }

  private async void LoadChip(int chipId)
  {
    await chipViewModel.LoadChip(chipId);
  }
}