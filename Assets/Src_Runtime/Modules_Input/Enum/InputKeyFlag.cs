using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {
    public enum InputKeyFlag {

        // World
        None = 0,
        Attack = 1 << 1,
        Weap1 = 1 << 2,
        Weap2 = 1 << 3,
        Weap3 = 1 << 4,
        Weap4 = 1 << 5,
        Tech = 1 << 6,

        Esc = 1 << 7,
        Tab = 1 << 8,

        MoveLeft = 1<<9,
        MoveRight = 1<<10,
        MoveUp = 1<<11,
        MoveDown = 1<<12,

        Grap = 1<<13,

        // player1
        Player1Jump = 1<<14,
        Player1Attack = 1<<15,
        Player1Defense = 1<<16,

        // player2
        Player2Jump = 1<<17,
        Player2Attack = 1<<18,
        Player2Defense = 1<<19,

        // UI
        Navigate = 1<<20,
        Submit = 1<<21,

    }
}