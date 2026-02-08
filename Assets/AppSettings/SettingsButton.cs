using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsButton : MonoBehaviour, IPointerClickHandler
{
  public GameObject SettingsPrefab;

  public void OnPointerClick(PointerEventData eventData)
  {
    AudioManager.Instance.PlaySettings();
    Instantiate(SettingsPrefab, transform.parent);
  }
}
