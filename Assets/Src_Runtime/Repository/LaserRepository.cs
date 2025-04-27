using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    public class LaserRepository {

        Dictionary<IDSignature, LaserEntity> all;

        LaserEntity[] temArray;

        public LaserRepository() {
            all = new Dictionary<IDSignature, LaserEntity>();
            temArray = new LaserEntity[100];
        }

        public void Add(LaserEntity entity) {
            all.Add(entity.idSig, entity);
        }

        public void Remove(LaserEntity entity) {
            all.Remove(entity.idSig);
        }

        public int TakeAll(out LaserEntity[] array) {
            if (all.Count > temArray.Length) {
                temArray = new LaserEntity[all.Count * 2];
            }
            all.Values.CopyTo(temArray, 0);
            array = temArray;
            return all.Count;
        }

        public bool TryGet(IDSignature idSig, out LaserEntity entity) {
            return all.TryGetValue(idSig, out entity);
        }

        public void Clear() {
            all.Clear();
        }
    }
}