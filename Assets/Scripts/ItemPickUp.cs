using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
   public enum itemType  {
    extraBombs,
    extraSpeed,
    extraRadius
    }
    public itemType type;

    private void OnItemPickUp(GameObject player)
    {
        SoundManager.Instance.PlayItemPickup();
        if (type == itemType.extraBombs)
        {
            player.GetComponent<BombController>().AddBomb();
        }
        else if (type == itemType.extraSpeed) {
            player.GetComponent<MovementController>().speed++;
        }
        else if (type == itemType.extraRadius) {
            player.GetComponent<BombController>().explosionRadius++;
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            OnItemPickUp(other.gameObject);
        }
    }
}
