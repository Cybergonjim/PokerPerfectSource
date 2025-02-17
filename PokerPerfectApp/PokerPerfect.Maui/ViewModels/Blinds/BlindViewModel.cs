using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokerPerfect.UseCases.Interfaces.Blinds;
using PokerPerfect.CoreBusiness;

namespace PokerPerfect.Maui.ViewModels.Blinds
{
	public partial class BlindViewModel : ObservableObject
	{
		private Blind blind;
		private readonly IViewBlindUseCase viewBlindUseCase;
		private readonly IEditBlindUseCase editBlindUseCase;
		private readonly IAddBlindUseCase addBlindUseCase;

		public Blind Blind
		{
			get => blind;
			set => SetProperty(ref blind, value);
		}

		public bool IsNameProvided { get; set; }

		public BlindViewModel(
				IViewBlindUseCase viewBlindUseCase,
				IEditBlindUseCase editBlindUseCase,
				IAddBlindUseCase addBlindUseCase
			)
		{
			Blind = new Blind();
			this.viewBlindUseCase = viewBlindUseCase;
			this.editBlindUseCase = editBlindUseCase;
			this.addBlindUseCase = addBlindUseCase;
		}

		public async Task LoadBlind(int blindId)
		{
			Blind = await viewBlindUseCase.ExecuteAsync(blindId);
		}

		[RelayCommand]
		public async Task EditBlind()
		{
			await editBlindUseCase.ExecuteAsync(blind.BlindId, blind);
			await Shell.Current.GoToAsync("..");
		}

		[RelayCommand]
		public static async Task RebuyBlind()
		{
			await Task.Run(() =>
			{
			});
		}

		[RelayCommand]
		public async Task AddBlind()
		{
      await addBlindUseCase.ExecuteAsync(blind);
      await Shell.Current.GoToAsync("..");
		}

		[RelayCommand]
		public static async Task BackToBlinds()
		{
			await Shell.Current.GoToAsync("..");
		}
	}
}
