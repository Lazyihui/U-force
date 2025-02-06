using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF
{

    public class MapRepository
    {
        Dictionary<IDSignature, MapEntity> all;

        MapEntity[] temArray;

        public MapRepository()
        {
            all = new Dictionary<IDSignature, MapEntity>();
            temArray = new MapEntity[100];
        }

        public void Add(MapEntity entity)
        {
            all.Add(entity.idSig, entity);
        }

        public void Remove(MapEntity entity)
        {
            all.Remove(entity.idSig);
        }

        public int TakeAll(out MapEntity[] array)
        {
            if (all.Count > temArray.Length)
            {
                temArray = new MapEntity[all.Count * 2];
            }
            all.Values.CopyTo(temArray, 0);
            array = temArray;
            return all.Count;
        }

        public bool TryGet(IDSignature idSig, out MapEntity entity)
        {
            return all.TryGetValue(idSig, out entity);
        }


        public void Clear()
        {
            all.Clear();
        }
    }
}