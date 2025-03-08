using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    public struct SaveDTO {
        // HashSet<int> stage_Passed;//通关的关卡
        // Dictionary<int, int> stage_PassedStar;//通关关卡的星级
        // HashSet<int> stage_UnLock;//到达的关卡
        // public int unLockStageCount;//解锁的关卡 
        // public int unLockChapterID;//解锁的章节
        // public int starAllCount;//总星数

        public List<uint> stage_Passed;//通关的关卡
        public List<uint> stage_PassedStar;//通关关卡的星级
        public List<uint> stage_UnLock;//到达的关卡
        public ulong unLockStageCount;//解锁的关卡
        public ulong unLockChapterID;//解锁的章节
        public ulong starAllCount;//总星数

        public int GetSizeForWrite() {
            int size = 0;
            size += sizeof(ulong) * 3;

            size += sizeof(int) * stage_Passed.Count * sizeof(uint);
            size += sizeof(int) * stage_PassedStar.Count * sizeof(uint);
            size += sizeof(int) * stage_UnLock.Count * sizeof(uint);

            Debug.Log("SaveDTO size: " + size);
            return size;
        }

        public void WriteToBuffer(byte[] dst, ref int offset) {

        }

        public void FromBytes(byte[] src, ref int offset) {

        }

    }
}