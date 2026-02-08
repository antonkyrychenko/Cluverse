using System.Collections;
using TMPro;
using UnityEngine;

public class CurrentScoreText : MonoBehaviour
{
  [SerializeField] private TMP_Text scoreText;

  public float duration = 1.2f;

  private void Start()
  {
    AnimateScore(ScoreManager.Instance.CurrentScore);
  }

  public void AnimateScore(int finalScore)
  {
    StopAllCoroutines();
    StartCoroutine(CountUp(finalScore));
  }

  IEnumerator CountUp(int finalScore)
  {
    float elapsed = 0f;
    int startScore = 0;

    while (elapsed < duration)
    {
      elapsed += Time.deltaTime;
      float t = elapsed / duration;

      int currentScore = Mathf.RoundToInt(
          Mathf.Lerp(startScore, finalScore, t)
      );

      scoreText.text = currentScore.ToString();
      yield return null;
    }

    scoreText.text = finalScore.ToString();
  }
}
