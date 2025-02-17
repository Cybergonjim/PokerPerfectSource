using HutongGames.PlayMaker;
using Random = UnityEngine.Random;
using static FunctionsNameSpace.Functions;
using UnityEngine;
using static JsonControl;
using System.Collections.Generic;
using System.Linq;
using static GameUtil;
using static PotControl;
using static DealerUtil;
using static PotUtil;
using System;

public static class GameUtil
{
  private static GameManagerObject gameManagerObject;

  public static void SetGameManagerObject(Fsm fsm) => gameManagerObject = fsm.Owner.gameObject.GetComponent<GameManagerObject>();

  public static GameManagerObject Game_ => gameManagerObject;

  public static List<CardObject> CardObjects { get; set; } = new();

  public static List<PlayerObject> PlayerObjects { get; set; } = new();

  public static WrappedList<PlayerObject> PlayersSeated { get; set; } = new();

  public static WrappedList<PlayerObject> PlayersHistory { get; set; } = new();

  public static List<PlayerObject> SortedPlayers { get; set; } = new();

  public static int BigBlindAmount { get; set; }

  public static int SmallBlindAmount { get; set; }

  public static int AnteAmount { get; set; }

  public static int LastRaiseAmount { get; set; }

  private static void GetChipIndex()
  {
    Games gamesData = (Games)Data.data[DataTypes.games.ToString()];

    // find the item with a denomination less than or equal to small blind
    int targetDenomination = gamesData.games.Last().blindStart / 2;

    chipIndex = FilteredChips.FindLastIndex(obj => obj.denomination <= targetDenomination) + 1;
  }

  private static int chipIndex = -1;
  public static int ChipIndex
  {
    get
    {
      if (chipIndex == -1)
        GetChipIndex();

      return chipIndex;
    }
  }

  public static int RoundCount { get; set; }

  public static int CallAmount { get; set; }

  private static void SetPlayerPosition(ref int currentIndex, int newIndex, object positionType)
  {
    int playerCount = PlayersSeated.Count;

    // Calculate the new index by wrapping around if necessary
    currentIndex = (newIndex + playerCount) % playerCount;

    // Set the new player's position to the specified positionType
    if (positionType != null)
    {
      PlayersSeated[currentIndex].PositionType = (PositionTypes)positionType;
      Debug.Log($"Index set. {positionType}-{currentIndex}");
    }
  }

  private static int dealerIndex = -1;
  public static int DealerIndex
  {
    get => dealerIndex;
    set => SetPlayerPosition(ref dealerIndex, value, PositionTypes.Dealer);
  }

  private static int bigBlindIndex = -1;
  public static int BigBlindIndex
  {
    get => bigBlindIndex;
    set => SetPlayerPosition(ref bigBlindIndex, value, PositionTypes.BigBlind);
  }

  private static int smallBlindIndex = -1;
  public static int SmallBlindIndex
  {
    get => smallBlindIndex;
    set => SetPlayerPosition(ref smallBlindIndex, value, PositionTypes.SmallBlind);
  }

  private static int actionIndex = -1;
  public static int ActionIndex
  {
    get => actionIndex;
    set
    {
      SetPlayerPosition(ref actionIndex, value, null);
      Debug.Log($"Index set. actionIndex-{actionIndex}");
    }

  }

  private static int lastActionIndex = -1;
  public static int LastActionIndex
  {
    get => lastActionIndex;
    set => SetPlayerPosition(ref lastActionIndex, value, null);
  }

  private static int nextDealerIndex;
  public static int NextDealerIndex
  {
    get => nextDealerIndex;

    set => nextDealerIndex = (value + PlayersSeated.Count) % PlayersSeated.Count;
  }
  
  private static int nextSmallBlindIndex;
  public static int NextSmallBlindIndex
  {
    get => nextSmallBlindIndex;

    set => nextSmallBlindIndex = (value + PlayersSeated.Count) % PlayersSeated.Count;
  }

  public static PlayerObject CurrentPlayer => PlayersSeated[ActionIndex];

  // these players still have an amount and are not folded so they can bet checked during hands
  public static int PlayersThatCanBetCount()
  {
    int count = PlayerObjects.Count(player => player.Amount > 0 && !player.Folded);
    return count;
  }

