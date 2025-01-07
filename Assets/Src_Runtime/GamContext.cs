using System;
using System.Collections;
using UnityEngine;

namespace UF {

    public class GameContext {
        public GameEntity game;

        // ---  Common  ---
        public IDService idService;

        // ---  Core  ---
        public AssetsCore assetsCore;
        public TemplateCore templateCore;

        // ---  RoleRepository  ---
        public RoleRepository roleRepository;

        public GameContext() {
            game = new GameEntity();
            idService = new IDService();

            assetsCore = new AssetsCore();
            templateCore = new TemplateCore();

            roleRepository = new RoleRepository();
        }

        public void Inject(Canvas ScreenCanvas) {

        }
    }

}