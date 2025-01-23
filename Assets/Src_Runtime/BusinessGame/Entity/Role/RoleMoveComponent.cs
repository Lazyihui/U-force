using System;
using UnityEngine;
using UF;

public class RoleMoveComponent
{
    Rigidbody2D rb;

    public Vector2 Velocity => rb.velocity;

    bool isGround;
    public bool IsGround => isGround;

    int allowMoveTimes;

    public RoleMoveComponent()
    {
        isGround = false;
    }

    public void Inject(Rigidbody2D rb)
    {
        this.rb = rb;
    }

    public void Move_Player(float jumpForce, float dt)
    {
        Vector2 oldVelo = rb.velocity;
        oldVelo.y = jumpForce;
        rb.velocity = oldVelo;
        allowMoveTimes -= 1;
        isGround = false;
    }

    public void Move_Ghost(float xAxis, float moveSpeed, float dt)
    {
        Vector2 oldVelo = rb.velocity;
        oldVelo.x = xAxis * moveSpeed;
        rb.velocity = oldVelo;
    }

    public void EnterGround()
    {
        allowMoveTimes = 1;
        isGround = true;
    }

    public bool AllowPlayerMove()
    {
        return allowMoveTimes > 0;
    }
}