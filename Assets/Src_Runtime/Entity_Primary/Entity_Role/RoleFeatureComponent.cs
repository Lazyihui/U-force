using System;
using UnityEngine;
namespace UF {

    public class RoleFeatureComponent {

        public FeatureType featureType;

        public RoleFeatureComponent() {
        }

        public void SetFeatureType(FeatureType value) {
            featureType = value;
        }

    }
}