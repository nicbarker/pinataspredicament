using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuBehaviour : MonoBehaviour
{
  void Start()
  {
    // Bind start menu
    var StartMenu = transform.Find("StartLevelUI");
    StartMenu.Find("StartButton").GetComponent<Button>().onClick.AddListener(() =>
    {
      var player = GameObject.Find("Player");
      player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
      player.GetComponent<SpriteRenderer>().enabled = true;
      StartMenu.GetComponent<Canvas>().enabled = false;
    });

    GameObject.Find("RetryButton").GetComponent<Button>().onClick.AddListener(() =>
    {
      GameObject.Find("SceneChanger").GetComponent<SceneChangerBehaviour>().RestartCurrentScene();
    });
  }

  public void ShowLevelEndScreen()
  {
    var GameData = GameObject.Find("GameData").GetComponent<GameData>();
    var EndMenu = transform.Find("EndLevelUI");
    EndMenu.GetComponent<Canvas>().enabled = true;
    EndMenu.Find("GemCountText").GetComponent<Text>().text = GameData.totalGems.ToString();
    EndMenu.Find("StarCountText").GetComponent<Text>().text = GameData.stars.ToString();
    EndMenu.Find("RankText").GetComponent<Text>().text = GameData.getStarRank(SceneManager.GetActiveScene().buildIndex);
    GameObject.Find("Player").GetComponent<PlayerBehaviour>().SetPlayerMovementEnabled(false);
  }

  // Update is called once per frame
  void Update()
  {

  }
}
