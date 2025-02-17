using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokerPerfect.Maui.Views_MVVM.Games;
using System.Collections.ObjectModel;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Games;
using PokerPerfect.UseCases.Interfaces.Chipsets;
using PokerPerfect.UseCases.Interfaces.Blinds;
using PokerPerfect.UseCases.Interfaces.Payouts;
using PokerPerfect.UseCases.Interfaces.Players;
using PokerPerfect.UseCases.Interfaces.Tables;

namespace PokerPerfect.Maui.ViewModels.Games
{
  public partial class GamesViewModel : ObservableObject
  {
    private readonly IViewGamesUseCase viewGamesUseCase;
    private readonly IDeleteGameUseCase deleteGameUseCase;
    private readonly IViewChipsetsUseCase viewChipsetsUseCase;
    private readonly IAddGameUseCase addGameUseCase;
    private readonly IViewBlindsUseCase viewBlindsUseCase;
    private readonly IAddBlindUseCase addBlindUseCase;
    private readonly IViewPayoutsUseCase viewPayoutsUseCase;
    private readonly IAddPayoutUseCase addPayoutUseCase;
    private readonly IDeleteBlindUseCase deleteBlindUseCase;
    private readonly IDeletePlayerUseCase deletePlayerUseCase;
    private readonly IDeleteTableUseCase deleteTableUseCase;
    private readonly IDeletePayoutUseCase deletePayoutUseCase;
    private readonly IViewPlayersUseCase viewPlayersUseCase;
    private readonly IViewTablesUseCase viewTablesUseCase;

    public ObservableCollection<Game> Games { get; set; }

    public GamesViewModel(
        IViewGamesUseCase viewGamesUseCase,
        IDeleteGameUseCase deleteGameUseCase,
        IViewChipsetsUseCase viewChipsetsUseCase,
        IAddGameUseCase addGameUseCase,
        IViewBlindsUseCase viewBlindsUseCase,
        IAddBlindUseCase addBlindUseCase,
        IViewPayoutsUseCase viewPayoutsUseCase,
        IAddPayoutUseCase addPayoutUseCase,
        IDeleteBlindUseCase deleteBlindUseCase,
        IDeletePlayerUseCase deletePlayerUseCase,
        IDeletePayoutUseCase deletePayoutUseCase,
        IDeleteTableUseCase deleteTableUseCase,
        IViewPlayersUseCase viewPlayersUseCase,
        IViewTablesUseCase viewTablesUseCase
        )
    {
      this.viewGamesUseCase = viewGamesUseCase;
      this.deleteGameUseCase = deleteGameUseCase;
      this.viewChipsetsUseCase = viewChipsetsUseCase;
      this.addGameUseCase = addGameUseCase;
      this.viewBlindsUseCase = viewBlindsUseCase;
      this.addBlindUseCase = addBlindUseCase;
      this.viewPayoutsUseCase = viewPayoutsUseCase;
      this.addPayoutUseCase = addPayoutUseCase;
      this.deleteBlindUseCase = deleteBlindUseCase;
      this.deletePlayerUseCase = deletePlayerUseCase;
      this.deletePayoutUseCase = deletePayoutUseCase;
      this.deleteTableUseCase = deleteTableUseCase;
      this.viewPlayersUseCase = viewPlayersUseCase;
      this.viewTablesUseCase = viewTablesUseCase;

    Games = new ObservableCollection<Game>();
    }

    public async Task LoadGames()
    { 
      Games.Clear();

      var games = await viewGamesUseCase.ExecuteAsync(null);

      games.Sort((x, y) => y.GameId.CompareTo(x.GameId));

      if (games != null && games.Count > 0)
        foreach (var game in games)
          Games.Add(game);

      Helper.Names.Clear();

      var chipsets = await viewChipsetsUseCase.ExecuteAsync(null);

      if (chipsets != null && chipsets.Count > 0)
        foreach (var chipset in chipsets)
          Helper.Names.Add(chipset.Description + " " + chipset.Denominations);
    }

    [RelayCommand]
    public async Task DeleteGame(int gameId)
    {
      await deleteGameUseCase.ExecuteAsync(gameId);

      var blinds = await viewBlindsUseCase.ExecuteAsync(gameId.ToString());
      await Helper.DeleteEntitiesAsync(blinds, async (Blind blind) => await deleteBlindUseCase.ExecuteAsync(blind.BlindId));

      var players = await viewPlayersUseCase.ExecuteAsync(gameId.ToString());
      await Helper.DeleteEntitiesAsync(players, async (Player player) => await deletePlayerUseCase.ExecuteAsync(player.PlayerId));

      var payouts = await viewPayoutsUseCase.ExecuteAsync(gameId.ToString());
      await Helper.DeleteEntitiesAsync(payouts, async (Payout payout) => await deletePayoutUseCase.ExecuteAsync(payout.PayoutId));

      var tables = await viewTablesUseCase.ExecuteAsync(gameId.ToString());
      await Helper.DeleteEntitiesAsync(tables, async (Table table) => await deleteTableUseCase.ExecuteAsync(table.TableId));

      await LoadGames();
    }

    [RelayCommand]
    public async Task GotoEditGame(int gameId)
    {
      await Shell.Current.GoToAsync($"{nameof(EditGamePage_MVVM)}?GameId={gameId}");
    }

    public async Task CopyBlinds(int newGameId)
    {
      var gameId = Helper.GameId.ToString();

      // load blinds with current GameId
      var blinds = await viewBlindsUseCase.ExecuteAsync(gameId);

      if (blinds != null && blinds?.Count > 0)  
        foreach (var blind in blinds)
        {
          Blind blind_ = new();

          Helper.MapProperties(blind, blind_);

          blind_.GameId = newGameId;

          await addBlindUseCase.ExecuteAsync(blind_);
        }
    }

    public async Task CopyPayouts(int newGameId)
    {
      var gameId = Helper.GameId.ToString();

      // load blinds with current GameId
      var payouts = await viewPayoutsUseCase.ExecuteAsync(gameId);

      if (payouts != null && payouts?.Count > 0)
        foreach (var payout in payouts)
        {
          Payout payout_ = new();

          Helper.MapProperties(payout, payout_);

          payout_.GameId = newGameId;

          await addPayoutUseCase.ExecuteAsync(payout_);
        }
    }

    [RelayCommand]
    public async Task GotoCopyGame()
    {
      int currentGameIndex = Games
          .Select((g, index) => new { Game = g, Index = index })
          .FirstOrDefault(item => item.Game.GameId == Helper.GameId)?.Index ?? -1;

      if (currentGameIndex != -1 && currentGameIndex < Games.Count)
      {
        Game game = new();

        // Ensure Helper.MapProperties correctly maps properties from source to target
        Helper.MapProperties(Games[currentGameIndex], game);

        // Add new game
        await addGameUseCase.ExecuteAsync(game);

        // newly added game should be first in list
        await LoadGames();

        await CopyBlinds(Games[0].GameId);

        await CopyPayouts(Games[0].GameId);
      }

    }

    [RelayCommand]
    public async Task GotoAddGame()
    {
      await Shell.Current.GoToAsync(nameof(AddGamePage_MVVM));
    }

    [RelayCommand]
    public static async Task GotoHome()
    {
      await Shell.Current.GoToAsync("..");
    }
  }
}