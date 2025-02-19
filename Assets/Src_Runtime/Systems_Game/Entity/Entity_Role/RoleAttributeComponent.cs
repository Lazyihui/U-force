using System;

namespace UF {

    public class RoleAttributeComponent {

        public int hp;
        public void SetHp(int value) => hp = value;
        public int GetHp() => hp;
        public int hpMax;
        public void SetHpMax(int value) => hpMax = value;
        public int GetHpMax() => hpMax;

    }
}