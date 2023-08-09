using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float damage;
    [SerializeField] private float colliderDistance;
    [SerializeField] private float range;

    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private bool isAttacking = false;
    private Health playerHealth;
    private Animator animator;
    private EnemyPatroll enemyPatroll;
    private void Awake()
    {
        enemyPatroll = GetComponentInParent<EnemyPatroll>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (playerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                Debug.Log(cooldownTimer);
                cooldownTimer = 0;
                animator.SetTrigger("Attack");
            } 
        }
        if(enemyPatroll != null)
        {
            if (playerInSight() || isAttacking)
            {
                    enemyPatroll.enabled = false;
            }
            else
                enemyPatroll.enabled = true;
        }
    }

    private bool playerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider2D.bounds.size.x * range,boxCollider2D.bounds.size.y,boxCollider2D.bounds.size.z),0,Vector2.left, 0,playerLayer);
        if (hit.collider != null)
        {
            isAttacking = true;
            playerHealth = hit.transform.GetComponent<Health>();   
        }
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider2D.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(boxCollider2D.bounds.size.x * range, boxCollider2D.bounds.size.y, boxCollider2D.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (playerInSight())
        {
            playerHealth.TakeDamage(damage);
        }
    }
    private void StopAttack()
    {
        isAttacking = false;
    }
}
