using TMPro;
using UnityEngine;
using static GameUtil;

public class HistoryCardSymbol : MonoBehaviour
{
  public int columnIndex;
  public int rowIndex;

  void Update()
  {
    if (rowIndex < PlayersHistory.Count)
    {
      PlayerObject playerObject = PlayersHistory[rowIndex];

      if (playerObject != null && playerObject.HistoryCards.Count > columnIndex)
      {
        CardObject cardObject = playerObject.HistoryCards[columnIndex];

        if (cardObject != null)
          gameObject.GetComponent<Renderer>().material = cardObject.SymbolMaterial;
      }
    }
    else
    {
      PlayerObject playerObject = PlayersSeated[0];
      gameObject.GetComponent<Renderer>().material = playerObject.symbolMaterials[^1];
    }

  }
}
