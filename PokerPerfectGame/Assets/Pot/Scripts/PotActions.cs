using HutongGames.PlayMaker;
using static FunctionsNameSpace.Functions;
using static PotUtil;

public static class PotUtil
{
  private static PotControl potControl;

  public static void SetPot(Fsm fsm) => potControl = fsm.Owner.gameObject.GetComponent<PotControl>();

  public static PotControl Pot_ => potControl;
}

public class PotWaitingAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter("Start");

    SetPot(Fsm);

    Finish();
  }
}

public class PotPullAmountsAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.PotPullAmountsEvent.ToString());

    Pot_.PullBetAmounts();

    Finish();
  }
}

public class PotDetermineWinnersAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.PotDetermineWinnersEvent.ToString());

    Pot_.DetermineWinners();

    Finish();
  }
}

public class PotDelayAction : FsmStateActionDebug
{
  public override void OnEnter()
  {
    Enter(EventTypes.PotDelayEvent.ToString());

    Pot_.PayWinners();

    Finish();
  }
}