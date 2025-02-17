using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static FunctionsNameSpace.Functions;
using static RankHands.RankUtil;

public class CardObject : MonoBehaviour, IPointerClickHandler
{
  public GameObject backObject;
  public GameObject faceObject;

  const float degreesInRotation = 360.0f;

  public Material RankMaterial { get; set; }

  public Material SymbolMaterial { get; set; }

  public RankTypes RankType { get; set; }

  public SuitTypes SuitType { get; set; }

  public static float Lerp(float a, float b, float t) => a + t * (b - a);

  public static float Wrap(float a, float b) => (a + b) % b;

  public IEnumerator MoveAndRotate(CardObject target, int index, Vector3 direction)
  {
    const float offset = 0.05f;
    float xOffset = direction.x * offset;
    float yOffset = direction.y * offset;
    const float travelTime = 0.5f;
    const float numberOfRotations = 2.0f;

    Vector3 newPosition = new Vector3(target.transform.position.x + index * xOffset,
      target.transform.position.y + index * yOffset,
      target.transform.position.z + index * offset / 10.0f);

    Vector3 startPosition = transform.position;

    float startTime = Time.time;
    float startAngle = transform.eulerAngles.z;
    float targetAngle = target.transform.eulerAngles.z;

    // Calculate the target rotation based on the desired number of rotations
    float targetRotation = Wrap(targetAngle - startAngle, degreesInRotation) + degreesInRotation * numberOfRotations;

    while (Time.time - startTime < travelTime)
    {
      float journeyTime = (Time.time - startTime) / travelTime;

      transform.position = Vector3.Lerp(startPosition, newPosition, journeyTime);

      // calculate the incremental rotation to apply
      float currentRotation = Lerp(0, targetRotation, journeyTime);
      transform.eulerAngles = new Vector3(0, 0, startAngle + currentRotation);

      yield return null;
    }

    transform.SetPositionAndRotation(newPosition, target.transform.rotation);
  }

  public IEnumerator TurnOver(float duration)
  {
    Quaternion startRotation = transform.rotation;
    Quaternion endRotation = transform.rotation * Quaternion.Euler(0, degreesInRotation / 2.0f, 0);

    float elapsed = 0f;

    while (elapsed < duration)
    {
      transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsed / duration);
      elapsed += Time.deltaTime;
      yield return null;
    }

    transform.rotation = endRotation;
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    transform.localScale *= 1.05f;

    PlayMakerFSM.BroadcastEvent(EventTypes.DealerBeginEvent.ToString());

    StartCoroutine(WaitForFunction());
  }

  IEnumerator WaitForFunction()
  {
    yield return new WaitForSeconds(0.25f);
    transform.localScale /= 1.05f;
  }
}
