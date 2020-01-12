using UnityEngine;

public class DiceGunProjectileBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        switch ((Layers)collision.gameObject.layer)
        {
            case Layers.FloorAndWalls:
            case Layers.Enemies:
                Destroy(gameObject);
                return;
            default:
                return;
        }
    }
}
