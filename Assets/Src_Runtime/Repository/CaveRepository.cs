using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    public class CaveRepository {

        Dictionary<IDSignature, CaveEntity> all;

        CaveEntity[] temArray;

        public CaveRepository() {
            all = new Dictionary<IDSignature, CaveEntity>();
            temArray = new CaveEntity[100];
            tempTower = null;
        }

        public void Add(CaveEntity entity) {
            all.Add(entity.idSig, entity);
        }

        public void Remove(CaveEntity entity) {
            all.Remove(entity.idSig);
        }

        public int TakeAll(out CaveEntity[] array) {
            if (all.Count > temArray.Length) {
                temArray = new CaveEntity[all.Count * 2];
            }
            all.Values.CopyTo(temArray, 0);
            array = temArray;
            return all.Count;
        }

        public bool TryGet(IDSignature idSig, out CaveEntity entity) {
            return all.TryGetValue(idSig, out entity);
        }

        public CaveEntity tempTower;
        public CaveEntity GetRandom() {
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