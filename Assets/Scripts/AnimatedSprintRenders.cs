using UnityEngine;

public class AnimatedSprintRenders : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float animationTime = 0.25f;
    private int animationFrame;

    public Sprite idleSprite;
    public Sprite[] animationSprites;

    public bool loop = true;
    public bool idle = true;



    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        spriteRenderer.enabled = true;
        animationFrame = -1;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }

    private void Start()
    {
        InvokeRepeating(nameof(NextFrame), animationTime, animationTime);
    }
    private void NextFrame()
    {
        animationFrame++;

        if (loop && animationFrame >= animationSprites.Length)
        {
            animationFrame = 0;
        }
        else if (!loop && animationFrame >= animationSprites.Length)
        {
           
            spriteRenderer.enabled = false;
            return;
        }

        if (idle)
            spriteRenderer.sprite = idleSprite;
        else if (animationFrame >= 0 && animationFrame < animationSprites.Length)
            spriteRenderer.sprite = animationSprites[animationFrame];
    }
}

