using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Toggle))]
public class SoundToggle : MonoBehaviour
{
  public bool MusicOn;

  public UnityEngine.UI.Toggle Toggle;

  public void Awake()
  {
    Toggle = GetComponent<UnityEngine.UI.Toggle>();
    Toggle.isOn = SettingsManager.Instance.MusicOn;
  }

  public void Start()
  {
    Toggle.onValueChanged.AddListener(SettingsManager.Instance.SetMusic);
  }
}
