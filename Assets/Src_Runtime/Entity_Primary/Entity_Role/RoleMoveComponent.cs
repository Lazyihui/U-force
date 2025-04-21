using System;
using UnityEngine;
using UF;
using Unity.VisualScripting;
using UnityEngine.Video;

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

    // 资源战(因为move持续执行, 不想每帧创建一次, 所以写在了这里)
    static Vector2 pos1;
    static Vector2 pos2;
    static Vector2 pos3;
    Vector2[] posArr;
    int index; // 固定移动的坐标点索引

    Vector2 point; // 随机移动目标点
    bool isMoveToPoint; // 是否正在移动到目标点

    public RoleMoveComponent() {
        isGround = false;
        pos1 = new Vector2(9, 9);
        pos2 = new Vector2(0, 0);
        pos3 = new Vector2(9, -9);
        posArr = new Vector2[] { pos1, pos2, pos3 };
        index = 0;
        isMoveToPoint = false;
    }

    public void Ctor() {
        // 
        rb.interpolation = (RigidbodyInterpolation2D)RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = (CollisionDetectionMode2D)CollisionDetectionMode.ContinuousDynamic;
    }

    public void Set_PlayerRbConstraints() {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    public void Set_MinionRbConstraints() {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
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

    // 直线运动
    public void Move_Ghost(float moveSpeed) {
        rb.gravityScale = 0;

        const float xAxis = -1;

        Vector2 oldVelo = rb.velocity;
        oldVelo.x = xAxis * moveSpeed;
        // oldVelo.y = 0;
        rb.velocity = oldVelo;
    }

    // 进攻战 玩家移动
    public void Move_Attack(Vector2 dir, float moveSpeed) {

        Vector2 mapMinBounds = new Vector2(-29, -15);
        Vector2 mapMaxBounds = new Vector2(29, 15);

        Vector2 oldVelo = rb.velocity;
        oldVelo = dir * moveSpeed;
        rb.velocity = oldVelo;

        // TODO:这一行要像一下优化 别放这里
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 边界限制
        if (rb.position.x < mapMinBounds.x) {
            rb.position = new Vector2(mapMinBounds.x, rb.position.y);
        } else if (rb.position.x > mapMaxBounds.x) {
            rb.position = new Vector2(mapMaxBounds.x, rb.position.y);
        }
        if (rb.position.y < mapMinBounds.y) {
            rb.position = new Vector2(rb.position.x, mapMinBounds.y);
        } else if (rb.position.y > mapMaxBounds.y) {
            rb.position = new Vector2(rb.position.x, mapMaxBounds.y);
        }
    }

    public void Move_Regular(Vector2 pos, float moveSpeed) {
        SetRbType(RigidbodyType2D.Dynamic);
        Set_Garvity(0);

        var dis = Vector2.Distance(pos, posArr[index]);

        if (dis < 0.1f) {
            index = (index + 1) % 3;
        }

        var moveToPos = posArr[index];
        Vector2 dir = (moveToPos - pos).normalized;

        Vector2 oldVelo = rb.velocity;
        oldVelo = dir * moveSpeed;
        rb.velocity = oldVelo;
    }

    public void Move_Random(Vector2 pos, float moveSpeed) {
        SetRbType(RigidbodyType2D.Dynamic);
        Set_Garvity(0);

        if (!isMoveToPoint) {
            Vector2 center = new Vector2(9, 0);
            point.x = UnityEngine.Random.Range(0, 18);
            point.y = UnityEngine.Random.Range(-9, 9);

            var pointDis = Vector2.Distance(center, point);
            if (pointDis >= 10) { return; }

            isMoveToPoint = true;
        }

        var dis = Vector2.Distance(pos, point);

        if (dis < 0.1f) {
            isMoveToPoint = false;
        }

        Vector2 dir = (point - pos).normalized;
        Vector2 oldVelo = rb.velocity;
        oldVelo = dir * moveSpeed;
        rb.velocity = oldVelo;
    }




    public void Move_Stop() {
        Debug.Log("Move_Stop 将rigidbody设置为静态");
        pauseVelo = rb.velocity;
        rb.bodyType = RigidbodyType2D.Static;
    }

    public void Move_Stretched_Stop() {
        rb.velocity = Vector2.zero;
        Set_Garvity(1);
    }
    public void Move_ChangeLine(Vector2 changeLineDir) { // 换行
        const float changeMoveSpeed = 20;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Vector2 oldVelo = rb.velocity;
        oldVelo.y = changeLineDir.y * changeMoveSpeed;
        rb.velocity = oldVelo;

        Debug.LogWarning("换行");
    }

    public void Move_Repulsed() { // 被击退
        const float changeMoveSpeed = 20;

        Vector2 dir = Vector2.right;

        Vector2 oldVelo = rb.velocity;
        oldVelo.x = dir.x * changeMoveSpeed;
        rb.velocity = oldVelo;

        Debug.LogWarning("被击退");
    }

    public void Fall() {
        rb.bodyType = RigidbodyType2D.Static;
        rb.bodyType = RigidbodyType2D.Dynamic;

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

    public void SetRbType(RigidbodyType2D type) {
        rb.bodyType = type;
    }
}