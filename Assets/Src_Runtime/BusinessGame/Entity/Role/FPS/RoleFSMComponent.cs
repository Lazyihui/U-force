using System;
using UnityEngine;


namespace UF {
    public class RoleFSMConponent {

        RoleFSMStatus status;

        // public RoleFSMStatus Status {
        //     get {
        //         return status;
        //     }
        // }
        public RoleFSMStatus Status => status;

        public RoleFSMConponent() {
            status = RoleFSMStatus.Normal;
        }

        public void Normal_Enter() {
            status = RoleFSMStatus.Normal;
        }

        public void Dead_Enter() {
            status = RoleFSMStatus.Dead;
        }

        public void Casting_Enter() {
            status = RoleFSMStatus.Casting;
        }

    }
}