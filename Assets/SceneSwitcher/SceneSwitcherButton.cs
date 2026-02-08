using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneSwitcherButton : MonoBehaviour
{
  public string sceneName;

  private Button button;

  void Awake()
  {
    button = GetComponent<Button>();
    button.onClick.AddListener(OnButtonClick);
  }

  protected virtual void OnButtonClick()
  {
    if (SceneSwitcher.Isntance != null)
    {
      ScoreManager.Instance.ResetScore();
      SceneSwitcher.Isntance.LoadSceneByName(sceneName);
    }
    else
    {
      Debug.LogError("SceneSwitcher is not assigned!");
    }
  }
}
