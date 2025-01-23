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
    int allowJumpTimes;

    public RoleMoveComponent()
    {
        isGround = false;
    }

    public void Inject(Rigidbody2D rb)
    {
        this.rb = rb;
    }

    public void Move_Player(float moveSpeed, float dt)
    {
        Vector2 oldVelo = rb.velocity;
        oldVelo.y = moveSpeed;
        rb.velocity = oldVelo;
        allowMoveTimes -= 1;
        isGround = false;
    }

    public void Move_Ghost(float xAxis, float moveSpeed, float dt)
    {
        rb.gravityScale = 0;

        Vector2 oldVelo = rb.velocity;
        oldVelo.x = xAxis * moveSpeed;
        oldVelo.y = 0;
        rb.velocity = oldVelo;
    }

    public void Jump(bool isJumpKeyDown, float jumpForce)
    {
        if(isJumpKeyDown && allowJumpTimes > 0)
        {
            Vector2 oldVelo = rb.velocity;
            oldVelo.y += jumpForce;
            rb.velocity = oldVelo;
            allowJumpTimes -= 1;
        }
    }

    public void EnterGround()
    {
        allowMoveTimes = 1;
        allowJumpTimes = 1;
        isGround = true;
    }

    public bool AllowPlayerMove()
    {
        return allowMoveTimes > 0;
    }
}