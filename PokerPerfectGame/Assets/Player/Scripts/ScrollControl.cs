using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollControl : MonoBehaviour
{
  public GameObject[] scrollCorners;
  public GameObject scrollMiddle;

  public float scrollMiddleScale;
  public float scrollCornerScale;
  public float scrollCornerXPos;
  public float scrollCornerYPos;

  // Start is called before the first frame update
  void Start()
    {
      float signX;
      float signY;

//      scrollMiddle.transform.localScale = new Vector3(scrollMiddleScale, scrollMiddleScale, 1);

    for (int i = 0; i < scrollCorners.Length; i++) 
    {
      signX = Mathf.Sign(scrollCorners[i].transform.localScale.x);
      signY = Mathf.Sign(scrollCorners[i].transform.localScale.y);

      scrollCorners[i].transform.localScale = new Vector3(scrollCornerScale * signX, scrollCornerScale * signY, 1);
      scrollCorners[i].transform.position = new Vector3(scrollCornerXPos * -signX, scrollCornerYPos * signY, 0);
    }
  }

    // Update is called once per frame
    void Update()
    {
        
    }
}
