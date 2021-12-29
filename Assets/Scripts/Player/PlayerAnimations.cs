using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [HideInInspector]
    Player player;

    Animator anim;

    public Transform rootBone;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleRunAnimation();
        HandleJumpAnimation();
        HandleWingFlapAnimation();
        HandleFallingAnimation();
        HandleDashAnimation();
        HandleTargetingMovementAnimation();
    }

    void HandleRunAnimation()
    {
        if (player.Movement.isGrounded && player.Movement.isMoving)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    void HandleJumpAnimation()
    {
        if (player.Movement.isJumping)
        {
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }
    }

    void HandleFallingAnimation()
    {
        if (player.Movement.isFalling)
        {
            anim.SetBool("isFalling", true);
        }
        else
        {
            anim.SetBool("isFalling", false);
        }
    }

    void HandleDashAnimation()
    {
        if (player.Movement.isDashing)
        {
            anim.SetBool("isDashing", true);
        }
        else
        {
            anim.SetBool("isDashing", false);
        }
    }

    void HandleWingFlapAnimation()
    {
        if (player.Movement.isFlapping)
        {
            anim.SetBool("startFlap", true);
            anim.SetBool("isFlapping", true);
        }
        else
        {
            anim.SetBool("isFlapping", false);
        }
    }

    public void StartedFlapping()
    {
        anim.SetBool("startFlap", false);
    }

    public void StartReceiveItemAnim()
    {
        anim.SetBool("isReceivingItem", true);
    }

    public void StopReceiveItemAnim()
    {
        anim.SetBool("isReceivingItem", false);
    }

    public void FootstepAnimEvent()
    {
        //SoundManager.Singleton.Play("Footstep");
    }

    void HandleTargetingMovementAnimation()
    {
        if (!player.Targeting.isTargeting)
        {
            if (anim.GetFloat("yRunning") != 1) anim.SetFloat("yRunning", 1);
            if (anim.GetFloat("xRunning") != 0) anim.SetFloat("xRunning", 0);

            return;
        }

        print("targeting movement");

        float x = 0;
        float y = 0;

        Vector2 moveDir = InputManager.Singleton.GetMoveInput();

        print("movDir: " + moveDir);

        if (moveDir != Vector2.zero)
        {
            float angle = Vector2.Angle(moveDir, Vector2.up);

            print("angle: " + angle);

            if (angle >= 0 && angle < 45)
            {
                y = 1;

            }
            else if (angle > 45 && angle < 135)
            {
                float rightAngle = Vector2.Angle(moveDir, Vector2.right);
                float leftAngle = Vector2.Angle(moveDir, Vector2.left);

                if (rightAngle < leftAngle)
                {
                    x = 1;
                }
                else
                {
                    x = -1;
                }
            }
            else if (angle > 135)
            {
                y = -1;
            }
        }
        print("x: " + x);
        print("y: " + y);

        anim.SetFloat("yRunning", y);
        anim.SetFloat("xRunning", x);
    }
}
