using UnityEngine;

interface DiceGunShotStatus
{
    bool HasBeenShot();

    System.TimeSpan TimeSinceLastShot();
}

class DiceGunHasNotBeenShot : DiceGunShotStatus
{
    public bool HasBeenShot()
    {
        return false;
    }

    public System.TimeSpan TimeSinceLastShot()
    {
        throw new System.NotImplementedException();
    }
}

class DiceGunHasBeenShot : DiceGunShotStatus
{
    public DiceGunHasBeenShot(System.DateTime lastShotAt)
    {
        this.lastShotAt = lastShotAt;
    }

    private System.DateTime lastShotAt { get; }

    public bool HasBeenShot()
    {
        return true;
    }

    public System.TimeSpan TimeSinceLastShot()
    {
        return System.DateTime.UtcNow.Subtract(lastShotAt);
    }
}

public class DiceGunBehaviour : MonoBehaviour
{
    public GameObject diceGunProjectilePrefab;

    private DiceGunShotStatus shotStatus = new DiceGunHasNotBeenShot();

    public bool TryShoot(Vector3 startPosition, bool isFacingLeft)
    {
        if (!CanShoot())
        {
            return false;
        }

        shotStatus = new DiceGunHasBeenShot(lastShotAt: System.DateTime.UtcNow);

        GameObject projectile = Instantiate(diceGunProjectilePrefab);
        projectile.transform.position = startPosition;

        var xAxisForceComponent = 1000 * (isFacingLeft ? -1 : 1);
        projectile
            .GetComponent<Rigidbody2D>()
            .AddForce(new Vector2(xAxisForceComponent, 0));

        return true;
    }

    private bool CanShoot()
    {
        if (shotStatus.HasBeenShot())
        {
            var timeSinceLastShot = shotStatus.TimeSinceLastShot();
            return timeSinceLastShot.TotalMilliseconds > 500;
        }

        return true;
    }
}
