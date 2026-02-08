using UnityEngine;
using UnityEngine.EventSystems;

public class ModalWindowBackground : MonoBehaviour, IPointerClickHandler
{
  public void OnPointerClick(PointerEventData eventData)
  {
    Destroy(transform.parent.gameObject);
  }
}
