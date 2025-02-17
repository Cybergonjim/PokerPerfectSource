using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokerPerfect.Maui.Views_MVVM.Chips;
using PokerPerfect.UseCases.Interfaces.Chips;
using System.Collections.ObjectModel;
using PokerPerfect.CoreBusiness;
using PokerPerfect.Maui.Views_MVVM.Chipsets;

namespace PokerPerfect.Maui.ViewModels.Chips
{
	public partial class ChipsViewModel : ObservableObject
	{
		private readonly IViewChipsUseCase viewChipsUseCase;
		private readonly IDeleteChipUseCase deleteChipUseCase;
		public readonly IEditChipUseCase editChipUseCase;

		public ObservableCollection<Chip> Chips { get; set; }

		public ChipsViewModel(
				IViewChipsUseCase viewChipsUseCase,
				IDeleteChipUseCase deleteChipUseCase,
				IEditChipUseCase editChipUseCase
				)
		{
			Chips = new ObservableCollection<Chip>();

			this.viewChipsUseCase = viewChipsUseCase;
			this.deleteChipUseCase = deleteChipUseCase;
			this.editChipUseCase = editChipUseCase;
		}

		public async Task LoadChips(string filterText = null)
		{
			Chips.Clear();

			var chips = await viewChipsUseCase.ExecuteAsync(filterText);

			if (chips != null && chips.Count > 0)
				foreach (var chip in chips)
					Chips.Add(chip);
		}

		[RelayCommand]
		public async Task DeleteChip(int chipId)
		{
			await deleteChipUseCase.ExecuteAsync(chipId);

			await LoadChips();
		}

		[RelayCommand]
		public async Task GotoEditChip(int chipId)
		{
			await Shell.Current.GoToAsync($"{nameof(EditChipPage_MVVM)}?ChipId={chipId}");
		}

		[RelayCommand]
		public async Task GotoAddChip()
		{
			await Shell.Current.GoToAsync(nameof(AddChipPage_MVVM));
		}

		[RelayCommand]
		public async Task GoBack()
		{
			await Shell.Current.GoToAsync("..");
		}
	}
}
