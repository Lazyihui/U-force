using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {
    public class UIEventCenter {

        public Action OnLoginStartHandle;

        public void OnLoginStart() {
            if (OnLoginStartHandle != null) {
                OnLoginStartHandle();
            }
        }
    }
}