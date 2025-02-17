using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokerPerfect.Maui.Views_MVVM.Chipsets;
using PokerPerfect.UseCases.Interfaces.Chipsets;
using System.Collections.ObjectModel;
using PokerPerfect.CoreBusiness;
using PokerPerfect.Maui.Views_MVVM.Chips;
using PokerPerfect.UseCases.Interfaces.Chips;

namespace PokerPerfect.Maui.ViewModels.Chipsets
{
	public partial class ChipsetsViewModel : ObservableObject
	{
		private readonly IViewChipsetsUseCase viewChipsetsUseCase;
    private readonly IDeleteChipsetUseCase deleteChipsetUseCase;
    private readonly IViewChipsUseCase viewChipsUseCase;
		private readonly IEditChipsetUseCase editChipsetUseCase;
    private readonly IDeleteChipUseCase deleteChipUseCase;
    private readonly IAddChipsetUseCase addChipsetUseCase;
    private readonly IAddChipUseCase addChipUseCase;

    public ObservableCollection<Chipset> Chipsets { get; set; }

		public ChipsetsViewModel(
				IViewChipsetsUseCase viewChipsetsUseCase,
        IDeleteChipsetUseCase deleteChipsetUseCase,
        IViewChipsUseCase viewChipsUseCase,
				IEditChipsetUseCase editChipsetUseCase,
        IDeleteChipUseCase deleteChipUseCase,
        IAddChipsetUseCase addChipsetUseCase,
        IAddChipUseCase addChipUseCase
        )
    {
			Chipsets = new ObservableCollection<Chipset>();

			this.viewChipsetsUseCase = viewChipsetsUseCase;
      this.deleteChipsetUseCase = deleteChipsetUseCase;
      this.viewChipsUseCase = viewChipsUseCase;
      this.editChipsetUseCase = editChipsetUseCase;
      this.deleteChipUseCase = deleteChipUseCase;
      this.addChipsetUseCase = addChipsetUseCase;
      this.addChipUseCase = addChipUseCase;
    }

    public async Task LoadChipsets()
		{
			Chipsets.Clear();

			var chipsets = await viewChipsetsUseCase.ExecuteAsync(Helper.ChipsetId.ToString());

			if (chipsets != null && chipsets.Count > 0)
				foreach (var chipset in chipsets)
				{
					string denominations = "";

					var chips = await viewChipsUseCase.ExecuteAsync(chipset.ChipsetId.ToString());

					foreach (var chip in chips)
						if (chip.Denomination >= 1000000)
							denominations += (chip.Denomination / 1000000).ToString() + "M, ";
						else if (chip.Denomination >= 1000)
							denominations += (chip.Denomination / 1000).ToString() + "K, ";
						else
							denominations += chip.Denomination.ToString() + ", ";

					if (denominations.EndsWith(", "))
						denominations = denominations.Substring(0, denominations.Length - 2);

					chipset.Denominations = denominations;

					await editChipsetUseCase.ExecuteAsync(chipset.ChipsetId, chipset);

					Chipsets.Add(chipset);
				}
		}

		[RelayCommand]
		public async Task DeleteChipset(int chipsetId)
		{
			await deleteChipsetUseCase.ExecuteAsync(chipsetId);

      var chips = await viewChipsUseCase.ExecuteAsync(chipsetId.ToString());
      await Helper.DeleteEntitiesAsync(chips, async (Chip chip) => await deleteChipUseCase.ExecuteAsync(chip.ChipId));

      await LoadChipsets();
		}

		[RelayCommand]
		public async Task GotoEditChipset(int chipsetId)
		{
			await Shell.Current.GoToAsync($"{nameof(EditChipsetPage_MVVM)}?ChipsetId={chipsetId}");
		}

		[RelayCommand]
		public async Task GotoAddChipset()
		{
			await Shell.Current.GoToAsync(nameof(AddChipsetPage_MVVM));
		}

    public async Task CopyChips(int newChipsetId)
    {
      var chipsetId = Helper.ChipsetId.ToString();

      // load blinds with current GameId
      var chips = await viewChipsUseCase.ExecuteAsync(chipsetId);

      if (chips != null && chips?.Count > 0)
        foreach (var chip in chips)
        {
          Chip chip_ = new();

          Helper.MapProperties(chip, chip_);

          chip_.ChipsetId = newChipsetId;

          await addChipUseCase.ExecuteAsync(chip_);
        }
    }

    [RelayCommand]
    public async Task GotoCopyChipset()
    {
      int currentChipsetIndex = Chipsets
          .Select((g, index) => new { Chipset = g, Index = index })
          .FirstOrDefault(item => item.Chipset.ChipsetId == Helper.ChipsetId)?.Index ?? -1;

      if (currentChipsetIndex != -1 && currentChipsetIndex < Chipsets.Count)
      {
        Chipset chipset = new();

        // Ensure Helper.MapProperties correctly maps properties from source to target
        Helper.MapProperties(Chipsets[currentChipsetIndex], chipset);

        // Add new game
        await addChipsetUseCase.ExecuteAsync(chipset);

        // newly added game should be last in list
        await LoadChipsets();

        await CopyChips(Chipsets[^1].ChipsetId);

        await LoadChipsets();
      }

    }
    [RelayCommand]
		public async Task GotoHome()
		{
			await Shell.Current.GoToAsync("..");
		}

		[RelayCommand]
		public async Task GotoChips()
		{
			await Shell.Current.GoToAsync(nameof(Chips_Page_MVVM));
		}
	}
}
