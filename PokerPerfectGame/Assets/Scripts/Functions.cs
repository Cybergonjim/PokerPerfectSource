using System;
using System.Collections.Generic;
using static JsonControl;
using Random = System.Random;
using System.ComponentModel;
using static GameUtil;
using System.Linq;

// version 12/05/2023
namespace FunctionsNameSpace
{
  public static class EnumExtensions
  {
    public static string GetDescription(this Enum value)
    {
      var fieldInfo = value.GetType().GetField(value.ToString());

      return fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Length > 0 ? attributes[0].Description : value.ToString();
    }
  }

  public class Functions
  {
    private static readonly Random rand = new();

    public const int stackSize = 30;
    public const int stackCount = 7;

    public enum PositionTypes
    {
      [Description("(D)")]
      Dealer,
      [Description("(SB)")]
      SmallBlind,
      [Description("(BB)")]
      BigBlind,
      [Description("")]
      None,
    }

    public enum ButtonTypes
    {
      Peek,
      Status,
      Fold,
      Call,
      Raise,
      Reset,
      Minus,
      Plus,
      AllIn,
    }

    public enum EventTypes
    {
      DealerBeginEvent,
      DealerEndEvent,
      GameBeginEvent,
      GameBetEndEvent,
      GameDealEndEvent,
      GameDealNextEvent,
      GameEndEvent,
      GameHandEndEvent,
      GameNextEvent,
      GameRoundEndEvent,
      GameShowdownEvent,
      PlayerStartBettingEvent,
      PlayerFoldEvent,
      PlayerHandBeginEvent,
      PlayerRankHandEvent,
      PlayerShowCardsEvent,
      PotDelayEvent,
      PotDetermineWinnersEvent,
      PotPullAmountsEvent,
    }

    private static Chipset GetFilteredChipset()
    {
      Chipsets chipsetsData = (Chipsets)Data.data[DataTypes.chipsets.ToString()];

      int targetChipsetIndex = FilteredGame.chipSet;

      return chipsetsData.chipsets[targetChipsetIndex];
    }
    private static Chipset chipset;
    public static Chipset Chipset
    {
      get 
      {
        chipset ??= GetFilteredChipset();

        return chipset; 
      } 
    }

    private static Game GetFilteredGame()
    {
      Games gamesData = (Games)Data.data[DataTypes.games.ToString()];

      return gamesData.games.Last();
    }
    private static Game filteredGame;
    public static Game FilteredGame
    {
      get
      {
        filteredGame ??= GetFilteredGame();

        return filteredGame;
      }
    }

    private static List<Chip> GetFilteredChips()
    {
      int targetChipsetId = Chipset.chipsetId;

      Chips chipsData = (Chips)Data.data[DataTypes.chips.ToString()];

      return chipsData.chips
        .Where(chip => chip.chipsetId == targetChipsetId)
        .OrderBy(chip => chip.chipId)
        .ToList();
    }
    private static List<Chip> filteredChips;
    public static List<Chip> FilteredChips
    {
      get
      {
        filteredChips ??= GetFilteredChips();

        return filteredChips;
      }
    }

    private static List<Player> GetFilteredPlayers()
    {
      int targetGameId = FilteredGame.gameId;

      Players playersData = (Players)Data.data[DataTypes.players.ToString()];

      return playersData.players
        .Where(player => player.gameId == targetGameId)
        .OrderBy(player => player.playerId)
        .ToList();
    }
    private static List<Player> filteredPlayers;
    public static List<Player> FilteredPlayers
    {
      get
      {
        filteredPlayers ??= GetFilteredPlayers();

        return filteredPlayers;
      }
    }

    private static List<int> GetDenominations()
    {
      List<int> denominations = new List<int>();

      for (int i = 0; i < filteredChips.Count; i++)
        denominations.Add(filteredChips[i].denomination);

      // Order the denominations in descending order
      return denominations.OrderByDescending(d => d).ToList();
    }
    private static List<int> denominations;
    public static List<int> Denominations
    {
      get
      {
        denominations ??= GetDenominations();

        return denominations;
      }
    }

    public static List<int> DistributeChips(int inputAmount)
    {
      int numberOfBins = stackCount;

      List<int> bins = new List<int>(numberOfBins);

      for (int binIndex = 0; binIndex < numberOfBins; binIndex++)
        bins.Add(0);

      // Process bins backward
      for (int binIndex = bins.Count - 1; binIndex >= 0; binIndex--)
        while (inputAmount >= denominations[binIndex + ChipIndex] && bins[binIndex] < stackSize)
        {
          inputAmount -= denominations[binIndex + ChipIndex];
          bins[binIndex]++;
        }

      if (inputAmount > 0)
      {
        int index;

        // Find last bin to add one and go to the next if full
        for (index = 0; index < bins.Count; index++)
          if (bins[index] != 0)
          {
            if (bins[index] == stackSize)
              index--;

            bins[index]++;

            // Now input value is over
            inputAmount -= denominations[index + ChipIndex];
            index++;

            break;
          }

        for (; index < bins.Count; index++)
          while ((-inputAmount >= denominations[index + ChipIndex]) && (bins[index] > 0))
          {
            // If the current bin is empty, then start on the next bin
            inputAmount += denominations[index + ChipIndex];
            bins[index]--;
          }
      }

      return bins;
    }

    public static string ConvertDenomination(int denomination)
    {
      return denomination >= 1000000 ? (denomination / 1000000) + "M" : denomination >= 1000 ? (denomination / 1000) + "K" : denomination.ToString();
    }

    public static string DenominationToString(int denomination)
    {
      string denom;

      if (denomination >= 1000000)
        denom = Math.Round((decimal)denomination / 1000000, 2) + "M";
      else if (denomination >= 1000)
        denom = Math.Round((decimal)denomination / 1000, 2) + "K";
      else
        denom = denomination.ToString();

      return denom;
    }

    public static void Randomize<T>(List<T> list)
    {
      for (int i = list.Count - 1; i > 0; i--)
      {
        var k = rand.Next(i + 1);
        var value = list[k];
        list[k] = list[i];
        list[i] = value;
      }
    }
  }
}