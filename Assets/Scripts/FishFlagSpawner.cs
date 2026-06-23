using UnityEngine;

public class FishFlagSpawner : MonoBehaviour
{
    public static FishFlagSpawner Instance;
    public Grid grid;
    public GameObject fishFlagPrefab;
    public float cellSize = 1f;
    public Vector2 spawnOffset = Vector2.zero;
    public int mapMinX = -7;
    public int mapMaxX = 7;
    public int mapMinY = -4;
    public int mapMaxY = 4;

    // Which layers block fish spawning (walls, obstacles)
    // Set this in Inspector — assign your Wall and Obstacle layers
    public LayerMask blockingLayers;

    private GameObject currentFish;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnFish();
    }

    public void FishExpired(FishFlag fish)
    {
        fish.gameObject.SetActive(false);
        Invoke(nameof(SpawnFish), 0.1f);
    }

    private void SpawnFish()
    {
        Vector3 pos = GetRandomPosition();

        if (currentFish == null)
            currentFish = Instantiate(fishFlagPrefab, pos, Quaternion.identity);
        else
        {
            currentFish.transform.position = pos;
            currentFish.SetActive(true);
        }
    }

   

    private Vector3 GetRandomPosition()
    {
        for (int i = 0; i < 50; i++)
        {
            int x = Random.Range(mapMinX, mapMaxX + 1);
            int y = Random.Range(mapMinY, mapMaxY + 1);

            // Snap to exact cell center using the grid
            Vector3Int cell = new Vector3Int(x, y, 0);
            Vector3 candidate = grid.GetCellCenterWorld(cell);

            Collider2D hit = Physics2D.OverlapCircle(candidate, 0.3f);

            if (hit == null) return candidate;

            if (!hit.CompareTag("Wall") &&
                !hit.CompareTag("Player") &&
                !hit.CompareTag("Untagged"))
                return candidate;
        }

        return grid.GetCellCenterWorld(Vector3Int.zero);
    }
}
