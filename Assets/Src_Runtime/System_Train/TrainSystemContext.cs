using System;
using UF.System_Train;
using UnityEngine;

namespace UF
{
    public class TrainSystemContext
    {
        public bool isRunning;
        public float restFixTime;

        public IDService idService;

        public AppUI appUI;
        public TemplateMoudle templateMoudle;
        public AssetsModule assetsModule;
        public InputModule inputModule;

        public RoleRepository roleRepository;

        public TrainEntity trainEntity;

        public TrainSystemContext()
        {
            isRunning = false;
            idService = new IDService();

            trainEntity = new TrainEntity();
        }

        public void Inject(AppUI appUI, TemplateMoudle templateMoudle, AssetsModule assetsModule, InputModule inputModule, RoleRepository roleRepository)
        {
            this.appUI = appUI;
            this.templateMoudle = templateMoudle;
            this.assetsModule = assetsModule;
            this.inputModule = inputModule;

            this.roleRepository = roleRepository;
        }
    }
}