using System;
using UnityEngine;
namespace UF {

    public class RoleFeatureComponent {

        public FeatureType featureType;

        public RoleFeatureComponent() {
        }

        public void SetFeatureType(FeatureType value) {
            Debug.Log("怪物特性状态:" + value);
            featureType = value;
        }

    }
}