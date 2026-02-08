using UnityEngine;
using TMPro;

public class BestScoreText : MonoBehaviour
{
  [SerializeField] private TMP_Text scoreText;

  private void Start()
  {
    UpdateUI(ScoreManager.Instance.BestScore);
    ScoreManager.Instance.OnBestScoreChanged += UpdateUI;
  }

  void UpdateUI(int score)
  {
    if (scoreText != null)
      scoreText.text = score.ToString();
  }
}
