using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Toggle))]
public class VibrationToggle : MonoBehaviour
{
  public UnityEngine.UI.Toggle Toggle;

  public void Awake()
  {
    Toggle = GetComponent<UnityEngine.UI.Toggle>();
    Toggle.isOn = SettingsManager.Instance.VibrationOn;
  }

  public void Start()
  {
    Toggle.onValueChanged.AddListener(SettingsManager.Instance.SetVibration);
  }
}
