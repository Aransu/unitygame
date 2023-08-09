using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public InputController controller;
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float downwardMovementMultiplier = 5f;
    [SerializeField] private float upwardMovementMultiplier = 3f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;

    private Attack attack;
    private Animator animator;
    private Rigidbody2D RB;
    private CollisionDataRetriever collision;
    private Vector2 velocity;
    private float defaultGravityScale, jumpSpeed, jumpBuffer;
    public float coyoteCounter, jumpBufferCounter;

    private bool desiredJump, onGround, hasJumped = false;
    public bool isJumping;
    private void Awake()
    {
        attack = GetComponent<Attack>();
        RB = GetComponent<Rigidbody2D>();
        collision = GetComponent<CollisionDataRetriever>();
        animator = GetComponent<Animator>();
        defaultGravityScale = 6f;
    }

    void Update()
    {
        if (attack.isAttacking) return;
        desiredJump |= controller.RetrieveJumpDownInput();
        if (onGround)
        {
            coyoteCounter = coyoteTime;
            isJumping = false;
        }
        else if (!onGround)
        {
            coyoteCounter -= Time.deltaTime;
        }
        animator.SetBool("IsJumping", !onGround && !collision.OnWall);
    }

    private void FixedUpdate()
    {
        if (attack.isAttacking) return;
        onGround = collision.OnGround;
        velocity = RB.velocity;
        if (desiredJump)
        {
            desiredJump = false;
            jumpBufferCounter = jumpBufferTime;
        }
        else if (!desiredJump && jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        if (jumpBufferCounter > 0f)
        {
            JumpAction();
        }

        if (controller.RetrieveJumpDownInput())
            hasJumped = true;
        if (onGround)
            hasJumped = false;
        if (hasJumped)
            return;
        if (controller.RetrieveJumpHoldInput() && RB.velocity.y > 0)
        {
            RB.gravityScale = upwardMovementMultiplier;
        }
        else if (!controller.RetrieveJumpHoldInput() || RB.velocity.y < 0)
        {
            RB.gravityScale = downwardMovementMultiplier;
        }
        else if (RB.velocity.y == 0)
        {
            RB.gravityScale = defaultGravityScale;
        }

        RB.velocity = velocity;
    }
    private void JumpAction()
    {
        isJumping = true;
        if (coyoteCounter > 0f)
        {
            collision.OnGround = false;
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0);
            }
            velocity.y += jumpSpeed;
        }
    }
}
