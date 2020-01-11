using UnityEngine;

public class DiceGunProjectileBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        switch ((Layers)collision.gameObject.layer)
        {
            case Layers.FloorAndWalls:
                Destroy(gameObject);
                return;
            case Layers.Enemies:
                Destroy(gameObject);
                Destroy(collision.gameObject);
                return;
            default:
                return;
        }
    }
}
