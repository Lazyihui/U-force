using System;
using UnityEngine;

namespace UF
{
    public class MapEntity : MonoBehaviour
    {
        public int mapID;

        public MapType type; // 地图类型

        public void Ctor()
        {

        }

        public void TearDown()
        {
            GameObject.Destroy(gameObject);
        }
    }
}