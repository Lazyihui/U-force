using System;
using UnityEngine;
using System.Collections.Generic;
using GameFunctions;

namespace UF
{
    [GFBufferEncoderMessage]
    public struct StageInfoDTO : IGFBufferEncoderMessage<StageInfoDTO>
    {
        public int slotID;
        public int stageID;
        public int stageCount;
        public int passStatus;
        public void WriteTo(byte[] dst, ref int offset)
        {
            GFBufferEncoderWriter.WriteInt32(dst, slotID, ref offset);
            GFBufferEncoderWriter.WriteInt32(dst, stageID, ref offset);
            GFBufferEncoderWriter.WriteInt32(dst, stageCount, ref offset);
            GFBufferEncoderWriter.WriteInt32(dst, passStatus, ref offset);
        }

        public void FromBytes(byte[] src, ref int offset)
        {
            if (src.Length <= offset)
            {
                return;
            }

            slotID = GFBufferEncoderReader.ReadInt32(src, ref offset);
            if (src.Length <= offset)
            {
                return;
            }

            stageID = GFBufferEncoderReader.ReadInt32(src, ref offset);
            if (src.Length <= offset)
            {
                return;
            }

            stageCount = GFBufferEncoderReader.ReadInt32(src, ref offset);
            if (src.Length <= offset)
            {
                return;
            }

            passStatus = GFBufferEncoderReader.ReadInt32(src, ref offset);
        }
    }
}