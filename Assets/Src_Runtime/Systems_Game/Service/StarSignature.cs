using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UF {

    [Serializable]
    [StructLayout(LayoutKind.Explicit)] // 叠加指 定结构体的内存布局为显式布局（Explicit），允许手动控制每个字段的偏移量。

    public struct StarSignature {
        [FieldOffset(0)]
        [UnityEngine.HideInInspector]
        public ulong value; // 0~8

        [FieldOffset(0)]
        public int stageID;

        [FieldOffset(4)]
        public int starCount;

        public StarSignature(int stageID, int starCount) {
            this.value = 0;
            this.stageID = stageID;
            this.starCount = starCount;
        }

        public StarSignature(ulong value) {
            this.stageID = 0;
            this.starCount = 0;
            this.value = value;
        }


        public override int GetHashCode() {
            return value.GetHashCode();
        }
    }
}