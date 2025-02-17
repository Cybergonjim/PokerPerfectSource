using PokerPerfect.Maui.ViewModels.Payouts;

namespace PokerPerfect.Maui.Views_MVVM.Payouts;

public partial class AddPayoutPage_MVVM : ContentPage
{
  private readonly PayoutViewModel payoutViewModel;

  public AddPayoutPage_MVVM(PayoutViewModel payoutViewModel)
  {
    InitializeComponent();

    BindingContext = this.payoutViewModel = payoutViewModel;
  }

  protected override void OnAppearing()
  {
    base.OnAppearing();

    payoutViewModel.Payout = new CoreBusiness.Payout()
    {
      GameId = Helper.GameId
    };
  }
}