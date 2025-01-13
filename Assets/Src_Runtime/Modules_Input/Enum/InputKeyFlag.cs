using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {
    public enum InputKeyFlag {
        None = 0,
        Jump = 1 << 0,
        Attack = 1 << 1,
    }
}