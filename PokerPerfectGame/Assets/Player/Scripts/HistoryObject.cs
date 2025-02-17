using TMPro;
using UnityEngine;
using static GameUtil;

public class HistoryObject : MonoBehaviour
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

      if (playerObject != null)
      {
        textMeshProComponent.text = playerObject.HistoryTexts[columnIndex];
        textMeshProComponent.gameObject.GetComponent<Renderer>().material = playerObject.TextMaterial;
      }
    }
    else
      textMeshProComponent.text = "";
  }
}
