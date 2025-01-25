using System;

namespace UF {

    [Flags]
    public enum FrameLoopFlag {
        Once = 0,
        LoopByTriggerTimesEnd = 1 << 0,
        LoopWhenTimeEnd = 1 << 1,

        // LoopForever = 1 << 1,
        // GroundEnd = 1 << 2,
        // HitEnd = 1 << 3,
        // BehaviourEnd = 1 << 5,
        // Reach Point End
    }

}