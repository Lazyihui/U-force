using System;

namespace UF {

    [Flags]
    public enum FrameLoopFlag {
        Once = 0,//时间运行完结束
        LoopByTriggerTimesEnd = 1 << 0,//触发次数结束
        LoopWhenTimeEnd = 1 << 1,//时间结束 和 noce 差不多
        HitEnd = 1 << 2,//击中结束

        // LoopForever = 1 << 1,
        // GroundEnd = 1 << 2,
        // HitEnd = 1 << 3,
        // BehaviourEnd = 1 << 5,
        // Reach Point End
    }

}   