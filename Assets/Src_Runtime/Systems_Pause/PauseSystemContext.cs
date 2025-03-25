using System;
using UnityEngine;

namespace UF {
    public class PauseSystemContext {
        public bool isRunning;
        public float restFixTime;
        public AppUI appUI;

        public AudioModule audioModule;

        // repo
        public RoleRepository roleRepository;
        public BulletRepository bulletRepository;
        public PropRepository propRepository;

        public PauseSystemContext() {
            isRunning = false;
        }

        public void Inject(AppUI appUI, RoleRepository roleRepository, BulletRepository bulletRepository, PropRepository propRepository, AudioModule audioModule) {
            this.appUI = appUI;
            this.roleRepository = roleRepository;
            this.bulletRepository = bulletRepository;
            this.propRepository = propRepository;
            this.audioModule = audioModule;
        }
    }
}