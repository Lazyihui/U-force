using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {
    public class SettingSystemEvents {
        #region Setting
        public Action OnSettingBackClickHandle;
        public void OnSettingBackClick() {
            if (OnSettingBackClickHandle != null) {
                OnSettingBackClickHandle();
            }
        }

        public Action OnSettingReselustion1920ClickHandle;
        public void OnSettingReselustion1920Click() {
            if (OnSettingReselustion1920ClickHandle != null) {
                OnSettingReselustion1920ClickHandle();
            }
        }

        public Action OnSettingReselustion1600ClickHandle;
        public void OnSettingReselustion1600Click() {
            if (OnSettingReselustion1600ClickHandle != null) {
                OnSettingReselustion1600ClickHandle();
            }
        }

        public Action OnSettingReselustion640ClickHandle;
        public void OnSettingReselustion640Click() {
            if (OnSettingReselustion640ClickHandle != null) {
                OnSettingReselustion640ClickHandle();
            }
        }
        #endregion
    }
}