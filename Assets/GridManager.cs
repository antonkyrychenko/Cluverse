using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GridManager : MonoBehaviour
{
  public static GridManager Instance;
  public ShapeSpawner ShapeSpawner;
  public SceneSwitcher SceneSwitcher;
  public GameObject cellPrefab;
  public GameObject noSpaceLeft;

  [Header("Grid Settings")]
  public int width = 8;
  public int height = 8;
  public float cellSize = 0.5f;
  public int paddingPercentage = 5;

  public Color gridLineColor = new(34, 139, 34);
  public float lineWidth = 0.05f;

  [Header("Background")]
  public Color backgroundColor = Color.green;
  private GameObject backgroundObj;

  private GameObject lineParent;
  [Header("References")]
  public GameObject blockPrefab; // Must have Block.cs attached

  private Vector2 gridOrigin; // bottom-left of grid
  private GameObject[,] grid;
  private GameObject cellParent;

  private void Awake()
  {
    if (Instance == null) Instance = this;
    else Destroy(gameObject);

    grid = new GameObject[width, height];
    CalculateCellSize();
    CenterGridOnScreen();
    DrawGridCells();
  }

  public bool HasAnyValidMove(Vector2Int[][] availableShapes)
  {
    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        foreach (var shape in availableShapes)
        {
          if (CanPlaceShape(new Vector2Int(x, y), shape))
          {
            return true;
          }
        }
      }
    }

    return false;
  }

  private void CalculateCellSize()
  {
    Camera cam = Camera.main;
    float screenHeight = 2f * cam.orthographicSize;
    float screenWidth = (screenHeight * cam.aspect) / 100 * (100 - paddingPercentage);

    float cellSizeX = screenWidth / width;
    float cellSizeY = screenHeight / height;

    cellSize = Mathf.Min(cellSizeX, cellSizeY);
  }

  public void CenterGridOnScreen()
  {
    float gridWidth = width * cellSize;
    float gridHeight = height * cellSize;

    Camera cam = Camera.main;
    if (cam == null) return;

    Vector3 camCenter = cam.transform.position;

    gridOrigin = new Vector2(
        camCenter.x - gridWidth / 2f,
        camCenter.y - gridHeight / 2f
    );
  }

  public Vector3 GridToWorld(int x, int y)
  {
    return new Vector3(
        gridOrigin.x + (x + 0.5f) * cellSize,
        gridOrigin.y + (y + 0.5f) * cellSize,
        0f
    );
  }

  public Vector2Int WorldToGrid(Vector3 worldPos)
  {
    int x = Mathf.FloorToInt((worldPos.x - gridOrigin.x) / cellSize);
    int y = Mathf.FloorToInt((worldPos.y - gridOrigin.y) / cellSize);

    return new Vector2Int(x, y);
  }

  public bool IsInsideGrid(int x, int y) => x >= 0 && x < width && y >= 0 && y < height;
  public bool IsCellEmpty(int x, int y) => IsInsideGrid(x, y) && grid[x, y] == null;

  public bool CanPlaceShape(Vector2Int basePos, Vector2Int[] shapeCells)
  {
    if (shapeCells.Length == 0) Debug.LogError("No shape cells");

    foreach (var cell in shapeCells)
    {
      int x = basePos.x + cell.x;
      int y = basePos.y + cell.y;

      if (!IsCellEmpty(x, y)) return false;
    }

    return true;
  }

  public ICollection<Vector3> GetShapePlacement(Vector2Int basePos, Vector2Int[] shapeCells)
  {
    var placement = new List<Vector3>();
    foreach (var cell in shapeCells)
    {
      int x = basePos.x + cell.x;
      int y = basePos.y + cell.y;

      if (!IsCellEmpty(x, y)) continue;

      var worldPosition = GridToWorld(x, y);

      placement.Add(worldPosition);
    }

    return placement;
  }

  public void PlaceShape(ShapeSpawner spawner, Vector2Int basePos, Vector2Int[] shapeCells)
  {
    AudioManager.Instance.PlaceShape();

    foreach (var cell in shapeCells)
    {
      int x = basePos.x + cell.x;
      int y = basePos.y + cell.y;

      if (IsInsideGrid(x, y) && grid[x, y] == null)
      {
        Vector3 worldPos = GridToWorld(x, y);
        GameObject block = Instantiate(blockPrefab, worldPos, Quaternion.identity);

        block.transform.localScale = Vector3.one * cellSize;

        grid[x, y] = block;
      }
    }

    spawner.OnShapePlaced();

    ScoreManager.Instance.AddScore(shapeCells.Length);
    CheckAndClearLines();

    if (!HasAnyValidMove(SpawnManager.Instance.GetAvailableShapes()))
    {
      Instantiate(noSpaceLeft, FindObjectOfType<Canvas>().transform, false);
      Invoke(nameof(PlayGameOver), 0.3f);
      Invoke(nameof(LoadGameOver), 3);
    }
  }

  private void PlayGameOver()
  {
    AudioManager.Instance.PlayGameOver();
  }

  private void LoadGameOver()
  {
    SceneSwitcher.LoadSceneByName("GameOver");
  }

  void CheckAndClearLines()
  {
    bool[] fullRows = new bool[height];
    bool[] fullColumns = new bool[width];

    for (int y = 0; y < height; y++)
    {
      bool full = true;
      for (int x = 0; x < width; x++)
        if (grid[x, y] == null) { full = false; break; }
      fullRows[y] = full;
    }

    for (int x = 0; x < width; x++)
    {
      bool full = true;
      for (int y = 0; y < height; y++)
        if (grid[x, y] == null) { full = false; break; }
      fullColumns[x] = full;
    }

    for (int y = 0; y < height; y++)
      if (fullRows[y]) ClearRow(y);

    for (int x = 0; x < width; x++)
      if (fullColumns[x]) ClearColumn(x);
  }

  void ClearRow(int y)
  {
    for (int x = 0; x < width; x++) ClearCell(x, y);
  }

  void ClearColumn(int x)
  {
    for (int y = 0; y < height; y++) ClearCell(x, y);
  }

  void ClearCell(int x, int y)
  {
    GameObject obj = grid[x, y];
    if (obj == null) return;

    Block block = obj.GetComponent<Block>();
    if (block != null) block.FadeAndDestroy();
    else Destroy(obj);

    grid[x, y] = null;
    ScoreManager.Instance.AddScore(1);
  }

  private void OnDestroy()
  {
    if (backgroundObj != null)
    {
      if (Application.isPlaying)
        Destroy(backgroundObj);
      else
        DestroyImmediate(backgroundObj);
    }

    if (lineParent != null)
    {
      if (Application.isPlaying)
        Destroy(lineParent);
      else
        DestroyImmediate(lineParent);
    }
  }

  public void DrawGridCells()
  {
    if (cellParent != null)
      Destroy(cellParent);

    cellParent = new GameObject("GridCells");

    // Effective size of a cell after spacing
    float cellVisualSize = cellSize - lineWidth;

    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        Vector3 position = GridToWorld(x, y);

        GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity, cellParent.transform);

        // Scale prefab (default size = 1 unit)
        cell.transform.localScale = Vector3.one * cellVisualSize;
      }
    }
  }
}
