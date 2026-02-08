using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DraggableShape : MonoBehaviour
{
  public ShapeSpawner shapeSpawner;

  private Rigidbody2D rb;
  private Vector2 offset;
  private Camera cam;
  private bool dragging;
  private Vector3? originalPosition;
  private GameObject ghostParent;
  private Vector2Int[] shapeCells;
  public GameObject previewPrefab;
  public float cellSize;
  private List<GameObject> ghostBlocks = new();

  void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    cam = Camera.main;
  }

  void Start()
  {
    transform.localScale = Vector2.one / 1.5f;

    var shape = GetComponent<Shape>();
    this.shapeCells = shape.cells;
  }

  void OnMouseDown()
  {
    originalPosition = transform.position;
    transform.localScale = Vector2.one;

    Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
    offset = rb.position - mouseWorld;
    dragging = true;
  }

  void OnMouseUp()
  {
    dragging = false;

    if (ghostParent == null)
    {
      if (originalPosition != null)
      {
        transform.localScale = Vector2.one / 1.5f;
        transform.position = originalPosition.Value;
        originalPosition = null;
        Destroy(ghostParent);
      }

      return;
    }

    var shape = GetComponent<Shape>();
    var zeroCellPosition = shape.GetCellWorldPosition(ghostParent.transform.position, shape.cells, shape.cellSize);

    Vector2Int baseGridPos = GridManager.Instance.WorldToGrid(zeroCellPosition);
    if (GridManager.Instance.CanPlaceShape(baseGridPos, shapeCells))
    {
      GridManager.Instance.PlaceShape(shapeSpawner, baseGridPos, shapeCells);
      Destroy(ghostParent);
      Destroy(gameObject);
    }
    else
    {
      if (originalPosition != null)
      {
        transform.localScale = Vector2.one / 1.4f;
        transform.position = originalPosition.Value;
        originalPosition = null;
        Destroy(ghostParent);
      }
    }
  }

  public Vector3 GetLowestBlock(Transform ghostParent)
  {
    Transform lowest = null;
    float lowestY = float.PositiveInfinity;

    foreach (Transform child in ghostParent)
    {
      if (child.position.y < lowestY)
      {
        lowestY = child.position.y;
        lowest = child;
      }
    }

    return lowest.position;
  }

  void FixedUpdate()
  {
    if (!dragging) return;

    Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
    rb.MovePosition(mouseWorld + offset);

    var shape = GetComponent<Shape>();
    var zeroCellPosition = shape.GetCellWorldPosition(mouseWorld + offset, shape.cells, shape.cellSize);
    var gridPosition = GridManager.Instance.WorldToGrid(zeroCellPosition);
    var isInsideGrid = GridManager.Instance.IsInsideGrid(gridPosition.x, gridPosition.y);
    var canPlaceShape = GridManager.Instance.CanPlaceShape(gridPosition, shapeCells);
    if (isInsideGrid && canPlaceShape)
    {
      if (ghostParent == null) CreateGhost();

      var placement = GridManager.Instance.GetShapePlacement(gridPosition, shapeCells);
      var placementCenter = CalculateCenter(placement.ToArray());
      ghostParent.transform.position = placementCenter;
    }

    if (!isInsideGrid)
    {
      Destroy(ghostParent);
    }
  }

  Vector3 CalculateCenter(IList<Vector3> positions)
  {
    Vector3 sum = Vector3.zero;

    foreach (var p in positions)
      sum += p;

    return sum / positions.Count;
  }

  private void CreateGhost()
  {
    if (ghostParent != null)
    {
      Destroy(ghostParent);
    }

    ghostParent = new GameObject("GhostPreview");
    ghostParent.transform.SetParent(transform.parent); // same parent as shape 

    foreach (var cell in shapeCells)
    {
      Vector2 localPos = (cell - CalculateCenter(shapeCells)) * cellSize;
      GameObject ghostBlock = Instantiate(previewPrefab, ghostParent.transform);
      ghostBlock.transform.localPosition = localPos;
      ghostBlock.transform.localScale = Vector3.one * cellSize; // Make it semi-transparent
      SpriteRenderer sr = ghostBlock.GetComponent<SpriteRenderer>();
      if (sr != null)
      {
        sr.color = new Color32(166, 166, 166, 255);
        sr.sortingOrder = 5;
      }

      ghostBlocks.Add(ghostBlock);
    }
  }

  private Vector2 CalculateCenter(Vector2Int[] shapeCells)
  {
    Vector2 sum = Vector2.zero;
    foreach (var c in shapeCells) sum += c;
    return sum / shapeCells.Length;
  }
}
