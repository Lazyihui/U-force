using System;
using System.Collections;
using UnityEngine;

namespace UF {

    public class GameContext {
        public GameEntity gameEntity;

        // ---  Common  ---
        public IDService idService;

        // ---  Core  ---
        public AssetsCore assetsCore;
        public TemplateCore templateCore;
        public AppUI appUI;

        // ---  RoleRepository  ---
        public RoleRepository roleRepository;

        public GameContext() {
            gameEntity = new GameEntity();
            idService = new IDService();

            assetsCore = new AssetsCore();
            templateCore = new TemplateCore();
            appUI = new AppUI();

            roleRepository = new RoleRepository();
        }

        public void Inject(Canvas ScreenCanvas) {
            appUI.Inject(ScreenCanvas, assetsCore);
        }

        public RoleEntity Get_Owner() {
            roleRepository.TryGet(gameEntity.OwnerIDsig, out RoleEntity role);
            return role;
        }
    }

}