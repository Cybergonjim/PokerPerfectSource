using PokerPerfect.CoreBusiness;
using PokerPerfect.Maui.ViewModels.Games;

namespace PokerPerfect.Maui.Views_MVVM.Games;

public partial class Games_Page_MVVM : ContentPage
{
  private readonly GamesViewModel gamesViewModel;

  public Games_Page_MVVM(GamesViewModel gamesViewModel)
  {
    InitializeComponent();

    BindingContext = this.gamesViewModel = gamesViewModel;
  }

  private void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    IReadOnlyList<object> currentSelection = e.CurrentSelection;

    var current = currentSelection.FirstOrDefault();

    Helper.GameId = (current as Game).GameId;

    //if (sender is CollectionView cv && cv.SelectedItem is Game selectedGame)
    //  ((GamesViewModel)BindingContext).GameId = selectedGame.GameId;
  }

  protected override async void OnAppearing()
  {
    base.OnAppearing();

    await gamesViewModel.LoadGames();
  }
}