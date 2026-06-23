using UnityEngine;
using UnityEngine.UI;

public class FishFlag : MonoBehaviour
{
    [Header("Timing")]
    public float lifetime = 10f;  // seconds before fish despawns if uncaptured
    public float captureTime = 5f;   // seconds player must stand on it to capture

    [Header("UI")]
    public Image fillBar;            // drag BarFill Image here in Inspector

    private float lifetimeTimer;
    private float captureTimer;
    private MovementController playerOn;

    private void OnEnable()
    {
        lifetimeTimer = lifetime;
        captureTimer = 0f;
        playerOn = null;

        if (fillBar != null)
        {
            fillBar.fillAmount = 0f;
            fillBar.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        lifetimeTimer -= Time.deltaTime;
        if (lifetimeTimer <= 0f)
        {
            FishFlagSpawner.Instance.FishExpired(this);
            return;
        }

        if (playerOn != null)
        {
            captureTimer += Time.deltaTime;
            if (fillBar != null)
                fillBar.fillAmount = captureTimer / captureTime;
            if (captureTimer >= captureTime)
                CapturedBy(playerOn);
        }
        else
        {
            captureTimer = Mathf.Max(0f, captureTimer - Time.deltaTime * 2f);
            if (fillBar != null)
                fillBar.fillAmount = captureTimer / captureTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerOn == null)
            playerOn = other.GetComponent<MovementController>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOn = null;
            captureTimer = 0f;

            if (fillBar != null)
                fillBar.fillAmount = 0f;
        }
    }

    private void CapturedBy(MovementController player)
    {
        SoundManager.Instance.PlayFishCapture();
        GameManager.Instance.FishCollected(player);
        FishFlagSpawner.Instance.FishExpired(this);
    }
}
