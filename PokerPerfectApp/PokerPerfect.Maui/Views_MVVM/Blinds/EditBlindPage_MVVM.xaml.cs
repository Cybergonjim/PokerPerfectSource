using PokerPerfect.Maui.ViewModels.Blinds;

namespace PokerPerfect.Maui.Views_MVVM.Blinds;

[QueryProperty(nameof(BlindId), "BlindId")]

public partial class EditBlindPage_MVVM : ContentPage
{
  private readonly BlindViewModel blindViewModel;

  public EditBlindPage_MVVM(BlindViewModel blindViewModel)
  {
    InitializeComponent();

    BindingContext = this.blindViewModel = blindViewModel;
  }

  public string BlindId
  {
    set
    {
      if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out int blindId))
        LoadBlind(blindId);
    }
  }

  private async void LoadBlind(int blindId)
  {
    await blindViewModel.LoadBlind(blindId);
  }
}