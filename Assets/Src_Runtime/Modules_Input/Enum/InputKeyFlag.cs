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

        MoveLeft = 1 << 9,
        MoveRight = 1 << 10,
        MoveUp = 1 << 11,
        MoveDown = 1 << 12,

        Grap = 1 << 13,

        // player1
        Player1Jump = 1 << 14,
        Player1Attack = 1 << 15,
        Player1Defense = 1 << 16,

        // player2
        Player2Jump = 1 << 17,
        Player2Attack = 1 << 18,
        Player2Defense = 1 << 19,

        // UI
        Navigate = 1 << 20,
        Submit = 1 << 21,

        Jump = 1 << 22,

    }

    public static class InputKeyFlagExtension {

        static readonly Dictionary<InputKeyFlag, string> enumToStringDict = new Dictionary<InputKeyFlag, string>() {
            // World
            { InputKeyFlag.None, "None" },
            { InputKeyFlag.Attack, "Attack" },
            { InputKeyFlag.Weap1, "Weap1" },
            { InputKeyFlag.Weap2, "Weap2" },
            { InputKeyFlag.Weap3, "Weap3" },
            { InputKeyFlag.Weap4, "Weap4" },
            { InputKeyFlag.Tech, "Tech" },
            { InputKeyFlag.Esc, "Esc" },
            { InputKeyFlag.Tab, "Tab" },
            { InputKeyFlag.MoveLeft, "MoveLeft" },
            { InputKeyFlag.MoveRight, "MoveRight" },
            { InputKeyFlag.MoveUp, "MoveUp" },
            { InputKeyFlag.MoveDown, "MoveDown" },
            { InputKeyFlag.Grap, "Grap" },
            { InputKeyFlag.Jump , "Jump " },

            // player1
            { InputKeyFlag.Player1Jump, "Player1Jump" },
            { InputKeyFlag.Player1Attack, "Player1Attack" },
            { InputKeyFlag.Player1Defense, "Player1Defense" },

            // player2
            { InputKeyFlag.Player2Jump, "Player2Jump" },
            { InputKeyFlag.Player2Attack, "Player2Attack" },
            { InputKeyFlag.Player2Defense, "Player2Defense" },

            // UI
            { InputKeyFlag.Navigate, "Navigate" },
            { InputKeyFlag.Submit, "Submit" },

            };

        public static string ToInputKeyString(this InputKeyFlag key) {
            if (enumToStringDict.TryGetValue(key, out string value)) {
                return value;
            }
            Debug.LogError($"InputKeyFlag: {key} not found in dictionary." + string.Empty);
            return string.Empty;
        }

    }

}