  // if this number gets to one then game is over only checked between hands
  public static int PlayersWithAmountCount()
  {
    int count = PlayerObjects.Count(player => player.Amount > 0);
    return count;
  }

  // these players have cards so still playing hand if this number gets to one then hand is over
  public static int PlayersInHandCount()
  {
    int count = PlayersSeated.Count(player => !player.Folded);
    return count;
  }
}

public class GameWaitingAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter("Start");

    SetGameManagerObject(Fsm);
  }

  public override void OnExit()
  {
    // this gets all players that have amounts. all others are not seated.
    // Amounts are zeroed and PositionTypes are set to None.

    PlayersSeated.Clear();

    // players are built counter-clockwise while dealing is clockwise. start at position 9 down to 0
    var playersSeated = PlayerObjects
      .Where(playerObject => !playerObject.Busted)
      .Reverse(); // add these in reverse order

    PlayersSeated.AddRange(playersSeated);

    NextDealerIndex =  Random.Range(0, PlayersWithAmountCount());
    NextSmallBlindIndex = NextDealerIndex + 1;
  }
}

public class GameHandBeginAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.GameBeginEvent.ToString());
    // Late Position
    // Late Position
    // Under the Gun
    // Early Position
    // Middle Position
    // Hijack
    // Cutoff
    // Button
    // Small Blind
    // Big Blind

    // this goes to each player to set up initial values
    PlayMakerFSM.BroadcastEvent(EventTypes.PlayerHandBeginEvent.ToString());

    // this sets up the initial table conditions before cards are dealt
    CallAmount = BigBlindAmount;
    LastRaiseAmount = BigBlindAmount;
    RoundCount = 0;
    LastPot = 0;

    // At start of the first hand, the SmallBlind is placed by the player to the left
    // of the dealer button and the BigBlind is then posted by the next player to
    // the left.

    // this happens even if the player has busted (dead dealer)
    DealerIndex = NextDealerIndex;

    // this happens even if the player has busted (dead small blind)
    SmallBlindIndex = NextSmallBlindIndex;

    // if only one player remains the game is over
    if (PlayersWithAmountCount() == 1)
    {
      Fsm.Event(EventTypes.GameEndEvent.ToString()); // to GameEndAction
    }
    else if (PlayersWithAmountCount() == 2)
    {
      // When there are only two players (a "heads-up" game), the player
      // on the Dealer button is the SmallBlind and the other player
      // is the BigBlind. The first player action is SmallBlind.

      // Find the player who should be big blind
      int saveBigBlind = FindNextActivePlayerIndex(SmallBlindIndex + 1);

      // Find the player who should be dealer/small blind
      int saveDealer = FindNextActivePlayerIndex(saveBigBlind + 1);

      // Remove busted players from the list
      PlayersSeated.RemoveAll(player => player.Busted);

      // Find the indices for dealer, big blind, and small blind in the updated list
      DealerIndex = PlayersSeated.FindIndex(player => player.Index == saveDealer);
      BigBlindIndex = PlayersSeated.FindIndex(player => player.Index == saveBigBlind);
      NextSmallBlindIndex = DealerIndex;

      // Set indices for the current and last actions
      LastActionIndex = BigBlindIndex;
      ActionIndex = NextSmallBlindIndex; // Assuming the action starts from the small blind

      SortedPlayers = PlayersSeated
        .OrderBy(player => (PlayersSeated.IndexOf(player) - DealerIndex + PlayersSeated.Count) % PlayersSeated.Count)
        .ToList();

      // Deal cards to remaining players
      Dealer_.Fsm.Event(EventTypes.DealerBeginEvent.ToString());
    }
    else
    {
      // Dealer and Small Blind have been set at this point they may be dead 
      // if the big blind has busted, move to the next player because do not skip a big blind
      int saveBigBlind = FindNextActivePlayerIndex(SmallBlindIndex + 1);

      // remove all busted players
      PlayersSeated.RemoveAll(player => player.Busted);

      // reset the big blind to the proper player
      BigBlindIndex = PlayersSeated.FindIndex(player => player.Index == saveBigBlind);

      // the LastRaiseIndex is used to determine when a betting round ends. 
      LastActionIndex = BigBlindIndex;

      // The first action is player past big blind
      ActionIndex = BigBlindIndex + 1;

      // the next small blind with be the current big blind
      NextSmallBlindIndex = BigBlindIndex;

      // the next dealer should be one behind the small blind as usual dead or not
      NextDealerIndex = NextSmallBlindIndex - 1;

      List<int> playerIndices = PlayersSeated.Select(player => PlayersSeated.IndexOf(player)).ToList();

      SortedPlayers = PlayersSeated
        .Select((player, index) => new { Player = player, Index = index })
        .OrderBy(item => (playerIndices[item.Index] - BigBlindIndex - 1 + PlayersSeated.Count) % PlayersSeated.Count)
        .Select(item => item.Player) // Extract the player objects back
        .ToList();

      // deal cards to players that are remaining
      Dealer_.Fsm.Event(EventTypes.DealerBeginEvent.ToString());
    }

    static int FindNextActivePlayerIndex(int startIndex)
    {
      int index = startIndex;

      while (PlayersSeated[index].Busted)
        index = (index + 1) % PlayersSeated.Count;

      return PlayersSeated[index].Index;
    }
  }
}

