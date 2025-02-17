using PokerPerfect.CoreBusiness;
using PokerPerfect.Maui.ViewModels.Chipsets;

namespace PokerPerfect.Maui.Views_MVVM.Chipsets;

public partial class Chipsets_Page_MVVM : ContentPage
{
  private readonly ChipsetsViewModel chipsetsViewModel;

  public Chipsets_Page_MVVM(ChipsetsViewModel chipsetsViewModel)
  {
    InitializeComponent();

    BindingContext = this.chipsetsViewModel = chipsetsViewModel;
  }

  protected override async void OnAppearing()
  {
    base.OnAppearing();

    await chipsetsViewModel.LoadChipsets();
  }

  private void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    IReadOnlyList<object> currentSelection = e.CurrentSelection;

    var current = currentSelection.FirstOrDefault();

    Helper.ChipsetId = (current as Chipset).ChipsetId;
  }
}