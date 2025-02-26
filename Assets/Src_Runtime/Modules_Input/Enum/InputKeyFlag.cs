using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {
    public enum InputKeyFlag {
        None = 0,
        Jump = 1 << 0,
        Attack = 1 << 1,
        Weap1 = 1 << 2,
        Weap2 = 1 << 3,
        Weap3 = 1 << 4,
        Weap4 = 1 << 5,
        Tech = 1 << 6,
        Grap = 1 << 7,
    }
}