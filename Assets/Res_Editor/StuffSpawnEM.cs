using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace UF {

    [ExecuteInEditMode]//在编辑器模式下执行
    public class StuffSpawnerEM : MonoBehaviour {
        public StuffSpawnerTM spawnTM;

        void Update() {


            var so = spawnTM.so;
            if (so == null) {
                return;
            }

            var tm = so.tm;
            string n = "Stuff_" + tm.typeName;
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
            Debug.Log("Save_StuffSpawnerEM");
            spawnTM.position = transform.position;
            spawnTM.rotation = transform.rotation.eulerAngles;
        }

    }
}