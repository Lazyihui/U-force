using System;
using UnityEngine;
using UF;
using Unity.VisualScripting;

public class RoleMoveComponent {
    Rigidbody2D rb;

    public Vector2 Velocity => rb.velocity;

    bool isGround;
    public bool IsGround => isGround;

    int allowMoveTimes;
    int allowJumpTimes;

    public float jumpForce;
    public void SetJumpForce(float value) => jumpForce = value;

    // -PauseMove
    // TODO:感觉要改，但还没想好
    Vector2 pauseVelo;

    public RoleMoveComponent() {
        isGround = false;
    }

    public void Set_PlayerRbConstraints() {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    public void Set_MinionRbConstraints() {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    public void Set_FallRbConstraints() {
        rb.constraints = RigidbodyConstraints2D.None;
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

    public void Move_Attack(Vector2 dir, float moveSpeed) { // 进攻战 玩家移动
        Vector2 oldVelo = rb.velocity;
        oldVelo = dir * moveSpeed;
        rb.velocity = oldVelo;

        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Move_Stop() {
        pauseVelo = rb.velocity;
        rb.bodyType = RigidbodyType2D.Static;
    }

    public void Fall() {
        rb.gravityScale = 1;
        rb.velocity = Vector2.zero;
    }

    public void Move_Reset() {
        rb.bodyType = RigidbodyType2D.Dynamic;

        if (rb.velocity == Vector2.zero) {
            if (pauseVelo == Vector2.zero) {
                return;
            } else {
                rb.velocity = pauseVelo;
            }
        }
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

    public void Jump_Stop() {
        Vector2 oldVelo = rb.velocity;

        if (oldVelo.y >= 0) {
            oldVelo.y = -oldVelo.y;
        }

        rb.velocity = oldVelo;
    }

    public void EnterGround() {
        allowMoveTimes = 1;
        allowJumpTimes = 1;
        isGround = true;
    }

    public bool AllowPlayerMove() {
        return allowMoveTimes > 0;
    }

    public void Set_Garvity(float value) {
        rb.gravityScale = value;
    }
}