using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private bool combatEnable = true;
    [SerializeField] private float inputTimer = 0.25f, attack1Radius, attack1Damage;
    [SerializeField] private Transform attackHitBoxPos;
    [SerializeField] private LayerMask whatIsDamageable;
    public PlayerController controller;
    private bool gotInput = false, isFisrtAttack = false;
    public bool isAttacking = false, isDashAttack = false;
    private float lastInputTime = Mathf.NegativeInfinity;

    private Dash dash;
    private Animator animator;
    private CollisionDataRetriever collisionDataRetriever;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("CanAttack", combatEnable);
        collisionDataRetriever = GetComponent<CollisionDataRetriever>();
        dash = GetComponent<Dash>();    
    }
    private void Update()
    {
        if (dash.isDashing || Input.GetKeyDown(KeyCode.Z))
        {
            FinishAttack1();
            return;
        }
        if (!collisionDataRetriever.OnGround) return;
        CheckCombatInput();
        CheckAttacks();
    }
    private void CheckCombatInput()
    {
        if (controller.RetrieveAttackInput())
        {
            if (combatEnable)
            {
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }

    private void CheckAttacks()
    {
        if (gotInput)
        {
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                isFisrtAttack = !isFisrtAttack;
                animator.SetBool("Attack1", true);
                animator.SetBool("FirstAttack", isFisrtAttack);                
                animator.SetBool("IsAttacking", isAttacking);
            }
        }

            if (Time.time >= lastInputTime + inputTimer)
            {
                gotInput = false;
            }
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackHitBoxPos.position,attack1Radius,whatIsDamageable);
        foreach(Collider2D collider in detectedObjects)
        {
            //collider.transform.parent.SendMessage("Damage",attack1Damage);
        }
    }

    public void FinishAttack1()
    {
        isAttacking = false;
        isDashAttack=false;
        animator.SetBool("IsAttacking", false);
        if (!isDashAttack)
            animator.SetBool("Attack1", false);
        else
            animator.SetBool("IsDashAttack", false);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackHitBoxPos.position,attack1Radius);
    }
}
