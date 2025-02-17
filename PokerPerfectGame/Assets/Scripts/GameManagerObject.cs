using static FunctionsNameSpace.Functions;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using HutongGames.PlayMaker;

public class GameManagerObject : MonoBehaviour
{
  public List<BlastDoorObject> blastDoorObjects;

  public Fsm Fsm => GetComponent<PlayMakerFSM>().Fsm;

  void Start()
  {
    StartCoroutine(StartGame());
  }

  IEnumerator StartGame()
  {
    // these should be replaced by actual start time from game setup
    yield return new WaitForSeconds(2.0f);

    // move blast doors away to reveal table take(4) limits it to first 4 which is not needed
    foreach (var blastDoorObject in blastDoorObjects.Take(4))
      StartCoroutine(blastDoorObject.MoveToSpot());

    Random.InitState((int)DateTime.Now.Ticks);

    PlayMakerFSM.BroadcastEvent(EventTypes.GameBeginEvent.ToString());
  }
}
