using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {
    public class UIEventCenter {

        //Login
        public Action OnLoginStartHandle;

        //Game_Defend
        public Action OnGamePauseHandle;
        public Action OnAttackHandle;

        public void OnLoginStart() {
            if (OnLoginStartHandle != null) {
                OnLoginStartHandle();
            }
        }
        
        public void OnGamePause()
        {
            if (OnGamePauseHandle!= null)
            {
                OnGamePauseHandle();
            }
        }

        public void OnAttack()
        {
            if (OnAttackHandle!= null)
            {
                OnAttackHandle();
            }
        }
    }
}