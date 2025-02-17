using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeButtonColor : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
  public bool isLeft;

  private MaterialApplier materialApplier;
  private const float compression = 0.9f;
  private const float duration = 0.5F;

  private ButtonObject button;
  private PlayerObject player;

  void Awake()
  {
    materialApplier = GetComponent<MaterialApplier>();
  }


  public void OnPointerDown(PointerEventData eventData)
  {
    materialApplier.ApplyOther();
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    materialApplier.ApplyOriginal();
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    if (button == null)
      button = transform.parent.transform.parent.transform.gameObject.GetComponent<ButtonObject>();

    if (button == null)
    {
      Debug.LogError("Button reference is not assigned!");
    }

    if (player == null)
      player = button.transform.parent.transform.gameObject.GetComponent<PlayerObject>();

    if (player == null)
    {
      Debug.LogError("Player reference is not assigned!");
    }

    button.buttonBody.transform.localScale *= compression;

    StartCoroutine(WaitForFunction());
  }

  IEnumerator WaitForFunction()
  {
    yield return new WaitForSeconds(duration);

    button.buttonBody.transform.localScale /= compression;
    player.ProcessButtons(button.buttonType, isLeft);
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    materialApplier.ApplyHover();
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    materialApplier.ApplyOriginal();
  }
}
