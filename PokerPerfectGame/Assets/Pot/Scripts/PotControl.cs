using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static FunctionsNameSpace.Functions;
using static GameUtil;
using static DealerUtil;

public class PotControl : MonoBehaviour
{
  public PotObject potPrefab;
  public TMP_Text amountText;
  public List<TMP_Text> playersText;

  public static int LastPot { get; set; }

  public Fsm Fsm => GetComponent<PlayMakerFSM>().Fsm;

  private List<PotObject> PotObjects { get; set; }

  private readonly List<Vector3> positions = new()
  {
    new Vector3 { x = -72.25f, y = 0, z = -8 }, // 0
    new Vector3 { x = 103, y = 24, z = -8 }, // 1
    new Vector3 { x = -103, y = 24, z = -8 }, // 2
    new Vector3 { x = -43, y = 26, z = -8 }, // 3
    new Vector3 { x = -43, y = -26, z = -8 }, // 4
    new Vector3 { x = 43, y = 26, z = -8 }, // 5
    new Vector3 { x = 43, y = -26, z = -8 }, // 6
    new Vector3 { x = -103, y = -24, z = -8 }, // 7
    new Vector3 { x = 103, y = -24, z = -8 } // 8
  };

  private void CreatePots()
  {
    PotObjects = positions.Select((position, index) =>
    {
      PotObject potObject = Instantiate(potPrefab, position, Quaternion.Euler(-90.0f, 0, 0));
      potObject.sphere.transform.Rotate(0, Random.Range(0.0f, 360.0f), 0);
      potObject.transform.parent = transform;
      potObject.name = $"Pot #{index + 1}";

      return potObject;
    }).ToList();
  }

  public void PullBetAmounts()
  {
    Debug.Log($"PotControl.PullBetAmounts()");

    // pull bet amounts from each player and sort bet amounts adding more pots as needed
    List<PlayerObject> playersBetting = PlayersSeated
      .Where(playerSeated => playerSeated.BetAmount > 0)
      .OrderBy(playerSeated => playerSeated.BetAmount)
      .ThenBy(playerSeated => PlayersSeated.IndexOf(playerSeated))
      .ToList();

    // keep repeating in case players cannot cover full amount so side pots are generated
    while (playersBetting.Count > 1)
    {
      // pull in all bets from betting players including those that have folded
      PotObjects[LastPot].PullAmounts(playersBetting);

      playersBetting.RemoveAll(playerObject => playerObject.BetAmount == 0);

      // add more pots as needed
      if (playersBetting.Count > 1)
        LastPot++;
    }

    // if any extra chips are left over, they go back to player
    if (playersBetting.Count == 1)
    {
      PlayerObject lastPlayer = playersBetting[0];

      lastPlayer.Amount += lastPlayer.BetAmount;
      lastPlayer.BetAmount = 0;
    }

    // remove players from main and side pots that have folded
    PotObjects.ForEach(potObject =>
    {
      potObject.PotPlayerObjects.RemoveAll(potPlayerObjects => potPlayerObjects.Folded);
      potObject.SetNames(); // add names to each pot for remaining players
    });
  }

  public void DetermineWinners()
  {
    Debug.Log($"PotControl.DetermineWinners()");

    // determine winners
    PotObjects.Where(potObject => potObject.PotPlayerObjects.Count > 0)
      .ToList()
      .ForEach(potObject => potObject.DetermineWinner());

    StartCoroutine(DelayPayWinners());
  }

  IEnumerator DelayPayWinners()
  {
    const float delay = 5.0f;

    yield return new WaitForSeconds(delay);

    Fsm.Event(EventTypes.PotDelayEvent.ToString());
    Game_.Fsm.Event(EventTypes.GameBeginEvent.ToString());
    Dealer_.Fsm.Event(EventTypes.DealerEndEvent.ToString());
  }

  public void PayWinners()
  {
    Debug.Log($"PotControl.PayWinners()");

    // turn off all hand buttons
    PlayerObjects.ForEach(playerObject => playerObject.handButton.SetActive(false));

    // pay remaining players
    PotObjects.Where(potObject => potObject.PotPlayerObjects.Count > 0)
      .ToList()
      .ForEach(potObject => potObject.PayWinner());
  }

  void Start()
  {
    CreatePots();
  }
}
