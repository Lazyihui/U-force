using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    [Serializable]
    public class CardTM {
        public int typeID;

        public List<RoleTM> roleTMs;

        public List<BulletTM> bulletTMs;
    }
}