public class GameDealNextAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.GameDealNextEvent.ToString());

    LastRaiseAmount = BigBlindAmount;
    CallAmount = 0;

    // after first cards are dealt, the BigBlind gets first action
    LastActionIndex = DealerIndex;
    ActionIndex = SmallBlindIndex;

    Dealer_.Fsm.Event(EventTypes.DealerBeginEvent.ToString());
  }
}

public class GameBetBeginAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.GameDealEndEvent.ToString());

    CurrentPlayer.Fsm.Event(EventTypes.PlayerStartBettingEvent.ToString());
  }
}

public class GameCheckRoundEndAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.GameBetEndEvent.ToString());

    if (LastActionIndex == ActionIndex++ || PlayersInHandCount() == 1) // no use going further if everyone folded
      // to GameRoundEndedAction
      Fsm.Event(EventTypes.GameRoundEndEvent.ToString()); // betting round ended
    else
      // to GameBetBeginAction
      Fsm.Event(EventTypes.GameNextEvent.ToString()); // next player bets
  }
}

public class GameRoundEndAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.GameRoundEndEvent.ToString());

    Debug.Log($"Betting Round Ended ***********************************************************"); ;

    // pull amounts from each player to pots after each round of betting sent to pot actions
    // this has to be done because several players could have betted and then folded
    Pot_.Fsm.Event(EventTypes.PotPullAmountsEvent.ToString());

    // if only one player still in hand, hand is over and no need to bet
    if (PlayersInHandCount() == 1)
      // to GamePayWinnersAction 
      Fsm.Event(EventTypes.GameHandEndEvent.ToString());
    else if (PlayersThatCanBetCount() <= 1 || RoundCount++ == 3)
    {
      // this is sent to ALL players
      PlayMakerFSMDebug.BroadcastEvent(Fsm.GameObject, EventTypes.PlayerShowCardsEvent.ToString());

      // to GameShowdownAction
      Fsm.Event(EventTypes.GameShowdownEvent.ToString());
    }
    else
      // process transitions back to this state for another card until round of betting count is 3
      Fsm.Event(EventTypes.GameDealNextEvent.ToString()); // to GameDealNextAct
  }
}

public class GameShowdownAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.GameShowdownEvent.ToString());

    if (RoundCount++ >= 3)
      // to GamePayWinnersAction
      Fsm.Event(EventTypes.GameHandEndEvent.ToString());
    else
      // to DealerActions.cs
      Dealer_.Fsm.Event(EventTypes.DealerBeginEvent.ToString()); 
  }
}

public class GamePayWinnersAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.GameHandEndEvent.ToString());

    PlayMakerFSM.BroadcastEvent(EventTypes.PlayerRankHandEvent.ToString());

    // copy the current player's hands to the history and order be the rank
    PlayersHistory = new WrappedList<PlayerObject>(PlayersSeated.OrderBy(player => player.Rank.Value).Reverse().ToList());

    // tell pots to determine winners after ranking hands which then tells pots to pay winners from pot(s) after card pressed
    Pot_.Fsm.Event(EventTypes.PotDetermineWinnersEvent.ToString());

    Finish();
  }
}

public class GameEndAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.GameEndEvent.ToString());
  }
}