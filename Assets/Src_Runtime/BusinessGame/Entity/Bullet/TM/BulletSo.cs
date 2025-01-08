using System;
using UnityEngine;

namespace UF {

    [CreateAssetMenu(fileName = "So_Bullet", menuName = "UF/So_Bullet", order = 0)]
    public class BulletSo : ScriptableObject {
        public BulletTM tm;
    }
}