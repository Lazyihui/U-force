using System;
using UnityEngine;
using UF;

public class RoleMoveComponent {
    Rigidbody2D rb;

    public Vector2 Velocity => rb.velocity;

    bool isGround;
    public bool IsGround => isGround;

    int allowMoveTimes;
    int allowJumpTimes;

    // -move
    public float moveSpeed;//用于调用
    public void SetMoveSpeed(float value) => moveSpeed = value;//用于赋值 只有一个地方赋值

    public float jumpForce;
    public void SetJumpForce(float value) => jumpForce = value;

    public RoleMoveComponent() {
        isGround = false;
    }

    public void Inject(Rigidbody2D rb) {
        this.rb = rb;
    }

    public void Move_Player(float moveSpeed) {
        Vector2 oldVelo = rb.velocity;
        oldVelo.y = moveSpeed;
        rb.velocity = oldVelo;
        allowMoveTimes -= 1;
        isGround = false;
    }

    public void Move_Ghost(float moveSpeed) {
        rb.gravityScale = 0;
        const float xAxis = -1;

        Vector2 oldVelo = rb.velocity;
        oldVelo.x = xAxis * moveSpeed;
        oldVelo.y = 0;
        rb.velocity = oldVelo;
    }

    public void Jump() {
        if (allowJumpTimes > 0) {
            Vector2 oldVelo = rb.velocity;

            if (oldVelo.y <= 0) {
                oldVelo.y = 0;
            }

            oldVelo.y += jumpForce;
            rb.velocity = oldVelo;
            allowJumpTimes -= 1;
        }
    }

    public void EnterGround() {
        allowMoveTimes = 1;
        allowJumpTimes = 1;
        isGround = true;
    }

    public bool AllowPlayerMove() {
        return allowMoveTimes > 0;
    }
}