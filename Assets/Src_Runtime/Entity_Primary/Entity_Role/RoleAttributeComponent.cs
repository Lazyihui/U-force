using System;

namespace UF {

    public class RoleAttributeComponent {

        public int hp;
        public void SetHp(int value) => hp = value;
        public int GetHp() => hp;
        public int hpMax;
        public void SetHpMax(int value) => hpMax = value;
        public int GetHpMax() => hpMax;

        public float moveSpeed;
        public void SetMoveSpeed(float value) => moveSpeed = value;
        public float GetMoveSpeed() => moveSpeed;

        public int fallDamage; // 掉落伤害先写在这
        public void SetFallDamage(int value) => fallDamage = value;
        public int GetFallDamage() => fallDamage;
    }
}