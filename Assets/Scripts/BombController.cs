using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
public class BombController : MonoBehaviour
{
    public Grid grid;
    [Header("Bomb")]
    public GameObject bombPrefab;
    public KeyCode inputKey = KeyCode.Return;
    public float bombUseTime = 3.0f;
    public int numberOfBombs = 1;
    private int remainingBombs;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Destructible")]
    public Tilemap destructibleTiles;
    public Destructable destructiblePrefab;

    private void OnEnable()
    {
        remainingBombs = numberOfBombs;
    }
    private void Update()
    {
        if (remainingBombs>0&& Input.GetKeyDown(inputKey))
        {
            StartCoroutine(PlaceBombs());
        }
    }

    private IEnumerator PlaceBombs()
    {
        Vector3Int cell = grid.WorldToCell(transform.position);
        Vector3 position = grid.GetCellCenterWorld(cell);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        remainingBombs--;

        yield return new WaitForSeconds(bombUseTime);

        position = bomb.transform.position;
        

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        SoundManager.Instance.PlayExplosion();
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        Destroy(bomb);
        remainingBombs++;

    }
    private void Explode(Vector3 position, Vector2 direction, int length)
    {
        if (length <= 0)
        {
            return;
        }

        position += (Vector3)direction;
        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearDestructible(position);
            return;
        }



        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, direction, length - 1);
    }
    


    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Bomb")) {
    //        other.isTrigger = false;
    //    }

    //}
    public void AddBomb()
    {
        numberOfBombs++;
        remainingBombs++;
    }

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
    //    {
    //        other.isTrigger = false;
    //    }
    //}

    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if (tile != null)
        {
            SoundManager.Instance.PlayTileDestroy();
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }
}

