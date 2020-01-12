﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public GameData gameData;
    public SceneChangerBehaviour sceneChanger;
    public DiceGunBehaviour diceGun;
    public bool isMoving = true;

    public float basePlayerSpeed = 10;

    private bool inContactWithGround = true;
    private bool isDoubleJumping = false;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
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
                GetComponent<Animator>().SetBool("Jumping", false);
            }
        }
        else
        {
            if (inContactWithGround)
            {
                GetComponent<Animator>().SetBool("Moving", false);
                GetComponent<Animator>().SetBool("Jumping", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnDiceGun();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)
            || Input.GetKeyDown(KeyCode.W))
        {
            OnJump();
        }
    }

    private bool CanApplySpeed(float speed)
    {
        return System.Math.Abs(speed) > 0.001f && (
            (speed > 0 && gameData.IsAbilityActive(Ability.MOVE_RIGHT))
            || (speed < 0 && gameData.IsAbilityActive(Ability.MOVE_LEFT))
        );
    }

    private void OnDiceGun()
    {
        if (!gameData.IsAbilityActive(Ability.DICE_GUN))
        {
            return;
        }

        var isFacingLeft = GetComponent<SpriteRenderer>().flipX;
        diceGun.TryShoot(startPosition: transform.position, isFacingLeft);
    }

    private void OnJump()
    {
        if (inContactWithGround && gameData.IsAbilityActive(Ability.JUMP))
        {
            PerformJump(force: 2800);
            GetComponent<Animator>().SetBool("Jumping", true);
        }
        else if (!isDoubleJumping && gameData.IsAbilityActive(Ability.DOUBLE_JUMP))
        {
            isDoubleJumping = true;
            PerformJump(force: 1500);
        }
    }

    private void PerformJump(int force)
    {
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
        // currentAnimationStep = 0;
    }

    private void Die()
    {
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
            case Layers.Gems:
                gameData.gems++;
                PlayPickupSoundAndDestroy(collision.gameObject);
                return;
            case Layers.Enemies:
            case Layers.DeathZone:
                Die();
                return;
            case Layers.Stars:
                gameData.stars++;
                PlayPickupSoundAndDestroy(collision.gameObject);
                return;
            default:
                return;
        }
    }

    private void PlayPickupSoundAndDestroy(GameObject gameObject)
    {
        GetComponent<AudioSource>().Play(0);
        Destroy(gameObject);
    }
}
