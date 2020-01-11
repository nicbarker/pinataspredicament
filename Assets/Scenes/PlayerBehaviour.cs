using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public GameData gameData;

    public float basePlayerSpeed = 10;
    public float animationStepTiming = 0.01f;
    public Sprite idleSprite;
    public Sprite[] walkingAnimationSteps;

    private bool inContactWithGround = true;
    private int currentAnimationStep = 0;
    private float currentAnimationStepTiming;

    // Start is called before the first frame update
    void Start()
    {
        currentAnimationStepTiming = animationStepTiming;
    }

    // Update is called once per frame
    void Update()
    {
        float speed = Input.GetAxisRaw("Horizontal") * Time.deltaTime * basePlayerSpeed;
        if (System.Math.Abs(speed) > 0.001f && (
            (speed > 0 && gameData.IsAbilityActive(Ability.MOVE_RIGHT))
            ||
            (speed < 0 && gameData.IsAbilityActive(Ability.MOVE_LEFT))
            ))
        {
            GetComponent<SpriteRenderer>().flipX = speed < 0;
            transform.position += new Vector3(speed, 0, 0);

            if (inContactWithGround)
            {
                currentAnimationStepTiming -= Time.deltaTime;

                if (currentAnimationStepTiming < 0)
                {
                    currentAnimationStep++;
                    if (currentAnimationStep > walkingAnimationSteps.Length - 1)
                    {
                        currentAnimationStep = 0;
                    }
                    currentAnimationStepTiming = animationStepTiming;
                    GetComponent<SpriteRenderer>().sprite = walkingAnimationSteps[currentAnimationStep];
                }
            }
        }
        else
        {
            currentAnimationStep = 0;
            GetComponent<SpriteRenderer>().sprite = idleSprite;
        }

        if (Input.GetKeyDown(KeyCode.Space) && inContactWithGround && gameData.IsAbilityActive(Ability.JUMP))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 2500));
            inContactWithGround = false;
            currentAnimationStep = 0;
        }
    }

    private void Die()
    {
        var activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        switch ((Layers)collision.gameObject.layer)
        {
            case Layers.FloorAndWalls:
                inContactWithGround = true;
                return;
            default:
                return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch ((Layers)collision.gameObject.layer)
        {
            case Layers.Gems:
                gameData.gems++;
                Destroy(collision.gameObject);
                return;
            case Layers.DeathZone:
                Die();
                return;
            default:
                return;
        }
    }
}
