using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static GameUtil;

public class StandingObject : MonoBehaviour
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

    if (rowIndex < SortedPlayers.Count)
    {
      PlayerObject playerObject = SortedPlayers[rowIndex];

      if (playerObject != null)
      {
        textMeshProComponent.text = playerObject.StandingsTexts[columnIndex];
        textMeshProComponent.gameObject.GetComponent<Renderer>().material = playerObject.TextMaterial;
      }
      else
      {
        textMeshProComponent.text = "";
      }
    }
  }



}
