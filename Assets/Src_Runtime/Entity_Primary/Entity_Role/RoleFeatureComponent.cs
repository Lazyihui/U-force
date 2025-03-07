using System;
using UnityEngine;
namespace UF {

    public class RoleFeatureComponent {

        public FeatureType featureType;

        public float AccelerateTimer;

        public RoleFeatureComponent() {
        }

        public void SetFeatureType(FeatureType value) {
            featureType = value;
        }

        public void Init(float accelerateTimer) {
            AccelerateTimer = accelerateTimer;
        }

    }
}