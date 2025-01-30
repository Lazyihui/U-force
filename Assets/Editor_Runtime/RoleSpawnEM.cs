using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace UF {

    [ExecuteInEditMode]//在编辑器模式下执行
    public class RoleSpawnerEM : MonoBehaviour {
        public RoleSpawnerTM spawnTM;

        void Upate() {

            var so = spawnTM.so;
            if (so == null) {
                return;
            }

            var tm = so.tm;
            string n = "roleSpawn_" + tm.typeName;
            if(gameObject.name != n) {
                gameObject.name = n;
            }

            // 没有图片报错
            if(tm.sprite == null){
                Debug.LogError("tm.sprite == null");
                return;
            }

            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if(sr == null){
                sr = gameObject.AddComponent<SpriteRenderer>();
            } 
            sr.sprite = tm.sprite;
            sr.sortingOrder = 0;

        }

        [ContextMenu("Save")]

        public void Save(){
            Debug.Log("Save_RoleSpawnerEM");
            spawnTM.position = transform.position;
            spawnTM.rotation = transform.rotation.eulerAngles; 
        }

    }
}