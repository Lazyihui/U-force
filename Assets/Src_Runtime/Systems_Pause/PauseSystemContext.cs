using System;
using UnityEngine;

namespace UF {
    public class PauseSystemContext {
        public bool isRunning;
        public float restFixTime;
        public AppUI appUI;

        // repo
        public RoleRepository roleRepository;

        public PauseSystemContext() {
            isRunning = false;
        }

        public void Inject(AppUI appUI, RoleRepository roleRepository) {
            this.appUI = appUI;
            this.roleRepository = roleRepository;
        }
    }
}