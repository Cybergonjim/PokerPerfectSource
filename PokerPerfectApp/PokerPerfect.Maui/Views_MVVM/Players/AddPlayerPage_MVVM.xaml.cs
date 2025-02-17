using PokerPerfect.Maui.ViewModels.Players;

namespace PokerPerfect.Maui.Views_MVVM.Players;

public partial class AddPlayerPage_MVVM : ContentPage
{
  private readonly PlayerViewModel playerViewModel;

  public AddPlayerPage_MVVM(PlayerViewModel playerViewModel)
  {
    InitializeComponent();

    BindingContext = this.playerViewModel = playerViewModel;
  }

  protected override void OnAppearing()
  {
    base.OnAppearing();

    playerViewModel.Player = new CoreBusiness.Player
    {
      GameId = Helper.GameId
    };
  }
}