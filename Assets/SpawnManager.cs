using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
  public static SpawnManager Instance;

  public ShapeSpawner[] Spawners;

  public void Awake()
  {
    Instance = this;
  }

  public void Start()
  {
    Spawners.ToList().ForEach(s => s.OnPlaced += OnPlaced);
  }

  public Vector2Int[][] GetAvailableShapes()
  {
    return Spawners.Select(e => e.GetShapeCells()).Where(s => s.Length != 0).ToArray();
  }

  private void OnPlaced()
  {
    if (Spawners.All(s => s.IsEmpty))
    {
      Spawners.ToList().ForEach(s => s.Spawn());
    }
  }
}
