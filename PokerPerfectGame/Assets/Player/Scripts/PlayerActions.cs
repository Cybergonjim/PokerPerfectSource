using HutongGames.PlayMaker;
using UnityEngine;
using static PlayerUtil;
using static DealerUtil;
using static GameUtil;
using System.Collections;
using static FunctionsNameSpace.Functions;

public static class PlayerUtil
{
  public static PlayerObject Player(Fsm fsm) => fsm.Owner.gameObject.GetComponent<PlayerObject>();
}

public class PlayerWaitingAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter("Start");
  }
}

public class PlayerStartBettingAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.PlayerStartBettingEvent.ToString());

    Player(Fsm).StartBetting();

    Finish();
  }
}

public class PlayerShowCardsAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.PlayerShowCardsEvent.ToString());

    Player(Fsm).ShowCards();

    Finish();
  }
}

public class PlayerHandBeginAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.PlayerHandBeginEvent.ToString());

    Player(Fsm).HandBegin();

    Finish();
  }
}

public class PlayerRankHandAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.PlayerRankHandEvent.ToString());

    if (!Player(Fsm).Folded && PlayersInHandCount() > 1)
      Player(Fsm).Rank.RankHand();

    Finish();
  }
}

public class PlayerFoldAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.PlayerFoldEvent.ToString());

    CardObject sourceCard = Player(Fsm).Cards[^1];

    Debug.Log($"Player Folded {Player(Fsm).name} | {sourceCard.name} | BurnIndex - {BurnIndex}");

    CardObject targetCard = Dealer_.BurnTargets[BurnIndex++];

    StartCoroutine(FoldCard(sourceCard, targetCard));

    Player(Fsm).Cards.Remove(sourceCard);
  }

  public IEnumerator FoldCard(CardObject sourceCard, CardObject targetCard)
  {
    const float delay = 0.25f;

    yield return new WaitForSeconds(delay);

    StartCoroutine(sourceCard.MoveAndRotate(targetCard, 0, Vector3.zero));

    Finish();
  }
}
