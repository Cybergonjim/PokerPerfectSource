using UnityEngine;
using TMPro;
using System.Collections.Generic;
using static FunctionsNameSpace.Functions;

public class ButtonObject : MonoBehaviour
{
  public TMP_Text buttonText;
  public GameObject buttonBody;
  public GameObject blocker;
  public Vector3 position;
  public int index;
  public ButtonTypes buttonType;

  public readonly List<Color> colors = new()
  {
    new Color(0.94f, 0.00f, 0.00f),
    new Color(0.94f, 0.29f, 0.00f),
    new Color(0.94f, 0.52f, 0.00f),
    new Color(0.73f, 0.81f, 0.00f),
    new Color(0.30f, 0.67f, 0.00f),
    new Color(0.00f, 0.35f, 0.29f),
    new Color(0.00f, 0.20f, 0.45f),
    new Color(0.27f, 0.00f, 0.43f),
    new Color(0.61f, 0.00f, 0.33f)
  };

  public readonly List<string> names = new() 
  { 
    "PEEK", 
    "STATUS", 
    "FOLD", 
    "CALL", 
    "RAISE", 
    "RESET", 
    "-", 
    "+", 
    "ALL-IN" 
  };

  void Start()
  {
    name = names[index].ToString();
    tag = names[index].ToString();
    transform.localPosition = position;

    // set names
    buttonText.text = names[index].ToString();

    // set shader colors
    buttonBody.GetComponent<Renderer>().material.SetColor("_BaseColor", colors[index]);
  }
}
