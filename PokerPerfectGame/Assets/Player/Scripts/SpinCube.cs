using UnityEngine;

public class SpinCube : MonoBehaviour
{
  public Vector3 RotateAmount;
  public GameObject quad;
  public float add = 0.29f;
  public float multiply = 0.145f;

  private PotObject potObject;

  const int textCount = 10;

  void Start()
  {
    potObject = GetComponentInParent<PotObject>();
  }

  void Update()
  {
    transform.Rotate(RotateAmount);

    // this moves the names to center of pot
    if (quad != null)
      quad.transform.localPosition = new Vector3(0, 0, add + multiply * (textCount - potObject.PotPlayerObjects.Count) / 2);
  }
}
