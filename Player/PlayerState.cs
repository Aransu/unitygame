using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    CollisionDataRetriever collision;
    Dash dash;
    Attack attack;
    public bool isDashing { get; private set; }
    public bool isAttacking { get; private set; }

    private void Awake()
    {
        dash = GetComponent<Dash>();
        attack = GetComponent<Attack>();    
        collision = GetComponent<CollisionDataRetriever>();
    }

    // Update is called once per frame
    void Update()
    {
        isDashing = dash.isDashing;
        isAttacking = attack.isAttacking;
    }
}
