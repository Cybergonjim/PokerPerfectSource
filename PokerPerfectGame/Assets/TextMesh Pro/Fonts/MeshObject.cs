using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MeshObject : MonoBehaviour
{
  // more colors https://www.rapidtables.com/web/color/RGB_Color.html

  public List<TMP_FontAsset> fontAssets; // Reference to your font asset

  private List<Color> colors = new()
  {
    FromArgb(255, 0, 0),
    FromArgb(255, 128, 0),
    FromArgb(255, 255, 0),
    FromArgb(128, 255, 0),
    FromArgb(0, 255, 0),
    FromArgb(0, 255, 128),
    FromArgb(0, 255, 255),
    FromArgb(0, 128, 255),
    FromArgb(0, 0, 255),
    FromArgb(128, 0, 255),
    FromArgb(255, 0, 255),
    FromArgb(255, 0, 128)
  };

  private static Color FromArgb(int r, int g, int b)
  {
    Color color = new Color(r / 255.0f, g / 255.0f, b / 255.0f);

    return color;
  }

  void Start()
  {
    for (int i =  0; i < fontAssets.Count; i++)
      CreateTextMeshProObject($"XXX COLOR {i} XXX", colors[11 - i], i);
  }

  void CreateTextMeshProObject(string text, Color color, int number)
  {
    // Create a new TextMeshPro object
    GameObject textObject = new GameObject("TextMeshProObject" + number);
    TextMeshPro textMeshPro = textObject.AddComponent<TextMeshPro>();
    textObject.layer = LayerMask.NameToLayer("Camera");

    // Set the font asset and material for the TextMeshPro component
    textMeshPro.font = fontAssets[number];

    Material material = fontAssets[number].material;

    material.SetColor(ShaderUtilities.ID_FaceColor, color * 1.5f); // 1.5f = 1.0f  2.25f = 1.5f & 3.0f = 2.0f
    material.SetFloat(ShaderUtilities.ID_FaceDilate, 0.1f);
    //material.SetColor(ShaderUtilities.ID_OutlineColor, color * 0.1f);
    //material.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.01f);
    //material.SetFloat(ShaderUtilities.ID_OutlineSoftness, 0.2f);

    //Color modifiedGlowColor = color;
    //modifiedGlowColor *= 12.0f;

    //material.SetColor(ShaderUtilities.ID_GlowColor, modifiedGlowColor);
    //material.SetFloat(ShaderUtilities.ID_GlowOuter, 0.01f);

    // Set the text and other properties
    textMeshPro.text = text;
    textMeshPro.fontSize = 100;
    textMeshPro.rectTransform.SetParent(this.transform);

    textMeshPro.transform.localPosition = new Vector3(100.0f, -number * 10.0f, 0.0f);

    RectTransform textTransform = textMeshPro.GetComponent<RectTransform>();
    textTransform.sizeDelta = new Vector2(200.0f, 100.0f);
  }
}
