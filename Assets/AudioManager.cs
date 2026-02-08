using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public static AudioManager Instance;

  public AudioSource source;

  public AudioClip SettingsSound;

  public AudioClip ToggleSound;

  public AudioClip PlaceShapeSound;

  public AudioClip GameOverSound;

  public AudioClip GameStartSound;

  void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
  }

  public void Start()
  {
    source.enabled = SettingsManager.Instance.MusicOn;
    SettingsManager.Instance.OnMusicChange += OnMusiChange;
  }

  public void OnMusiChange(bool musicOn)
  {
    source.enabled = musicOn;
  }

  public void PlaySettings()
  {
    source.PlayOneShot(SettingsSound);
  }

  public void PlayToggle()
  {
    source.PlayOneShot(ToggleSound);
  }

  public void PlaceShape()
  {
    source.PlayOneShot(PlaceShapeSound);
  }

  public void PlayGameOver()
  {
    source.PlayOneShot(GameOverSound, 0.3f);
  }

  public void PlayGameStart()
  {
    source.PlayOneShot(GameStartSound, 0.3f);
  }

  public void OnDestroy()
  {
    SettingsManager.Instance.OnMusicChange -= OnMusiChange;
  }
}
