using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PlayerBehaviour : MonoBehaviour
{
  private GameData gameData;
  public SceneChangerBehaviour sceneChanger;
  public DiceGunBehaviour diceGun;
  public GameObject GemPrefab;
  public bool isMoving = true;

  public float basePlayerSpeed = 10;

  private bool inContactWithGround = true;
  private bool isDoubleJumping = false;
  private bool isDead = false;

  private AudioSource pickupAudioSource;
  private AudioSource jumpAudioSource;
  private AudioSource fallAudioSource;

  private float colorFlashTimer;
  private Color flashColor = new Color(0.41f, 0.65f, 0.76f);
  // Managing moving platforms
  private GameObject connectedFloor;
  private float previousConnectedFloorPosition;

  private bool movementEnabled = true;

  // Start is called before the first frame update
  void Start()
  {
    this.gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
    var audioSources = GetComponents<AudioSource>();
    pickupAudioSource = audioSources[0];
    jumpAudioSource = audioSources[1];
    fallAudioSource = audioSources[2];
  }

  // Update is called once per frame
  void Update()
  {
    if (isDead || !movementEnabled)
    {
      return;
    }

    float speed = Input.GetAxisRaw("Horizontal") * Time.deltaTime * basePlayerSpeed;
    if (CanApplySpeed(speed))
    {
      GetComponent<SpriteRenderer>().flipX = speed < 0;
      transform.position += new Vector3(speed, 0, 0);

      if (inContactWithGround)
      {
        GetComponent<Animator>().SetBool("Moving", true);
      }
    }
    else if (GetComponent<Animator>().GetBool("Moving"))
    {
      GetComponent<Animator>().SetBool("Moving", false);
    }

    if (Input.GetKeyDown(KeyCode.Space))
    {
      OnJump();
    }

    if (colorFlashTimer > 0)
    {
      GetComponent<SpriteRenderer>().color = Color.Lerp(flashColor, new Color(1, 1, 1), 1 - colorFlashTimer / 0.5f);
      colorFlashTimer -= Time.deltaTime;
    }

    // If the player is on a moving platform, update its position to match the platform's movement
    if (connectedFloor != null)
    {
      var movement = connectedFloor.transform.position.x - previousConnectedFloorPosition;
      if (Mathf.Abs(movement) > 0)
      {
        transform.position = new Vector3(transform.position.x + movement, transform.position.y, transform.position.z);
      }
      previousConnectedFloorPosition = connectedFloor.transform.position.x;
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
    if (inContactWithGround)
    {
      var wasActiveBefore = gameData.IsAbilityActive(Ability.JUMP);
      var isActiveNow = gameData.TryUseAbility(Ability.JUMP);
      if (!wasActiveBefore && isActiveNow)
      {
        playGemMinusAnimation();
      }
      if (isActiveNow)
      {
        PerformJump(force: 2800);
        GetComponent<Animator>().SetBool("Jumping", true);
      }
    }
    else if (!isDoubleJumping && gameData.TryUseAbility(Ability.DOUBLE_JUMP))
    {
      isDoubleJumping = true;
      PerformJump(force: 1500);
    }
  }

  private void PerformJump(int force)
  {
    jumpAudioSource.Play(0);

    var rigidBodyComponent = GetComponent<Rigidbody2D>();

    // Remove all velocity in the y axis. Improves the feel of double jumping
    // for two reasons:
    //   1. spamming double jumping twice doesn't apply ~5000 units of force,
    //      just applies 2500 again shortly after the first jump
    //   2. jumping while falling gives you the full jump height rather than
    //      slowing descent
    rigidBodyComponent.velocity = new Vector2(rigidBodyComponent.velocity.x, 0);

    rigidBodyComponent.AddForce(new Vector2(0, force));

    inContactWithGround = false;
    GetComponent<Animator>().SetBool("Jumping", true);
    connectedFloor = null;
    previousConnectedFloorPosition = 0;
  }

  private void Die()
  {
    fallAudioSource.Play();
    isDead = true;

    GetComponent<Animator>().SetBool("Dead", true);
    GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

    var activeScene = SceneManager.GetActiveScene();
    sceneChanger.FadeToScene(activeScene.buildIndex);
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {

    switch ((Layers)collision.gameObject.layer)
    {
      case Layers.FloorAndWalls:
        inContactWithGround = true;
        connectedFloor = collision.gameObject;
        previousConnectedFloorPosition = collision.gameObject.transform.position.x;
        GetComponent<Animator>().SetBool("Jumping", false);
        isDoubleJumping = false;
        return;
      default:
        return;
    }
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
    GetComponent<Animator>().SetBool("Moving", false);
  }
}
