using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static FunctionsNameSpace.Functions;
using static GameUtil;
using static TMPro.ShaderUtilities;

public class PlayerControl : MonoBehaviour
{
  public List<TMP_FontAsset> fontAssets; // Reference to your font asset
  public PlayerObject playerPrefab;
  public GameObject table;

  public static float xTableSize;
  public static float yTableSize;
  public static float xOffset;
  public static float yOffset;

  private readonly List<Material> textMaterials = new();

  private const int playerCount = 10;

  // more colors https://www.rapidtables.com/web/color/RGB_Color.html
  private List<Color> colors = new()
  {
    FromArgb(255, 0, 0), // - Red
    FromArgb(0, 255, 0), //- Green
    FromArgb(0, 0, 255), //- Blue
    FromArgb(255, 255, 0), //- Yellow
    FromArgb(255, 0, 255), //- Magenta
    FromArgb(0, 255, 255), //- Cyan
    FromArgb(128, 0, 128), //- Purple
    FromArgb(255, 165, 0), //- Orange
    FromArgb(0, 128, 0), //- Dark Green
    FromArgb(128, 255, 0), //- Lime Green
    FromArgb(0, 255, 128), //- Sea Green
    FromArgb(0, 128, 255), //- Sky Blue
    FromArgb(128, 0, 255), //- Purple
    FromArgb(255, 0, 128) //- Deep Pink
  };

  private static Color FromArgb(int r, int g, int b)
  {
    Color color = new Color(r / 255.0f, g / 255.0f, b / 255.0f);

    return color;
  }

  void CreatePlayer()
  {
    PlayerObject playerObject = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

    playerObject.transform.parent = transform;
    playerObject.Index = PlayerObjects.Count;
    playerObject.TextMaterial = textMaterials[PlayerObjects.Count];

    playerObject.SetupPlayerObject();

    PlayerObjects.Add(playerObject);
  }

  void CreatePlayers()
  {
    // Add extra empty seats if player.Count < 10
    while (FilteredPlayers.Count < playerCount)
      FilteredPlayers.Add(new Player { name = "Empty Seat", amount = 0 });

    Randomize(FilteredPlayers);

    xTableSize = table.transform.localScale.x;
    yTableSize = table.transform.localScale.y;

    for (int i = 0; i < playerCount; i++)
      CreatePlayer();
  }

  private void CreateMaterial()
  {
    Material material = fontAssets[textMaterials.Count].material;

    // the multiply adds glow to text items
    material.SetColor(ID_FaceColor, colors[textMaterials.Count] * 1.75f); // 1.5f = 1.0f  2.25f = 1.5f & 3.0f = 2.0f
    material.SetFloat(ID_FaceDilate, 0.1f);

    textMaterials.Add(material);

    Randomize(textMaterials);
  }

  private void CreateMaterials()
  {
    for (int i = 0; i < fontAssets.Count; i++)
      CreateMaterial();
  }

  void Start()
  {
    CreateMaterials();
    CreatePlayers();
  }
}