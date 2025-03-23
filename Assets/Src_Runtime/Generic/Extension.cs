using System;
using UnityEngine;

namespace UF {

    public static class Extension {
        // 用于判断一个浮点数 (float) 是否接近于零
        public static bool IsAlmostZero(this float value, float epsilon) {
            return Mathf.Abs(value) <= epsilon;
        }
    }
}