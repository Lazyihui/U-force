using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    [Serializable]
    public class CardTM {
        public int typeID;

        public List<RoleSo> roleSoArray;

        public List<BulletSo> bulletSoArray;
    }
}