using System;
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

        public RoleRepository roleRepository;

        public TrainSystemContext()
        {
            isRunning = false;
            idService = new IDService();
        }

        public void Inject(AppUI appUI, TemplateMoudle templateMoudle, AssetsModule assetsModule, RoleRepository roleRepository)
        {
            this.appUI = appUI;
            this.templateMoudle = templateMoudle;
            this.assetsModule = assetsModule;

            this.roleRepository = roleRepository;
        }
    }
}