using System;
using System.Collections;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace UF {

    [Serializable]
    public class CardTM {
        public int typeID;
        public int slotMax;
        public int stageCurType;


        public bool isTrain;
        [ShowIf(nameof(isTrain))] public List<RoleSo> aiSoArray;

        public List<RoleSo> roleSoArray;
        public List<BulletSo> bulletAgrSoArray;
        public List<BulletSo> bulletIndSoArray;
        public List<BulletSo> bulletSciSoArray;

        public List<RoleSo> roleEnemySoArray; // 小怪和boss, 用于瞭望塔
    }
}