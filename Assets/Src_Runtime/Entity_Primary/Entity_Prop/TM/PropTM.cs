using System;
using UnityEngine;
using TriInspector;

namespace UF
{
    [Serializable]
    public class PropTM
    {
        public int typeID;
        public string typeName;

        public PropType type;

        public bool isPlayerProp;
        

        [Title("Locomotion")]
        public PropMod modPrefab;
    }
}