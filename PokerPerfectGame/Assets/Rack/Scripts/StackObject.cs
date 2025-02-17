using System.Collections.Generic;
using UnityEngine;
using static FunctionsNameSpace.Functions;

public class StackObject : MonoBehaviour
{
  public ChipObject chipPrefab;
  public List<ChipObject> chipObjects;

  private const float scale = 1.0f;
  public const float chipSize = 5.7f;

  // these are set by rackObject
  public int AmountCurrent { get; set; }
  public int AmountTarget { get; set; }
  public int StackTarget { get; set; }
  public int StackCount { get; set; }
  public bool ParentChanged { get; set; }

  // this function returns position of where the chip is supposed to be based on it index position
  //  30 ---          max chip is at top
  //  X     ...       if there are X chips in stack, new ones are added in top of those
  //  0        ___    chip #0 is at bottom
  private Vector3 GetPosition(int index)
  {
    float xOffset = 6.5f;
    float yOffset = 0.5f;
    float zOffset = 0.60f;

    float ratio = index / (float)stackSize * scale;

    float startingPosition = (float)(StackCount - 1) / 2.0f;

    float x = (StackTarget - startingPosition) * (xOffset + ratio);  // + randX;
    float y = (stackSize - index) * -yOffset;  // + randY;
    float z = (stackSize - index) * zOffset - 10.0f;

    return new Vector3(x, y, z);
  }

  // this function returns perspective scale of a chip where the top ones are larger than lower ones
  private Vector3 GetScale(int index)
  {
    float ratio = (float)(stackSize - index) / (float)stackSize * scale;

    return new Vector3(chipSize - ratio, chipSize, chipSize - ratio);
  }

  private void CreateChips()
  {
    for (int i = 0; i < stackSize; i++)
    {
      ChipObject chipObject = Instantiate(chipPrefab, new Vector3(0, 0, 0), Quaternion.Euler(90.0f, 0, 0));

      chipObject.name = $"Chip #{i}";
      chipObject.transform.parent = transform;
      chipObject.transform.localScale = GetScale(i);
      chipObject.targetPosition = chipObject.transform.localPosition = GetPosition(i);
      chipObject.transform.Rotate(0, Random.Range(0.0f, 360.0f), 0, Space.Self);
      chipObject.transform.gameObject.SetActive(false);

      chipObjects.Add(chipObject);
    }
  }

  void Start()
  {
    CreateChips();
  }

  void Update()
  {
    if ((AmountCurrent == AmountTarget) && (!ParentChanged))
      return;

    ParentChanged = false;

    for (int i = 0; i < stackSize; i++)
      chipObjects[i].transform.gameObject.SetActive(i < AmountTarget);

    AmountCurrent = AmountTarget;

    // the chipObject will move these into proper location during update
    for (int i = 0; i < stackSize; i++)
      chipObjects[i].targetPosition = GetPosition(i);
  }
}