using System;
using UnityEngine;
using UnityEditor;

namespace UF {

    [ExecuteInEditMode]
    public class TowerSpawnEM : MonoBehaviour {

        public TowerSpawnTM spawnTM;

        public void Upate() {
            var so = spawnTM.so;
            if (so == null) {
                return;
            }

            var tm = so.tm;
            string n = "towerSpawn_" + tm.typeName;
            if (gameObject.name != n) {
                gameObject.name = n;
            }

            // 没有图片报错
            if (tm.sprite == null) {
                Debug.LogError("tm.sprite == null");
                return;
            }

            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr == null) {
                sr = gameObject.AddComponent<SpriteRenderer>();
            }
            sr.sprite = tm.sprite;
            sr.sortingOrder = 0;

        }

        [ContextMenu("Save")]

        public void Save() {
            Debug.Log("Save_TowerSpawnEM");
            spawnTM.position = transform.position;
            spawnTM.rotation = transform.rotation.eulerAngles;
        }

    }
}