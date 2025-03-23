using System;
using System.Collections.Generic;
using GameFunctions;

namespace UF {
    [GFBufferEncoderMessage]
    public struct GameSaveDTO : IGFBufferEncoderMessage<GameSaveDTO> {
        public int slotID;
        public List<StageInfoDTO> all;

        public void Creat(int slotID) {
            this.slotID = slotID;
            all = new List<StageInfoDTO>();
        }
        public void WriteTo(byte[] dst, ref int offset) {
            GFBufferEncoderWriter.WriteInt32(dst, slotID, ref offset);
            GFBufferEncoderWriter.WriteMessageList(dst, all, ref offset);
        }

        public void FromBytes(byte[] src, ref int offset) {
            if (src.Length <= offset) {
                return;
            }

            slotID = GFBufferEncoderReader.ReadInt32(src, ref offset);
            if (src.Length <= offset) {
                return;
            }

            all = GFBufferEncoderReader.ReadMessageList(src, () => new StageInfoDTO(), ref offset);
        }
    }
}