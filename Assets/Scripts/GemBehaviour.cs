using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemBehaviour : MonoBehaviour
{
  private GameData gameData;
  public enum GemAnimations : int
  {
    PickupAnimationEnded = 8,
  }

  // Start is called before the first frame update
  void Start()
  {
    this.gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    switch ((Layers)collision.gameObject.layer)
    {
      case Layers.Player:
        gameData.IncrementGems();
        GetComponent<PolygonCollider2D>().enabled = false;
        GetComponent<Animator>().SetBool("PickedUp", true);
        return;
      default:
        return;
    }
  }

  public void AlertObservers(GemAnimations animation)
  {
    if (animation.Equals(GemAnimations.PickupAnimationEnded))
    {
      Destroy(this.gameObject);
      // Do other things based on an attack ending.
    }
  }
}
