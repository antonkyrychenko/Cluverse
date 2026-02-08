using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
  public static ScoreManager Instance;

  public int CurrentScore { get; private set; }
  public int BestScore { get; private set; }

  private const string BEST_SCORE_KEY = "BEST_SCORE";

  public event Action<int> OnScoreChanged;

  public event Action<int> OnBestScoreChanged;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);

    BestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
    OnBestScoreChanged?.Invoke(BestScore);
  }

  public void AddScore(int amount)
  {
    CurrentScore += amount;

    OnScoreChanged?.Invoke(CurrentScore);
    CheckForBestScore();
  }

  public void ResetScore()
  {
    CurrentScore = 0;
    OnScoreChanged?.Invoke(CurrentScore);
  }

  public void CheckForBestScore()
  {
    if (CurrentScore > BestScore)
    {
      BestScore = CurrentScore;
      PlayerPrefs.SetInt(BEST_SCORE_KEY, BestScore);
      PlayerPrefs.Save();
      OnBestScoreChanged?.Invoke(CurrentScore);
    }
  }
}
