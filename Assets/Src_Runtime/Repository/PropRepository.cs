using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    public class PropRepository {

        Dictionary<IDSignature, PropEntity> all;

        PropEntity[] temArray;

        public PropRepository() {
            all = new Dictionary<IDSignature, PropEntity>();
            temArray = new PropEntity[100];
        }

        public void Add(PropEntity entity) {
            all.Add(entity.idSig, entity);
        }

        public void Remove(PropEntity entity) {
            all.Remove(entity.idSig);
        }

        public int TakeAll(out PropEntity[] array) {
            if (all.Count > temArray.Length) {
                temArray = new PropEntity[all.Count * 2];
            }
            all.Values.CopyTo(temArray, 0);
            array = temArray;
            return all.Count;
        }

        public bool TryGet(IDSignature idSig, out PropEntity entity) {
            return all.TryGetValue(idSig, out entity);
        }

        public void Clear() {
            all.Clear();
        }
    }
}