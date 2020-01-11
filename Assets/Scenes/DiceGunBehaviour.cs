﻿using UnityEngine;

public class DiceGunBehaviour : MonoBehaviour
{
    public GameObject diceGunProjectilePrefab;

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
        GameObject projectile = Instantiate(diceGunProjectilePrefab);
        projectile.transform.position = startPosition;

        var xAxisForceComponent = 1000 * (isFacingLeft ? -1 : 1);
        projectile
            .GetComponent<Rigidbody2D>()
            .AddForce(new Vector2(xAxisForceComponent, 0));
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