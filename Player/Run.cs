using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]
public class Run : MonoBehaviour
{
    public PlayerController controller;
    private Rigidbody2D RB;
    public Vector2 moveInput;
    public bool isFacingRight;
    public Animator animator;
    public bool canFlip = true;
    private Dash dash;

    [Header("Run")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float maxAcceleration = 35f;
    [SerializeField] private float maxAirAcceleration = 100f;
    [SerializeField] private float wallStickTime = 0.25f;
    private float maxSpeedChange, acceleration, timeMove = 0.5f;
    public float wallStickCounter;
    private Vector2 desiredVelocity;
    public Vector2 velocity;
    public bool bounce = false, canMove = true;
    public float timeToFlip = 0.1f, timeToFlipCounter, timeMoveCounter;

    private Attack attack;
    private WallInteractor wallInteractor;
    private CollisionDataRetriever collision;
    public bool onGround;
    private Vector2 vecGravity;


    private void Awake()
    {
        attack = GetComponent<Attack>();
        this.isFacingRight = true;
        this.animator = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        collision = GetComponent<CollisionDataRetriever>();
        wallInteractor = GetComponent<WallInteractor>();
        dash = GetComponent<Dash>();
    }




    // Update is called once per frame
    void Update()
    {
        if (attack.isAttacking)
        {
            timeToFlipCounter = timeToFlip;
            canMove = false;
        }
        else if(!attack.isAttacking)
        {
            canMove = true;
        }
        timeMoveCounter -= Time.deltaTime;
        if (timeMoveCounter < 0) canMove = true;
        if (controller.RetrieveAttackInput())
        {
            timeMoveCounter = timeMove;
            return;
        }
        
        moveInput.x = controller.RetrieveMoveInput();
        desiredVelocity = new Vector2(moveInput.x, 0f) * Mathf.Max(maxSpeed - collision.Friction, 0);
        if(collision.OnWall)
        {
            canFlip = false;
        }
        if (collision.OnWall)
            moveInput.x = 0;
        if (collision.OnWall)
        {
            timeToFlipCounter = timeToFlip;
        }
        else
        {
            timeToFlipCounter -= Time.deltaTime;
        }
        canFlip = timeToFlipCounter < 0;
        if (wallInteractor.onWall)
        {
            return;
        }
        if (dash.isDashing)
        {
            timeToFlipCounter = timeToFlip;
        }
        Flip();
    }

    private void FixedUpdate()
    {
        onGround = collision.OnGround;
        animator.SetBool("Grounded", onGround);
        StartRun();
        if (this.collision.OnWall && !this.collision.OnGround && !wallInteractor.wallJumping)
        {
            if (wallStickCounter > 0)
            {
                velocity.x = 0;
                if (controller.RetrieveMoveInput() == collision._normal.x)
                {
                    wallStickCounter -= Time.fixedDeltaTime;
                }
                else
                {
                    wallStickCounter = wallStickTime;
                }
            }
            else
            {
                wallStickCounter = wallStickTime;
            }
        }
        if(canMove)
            RB.velocity = velocity;
        else if (!canMove)
        {
            RB.velocity = Vector2.zero;
            
        }

    }
    private void StartRun()
    {
        animator.SetBool("Walk", moveInput.x != 0 && onGround);
        velocity = RB.velocity;
        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
    }

    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        timeToFlipCounter = -1;
    }
    private void Flip()
    {
        bool activeFlip = (moveInput.x < 0 && isFacingRight || !isFacingRight && moveInput.x > 0) && canFlip;
        if (activeFlip || bounce)
        {
            bounce = false;
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

}
