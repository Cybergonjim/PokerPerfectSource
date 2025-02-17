using PokerPerfect.Maui.ViewModels.Games;

namespace PokerPerfect.Maui.Views_MVVM.Games;

[QueryProperty(nameof(GameId), "GameId")]

public partial class EditGamePage_MVVM : ContentPage
{
  private readonly GameViewModel gameViewModel;

  public EditGamePage_MVVM(GameViewModel gameViewModel)
  {
    InitializeComponent();

		BindingContext = this.gameViewModel = gameViewModel;
  }

  public string GameId
  {
    set
    {
      if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out int gameId))
      {
        LoadGame(gameId);
				Helper.GameId = gameId;
			};
    }
  }

  private async void LoadGame(int gameId)
  {
    await gameViewModel.LoadGame(gameId);
  }
}