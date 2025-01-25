using System;

namespace UF {

    [Serializable]
    public class LoopTM {
        public FrameLoopFlag loopFlag;

        public float maintainFrame;

        public float triggerIntervalSec;
        public float triggerTimesMax;

    }
}