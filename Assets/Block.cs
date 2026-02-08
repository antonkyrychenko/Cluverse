using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
  [SerializeField] private float fadeDuration = 0.25f;

  private SpriteRenderer[] renderers;

  private void Awake()
  {
    // Supports sprites on children too
    renderers = GetComponentsInChildren<SpriteRenderer>();
  }

  public void FadeAndDestroy()
  {
    StartCoroutine(FadeCoroutine());
  }

  private IEnumerator FadeCoroutine()
  {
    float t = 0f;

    Color[] startColors = new Color[renderers.Length];
    for (int i = 0; i < renderers.Length; i++)
      startColors[i] = renderers[i].color;

    while (t < fadeDuration)
    {
      t += Time.deltaTime;
      float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);

      for (int i = 0; i < renderers.Length; i++)
      {
        if (renderers[i] != null)
        {
          Color c = startColors[i];
          renderers[i].color = new Color(c.r, c.g, c.b, alpha);
        }
      }

      yield return null;
    }

    Destroy(gameObject);
  }
}
