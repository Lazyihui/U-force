using System;
using UnityEngine;

namespace UF {
    public class PauseSystemContext {
        public GameEntity gameEntity;
        public AppUI appUI;

        // repo
        public RoleRepository roleRepository;

        public void Inject(GameEntity gameEntity, AppUI appUI, RoleRepository roleRepository) {
            this.appUI = appUI;
            this.gameEntity = gameEntity;
            this.roleRepository = roleRepository;
        }
    }
}