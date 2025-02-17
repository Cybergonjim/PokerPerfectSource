using PokerPerfect.Maui.ViewModels.Chipsets;

namespace PokerPerfect.Maui.Views_MVVM.Chipsets;

public partial class AddChipsetPage_MVVM : ContentPage
{
  private readonly ChipsetViewModel chipsetViewModel;

  public AddChipsetPage_MVVM(ChipsetViewModel chipsetViewModel)
  {
    InitializeComponent();

    BindingContext = this.chipsetViewModel = chipsetViewModel;
  }

  protected override void OnAppearing()
  {
    base.OnAppearing();

    chipsetViewModel.Chipset = new CoreBusiness.Chipset();
  }
}