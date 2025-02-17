using System.Collections;
using static FunctionsNameSpace.Functions;
using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using static JsonControl;
using static GameUtil;

public class BlindObject : MonoBehaviour
{
  public Vector3 RotateAmount;
  public List<TMP_Text> cubeText;
  
  private int blindDuration;
  private int blindIndex;
  
  private Games gamesData;
  private Blinds blindsData;
  private float elapsedTime;

  void Start()
  {
    gamesData = (Games)Data.data[DataTypes.games.ToString()];
    blindsData = (Blinds)Data.data[DataTypes.blinds.ToString()];

    blindDuration = gamesData.games[^1].blindTime;
    blindIndex = 0;

    StartCoroutine(UpdateBlind());
  }

  void Update()
  {
    const float secondsInMinute = 60.0f;

    elapsedTime += Time.deltaTime;

    // blindDuration is in minutes
    TimeSpan timeSpan = TimeSpan.FromSeconds(blindDuration * secondsInMinute - elapsedTime);

    string formattedTime = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";

    cubeText[(int)PositionTypes.Dealer].text = formattedTime;

    transform.Rotate(RotateAmount);
  }

  public IEnumerator UpdateBlind()
  {
    while (true)
    {
      elapsedTime = 0;

      BigBlindAmount = blindsData.blinds[blindIndex].amount;
      SmallBlindAmount = BigBlindAmount / 2;

      // ante is percent of bigBlind and Denominations[^1] is the smallest current denomination
      if (BigBlindAmount * (blindsData.blinds[blindIndex].ante / 100.0f) >= Denominations[^1])
        AnteAmount = (int)(BigBlindAmount * (blindsData.blinds[blindIndex].ante / 100.0f));

      cubeText[(int)PositionTypes.BigBlind].text = "BB\n" + BigBlindAmount.ToString();
      cubeText[(int)PositionTypes.SmallBlind].text = "SB\n" + SmallBlindAmount.ToString();

      blindIndex++;

      yield return new WaitForSeconds(blindDuration * 60.0f);
    }
  }
}
