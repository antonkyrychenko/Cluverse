using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GridBackground : MonoBehaviour
{
  void Start()
  {
    Fit();
  }

  void Fit()
  {
    Camera cam = Camera.main;
    SpriteRenderer sr = GetComponent<SpriteRenderer>();

    float worldScreenHeight = cam.orthographicSize * 2f;
    float worldScreenWidth = worldScreenHeight * cam.aspect * 0.99f;

    Vector2 spriteSize = sr.sprite.bounds.size;

    Vector3 scale = transform.localScale;
    scale.x = worldScreenWidth / spriteSize.x;
    scale.y = scale.x;

    transform.localScale = scale;
  }
}
