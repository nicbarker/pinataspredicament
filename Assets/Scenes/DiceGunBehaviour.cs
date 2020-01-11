using UnityEngine;

public class DiceGunBehaviour : MonoBehaviour
{
    public GameObject diceGunProjectilePrefab;

    public bool TryShoot(Vector3 startPosition, bool isFacingLeft)
    {
        GameObject projectile = Instantiate(diceGunProjectilePrefab);
        projectile.transform.position = startPosition;

        var xAxisForceComponent = 1000 * (isFacingLeft ? -1 : 1);
        projectile
            .GetComponent<Rigidbody2D>()
            .AddForce(new Vector2(xAxisForceComponent, 0));

        return true;
    }
}
