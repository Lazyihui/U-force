using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    public class TowerRepository {

        Dictionary<IDSignature, TowerEntity> all;

        TowerEntity[] temArray;

        public TowerRepository() {
            all = new Dictionary<IDSignature, TowerEntity>();
            temArray = new TowerEntity[100];
            tempTower = null;
        }

        public void Add(TowerEntity entity) {
            all.Add(entity.idSig, entity);
        }

        public void Remove(TowerEntity entity) {
            all.Remove(entity.idSig);
        }

        public int TakeAll(out TowerEntity[] array) {
            if (all.Count > temArray.Length) {
                temArray = new TowerEntity[all.Count * 2];
            }
            all.Values.CopyTo(temArray, 0);
            array = temArray;
            return all.Count;
        }

        public bool TryGet(IDSignature idSig, out TowerEntity entity) {
            return all.TryGetValue(idSig, out entity);
        }

        public TowerEntity tempTower;
        public TowerEntity GetRandom() {
            if (all.Count == 0) {
                return null;
            }

            if (tempTower != null) {
                return tempTower;
            }
            int index = -1;
            index = UnityEngine.Random.Range(0, all.Count);
            tempTower = temArray[index];
            return tempTower;
        }

        public void Clear() {
            all.Clear();
        }
    }
}