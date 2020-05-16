using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller2D))]
public class PlayerBehaviour : MonoBehaviour
{
  private GameData gameData;
  public SceneChangerBehaviour sceneChanger;
  public DiceGunBehaviour diceGun;
  public GameObject GemPrefab;
  public bool isMoving = true;

  public float basePlayerSpeed = 10;

  private bool isDoubleJumping = false;
  private bool isDead = false;

  private AudioSource pickupAudioSource;
  private AudioSource jumpAudioSource;
  private AudioSource fallAudioSource;

  private float colorFlashTimer;
  private Color flashColor = new Color(0.41f, 0.65f, 0.76f);
  // Managing moving platforms
  private GameObject connectedFloor;
  private Vector2 previousConnectedFloorPosition;
  // After the first left or right collision with a wall, block additional movement in that direction
  // to prevent physics jank
  private bool moveLeftBlocked;
  private bool moveRightBlocked;

  private bool movementEnabled = false;

  float jumpHeight = 4;
  float timeToJumpApex = 0.3f;
  Controller2D controller;
  float gravity = -50;
  Vector3 velocity;
  float velocityXSmoothing;

  // Start is called before the first frame update
  void Start()
  {
    this.gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
    var audioSources = GetComponents<AudioSource>();
    pickupAudioSource = audioSources[0];
    jumpAudioSource = audioSources[1];
    fallAudioSource = audioSources[2];
    controller = GetComponent<Controller2D>();
  }

  // Update is called once per frame
  void Update()
  {
    if (!movementEnabled)
    {
      return;
    }

    Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    float speed = input.x * basePlayerSpeed;
    if (CanApplySpeed(speed) && !isDead)
    {
      GetComponent<SpriteRenderer>().flipX = speed < 0;
      float targetVelocityX = input.x * basePlayerSpeed;
      velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, 0.05f);
      GetComponent<Animator>().SetInteger("xVelocity", speed > 0 ? 1 : -1);
    }
    else
    {
      GetComponent<Animator>().SetInteger("xVelocity", 0);
      velocity.x = 0;
    }

    if (controller.collisions.above || controller.collisions.below)
    {
      velocity.y = 0;

      if (controller.collisions.below && controller.collisions.connectedBody)
      {
        // Move player with moving platforms
        controller.Move(new Vector2(
          controller.collisions.connectedBody.position.x - controller.collisions.oldConnectedBodyPosition.x,
          controller.collisions.connectedBody.position.y - controller.collisions.oldConnectedBodyPosition.y
        ));
        isDoubleJumping = false;
      }
    }

    if (Input.GetKeyDown(KeyCode.Space) && !isDead)
    {
      OnJump();
    }

    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);
    if (!controller.collisions.below && velocity.y > 2 && !controller.collisions.climbingSlope)
    {
      GetComponent<Animator>().SetInteger("yVelocity", 1);
    }
    else if (!controller.collisions.below && velocity.y < -1 && !controller.collisions.climbingSlope)
    {
      GetComponent<Animator>().SetInteger("yVelocity", -1);
    }
    else
    {
      GetComponent<Animator>().SetInteger("yVelocity", 0);
    }

    if (colorFlashTimer > 0)
    {
      GetComponent<SpriteRenderer>().color = Color.Lerp(flashColor, new Color(1, 1, 1), 1 - colorFlashTimer / 0.5f);
      colorFlashTimer -= Time.deltaTime;
    }

    // If the player is on a moving platform, update its position to match the platform's movement
    if (connectedFloor != null)
    {
      var movementX = connectedFloor.transform.position.x - previousConnectedFloorPosition.x;
      if (Mathf.Abs(movementX) > 0)
      {
        transform.position = new Vector3(transform.position.x + movementX, transform.position.y, transform.position.z);
      }
      previousConnectedFloorPosition.x = connectedFloor.transform.position.x;

      var movementY = connectedFloor.transform.position.y - previousConnectedFloorPosition.y;
      if (Mathf.Abs(movementY) > 0)
      {
        transform.position = new Vector3(transform.position.x, transform.position.y + movementY, transform.position.z);
      }
      previousConnectedFloorPosition.y = connectedFloor.transform.position.y;
    }
  }

  private void playGemMinusAnimation()
  {
    var newGem = Instantiate(GemPrefab);
    newGem.GetComponent<PolygonCollider2D>().enabled = false;
    newGem.transform.position = new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.z);
    newGem.GetComponent<Animator>().SetBool("GemMinus", true);
    colorFlashTimer = 0.5f;
    flashColor = new Color(0.76f, 0.42f, 0.44f);
  }

  private bool CanApplySpeed(float speed)
  {
    if (System.Math.Abs(speed) > 0.001f)
    {
      var wasActiveBefore = gameData.IsAbilityActive(speed > 0 ? Ability.MOVE_RIGHT : Ability.MOVE_LEFT);
      var isActiveNow = gameData.TryUseAbility(speed > 0 ? Ability.MOVE_RIGHT : Ability.MOVE_LEFT);
      if (!wasActiveBefore && isActiveNow)
      {
        playGemMinusAnimation();
      }
      return isActiveNow;
    }
    return false;
  }

  private void OnDiceGun()
  {
    if (!gameData.TryUseAbility(Ability.DICE_GUN))
    {
      return;
    }

    var isFacingLeft = GetComponent<SpriteRenderer>().flipX;
    diceGun.TryShoot(startPosition: transform.position, isFacingLeft);
  }

  private void OnJump()
  {
    if (controller.collisions.below)
    {
      var wasActiveBefore = gameData.IsAbilityActive(Ability.JUMP);
      var isActiveNow = gameData.TryUseAbility(Ability.JUMP);
      if (!wasActiveBefore && isActiveNow)
      {
        playGemMinusAnimation();
      }
      if (isActiveNow)
      {
        jumpAudioSource.Play(0);
        velocity.y = 20;
      }

      var platformBehaviour = controller.collisions.connectedBody.gameObject.GetComponent<MovingPlatformBehaviour>();
      if (platformBehaviour != null)
      {
        platformBehaviour.playBounceAnimation();
      }
    }
    else if (!isDoubleJumping && gameData.TryUseAbility(Ability.DOUBLE_JUMP))
    {
      isDoubleJumping = true;
      jumpAudioSource.Play(0);
      velocity.y = 20;
    }
  }

  private void Die()
  {
    fallAudioSource.Play();
    isDead = true;

    // GetComponent<Animator>().SetBool("Dead", true);
    // GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

    var activeScene = SceneManager.GetActiveScene();
    sceneChanger.FadeToScene(activeScene.buildIndex);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    switch ((Layers)collision.gameObject.layer)
    {
      case Layers.Enemies:
      case Layers.DeathZone:
        Die();
        return;
      case Layers.Stars:
        gameData.stars++;
        PlayPickupSoundAndDestroy(collision.gameObject);
        return;
      case Layers.Gems:
        flashColor = new Color(0.41f, 0.65f, 0.76f);
        colorFlashTimer = 0.5f;
        return;
      case Layers.EndZone:
        movementEnabled = false;
        GameObject.Find("MenuUI").GetComponent<GameMenuBehaviour>().ShowLevelEndScreen();
        break;
      default:
        return;
    }
  }

  private void PlayPickupSoundAndDestroy(GameObject gameObject)
  {
    pickupAudioSource.Play(0);
    Destroy(gameObject);
  }

  public void SetPlayerMovementEnabled(bool movementEnabled)
  {
    this.movementEnabled = movementEnabled;
  }
}
