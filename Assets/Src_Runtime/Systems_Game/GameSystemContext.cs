using System;
using System.Collections;
using UnityEngine;

namespace UF {

    public class GameSystemContext {
        public bool isRunning;

        public GameEntity gameEntity;
        public SaveCore saveCore;

        // ---  Common  ---
        public IDService idService;

        // ---  Core  ---
        public AssetsModule assetsModule;
        public TemplateMoudle templateCore;
        public AppUI appUI;
        public ModuleCamera moduleCamera;

        public InputModule inputModule;

        // ---  RoleRepository  ---
        public RoleRepository roleRepository;
        public BulletRepository bulletRepository;
        public CaveRepository towerRepository;
        public StuffRepository stuffRepository;

        public RopeEntity ropeEntity;

        public GameSystemContext() {
            isRunning = false;

            gameEntity = new GameEntity();
            idService = new IDService();


            towerRepository = new CaveRepository();
            stuffRepository = new StuffRepository();

            //  module
            moduleCamera = new ModuleCamera();
        }

        public void Inject(Canvas ScreenCanvas, AssetsModule assetsCore, TemplateMoudle templateCore, AppUI appUI, InputModule inputModule, RoleRepository roleRepository, BulletRepository bulletRepository, GameSaveModel playerSaveData, Camera camera, SaveCore saveCore) {
            this.assetsModule = assetsCore;
            this.templateCore = templateCore;
            this.appUI = appUI;
            this.inputModule = inputModule;

            this.moduleCamera.Inject(camera);

            // ---  Repos  ---
            this.roleRepository = roleRepository;
            this.bulletRepository = bulletRepository;
            this.saveCore = saveCore;
            gameEntity.Inject(playerSaveData);
        }

        public RoleEntity Get_Owner() {
            roleRepository.TryGet(gameEntity.OwnerIDsig, out RoleEntity role);
            return role;
        }

        public bool isRopeUnExist() {
            if (ropeEntity == null) {
                return true;
            }
            return false;
        }

    }

}