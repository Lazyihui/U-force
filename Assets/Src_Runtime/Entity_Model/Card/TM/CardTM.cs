using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    [Serializable]
    public class CardTM {
        public int typeID;
        public int slotMax;
        public List<RoleSo> roleSoArray;
        public List<BulletSo> bulletAgrSoArray;
        public List<BulletSo> bulletIndSoArray;
        public List<BulletSo> bulletSciSoArray;

        public List<RoleSo> roleEnemySoArray; // 小怪和boss, 用于瞭望塔

        public bool isTrain;
    }
}