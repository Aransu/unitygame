using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDataRetriever : MonoBehaviour
{
    public bool OnGround, OnWall, onRightWall, onLeftWall;
    public float Friction { get; private set; }
    public Vector2 _normal { get; private set; }
    public Vector2 rightOffset;
    public Vector2 leftOffset;
    public float collisionRadius;
    private PhysicsMaterial2D _material;
    public PlayerController controller;
    public LayerMask groundLayer;
    public Transform wallCheck;

    private void Awake()
    {
        wallCheck = GameObject.Find("WallCheck").transform;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        OnGround = false;
        OnWall = false;
        Friction = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    public void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            _normal = collision.GetContact(i).normal;
            OnGround |= _normal.y >= 0.9f;
            if (OnGround)
            {
                OnWall = false;
            }
            else
                OnWall = Physics2D.OverlapBox(wallCheck.position, new Vector2(0.2f, 0.7f), groundLayer);
        }
    }


    private void RetrieveFriction(Collision2D collision)
    {
        _material = collision.rigidbody.sharedMaterial;

        Friction = 0;

        if (_material != null)
        {
            Friction = _material.friction;
        }
    }
}

