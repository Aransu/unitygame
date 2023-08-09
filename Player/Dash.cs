using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private CollisionDataRetriever collision;
    [SerializeField] private PlayerController controller;
    private Rigidbody2D RB;

    private Vector2 dashDir;
    private bool canDash = true;
    public bool isDashing = false;
    private float timeStartedDash;
    private Animator animator;
    private int particleCount = 0;
    private float dashCooldown = 1f;

    [SerializeField] ParticleSystem dashParticle;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.26f;


    private void Awake()
    {
        collision = GetComponent<CollisionDataRetriever>();
        RB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dashParticle.Stop();
    }

    void Update()
    {
        if (controller.RetrieveJumpDownInput() && collision.OnGround && isDashing)
        {
            animator.SetBool("IsDashing", false);
            isDashing = false;
            return;
        }
        animator.SetBool("IsDashing", isDashing);
        bool dashInput = Input.GetKeyDown(KeyCode.Z);
        if (dashInput && canDash && !collision.OnWall)
        {
            isDashing = true;
            canDash = false;
            dashDir = new Vector2(controller.RetrieveMoveInput(), controller.RetrieveYMoveInput());
            if (dashDir == Vector2.zero)
            {
                dashDir = new Vector2(transform.localScale.x, 0);
            }
            StartCoroutine(StopDashing());
        }
        if (!isDashing)
            particleCount = 0;
        else
        {
            if (particleCount == 0)
            {
                dashParticle.Play();
                particleCount++;
            }
            if (collision.OnGround)
                RB.velocity = dashDir.normalized * (dashSpeed - 5f);
            else
                RB.velocity = dashDir.normalized * dashSpeed;
        }
        dashCooldown -= Time.deltaTime;
        if ((collision.OnGround || collision.OnWall) && dashCooldown < 0f)
        {
            canDash = true;
            dashCooldown = 1f;
        }
    }

    public void Stopdash()
    {
        isDashing = false;
        animator.SetBool("IsDashing", false);
    }
    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }

}
