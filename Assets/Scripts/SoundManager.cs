using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Sound Effects")]
    public AudioClip bombExplosionSound;
    public AudioClip itemPickupSound;
    public AudioClip fishCaptureSound;
    public AudioClip meowSound;
    public AudioClip deathSound;
    public AudioClip tileDestroySound;
    public AudioClip countdownBeepSound;

    private AudioSource audioSource;
   // private bool beepStarted = false;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayExplosion() => audioSource.PlayOneShot(bombExplosionSound);
    public void PlayItemPickup() => audioSource.PlayOneShot(itemPickupSound);
    public void PlayFishCapture() => audioSource.PlayOneShot(fishCaptureSound);
   // public void PlayMeow() => audioSource.PlayOneShot(meowSound);
    public void PlayDeath() => audioSource.PlayOneShot(deathSound);
    public void PlayTileDestroy() => audioSource.PlayOneShot(tileDestroySound);
    //public void PlayBeep() => audioSource.PlayOneShot(countdownBeepSound);
    //public void StopAll() => audioSource.Stop();
}
