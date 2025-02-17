using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokerPerfect.Maui.Views_MVVM.Chips;
using PokerPerfect.UseCases.Interfaces.Chips;
using PokerPerfect.CoreBusiness;

namespace PokerPerfect.Maui.ViewModels.Chips
{
	public partial class ChipViewModel : ObservableObject
	{
		private Chip chip;
		private readonly IViewChipUseCase viewChipUseCase;
		private readonly IEditChipUseCase editChipUseCase;
		private readonly IAddChipUseCase addChipUseCase;

		public Chip Chip
		{
			get => chip;
			set => SetProperty(ref chip, value);
		}

		public bool IsNameProvided { get; set; }

		public ChipViewModel(
				IViewChipUseCase viewChipUseCase,
				IEditChipUseCase editChipUseCase,
				IAddChipUseCase addChipUseCase)
		{
			Chip = new Chip();
			this.viewChipUseCase = viewChipUseCase;
			this.editChipUseCase = editChipUseCase;
			this.addChipUseCase = addChipUseCase;
		}

		public async Task LoadChip(int chipId)
		{
			Chip = await viewChipUseCase.ExecuteAsync(chipId);
		}

		[RelayCommand]
		public async Task EditChip()
		{
			await editChipUseCase.ExecuteAsync(chip.ChipId, chip);
			await Shell.Current.GoToAsync("..");
		}

		[RelayCommand]
		public async Task AddChip()
		{
			await addChipUseCase.ExecuteAsync(chip);
			await Shell.Current.GoToAsync("..");
		}

		[RelayCommand]
		public async Task BackToChips()
		{
			await Shell.Current.GoToAsync("..");
		}
	}
}
