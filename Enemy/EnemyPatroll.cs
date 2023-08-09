using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroll : MonoBehaviour
{
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [SerializeField] private Transform enemy;

    [SerializeField] private float speed;
    [SerializeField] private float idleDuration;
    private float idleTimer;

    private Vector3 initScale;
    private bool movingLeft;
    [SerializeField] private Animator animator;
    private void OnDisable()
    {
        animator.SetBool("IsWalking", false);
    }
    private void Awake()
    {
        initScale = transform.localScale;
    }
    private void MoveInDirection(int direction)
    {
        idleTimer = 0;
        animator.SetBool("IsWalking", true);
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * speed,
            enemy.position.y, enemy.position.z);

    }

    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
            {
                directionChange();
            }
        }
        else
        {
            if(enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
            {
                directionChange();
            }
        }
    }

    private void directionChange()
    {
        animator.SetBool("IsWalking", false);
        idleTimer += Time.deltaTime; 
        if(idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }
}
