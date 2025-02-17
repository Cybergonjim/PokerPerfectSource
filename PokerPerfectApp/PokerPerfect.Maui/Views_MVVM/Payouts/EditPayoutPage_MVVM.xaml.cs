using PokerPerfect.Maui.ViewModels.Payouts;

namespace PokerPerfect.Maui.Views_MVVM.Payouts;

[QueryProperty(nameof(PayoutId), "PayoutId")]

public partial class EditPayoutPage_MVVM : ContentPage
{
  private readonly PayoutViewModel payoutViewModel;

  public EditPayoutPage_MVVM(PayoutViewModel payoutViewModel)
  {
    InitializeComponent();

    BindingContext = this.payoutViewModel = payoutViewModel;
  }

  public string PayoutId
  {
    set
    {
      if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out int payoutId))
        LoadPayout(payoutId);
    }
  }

  private async void LoadPayout(int payoutId)
  {
    await payoutViewModel.LoadPayout(payoutId);
  }
}