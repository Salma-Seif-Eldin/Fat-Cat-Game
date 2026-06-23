using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction = Vector2.down;

    public float speed = 5f;
    private float baseSpeed = 5f;

    public float respawnDelay = 5f;
    private Vector3 spawnPosition;

    public KeyCode inputUp = KeyCode.UpArrow;
    public KeyCode inputDown = KeyCode.DownArrow;
    public KeyCode inputLeft = KeyCode.LeftArrow;
    public KeyCode inputRight = KeyCode.RightArrow;

    public AnimatedSprintRenders spriteRendererUp;
    public AnimatedSprintRenders spriteRendererDown;
    public AnimatedSprintRenders spriteRendererLeft;
    public AnimatedSprintRenders spriteRendererRight;
    public AnimatedSprintRenders spriteRendererDeath;

    private AnimatedSprintRenders activeSpriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;
    }

    private void Start()
    {
        spawnPosition = transform.position;
        baseSpeed = speed;
    }

    private void Update()
    {
        if (Input.GetKey(inputUp))
            SetDirection(Vector2.up, spriteRendererUp);
        else if (Input.GetKey(inputDown))
            SetDirection(Vector2.down, spriteRendererDown);
        else if (Input.GetKey(inputLeft))
            SetDirection(Vector2.left, spriteRendererLeft);
        else if (Input.GetKey(inputRight))
            SetDirection(Vector2.right, spriteRendererRight);
        else
            SetDirection(Vector2.zero, activeSpriteRenderer);
    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        Vector2 translation = speed * Time.fixedDeltaTime * direction;
        rb.MovePosition(position + translation);
    }

    private void SetDirection(Vector2 newDirection, AnimatedSprintRenders spriteRenderer)
    {
        direction = newDirection;

        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        activeSpriteRenderer = spriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
            DeathSequence();
    }



    private void DeathSequence()
    {
        SoundManager.Instance.PlayDeath();
        enabled = false;
        GetComponent<BombController>().enabled = false;

        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDeath.enabled = true;

        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

       
        spriteRendererDeath.enabled = false;

       
        transform.position = spawnPosition;
        rb.position = spawnPosition;

        yield return null;
     
        enabled = true;
        GetComponent<BombController>().enabled = true;

     
        spriteRendererDown.enabled = true;
        activeSpriteRenderer = spriteRendererDown;
        activeSpriteRenderer.idle = true;
    }

    
    public void ApplyFishPenalty(int fishCount)
    {
       
        speed = Mathf.Max(2f, baseSpeed - fishCount * 0.3f);
    }

  
    public void ApplyTemporarySpeed(float boostedSpeed, float duration)
    {
        StartCoroutine(TemporarySpeedRoutine(boostedSpeed, duration));
    }

    private IEnumerator TemporarySpeedRoutine(float boostedSpeed, float duration)
    {
        float speedBefore = speed;
        speed = boostedSpeed;
        yield return new WaitForSeconds(duration);
        speed = speedBefore; 
    }
}


