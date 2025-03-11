using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using GameFunctions;

namespace UF {

    public struct SaveDTO {
        public int slotID;// 存档ID

        public List<int> stage_Passed;//通关的关卡
        public List<int> stage_PassedStar;//通关关卡的星级
        public List<int> stage_UnLock;//到达的关卡
        public int unLockChapterID;//解锁的章节
        public int starAllCount;//总星数

        public int GetSizeForWrite() {
            int size = 0;
            size += sizeof(int);

            size += sizeof(int) * 2;

            size += sizeof(int) * stage_Passed.Count * sizeof(int);
            size += sizeof(int) * stage_PassedStar.Count * sizeof(int);
            size += sizeof(int) * stage_UnLock.Count * sizeof(int);

            return size;
        }

        public void WriteTo(byte[] dst, ref int offset) {
            GFBufferEncoderWriter.WriteInt32(dst, slotID, ref offset);

            GFBufferEncoderWriter.WriteInt32List(dst, stage_Passed, ref offset);
            GFBufferEncoderWriter.WriteInt32List(dst, stage_PassedStar, ref offset);
            GFBufferEncoderWriter.WriteInt32List(dst, stage_UnLock, ref offset);

            GFBufferEncoderWriter.WriteInt32(dst, unLockChapterID, ref offset);
            GFBufferEncoderWriter.WriteInt32(dst, starAllCount, ref offset);
        }

        public void FromBytes(byte[] src, ref int offset) {

            slotID = GFBufferEncoderReader.ReadInt32(src, ref offset);

            stage_Passed = GFBufferEncoderReader.ReadInt32List(src, ref offset);
            stage_PassedStar = GFBufferEncoderReader.ReadInt32List(src, ref offset);
            stage_UnLock = GFBufferEncoderReader.ReadInt32List(src, ref offset);

            unLockChapterID = GFBufferEncoderReader.ReadInt32(src, ref offset);
            starAllCount = GFBufferEncoderReader.ReadInt32(src, ref offset);
        }

    }
}