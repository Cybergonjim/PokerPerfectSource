using UnityEngine;
using UnityEngine.EventSystems;

public class ExitOnClick : MonoBehaviour, IPointerClickHandler
{
  public void OnPointerClick(PointerEventData eventData)
  {
    // Check if the object is clicked
    Debug.Log("Cube clicked, exiting program.");
    Application.Quit();

    // If running in the Unity editor, stop the play mode
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
  }
}