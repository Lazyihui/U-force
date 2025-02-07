using System;
using System.Collections;
using UnityEngine;

namespace UF {

    public class GameContext {

        public GameEntity gameEntity;

        // ---  Common  ---
        public IDService idService;

        // ---  Core  ---
        public AssetsModule assetsModule;
        public TemplateMoudle templateCore;
        public AppUI appUI;

        public InputModule inputModule;

        // ---  RoleRepository  ---
        public RoleRepository roleRepository;
        public BulletRepository bulletRepository;
        public CaveRepository towerRepository;
        public StuffRepository stuffRepository;
        public MapRepository mapRepository;

        public GameContext() {
            idService = new IDService();


            bulletRepository = new BulletRepository();
            towerRepository = new CaveRepository();
            stuffRepository = new StuffRepository();
            mapRepository = new MapRepository();
        }

        public void Inject(GameEntity gameEntity, Canvas ScreenCanvas, AssetsModule assetsCore, TemplateMoudle templateCore, AppUI appUI, InputModule inputModule, RoleRepository roleRepository) {
            this.assetsModule = assetsCore;
            this.templateCore = templateCore;
            this.appUI = appUI;
            this.inputModule = inputModule;

            this.gameEntity = gameEntity;

            // ---  Repos  ---
            this.roleRepository = roleRepository;
        }

        public RoleEntity Get_Owner() {
            roleRepository.TryGet(gameEntity.OwnerIDsig, out RoleEntity role);
            return role;
        }
    }

}