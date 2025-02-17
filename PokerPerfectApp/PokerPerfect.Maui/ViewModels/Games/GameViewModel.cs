using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Games;
using PokerPerfect.Maui.Views_MVVM.Players;
using PokerPerfect.Maui.Views_MVVM.Tables;
using PokerPerfect.Maui.Views_MVVM.Payouts;
using PokerPerfect.Maui.Views_MVVM.Blinds;

namespace PokerPerfect.Maui.ViewModels.Games
{
	public class ChipsetName
	{
		public string Name { get; set; }
	}

	public partial class GameViewModel : ObservableObject
	{
		public Game game;
		private readonly IViewGameUseCase viewGameUseCase;
		private readonly IEditGameUseCase editGameUseCase;
    private readonly IAddGameUseCase addGameUseCase;

    public Game Game
		{
			get => game;
			set => SetProperty(ref game, value);
		}

		public bool IsNameProvided { get; set; }

    public GameViewModel(
				IViewGameUseCase viewGameUseCase,
				IEditGameUseCase editGameUseCase,
        IAddGameUseCase addGameUseCase
        )
    {
			Game = new();

			this.viewGameUseCase = viewGameUseCase;
			this.editGameUseCase = editGameUseCase;
      this.addGameUseCase = addGameUseCase;
    }

    public async Task LoadGame(int gameId)
		{
			Game = await viewGameUseCase.ExecuteAsync(gameId);
		}

		[RelayCommand]
		public async Task EditGame()
		{
			await editGameUseCase.ExecuteAsync(game.GameId, game);
			await Shell.Current.GoToAsync("..");
		}

		[RelayCommand]
		public async Task AddGame()
		{
      await addGameUseCase.ExecuteAsync(game);
      await Shell.Current.GoToAsync("..");
		}

		[RelayCommand]
		public async Task BackToGames()
		{
			await Shell.Current.GoToAsync("..");
		}

		[RelayCommand]
		public async Task EditPlayers()
		{
      await Shell.Current.GoToAsync(nameof(Players_Page_MVVM));
    }

    [RelayCommand]
		public async Task EditTables()
		{
			await Shell.Current.GoToAsync(nameof(Tables_Page_MVVM));
		}

		[RelayCommand]
		public async Task EditPayouts()
		{
			await Shell.Current.GoToAsync(nameof(Payouts_Page_MVVM));
		}

		[RelayCommand]
		public async Task EditBlinds()
		{
			await Shell.Current.GoToAsync(nameof(Blinds_Page_MVVM));
		}
	}
}
