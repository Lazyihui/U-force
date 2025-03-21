using System;
using UnityEngine;

namespace UF {

    public struct StageInfoModel {

        public int slotID;
        public int stageID;
        public int starCount;
        public StagePassStatus passStatus;

        public void FromDTO(in StageInfoDTO dto) {
            slotID = dto.slotID;
            stageID = dto.stageID;
            starCount = dto.stageCount;
            passStatus = (StagePassStatus)dto.passStatus;

        }

        public StageInfoDTO ToDTO() {
            StageInfoDTO dto;
            dto.slotID = slotID;
            dto.stageID = stageID;
            dto.stageCount = starCount;
            dto.passStatus = (int)passStatus;
            return dto;
        }

    }

}