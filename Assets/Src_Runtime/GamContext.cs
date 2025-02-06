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

        public InputModule inputModule;

        // ---  RoleRepository  ---
        public RoleRepository roleRepository;
        public BulletRepository bulletRepository;
        public CaveRepository towerRepository;
        public StuffRepository stuffRepository;
        public MapRepository mapRepository;

        public GameContext() {
            gameEntity = new GameEntity();
            idService = new IDService();

            assetsCore = new AssetsCore();
            templateCore = new TemplateCore();
            appUI = new AppUI();

            inputModule = new InputModule();

            roleRepository = new RoleRepository();
            bulletRepository = new BulletRepository();
            towerRepository = new CaveRepository();
            stuffRepository = new StuffRepository();
            mapRepository = new MapRepository();
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