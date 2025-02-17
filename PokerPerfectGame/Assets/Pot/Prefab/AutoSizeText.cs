using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoSizeText : MonoBehaviour
{
  public Button button;
  public float multiplyX;

  void Update()
  {
    float preferredWidth;
    float preferredHeight;

    TMP_Text textComponent = GetComponent<TMP_Text>();

    if (textComponent != null )
    {
      Vector2 preferredSize = textComponent.GetPreferredValues(textComponent.text);

      preferredWidth = preferredSize.x * multiplyX;

      if (button != null)
      {
        RectTransform buttonRectTransform = button.GetComponent<RectTransform>();

        if (buttonRectTransform != null)
        {
          preferredHeight = buttonRectTransform.sizeDelta.y;

          buttonRectTransform.sizeDelta = new Vector2(preferredWidth, preferredHeight);
        }
      }
    }
    else
    {
      string fileName = System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);

      Debug.Log($"Error in {fileName} (textComponent == null) in Update().");
    }
  }
}
