using PokerPerfect.Maui.ViewModels.Chips;
using PokerPerfect.CoreBusiness;

namespace PokerPerfect.Maui.Views_MVVM.Chips;

public partial class Chips_Page_MVVM : ContentPage
{
  private readonly ChipsViewModel chipsViewModel;
  private Chip chipToModify;
  private int colorToModify;
  private Button buttonToModify;

  public Chips_Page_MVVM(ChipsViewModel chipsViewModel)
  {
    InitializeComponent();

    BindingContext = this.chipsViewModel = chipsViewModel;
  }

  protected async override void OnAppearing()
  {
    base.OnAppearing();

	await chipsViewModel.LoadChips();
  }

  private void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    IReadOnlyList<object> currentSelection = e.CurrentSelection;

    var current = currentSelection.FirstOrDefault();

    chipToModify = current as Chip;
  }

  private void BaseColorClicked(object sender, EventArgs e)
  {
    buttonToModify = sender as Button;
    colorToModify = 1;
  }

  private void SpokeColorClicked(object sender, EventArgs e)
  {
    buttonToModify = sender as Button;
    colorToModify = 2;
  }

  private void DotColorClicked(object sender, EventArgs e)
  {
    buttonToModify = sender as Button;
    colorToModify = 3;
  }

  private async void ColorPickerChanged(object sender, Color e)
  {
    if ((chipToModify != null) && (buttonToModify != null))
    {
      if (colorToModify == 1)
        chipToModify.ColorBaseRGB = e;
      else if (colorToModify == 2)
        chipToModify.ColorSpokeRGB = e;
      else if (colorToModify == 3)
        chipToModify.ColorDotRGB = e;

      buttonToModify.BackgroundColor = e;
      colorToModify = 0;

      await chipsViewModel.editChipUseCase.ExecuteAsync(chipToModify.ChipId, chipToModify);
    }
  }
}