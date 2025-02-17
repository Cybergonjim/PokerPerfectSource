using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FunctionsNameSpace.Functions;
using static DealerUtil;
using static GameUtil;
using HutongGames.PlayMaker.Actions;
//using static UnityEngine.Rendering.GPUSort;

public static class DealerUtil
{
  public const int CardCount = 52;
  public const int RankCount = 13;
  
  public static int CardIndex { get; set; }

  public static int BurnIndex { get; set; }

  public static int LandingIndex { get; set; }

  public static List<CardObject> CommunityCards { get; set; } = new List<CardObject>();

  private static DealerObject dealerObject;

  public static void SetDealer(Fsm fsm) => dealerObject = fsm.Owner.gameObject.GetComponent<DealerObject>();

  public static DealerObject Dealer_ => dealerObject;
}

public class DealerWaitForStartAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.DealerBeginEvent.ToString());

    SetDealer(Fsm);
  }
}

public class DealerMoveCardsAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.DealerBeginEvent.ToString());

    // reset card positions before the deal
    CardIndex = 0;
    BurnIndex = 0;
    LandingIndex = 0;

    CommunityCards.Clear();

    StartCoroutine(Dealer_.ChangeBackColor()); // change card back color over time
    
    StartCoroutine(DealMoveCards());
  }

  IEnumerator DealMoveCards()
  {
    const float delay = 0.02f;

    Vector3 direction = PlayersSeated[DealerIndex].transform.position -
      PlayersSeated[DealerIndex].CardDealer.transform.TransformPoint(Vector3.zero);

    direction.Normalize();

    for (int i = 0; i < CardObjects.Count; i++)
    {
      StartCoroutine(CardObjects[i].MoveAndRotate(PlayersSeated[DealerIndex].CardDealer, i, direction));

      yield return new WaitForSeconds(delay);
    }
  }
}

public class DealerCardsToPlayersAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.DealerBeginEvent.ToString());

    StartCoroutine(DealCardsToPlayers());
  }

  IEnumerator DealCardsToPlayers()
  {
    const float delay = 0.25f;
    const int cardCount = 2;

    for (int j = 0; j < cardCount; j++)
      for (int i = 0; i < PlayersSeated.Count; i++)
      {
        PlayerObject playerObject = PlayersSeated[DealerIndex + 1 + i];

        CardObject cardTarget = playerObject.cardPositions[j];

        playerObject.Cards.Add(CardObjects[CardIndex]);

        StartCoroutine(CardObjects[CardIndex++].MoveAndRotate(cardTarget, 0, Vector3.zero));

        yield return new WaitForSeconds(delay);
      }

    Game_.Fsm.Event(EventTypes.GameDealEndEvent.ToString());
  }
}

public class DealerFlopAction  : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.DealerBeginEvent.ToString());

    StartCoroutine(DealFlop());
  }

  IEnumerator DealFlop()
  {
    const int FlopCount = 3;
    const float delay = 2.0f;
    const float fastDelay = 0.25f;
    const float turnoverDelay = 1.5f;

    StartCoroutine(CardObjects[CardIndex++].MoveAndRotate(Dealer_.BurnTargets[BurnIndex++], 0, Vector3.zero));
    
    yield return new WaitForSeconds(fastDelay);

    for (int i = 0; i < FlopCount; i++)
    {
      CommunityCards.Add(CardObjects[CardIndex + i]);

      StartCoroutine(CardObjects[CardIndex + i].MoveAndRotate(Dealer_.LandingTargets[LandingIndex++], 0, Vector3.zero));

      yield return new WaitForSeconds(fastDelay);
    }

    yield return new WaitForSeconds(delay);

    for (int i = 0; i < FlopCount; i++)
      StartCoroutine(CardObjects[CardIndex++].TurnOver(turnoverDelay));

    yield return new WaitForSeconds(delay);

    Game_.Fsm.Event(EventTypes.GameDealEndEvent.ToString());
  }
}

public class DealerTurnAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.DealerBeginEvent.ToString());

    StartCoroutine(DealTurn());
  }

  IEnumerator DealTurn()
  {
    const float delay = 2.0f;

    StartCoroutine(CardObjects[CardIndex++].MoveAndRotate(Dealer_.BurnTargets[BurnIndex++], 0, Vector3.zero));

    yield return new WaitForSeconds(delay);

    StartCoroutine(CardObjects[CardIndex].MoveAndRotate(Dealer_.LandingTargets[LandingIndex++], 0, Vector3.zero));

    yield return new WaitForSeconds(delay);

    CommunityCards.Add( CardObjects[CardIndex]);

    StartCoroutine(CardObjects[CardIndex++].TurnOver(1.5f));

    yield return new WaitForSeconds(delay);

    Game_.Fsm.Event(EventTypes.GameDealEndEvent.ToString());
  }
}

public class DealerRiverAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.DealerBeginEvent.ToString());

    StartCoroutine(DealRiver());
  }

  IEnumerator DealRiver()
  {
    const float delay = 2.0f;

    StartCoroutine(CardObjects[CardIndex++].MoveAndRotate(Dealer_.BurnTargets[BurnIndex++], 0, Vector3.zero));

    yield return new WaitForSeconds(delay);

    StartCoroutine(CardObjects[CardIndex].MoveAndRotate(Dealer_.LandingTargets[LandingIndex], 0, Vector3.zero));

    yield return new WaitForSeconds(delay);

    CommunityCards.Add(CardObjects[CardIndex]);

    StartCoroutine(CardObjects[CardIndex++].TurnOver(delay));

    yield return new WaitForSeconds(delay);

    Game_.Fsm.Event(EventTypes.GameDealEndEvent.ToString());
  }
}

public class DealerRandomizeCardsAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.DealerEndEvent.ToString());

    Dealer_.HistoryCards.Clear();
    Dealer_.HistoryCards.AddRange(CommunityCards);

    Randomize(CardObjects);

    Finish();
  }
}


