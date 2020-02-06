using UnityEngine;

public class DiceGunBehaviour : MonoBehaviour
{
    public GameObject diceGunProjectilePrefab;
    public float projectileYOffset = 1f;

    private System.DateTime? lastShotAt = null;

    public bool TryShoot(Vector3 startPosition, bool isFacingLeft)
    {
        if (!CanShoot())
        {
            return false;
        }

        lastShotAt = System.DateTime.UtcNow;

        Shoot(startPosition, isFacingLeft);

        return true;
    }

    private void Shoot(Vector3 startPosition, bool isFacingLeft)
    {
        GetComponent<AudioSource>().Play(0);


        GameObject projectile = Instantiate(diceGunProjectilePrefab);
        projectile.transform.position = new Vector2(startPosition.x, startPosition.y + projectileYOffset);

        var rb = projectile.GetComponent<Rigidbody2D>();

        var xAxisForceComponent = 800 * (isFacingLeft ? -1 : 1);
        rb.AddForce(new Vector2(xAxisForceComponent, 0));

        rb.AddTorque(500.0f);
    }

    private bool CanShoot()
    {
        switch (lastShotAt)
        {
            // has not been shot yet
            case null:
                return true;
            case System.DateTime lastShotAt:
                return MillisecondsSince(lastShotAt) > 500;
        }
    }

    private double MillisecondsSince(System.DateTime time)
    {
        return System.DateTime.UtcNow.Subtract(time).TotalMilliseconds;
    }
}
