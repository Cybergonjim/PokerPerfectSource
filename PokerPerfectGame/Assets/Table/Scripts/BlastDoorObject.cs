using System.Collections;
using UnityEngine;

public class BlastDoorObject : MonoBehaviour
{
  public Vector3 distance;

  public IEnumerator MoveToSpot()
  {
    Vector3 GotoPosition = new Vector3(transform.position.x + distance.x, transform.position.y + distance.y, transform.position.z);
    Vector3 currentPos = transform.position;
    float waitTime = 1.0f;
    float elapsedTime = 0;

    while (elapsedTime < waitTime)
    {
      transform.position = Vector3.Lerp(currentPos, GotoPosition, (elapsedTime / waitTime));
      elapsedTime += Time.deltaTime;

      yield return null;
    }

    transform.position = GotoPosition;

    yield return null;

    GetComponent<MeshRenderer>().enabled = false;
  }
}
