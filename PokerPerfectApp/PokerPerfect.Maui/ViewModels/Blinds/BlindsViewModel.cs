using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokerPerfect.Maui.Views_MVVM.Blinds;
using System.Collections.ObjectModel;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Blinds;
using PokerPerfect.UseCases.Interfaces.Games;
using PokerPerfect.UseCases.Interfaces.Chips;

namespace PokerPerfect.Maui.ViewModels.Blinds
{
  public partial class BlindsViewModel : ObservableObject
  {
    private readonly IViewBlindsUseCase viewBlindsUseCase;
    private readonly IDeleteBlindUseCase deleteBlindUseCase;
    private readonly IRebuyBlindUseCase rebuyBlindUseCase;
    private readonly IEditBlindUseCase editBlindUseCase;
    private readonly IViewGameUseCase viewGameUseCase;
    private readonly IViewChipsUseCase viewChipsUseCase;
    private readonly IAddBlindUseCase addBlindUseCase;

    public ObservableCollection<Blind> Blinds { get; set; }

    public BlindsViewModel(
        IViewBlindsUseCase viewBlindsUseCase,
        IDeleteBlindUseCase deleteBlindUseCase,
        IRebuyBlindUseCase rebuyBlindUseCase,
        IEditBlindUseCase editBlindUseCase,
        IViewGameUseCase viewGameUseCase,
        IViewChipsUseCase viewChipsUseCase,
        IAddBlindUseCase addBlindUseCase
        )
    {
      Blinds = new ObservableCollection<Blind>();

      this.viewBlindsUseCase = viewBlindsUseCase;
      this.deleteBlindUseCase = deleteBlindUseCase;
      this.rebuyBlindUseCase = rebuyBlindUseCase;
      this.editBlindUseCase = editBlindUseCase;
      this.viewGameUseCase = viewGameUseCase;
      this.viewChipsUseCase = viewChipsUseCase;
      this.addBlindUseCase = addBlindUseCase;
    }

    public async Task LoadBlinds()
    {
      Blinds.Clear();

      var gameId = Helper.GameId.ToString();
      var blinds = await viewBlindsUseCase.ExecuteAsync(gameId);

      if (blinds?.Count > 0)
        foreach (var blind in blinds)
          Blinds.Add(blind);
    }

    public int FindBestFitAmount(double targetAmount, List<Chip> chips)
    {
      // arrays to store calculated values and errors
      int[] possibleAmounts = new int[3];
      double[] errors = new double[3];

      int bestIndex = 0; // initialize to 0 to indicate to use first chip found
      possibleAmounts[0] = (int)(Math.Round(targetAmount / chips[0].Denomination) * chips[0].Denomination);

      // iterate through the list of chips
      for (int i = 0; i < chips.Count; i++)
      {
        int value = (int)Math.Round(targetAmount) / chips[i].Denomination;

        // check if the current chip can be used to form the target amount
        if (value >= 1)
        {
          // calculate amounts and errors for the current chip and two lower denominations
          for (int j = 0; j <= 2; j++)
          {
            if (i + j - 1 < chips.Count)
            {
              possibleAmounts[j] = (int)(Math.Round(targetAmount / chips[i + j - 1].Denomination) * chips[i + j - 1].Denomination);
              errors[j] = Math.Abs(possibleAmounts[j] - targetAmount) / targetAmount;
            }
            else // chips.Count exceeded
              errors[j] = double.MaxValue;
          }

          // find the index of the minimum error
          bestIndex = Array.IndexOf(errors, errors.Min());

          // break out of the loop since a suitable chip is found
          break;
        }
      }

      // return the best fit amount
      return possibleAmounts[bestIndex];
    }

    private async Task AddBlinds()
    {
      // most poker tournaments begin to end when 10 blinds are left
      const int blindsLeft = 10;
      const int extraLevels = 3;

      Game game = await viewGameUseCase.ExecuteAsync(Helper.GameId);

      int duration = game.DurationExp;
      int blindTime = game.BlindTime;

      // levelCount should be duration / blindtime 
      int levelCount = duration / blindTime;

      int total = (game.PlayersExp * game.ChipsStart + game.RebuyExp * game.RebuyChips + game.AddonExp * game.AddonChips) / blindsLeft;

      var chips = await viewChipsUseCase.ExecuteAsync((game.ChipSet).ToString());

      chips.Sort((x, y) => y.Denomination.CompareTo(x.Denomination));

      // find the item with a denomination less than or equal to small blind
      int targetDenomination = game.BlindStart / 2;
      
      int indexToKeep = chips.FindIndex(obj => obj.Denomination <= targetDenomination);

      // delete items in the list that are less than the found item
      chips.RemoveRange(indexToKeep + 1, chips.Count - indexToKeep - 1);

      // the blinds should increase exponentially where y = a * b^x
      // a is the BlindStart and b = (total / a)^(1/levelCount)
      double a = game.BlindStart;
      double b = Math.Pow(total / a, 1.0 / levelCount);

      // add extra blind levels in case tournament lasts longer
      for (int i = 0; i < levelCount + extraLevels; i++)
      {
        double target = Math.Round(a * Math.Pow(b, i), 0);

        int actual = FindBestFitAmount(target / 2, chips) * 2; // convert target for small blind and result back to big blind

        Blind blind = new()
        {
          GameId = Helper.GameId,
          BlindNo = i + 1,
          Amount = actual,
          Ante = (int)target
        };

        await addBlindUseCase.ExecuteAsync(blind);
      }
    }

    [RelayCommand]
    public async Task CreateBlinds()
    {
      Blinds.Clear();

      var blinds = await viewBlindsUseCase.ExecuteAsync(Helper.GameId.ToString());
      await Helper.DeleteEntitiesAsync(blinds, async (Blind blind) => await deleteBlindUseCase.ExecuteAsync(blind.BlindId));

      await AddBlinds();

      await LoadBlinds();
    }

    [RelayCommand]
    public async Task DeleteBlind(int blindId)
    {
      await deleteBlindUseCase.ExecuteAsync(blindId);

      await LoadBlinds();
    }

    [RelayCommand]
    public async Task GotoEditBlind(int blindId)
    {
      await Shell.Current.GoToAsync($"{nameof(EditBlindPage_MVVM)}?BlindId={blindId}");
    }

    [RelayCommand]
    public async Task GotoAddBlind()
    {
      await Shell.Current.GoToAsync(nameof(AddBlindPage_MVVM));
    }

    [RelayCommand]
    public async Task GoBack()
    {
      await Shell.Current.GoToAsync("..");
    }
  }
}
