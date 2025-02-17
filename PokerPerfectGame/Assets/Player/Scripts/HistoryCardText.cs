using FunctionsNameSpace;
using RankHands;
using TMPro;
using UnityEngine;
using static GameUtil;

public class HistoryCardText : MonoBehaviour
{
  public int columnIndex;
  public int rowIndex;

  private TextMeshPro textMeshProComponent;

  void Start()
  {
    textMeshProComponent = GetComponent<TextMeshPro>();
  }

  void Update()
  {
    if (rowIndex < PlayersHistory.Count)
    {
      PlayerObject playerObject = PlayersHistory[rowIndex];

      if (playerObject != null && playerObject.HistoryCards.Count == 2)
      {
        CardObject cardObject = playerObject.HistoryCards[columnIndex];

        if (cardObject != null)
        {
          textMeshProComponent.text = cardObject.RankType.GetDescription();
          textMeshProComponent.gameObject.GetComponent<Renderer>().material = cardObject.RankMaterial;
        }
      }
    }
    else
      textMeshProComponent.text = "";
  }
}
