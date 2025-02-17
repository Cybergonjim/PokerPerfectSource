using PokerPerfect.Maui.ViewModels.Players;

namespace PokerPerfect.Maui.Views_MVVM.Players;

[QueryProperty(nameof(PlayerId), "PlayerId")]

public partial class EditPlayerPage_MVVM : ContentPage
{
  private readonly PlayerViewModel playerViewModel;

  public EditPlayerPage_MVVM(PlayerViewModel playerViewModel)
  {
    InitializeComponent();

    BindingContext = this.playerViewModel = playerViewModel;
  }

  public string PlayerId
  {
    set
    {
      if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out int playerId))
        LoadPlayer(playerId);
    }
  }

  private async void LoadPlayer(int playerId)
  {
    await playerViewModel.LoadPlayer(playerId);
  }
}