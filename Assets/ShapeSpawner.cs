using System;
using System.Linq;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{
  public GameObject[] shapePrefabs;
  private GameObject currentShape;

  public Action OnPlaced;

  public bool IsEmpty => currentShape == null;

  private void Start()
  {
    SpawnShape(GetRandomShape());
  }

  public void Spawn()
  {
    SpawnShape(GetRandomShape());
  }

  private GameObject GetRandomShape()
  {
    int index = UnityEngine.Random.Range(0, shapePrefabs.Length);
    GameObject prefab = shapePrefabs[index];

    return prefab;
  }

  private void SpawnShape(GameObject prefab)
  {
    if (currentShape != null) return;
    if (shapePrefabs.Length == 0 || GridManager.Instance == null) return;

    currentShape = Instantiate(prefab, transform.position, Quaternion.identity);
    var draggableShape = currentShape.GetComponent<DraggableShape>();
    if (draggableShape) draggableShape.cellSize = GridManager.Instance.cellSize;

    var shape = currentShape.GetComponent<Shape>();
    shape.cellSize = GridManager.Instance.cellSize;

    var drag = currentShape.GetComponent<DraggableShape>();
    drag.shapeSpawner = this;
  }

  public Vector2Int[] GetShapeCells()
  {
    var shape = currentShape?.GetComponent<Shape>();

    return shape?.cells ?? Enumerable.Empty<Vector2Int>().ToArray();
  }

  public void OnShapePlaced()
  {
    currentShape = null;
    OnPlaced?.Invoke();
  }
}
