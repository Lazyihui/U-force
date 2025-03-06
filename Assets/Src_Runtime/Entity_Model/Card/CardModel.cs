using System;
using System.Collections;
using System.Collections.Generic;
using TriInspector;

namespace UF {

    public class CardModel {
        public int typeID;
        public int slotMax;
        public int stageCurType;

        public bool isTrain;
        [ShowIf(nameof(isTrain))] public List<RoleTM> aiTms;
        public List<RoleTM> roleTMs;
        public List<BulletTM> bulletAgrTMs;//农业
        public List<BulletTM> bulletIndTMs;//工业
        public List<BulletTM> bulletSciTMs;

        public List<RoleTM> roleEnemyTMs;

        public CardModel() {
            aiTms = new List<RoleTM>();
            roleTMs = new List<RoleTM>();
            stageCurType = 0;
            bulletAgrTMs = new List<BulletTM>();
            bulletIndTMs = new List<BulletTM>();
            bulletSciTMs = new List<BulletTM>();

            roleEnemyTMs = new List<RoleTM>();
        }

        public void AddRoleTM(RoleTM roleTM) {
            roleTMs.Add(roleTM);
        }

        public void AddBulletTM(BulletTM bulletTM) {
            bulletAgrTMs.Add(bulletTM);
        }

        public void AddBulletIndTM(BulletTM bulletTM) {
            bulletIndTMs.Add(bulletTM);
        }

        public void AddBulletSciTM(BulletTM bulletTM) {
            bulletSciTMs.Add(bulletTM);

        }

        public void AddRoleEnemyTM(RoleTM roleTM) {
            roleEnemyTMs.Add(roleTM);
        }

        public int GetRoleTMsCount() {
            return roleTMs.Count;
        }

        public int GetBulletAgrTMsCount() {
            return bulletAgrTMs.Count;
        }

        public int GetBulletIndTMsCount() {
            return bulletIndTMs.Count;
        }

        public int GetBulletSciTMsCount() {
            return bulletSciTMs.Count;
        }

        public int GetPropTMsCount() {
            return bulletAgrTMs.Count + bulletIndTMs.Count;
        }


    }
}