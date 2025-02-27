using System;

namespace UF
{
    public enum BulletSkillType // 子弹技能(效果)
    {
        None,
        Boom, // 爆炸
        Slow, // 减速
        Stop, // 停顿 + 持续伤害
        Down, // 下沉
        Appear, // 现形
        Back, // 后推
        Elec, // 持续伤害
        Converge, // 汇聚
    }
}