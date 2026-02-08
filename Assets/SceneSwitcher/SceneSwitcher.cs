using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
  public static SceneSwitcher Isntance;

  public void Awake()
  {
    Isntance = this;
  }

  public void LoadSceneByName(string sceneName)
  {
    SceneManager.LoadScene(sceneName);
  }

  public void LoadSceneByIndex(int sceneIndex)
  {
    SceneManager.LoadScene(sceneIndex);
  }
}
