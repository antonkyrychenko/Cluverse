using System;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
  public static SettingsManager Instance;

  public bool MusicOn;

  public bool VibrationOn;

  public Action<bool> OnMusicChange;

  public Action<bool> OnVibrationChange;

  private const string VibrationKey = "VibrationOn";

  private const string MusicKey = "MusicOn";

  public void Awake()
  {
    Instance = this;
    MusicOn = PlayerPrefs.GetInt(MusicKey) == 1;
    VibrationOn = PlayerPrefs.GetInt(VibrationKey) == 1;
  }

  public void SetVibration(bool vibrationOn)
  {
    AudioManager.Instance.PlayToggle();
    VibrationOn = vibrationOn;
    PlayerPrefs.SetInt(VibrationKey, vibrationOn ? 1 : 0);
    OnVibrationChange?.Invoke(vibrationOn);
  }

  public void SetMusic(bool musicOn)
  {
    MusicOn = musicOn;
    PlayerPrefs.SetInt(MusicKey, musicOn ? 1 : 0);
    OnMusicChange?.Invoke(musicOn);
    AudioManager.Instance.PlayToggle();
  }
}
