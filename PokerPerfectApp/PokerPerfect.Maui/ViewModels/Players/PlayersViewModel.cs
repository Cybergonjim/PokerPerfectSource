using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokerPerfect.Maui.Views_MVVM.Players;
using System.Collections.ObjectModel;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Players;
using PokerPerfect.Maui.Views_MVVM.Contacts;
using PokerPerfect.UseCases.Interfaces.Games;

namespace PokerPerfect.Maui.ViewModels.Players
{
	public partial class PlayersViewModel : ObservableObject
	{
		private readonly IViewPlayersUseCase viewPlayersUseCase;
    private readonly IDeletePlayerUseCase deletePlayerUseCase;
    private readonly IRebuyPlayerUseCase rebuyPlayerUseCase;
		private readonly IEditPlayerUseCase editPlayerUseCase;
		private readonly IViewPlayerUseCase viewPlayerUseCase;
		private readonly IViewGameUseCase viewGameUseCase;
		private string filterText;

		public ObservableCollection<Player> Players { get; set; }

		public string FilterText
		{
			get => filterText;
			set => _ = LoadPlayers(filterText = value);
		}

		public PlayersViewModel(
				IViewPlayersUseCase viewPlayersUseCase,
				IDeletePlayerUseCase deletePlayerUseCase,
				IRebuyPlayerUseCase rebuyPlayerUseCase,
				IEditPlayerUseCase editPlayerUseCase,
				IViewPlayerUseCase viewPlayerUseCase,
				IViewGameUseCase viewGameUseCase
		)
		{
			Players = new ObservableCollection<Player>();

			this.viewPlayersUseCase = viewPlayersUseCase;
			this.deletePlayerUseCase = deletePlayerUseCase;
			this.rebuyPlayerUseCase = rebuyPlayerUseCase;
			this.editPlayerUseCase = editPlayerUseCase;
			this.viewPlayerUseCase = viewPlayerUseCase;
			this.viewGameUseCase = viewGameUseCase;
		}

		public async Task LoadPlayers(string filterText = null)
		{
			Players.Clear();

			// this works with search bar so added items must be part of search and GameId
			var players = await viewPlayersUseCase.ExecuteAsync(filterText);

			players.Sort((x, y) => x.Name.CompareTo(y.Name));

			if (players != null && players.Count > 0)
				foreach (var player in players)
					if (player.GameId == Helper.GameId)
						Players.Add(player);
		}

		[RelayCommand]
		public async Task RebuyPlayer(int playerId)
		{
			Player player = await viewPlayerUseCase.ExecuteAsync(playerId);
			Game game = await viewGameUseCase.ExecuteAsync(Helper.GameId);

			if (player.Rebuys > 0)
			{
				player.Amount += game.RebuyChips;
				player.Rebuys -= 1;

				await editPlayerUseCase.ExecuteAsync(playerId, player);

				await LoadPlayers();
			}
		}

		[RelayCommand]
		public async Task DeletePlayer(int playerId)
		{
			await deletePlayerUseCase.ExecuteAsync(playerId);

			await LoadPlayers();
		}

		[RelayCommand]
		public async Task GotoEditPlayer(int playerId)
		{
			await Shell.Current.GoToAsync(nameof(EditPlayerPage_MVVM));
		}

		[RelayCommand]
		public async Task GotoAddContacts()
		{
			await Shell.Current.GoToAsync(nameof(Contacts_Page_MVVM));
		}

		[RelayCommand]
		public async Task GoBack()
		{
			await Shell.Current.GoToAsync("..");
		}
	}
}
