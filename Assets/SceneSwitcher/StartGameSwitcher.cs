public class StartGameSwitcher : SceneSwitcherButton
{
  protected override void OnButtonClick()
  {
    AudioManager.Instance.PlayGameStart();
    sceneName = "Game";
    base.OnButtonClick();
  }
}
