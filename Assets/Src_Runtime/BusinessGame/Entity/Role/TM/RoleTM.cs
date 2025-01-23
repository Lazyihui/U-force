using System;
using UnityEngine;

namespace UF{

    [Serializable]
    public class RoleTM{
        public string typeName; 
        public RoleType roleType;
        public int typeID;

        public SkillorSo[] skillorSoArray;

        public RoleMod modPrefab;

    }
}
