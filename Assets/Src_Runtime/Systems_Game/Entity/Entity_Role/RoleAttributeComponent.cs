using System;

namespace UF {

    public class RoleAttributeComponent {

        public int hp;
        public void SetHp(int value) => hp = value;
        public int GetHp() => hp;
        public int hpMax;
        public void SetHpMax(int value) => hpMax = value;
        public int GetHpMax() => hpMax;

        public int ownerShootCount;
        public void SetOwnerShootCount(int value) => ownerShootCount = value;
        public int hitTimes;//射中次数
        public void SetHitNumber(int value) => hitTimes = value;
        public int shootAllTimes;//射击所有次数
        public void SetHitNumberMax(int value) => shootAllTimes = value;

        public float GetRoleHitRate() {
            if (shootAllTimes == 0) {
                return 0;
            }
            return (float)hitTimes / shootAllTimes;
        }

        public int GetRoleGetHurt() {
            return hpMax - hp;
        }

    }
}