using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuBehaviour : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    GameObject.Find("StartButton").GetComponent<Button>().onClick.AddListener(() =>
    {
      var player = GameObject.Find("Player");
      player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
      player.GetComponent<SpriteRenderer>().enabled = true;
      GetComponent<Canvas>().enabled = false;
    });
  }

  // Update is called once per frame
  void Update()
  {

  }
}
