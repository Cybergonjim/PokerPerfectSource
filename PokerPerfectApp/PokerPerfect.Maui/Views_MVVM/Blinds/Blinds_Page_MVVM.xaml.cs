using PokerPerfect.Maui.ViewModels.Blinds;

namespace PokerPerfect.Maui.Views_MVVM.Blinds;

public partial class Blinds_Page_MVVM : ContentPage
{
  private readonly BlindsViewModel blindsViewModel;

  public Blinds_Page_MVVM(BlindsViewModel blindsViewModel)
  {
    InitializeComponent();

    BindingContext = this.blindsViewModel = blindsViewModel;
  }

  protected async override void OnAppearing()
  {
    base.OnAppearing();

	await blindsViewModel.LoadBlinds();
  }
}