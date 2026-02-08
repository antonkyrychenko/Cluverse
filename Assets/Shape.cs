using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Shape : MonoBehaviour
{
  public GameObject blockPrefab;
  public Vector2Int[] cells;
  public float cellSize = 1;

  private readonly List<GameObject> blocks = new();

  public void Start()
  {
    Build();
  }

  private void Build()
  {
    foreach (var b in blocks)
      DestroyImmediate(b);

    blocks.Clear();

    Vector2 center = CalculateCenter(cells);

    foreach (Vector2Int cell in cells)
    {
      Vector2 localPos = (cell - center) * cellSize;

      GameObject block = Instantiate(blockPrefab, transform);
      block.transform.localPosition = localPos;
      block.transform.localScale = Vector3.one * cellSize;

      var spriteRenderer = block.GetComponent<SpriteRenderer>();
      spriteRenderer.sortingLayerID = SortingLayer.NameToID("GridShapes");

      blocks.Add(block);
    }
  }

  public Vector2 GetCellWorldPosition(
      Vector2 shapeWorldCenter,
      Vector2Int[] shapeCells,
      float cellSize)
  {
    Vector2 center = CalculateCenter(shapeCells);

    var zeroCell = new Vector2(0, 0);

    return shapeWorldCenter + (zeroCell - center) * cellSize;
  }

  public Vector2 CalculateCenter(Vector2Int[] shapeCells)
  {
    Vector2 sum = Vector2.zero;
    foreach (var c in shapeCells) sum += c;

    return sum / shapeCells.Length;
  }
}
