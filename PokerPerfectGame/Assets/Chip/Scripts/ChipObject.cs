using UnityEngine;
using System.Collections;
using TMPro;
using static FunctionsNameSpace.Functions;

public class ChipObject : MonoBehaviour
{
  public Vector3 targetPosition;
  public int denomination;
  public Rigidbody rigidBody;
  public bool moving = false;
  public GameObject body;
  public GameObject top;
  public GameObject bottom;
  public TMP_Text topText;
  public TMP_Text bottomText;
  public int index;

  void Update()
  {
    if (transform.localPosition != targetPosition && !moving)
      StartCoroutine(MoveChip(0.1f));
  }

  public IEnumerator MoveChip(float time)
  {
    //moving = true;

    Vector3 startPosition = transform.localPosition;

    float elapsedTime = 0;

    while (elapsedTime < time)
    {
      transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / time);
      elapsedTime += Time.deltaTime;

      yield return null;
    }

    transform.localPosition = targetPosition;

    //moving = false;
  }
}
