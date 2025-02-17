using UnityEngine;
using HutongGames.PlayMaker;

public class FsmStateActionDebug : FsmStateAction
{
  public void Enter(string EventName)
  {
    Debug.Log($"State entered: {Fsm.GameObjectName}-{Fsm.ActiveStateName} - Event: {EventName}");
  }

  public new void Finish()
  {
    Debug.Log($"State finished. {Fsm.GameObjectName}-{Fsm.ActiveStateName}");
    base.Finish();
  }
}

public static class PlayMakerFSMDebug
{
  public static void BroadcastEvent(GameObject gameObject, string fsmEventName)
  {
    Debug.Log($"Event broadcast {gameObject.name}-{fsmEventName}");

    PlayMakerFSM.BroadcastEvent(fsmEventName);
  }
}

