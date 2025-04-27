using System;
using UnityEngine;

namespace UF {

    [CreateAssetMenu(fileName = "So_Laser", menuName = "UF/So_Laser", order = 0)]
    public class LaserSo : ScriptableObject {
        public LaserTM tm;
    }
}