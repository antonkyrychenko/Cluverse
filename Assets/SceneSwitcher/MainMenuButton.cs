public class MainMenuButton : SceneSwitcherButton
{
  protected override void OnButtonClick()
  {
    AudioManager.Instance.PlayToggle();
    sceneName = "MainMenu";
    base.OnButtonClick();
  }
}
