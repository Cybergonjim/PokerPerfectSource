using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static FunctionsNameSpace.Functions;
using static GameUtil;

public class PotObject : MonoBehaviour
{
  public TMP_Text handText;
  public TMP_Text amountText;

  public List<TMP_Text> playerTexts;
  public GameObject sphere;

  private int PositionCounter { get; set; }

  private int ChipCounter { get; set; }

  private int HighestRank { get; set; }

  private List<ChipObject> ChipObjects { get; set; } = new();

  public List<PlayerObject> PotPlayerObjects { get; set; } = new();

  private List<Vector3> Vectors { get; set; } = new();

  private int amount;
  public int Amount
  {
    get => amount;

    set
    {
      // value contains total amount of chips
      if (value == 0)
      {
        transform.gameObject.SetActive(false);

        DeleteChips();
      }
      else
      {
        transform.gameObject.SetActive(true);

        // adds to existing amount where 
        CreateChips(value - amount);
      }

      amount = value;
    }
  }

  private const int stackCount = 7;
  private const float potChipSize = 0.12f;

  private void CreateChips(int amount)
  {
    List<int> chipTypes = DistributeChips(amount);
    
    CreateVectors(chipTypes);

    PositionCounter = 0;

    for (int i = 0; i < chipTypes.Count; i++)
      for (int j = 0; j < chipTypes[i]; j++)
        CreateChip(i, j);
  }

  private void DeleteChips()
  {
    foreach (var chip in ChipObjects)
      Destroy(chip);

    ChipObjects.Clear();
  }

  void CreateVectors(List<int> chipTypes)
  {
    Vectors.Clear();

    ChipCounter = 0;

    foreach (int chipCount in chipTypes)
      for (int j = 0; j < chipCount; j++)
      {
        float angle = Random.Range(0.0f, 360.0f);
        float radius = Random.Range(0.0f, 0.4f);

        float x = radius * Mathf.Cos(angle);
        float y = radius * Mathf.Sin(angle);

        Vectors.Add(new Vector3(x, ChipCounter++ * 0.005f, y));
      }

    Randomize(Vectors);
  }

  void CreateChip(int stack, int position)
  {
    int prefabIndex = stackCount - stack + ChipIndex - 2;

    ChipObject chipObject = Instantiate(ChipControl.ChipPrefabs[prefabIndex], Vector3.zero, Quaternion.Euler(90.0f, 0.0f, 0.0f));

    string chipName = $"Chip #{stack}-{position}";
    chipObject.name = chipName;

    Transform chipTransform = chipObject.transform;
    chipTransform.parent = transform;
    chipTransform.localPosition = Vectors[PositionCounter++];
    chipTransform.localScale = new Vector3(potChipSize, potChipSize, potChipSize);
    chipTransform.gameObject.SetActive(true);
    chipTransform.Rotate(0, Random.Range(0.0f, 360.0f), 0);

    Rigidbody chipRigidBody = chipObject.rigidBody;
    chipRigidBody.isKinematic = false;
    chipRigidBody.detectCollisions = true;

    chipObject.moving = true;

    ChipObjects.Add(chipObject);
  }

  public void PullAmounts(List<PlayerObject> potPlayersBetting)
  {
    int smallestBet = potPlayersBetting.Min(playerObject => playerObject.BetAmount);

    // create a list of all players with smallest amount
    List<PlayerObject> playersWithSmallestBet = potPlayersBetting.Where(playerObject => playerObject.BetAmount >= smallestBet).ToList();

    // remove smallest amount from all players
    playersWithSmallestBet.ForEach(player =>
    {
      player.BetAmount -= smallestBet;

      // add player if has not folded and not already in list
      if (!player.Folded && !PotPlayerObjects.Contains(player))
        PotPlayerObjects.Add(player);
    });

    // add amount to current pot to add the chips
    Amount += playersWithSmallestBet.Count * smallestBet;
  }

  public void SetNames()
  {
    playerTexts.ForEach(playerText => playerText.text = string.Empty);

    if (Amount > 0)
    {
      amountText.text = " · " + Amount.ToString("N0") + " · ";

      PotPlayerObjects.Select((potPlayerObject, index) =>
      {
        // Add · player name ·
        playerTexts[index].text = $" · {potPlayerObject.nameText.text} · ";

        // Set color matching
        playerTexts[index].fontMaterial = potPlayerObject.TextMaterial;

        return index;
      }).ToList();
    }
    else
    {
      handText.text = string.Empty;
      amountText.text = string.Empty;
    }
  }

  public void DetermineWinner()
  {
    if (PotPlayerObjects.Count > 1)
    {
      // turn on all hands button
      PotPlayerObjects.ForEach(playerObject =>
      {
        playerObject.rankText.text = playerObject.Rank.HandDescription;
        playerObject.kickersText.text = playerObject.Rank.KickersDescription;
        playerObject.handButton.SetActive(true);
      });

      // find the winning hand
      HighestRank = PotPlayerObjects.Max(playerObject => playerObject.Rank.Value);

      // remove players from this pot that are not winners
      PotPlayerObjects.RemoveAll(playerObject => playerObject.Rank.Value != HighestRank);

      SetNames();

      handText.text = PotPlayerObjects[0].Rank.HandDescription;
      amountText.text = PotPlayerObjects[0].Rank.KickersDescription;
    }
  }

  public void PayWinner()
  {
    // calculate the fractional number of chips to split
    float potSplit = Amount / PotPlayerObjects.Count;
    
    // round potSplit up to the nearest smallest denomination
    int smallestDenomination = Denominations[^ChipIndex];

    // find number of chips and convert to amount
    int payoutSplit = (int)System.Math.Ceiling(potSplit / smallestDenomination) * smallestDenomination;

    PotPlayerObjects.ForEach(playerObject => print($"{playerObject.Handle} | Amount = {playerObject.Amount} | BetAmount = {playerObject.BetAmount} > Wins = {payoutSplit}"));

    // distribute the winnings to players
    PotPlayerObjects.ForEach(playerObject => playerObject.Amount += payoutSplit);

    PotPlayerObjects.ForEach(playerObject => print($"{playerObject.Handle} | Amount = {playerObject.Amount}"));

    PotPlayerObjects.Clear();

    Amount = 0;

    SetNames();
  }

}