using PokerPerfect.Maui.ViewModels.Blinds;

namespace PokerPerfect.Maui.Views_MVVM.Blinds;

public partial class AddBlindPage_MVVM : ContentPage
{
  private readonly BlindViewModel blindViewModel;

  public AddBlindPage_MVVM(BlindViewModel blindViewModel)
  {
    InitializeComponent();

    BindingContext = this.blindViewModel = blindViewModel;
  }

  protected override void OnAppearing()
  {
    base.OnAppearing();

    blindViewModel.Blind = new CoreBusiness.Blind
    {
      GameId = Helper.GameId
    };
  }
}