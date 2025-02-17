using System.Collections.Generic;
using UnityEngine;
using static GameUtil;
using static DealerUtil;
using static RankHands.RankUtil;
using System.Collections;
using HutongGames.PlayMaker;
using static FunctionsNameSpace.Functions;

public class DealerObject : MonoBehaviour
{
  public CardObject cardPrefab;
  public CardObject landingPrefab;
  public List<Material> faces;
  public List<Material> backs;
  public Material sharedMaterial;
  public GameObject landingTarget;
  public GameObject burnTarget;
  public Material noneMaterial;

  private List<CardObject> landingTargets = new();
  public List<CardObject> LandingTargets {  get { return landingTargets; } }
  
  private List<CardObject> burnTargets = new();
  public List<CardObject> BurnTargets { get { return burnTargets; } }

  private readonly List<string> textures = new() { "_Red", "_Black", "_Green", "_Blue" };
  
  public List<Material> rankMaterials;
  public List<Material> symbolMaterials;

  public List<CardObject> HistoryCards { get; set; } = new(5);

  public Fsm Fsm => GetComponent<PlayMakerFSM>().Fsm;

  private int cardBackIndex;
  private int CardBackIndex
  {
    get => cardBackIndex;
    set => cardBackIndex = value % textures.Count;
  }

  private void CreateCards()
  {
    const int cardSpacing = 5;
    const int midPosition = cardSpacing / 2;
    const int offset = 23;

    for (int i = 0; i < CardCount; i++)
    {
      // this places cards face down below and midway on table
      CardObject cardObject = Instantiate(cardPrefab);

      cardObject.transform.parent = transform;

      cardObject.RankType = (RankTypes)(i % RankCount);
      cardObject.SuitType = (SuitTypes)(i / RankCount);
      cardObject.name = $"{cardObject.RankType} of {cardObject.SuitType}";

      cardObject.faceObject.GetComponent<Renderer>().material = faces[i];

      cardObject.RankMaterial = rankMaterials[(int)cardObject.SuitType];
      cardObject.SymbolMaterial = symbolMaterials[(int)cardObject.SuitType];

      CardObjects.Add(cardObject);
    }

    Randomize(CardObjects);

    int j = 0;

    foreach (CardObject cardObject in CardObjects)
    {
      float xPosition = -CardObjects.Count * midPosition + j * cardSpacing - offset;
      Vector3 newPosition = new Vector3(xPosition, 0, j - 50);

      cardObject.transform.SetPositionAndRotation(newPosition, Quaternion.identity);

      j++;
    }

  }

  private void CreateLandingObjects()
  {
    const float cardSpacing = -15.0f;
    const float cardStart = 30.0f;
    const int landingCount = 5;

    for (int i = 0; i < landingCount; i++)
    {
      // this places cards face down below and midway on table
      CardObject cardObject = Instantiate(landingPrefab, new Vector3(cardStart + i * cardSpacing, 0, -8), Quaternion.Euler(0, 0, 0));

      cardObject.transform.parent = landingTarget.transform;

      cardObject.name = $"LandingTarget{i}";

      landingTargets.Add(cardObject);
    }
  }

  private void CreateBurnObjects()
  {
    const float cardSpacing = 1.0f;
    const float cardStart = 48.0f;
    const int burnCount = 24;
    const float yElevation = -0.5f;

    for (int i = 0; i < burnCount; i++)
    {
      // this places cards face down below and midway on table
      CardObject cardObject = Instantiate(landingPrefab, new Vector3(cardStart + i * cardSpacing, 0, i * yElevation), Quaternion.Euler(0, 0, 0));

      cardObject.transform.parent = burnTarget.transform;

      cardObject.name = $"BurnTarget{i}";

      burnTargets.Add(cardObject);
    }
  }

  void Start()
  {
    CardBackIndex = Random.Range(0, textures.Count);
    
    StartCoroutine(ChangeBackColor());

    CreateCards();

    CreateLandingObjects();

    CreateBurnObjects();
  }

  public IEnumerator ChangeBackColor()
  {
    float timeElapsed = 0f;
    const float duration = 1.0f;

    // this lerps between textures Green 0-1, Blue 1-2, Red 2-3, Black 3-4 
    while (timeElapsed < duration)
    {
      timeElapsed += Time.deltaTime;

      // Calculate the interpolation value using Lerp
      float journey = Mathf.Clamp01(timeElapsed / duration);

      // gives value of 0.0f - 1.0f 
      float lerp = Mathf.Lerp(0, 1.0f, journey) + CardBackIndex;

      // Update the material property value
      sharedMaterial.SetFloat("_Value", lerp);

      yield return null;
    }

    sharedMaterial.SetFloat("_Value", CardBackIndex++);
  }

}

