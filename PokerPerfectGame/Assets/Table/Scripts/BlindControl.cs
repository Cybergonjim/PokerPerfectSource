using UnityEngine;

public class BlindControl : MonoBehaviour
{
  public BlindObject blindObjectPrefab;
  public int positionX;
  public int positionY;

  void CreateBlinds()
  {
    for (int i = -1, count = 0; i <= 1; i += 2)
      for (int j = -1; j <= 1; j += 2)
      {
        BlindObject blindObject = Instantiate(blindObjectPrefab, new Vector3(i * positionX, j * positionY, 0), Quaternion.identity);
        blindObject.name = $"Blind #{count++}";
        blindObject.transform.parent = transform;
      }
  }

  void Start()
  {
    CreateBlinds();
  }
}
