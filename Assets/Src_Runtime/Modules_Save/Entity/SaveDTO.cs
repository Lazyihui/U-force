using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using GameFunctions;

namespace UF {

    public struct SaveDTO {
        public int slotID;// 存档ID

        public List<uint> stage_Passed;//通关的关卡
        public List<uint> stage_PassedStar;//通关关卡的星级
        public List<uint> stage_UnLock;//到达的关卡
        public ushort unLockStageCount;//解锁的关卡
        public ushort unLockChapterID;//解锁的章节
        public ushort starAllCount;//总星数

        public int GetSizeForWrite() {
            int size = 0;
            size += sizeof(int);

            size += sizeof(ushort) * 3;

            size += sizeof(int) * stage_Passed.Count * sizeof(uint);
            size += sizeof(int) * stage_PassedStar.Count * sizeof(uint);
            size += sizeof(int) * stage_UnLock.Count * sizeof(uint);

            Debug.Log("SaveDTO size: " + size);
            return size;
        }

        public void WriteToBuffer(byte[] dst, ref int offset) {
            GFBufferEncoderWriter.WriteInt32(dst, slotID, ref offset);

            GFBufferEncoderWriter.WriteUInt32List(dst, stage_Passed, ref offset);
            GFBufferEncoderWriter.WriteUInt32List(dst, stage_PassedStar, ref offset);
            GFBufferEncoderWriter.WriteUInt32List(dst, stage_UnLock, ref offset);

            GFBufferEncoderWriter.WriteUInt16(dst, unLockStageCount, ref offset);
            GFBufferEncoderWriter.WriteUInt16(dst, unLockChapterID, ref offset);
            GFBufferEncoderWriter.WriteUInt16(dst, starAllCount, ref offset);

        }

        public void FromBytes(byte[] src, ref int offset) {

            slotID = GFBufferEncoderReader.ReadInt32(src, ref offset);

            stage_Passed = GFBufferEncoderReader.ReadUInt32List(src, ref offset);
            stage_PassedStar = GFBufferEncoderReader.ReadUInt32List(src, ref offset);
            stage_UnLock = GFBufferEncoderReader.ReadUInt32List(src, ref offset);

            unLockStageCount = GFBufferEncoderReader.ReadUInt16(src, ref offset);
            unLockChapterID = GFBufferEncoderReader.ReadUInt16(src, ref offset);
            starAllCount = GFBufferEncoderReader.ReadUInt16(src, ref offset);
        }

    }
}