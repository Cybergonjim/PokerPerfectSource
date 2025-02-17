using UnityEngine;
using static DealerUtil;

public class HistoryCard : MonoBehaviour
{
  public int columnIndex;

  void Update()
  {
    if (columnIndex < Dealer_.HistoryCards.Count)
    {
      CardObject historyCard = Dealer_.HistoryCards[columnIndex];

      if (historyCard != null)
        GetComponent<Renderer>().material = historyCard.faceObject.GetComponent<Renderer>().material;
    }
    else
      GetComponent<Renderer>().material = Dealer_.noneMaterial;
  }
}
