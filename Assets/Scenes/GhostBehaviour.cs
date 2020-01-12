using System.Collections;
using UnityEngine;

public class GhostBehaviour : MonoBehaviour
{
    public float floatTimer = 3.0f;
    public float verticalFloatDistance = 0.5f;
    public float movementSpeed = 0.1f;
    public bool movingLeft = true;
    public int hitPoints = 3;

    private bool downwards = true;
    private float currentFloatTimer;
    private int currentHitPoints;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentFloatTimer = floatTimer;
        currentHitPoints = hitPoints;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        currentFloatTimer -= Time.deltaTime;

        transform.position = new Vector3(
            transform.position.x + (movementSpeed * Time.deltaTime) * (movingLeft ? -1 : 1),
            transform.position.y + (verticalFloatDistance * (Time.deltaTime / floatTimer)) * (downwards ? -1 : 1),
            transform.position.y
        );

        if (currentFloatTimer < 0)
        {
            downwards = !downwards;
            currentFloatTimer = floatTimer;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch ((Layers)collision.gameObject.layer)
        {
            case Layers.EdgeCollider:
                movingLeft = !movingLeft;
                spriteRenderer.flipX = !movingLeft;
                return;
            case Layers.Projectile:
                OnHitByProjectile();
                return;
            default:
                return;
        }
    }

    private void OnHitByProjectile()
    {
        currentHitPoints--;
        StartCoroutine(TakeDamage());
    }

    private IEnumerator TakeDamage()
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.Play(0);

        if (currentHitPoints <= 0)
        {
            yield return Die(audioSource.clip.length);
        }
        else
        {
            yield return FlashSprite();
        }
    }

    private IEnumerator Die(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private IEnumerator FlashSprite()
    {
        for (var i = 0; i < 3; i++)
        {
            spriteRenderer.color = WithAlpha(spriteRenderer.color, 1.0f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = WithAlpha(spriteRenderer.color, 0.5f);
            yield return new WaitForSeconds(0.1f);
        }
        spriteRenderer.color = WithAlpha(spriteRenderer.color, 1.0f);
    }

    private Color WithAlpha(Color color, float alpha)
    {
        return new Color(r: color.r, g: color.g, b: color.b, a: alpha);
    }
}
