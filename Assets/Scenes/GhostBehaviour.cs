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

    void Start()
    {
        currentFloatTimer = floatTimer;
        currentHitPoints = hitPoints;
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
                GetComponent<SpriteRenderer>().flipX = !movingLeft;
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
        if (currentHitPoints <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(FlashSprite());
        }
    }

    private IEnumerator FlashSprite()
    {
        var sprite = GetComponent<SpriteRenderer>();
        for (var i = 0; i < 3; i++)
        {
            sprite.color = WithAlpha(sprite.color, 1.0f);
            yield return new WaitForSeconds(0.1f);
            sprite.color = WithAlpha(sprite.color, 0.5f);
            yield return new WaitForSeconds(0.1f);
        }
        sprite.color = WithAlpha(sprite.color, 1.0f);
    }

    private Color WithAlpha(Color color, float alpha)
    {
        return new Color(r: color.r, g: color.g, b: color.b, a: alpha);
    }
}
