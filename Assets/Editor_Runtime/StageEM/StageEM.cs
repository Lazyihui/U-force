using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UF {

    public class StageEM : MonoBehaviour {
        public int stageID;
        public StageSo so;

        [SerializeField] GameObject mapEntity;

        [ContextMenu("save")]
        public void Save() {

            string n = "stage_" + stageID;

            if (gameObject.name != n) {
                gameObject.name = n;
            }

            so.tm.stageID = stageID;

            Debug.Log("Save " + n);

            // 引用所有的TM
            SaveRole();
            SaveTower();
            SaveStuff();
        }


        public void SaveRole() {
            RoleSpawnerEM[] rolesEM = GetComponentsInChildren<RoleSpawnerEM>();
            Debug.Log("roleEM.Length = " + rolesEM.Length);
            RoleSpawnerTM[] rolesTM = new RoleSpawnerTM[rolesEM.Length];

            for (int i = 0; i < rolesEM.Length; i++) {
                RoleSpawnerEM em = rolesEM[i];

                em.Save();
                rolesTM[i] = em.spawnTM;
            }

            so.tm.roleSpawns = rolesTM;
            EditorUtility.SetDirty(so);

        }

        public void SaveTower() {
            TowerSpawnEM[] towersEM = GetComponentsInChildren<TowerSpawnEM>();
            Debug.Log("towerEM.Length = " + towersEM.Length);
            CaveSpawnerTM[] towersTM = new CaveSpawnerTM[towersEM.Length];

            for (int i = 0; i < towersEM.Length; i++) {
                TowerSpawnEM em = towersEM[i];

                em.Save();
                towersTM[i] = em.spawnTM;
            }

            so.tm.towerSpawns = towersTM;
            EditorUtility.SetDirty(so);
        }

        public void SaveStuff() {
            StuffSpawnerEM[] stuffsEM = GetComponentsInChildren<StuffSpawnerEM>();
            Debug.Log("stuffEM.Length = " + stuffsEM.Length);
            StuffSpawnerTM[] stuffsTM = new StuffSpawnerTM[stuffsEM.Length];

            for (int i = 0; i < stuffsEM.Length; i++) {
                StuffSpawnerEM em = stuffsEM[i];

                em.Save();
                stuffsTM[i] = em.spawnTM;
            }

            so.tm.stuffSpawns = stuffsTM;
            EditorUtility.SetDirty(so);
        }

    }
}