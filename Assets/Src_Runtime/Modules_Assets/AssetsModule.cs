using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace UF {


    public class AssetsModule : MonoBehaviour {
        public Dictionary<string, GameObject> entities;

        public AsyncOperationHandle entitiesHandle;

        public Dictionary<string, GameObject> panels;

        public AsyncOperationHandle panelsHandle;

        public void Ctor() {
            entities = new Dictionary<string, GameObject>();
            panels = new Dictionary<string, GameObject>();
        }


        public async Task LoadAll() {
            {
                AssetLabelReference labelReference = new AssetLabelReference();

                labelReference.labelString = "Entity";
                var handle = Addressables.LoadAssetsAsync<GameObject>(labelReference, null);

                var all = await handle.Task;
                foreach (var item in all) {
                    entities.Add(item.name, item);
                }

                entitiesHandle = handle;
            }

            {
                AssetLabelReference labelReference = new AssetLabelReference();
                labelReference.labelString = "Panel";
                var handle = Addressables.LoadAssetsAsync<GameObject>(labelReference, null);

                var all = await handle.Task;

                foreach (var item in all) {
                    panels.Add(item.name, item);
                }

                panelsHandle = handle;
            }
        }


        public void UnLoadAll() {
            if (entitiesHandle.IsValid()) {
                Addressables.Release(entitiesHandle);
            }
            if (panelsHandle.IsValid()) {
                Addressables.Release(panelsHandle);
            }
        }
        // ---  Entity  ---
        public GameObject Entity_GetRole() {
            entities.TryGetValue("Entity_Role", out GameObject role);
            return role;
        }

        public GameObject Entity_GetBullet() {
            entities.TryGetValue("Entity_Bullet", out GameObject bullet);
            return bullet;
        }

        public GameObject Entity_GetTower() {
            entities.TryGetValue("Entity_Tower", out GameObject tower);
            return tower;
        }

        public GameObject Entity_GetStuff() {
            entities.TryGetValue("Entity_Stuff", out GameObject stuff);
            return stuff;
        }

        //  ---  Panel  ---
        public GameObject Panel_GetLogin() {
            panels.TryGetValue("Panel_Login", out GameObject panel);
            return panel;
        }

        public GameObject Panel_GetGamePause() {
            panels.TryGetValue("Panel_GamePause", out GameObject panel);
            return panel;
        }

        public GameObject Panel_GetGameDefend_Button() {
            panels.TryGetValue("Panel_GameDefend_Button", out GameObject panel);
            return panel;
        }

        public GameObject Panel_GetGameOver() {
            panels.TryGetValue("Panel_GameOver", out GameObject panel);
            return panel;
        }
        public GameObject Panel_GetStageSelection() {
            panels.TryGetValue("Panel_StageSelection", out GameObject panel);
            return panel;
        }

        public GameObject Panel_GetCardSelection() {
            panels.TryGetValue("Panel_CardSelection", out GameObject panel);
            return panel;
        }

        public GameObject Panel_GetCardDetails() {
            panels.TryGetValue("Panel_CardDetails", out GameObject panel);
            return panel;
        }

        public GameObject Panel_GetBackSelection() {
            panels.TryGetValue("Panel_BackSelection", out GameObject panel);
            return panel;
        }

        public GameObject Panel_GetWatchTower() {
            panels.TryGetValue("Panel_WatchTower", out GameObject panel);
            return panel;
        }

        public GameObject Panel_GetWatchDetail() {
            panels.TryGetValue("Panel_WatchDetail", out GameObject panel);
            return panel;
        }

        public GameObject Panel_GetTraining_Select() {
            panels.TryGetValue("Panel_Training_Select", out GameObject panel);
            return panel;
        }

        public GameObject Panel_GetTraining_Tip() {
            panels.TryGetValue("Panel_Training_Tip", out GameObject panel);
            return panel;
        }

        // HUD
        public GameObject HUD_GetProgressbar() {
            panels.TryGetValue("HUD_Progressbar", out GameObject hud);
            return hud;
        }

        public GameObject HUD_GetCD() {
            panels.TryGetValue("HUD_CD", out GameObject hud);
            return hud;
        }
    }
}