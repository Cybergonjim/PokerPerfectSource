using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static FunctionsNameSpace.Functions;
using static GameUtil;

public class RackObject : MonoBehaviour
{
  public StackObject stackPrefab;
  
  private int amountCurrent;
  public int AmountTarget { get; set; }

  private List<StackObject> stackObjects = new();
  const int stackCount = 7;

  void Start()
  {
    StartCoroutine(CreateStacks());
  }

  void Update()
  {
    if (stackObjects.Count < stackCount)
      return;

    if (amountCurrent != AmountTarget)
    {
      List<int> stacks = DistributeChips(AmountTarget);

      int stackCount = stacks.Count(stack => stack != 0);

      foreach (StackObject stackObject in stackObjects)
      {
        int index = stackObjects.IndexOf(stackObject);

        stackObject.AmountTarget = stacks[index];
        stackObject.StackCount = stackCount;
        stackObject.StackTarget = index - (7 - stackCount);
        stackObject.ParentChanged = true;
      }

      amountCurrent = AmountTarget;
    }
  }

  IEnumerator CreateStacks()
  {
    for (int i = 0; i < stackCount; i++)
    {
      int index = stackCount - i + ChipIndex - 2;

      StackObject stackObject = Instantiate(stackPrefab, Vector3.zero, Quaternion.identity);

      stackObject.name = $"Stack #{i}";
      stackObject.transform.parent = transform;
      stackObject.transform.localPosition = Vector3.zero;

      stackObject.chipPrefab = ChipControl.ChipPrefabs[index];
      stackObject.AmountTarget = 0;
      stackObject.ParentChanged = false;

      stackObjects.Add(stackObject);

      yield return null;
    }

    transform.Rotate(new Vector3(0, 0, transform.parent.rotation.eulerAngles.z));

    yield return null;
  }
}
