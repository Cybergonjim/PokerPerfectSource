using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static FunctionsNameSpace.Functions;
using static JsonControl;

public class ChipControl : MonoBehaviour
{
  public ChipObject chipBase;
  public Material sideMaterialPrefab;
  public Material faceMaterialPrefab;

  public static List<ChipObject> ChipPrefabs { get; set; } = new();

  private Chips chipData;

  public static Color IntToColor(int colorValue)
  {
    byte r = (byte)((colorValue >> 16) & 0xFF);
    byte g = (byte)((colorValue >> 8) & 0xFF);
    byte b = (byte)(colorValue & 0xFF);

    return new Color32(r, g, b, 255);
  }

  void SetColor(Material material, int color, string property)
  {
    Color convertedColor = IntToColor(color);
    material.SetColor(property, convertedColor);
  }

  void SetMaterial(Material material, Chip chip)
  {
    SetColor(material, chip.colorBase, "_colorBase");
    SetColor(material, chip.colorSpoke, "_colorSpoke");
    SetColor(material, chip.colorDot, "_colorDot");
  }

  void SetChipMaterialColor(ChipObject chipObject, Chip chip)
  {
    Material sideMaterial = Instantiate(sideMaterialPrefab);
    SetMaterial(sideMaterial, chip);

    chipObject.body.GetComponent<MeshRenderer>().sharedMaterial = sideMaterial;

    Material faceMaterial = Instantiate(faceMaterialPrefab);
    SetMaterial(faceMaterial, chip);

    chipObject.top.GetComponent<MeshRenderer>().sharedMaterial = faceMaterial;
    chipObject.bottom.GetComponent<MeshRenderer>().sharedMaterial = faceMaterial;

    chipObject.denomination = chip.denomination;

    chipObject.topText.text = ConvertDenomination(chip.denomination);
    chipObject.bottomText.text = ConvertDenomination(chip.denomination);
  }

  void CreateChipPrefabs()
  {
    foreach (Chip chip in FilteredChips)
    {
      ChipObject chipObject = Instantiate(chipBase, new Vector3(0, 0, 0), Quaternion.identity);

      chipObject.transform.parent = transform;
      chipObject.index = FilteredChips.IndexOf(chip);
      chipObject.name = $"ChipPrefab #{FilteredChips.IndexOf(chip)}";

      SetChipMaterialColor(chipObject, chip);

      ChipPrefabs.Add(chipObject);
    }

    // this sorts from lowest to highest denomination
    ChipPrefabs.Sort((a, b) => a.denomination.CompareTo(b.denomination));
  }

  void Start()
  {
    CreateChipPrefabs();
  }
}
