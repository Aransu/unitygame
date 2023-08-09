using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]
public class WallInteractor : MonoBehaviour
{
    public bool wallJumping { get; private set; }
    private Animator animator;
    [Header("Wall Slide")]
    [SerializeField] private float wallSlideMaxSpeed = 2f;

    [Header("Wall Jump")]
    [SerializeField] Vector2 wallJumpClimb = new Vector2(15f, 20f);
    [SerializeField] Vector2 wallJumpBounce = new Vector2(25f, 20f);
    [SerializeField] Vector2 wallJumpLeap = new Vector2(30f, 15f);

    private CollisionDataRetriever collision;
    private Rigidbody2D RB;
    public PlayerController controller;
    public Run run;
    private bool isFacingRight;
    private Vector2 velocity;
    public bool onWall, onGround, desiredJump;
    private float wallDirection;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collision = GetComponent<CollisionDataRetriever>();
        RB = GetComponent<Rigidbody2D>();
        run = GetComponent<Run>();
    }

    void Update()
    {
        if (onWall && !onGround)
        {
            desiredJump |= controller.RetrieveJumpDownInput();
        }
        onWall = collision.OnWall;
        onGround = collision.OnGround;
        wallDirection = collision._normal.x;
        velocity = RB.velocity;
        onWall = collision.OnWall;

        if (onWall)
        {
            run.timeToFlipCounter = run.timeToFlip;
            if (velocity.y < -wallSlideMaxSpeed)
            {
                velocity.y = -wallSlideMaxSpeed;
            }
        }

        if ((onWall && velocity.x == 0) || onGround)
        {
            wallJumping = false;
        }
        if (desiredJump)
        {
            if (-wallDirection == controller.RetrieveMoveInput())
            {
                velocity = new Vector2(wallJumpClimb.x * wallDirection, wallJumpClimb.y);
                wallJumping = true;
                desiredJump = false;
            }
            else if (controller.RetrieveMoveInput() == 0)
            {
                velocity = new Vector2(wallJumpBounce.x * wallDirection, wallJumpBounce.y);
                wallJumping = true;
                desiredJump = false;
                run.bounce = true;
            }
            else if (wallDirection == controller.RetrieveMoveInput())
            {
                velocity = new Vector2(wallJumpLeap.x * wallDirection, wallJumpLeap.y);
                wallJumping = true;
                desiredJump = false;
            }
        }
        RB.velocity = velocity;
    }

    private void FixedUpdate()
    {
        animator.SetBool("IsWallSliding", onWall);
        if (onWall)
            animator.SetBool("IsJumping", false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.collision.EvaluateCollision(collision);
        if (this.collision.OnWall && !this.collision.OnGround && wallJumping)
        {
            RB.velocity = Vector2.zero;
        }
    }
}


