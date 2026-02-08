using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreText : MonoBehaviour
{
  public float duration = 1.2f;

  [SerializeField] private TMP_Text scoreText;
  private int targetScore;
  private Coroutine animateCoroutine;
  private float displayedScore;


  private void Start()
  {
    ScoreManager.Instance.OnScoreChanged += OnScoreChanged;
  }

  private void OnScoreChanged(int newScore)
  {
    targetScore = newScore;
    if (animateCoroutine == null)
    {
      animateCoroutine = StartCoroutine(AnimateScoreCoroutine());
    }
  }

  private IEnumerator AnimateScoreCoroutine()
  {
    while (Mathf.RoundToInt(displayedScore) != targetScore)
    {
      displayedScore = Mathf.MoveTowards(
          displayedScore,
          targetScore,
          (targetScore - displayedScore) / duration * Time.deltaTime
      );

      scoreText.text = Mathf.RoundToInt(displayedScore).ToString();
      yield return null;
    }

    animateCoroutine = null;
  }

  void OnDestroy()
  {
    ScoreManager.Instance.OnScoreChanged -= OnScoreChanged;
  }
}
