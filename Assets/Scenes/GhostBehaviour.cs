using System.Collections;
using UnityEngine;

public class GhostBehaviour : MonoBehaviour
{
    public float floatTimer = 3.0f;
    public float verticalFloatDistance = 0.5f;
    public float movementSpeed = 0.1f;
    public bool movingLeft = true;
    public int hitPoints = 3;

    public AudioSource damageAudioSource;
    public AudioSource deathAudioSource;

    private bool downwards = true;
    private bool isDead = false;
    private float currentFloatTimer;
    private int currentHitPoints;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentFloatTimer = floatTimer;
        currentHitPoints = hitPoints;
        spriteRenderer = GetComponent<SpriteRenderer>();

        var audioSources = GetComponents<AudioSource>();
        damageAudioSource = audioSources[0];
        deathAudioSource = audioSources[1];
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

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
        if (currentHitPoints <= 0)
        {
            deathAudioSource.Play();
            yield return Die(deathAudioSource.clip.length);
        }
        else
        {
            damageAudioSource.Play();
            yield return spriteRenderer.FlashTimes(3);
        }
    }

    private IEnumerator Die(float delay)
    {
        isDead = true;
        GetComponent<PolygonCollider2D>().enabled = false;
        StartCoroutine(spriteRenderer.FlashForever());
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
