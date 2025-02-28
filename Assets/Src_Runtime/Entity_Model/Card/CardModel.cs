using System;
using System.Collections;
using System.Collections.Generic;

namespace UF {

    public class CardModel {
        public int typeID;
        public int slotMax;
        public List<RoleTM> roleTMs;
        public List<BulletTM> bulletAgrTMs;
        public List<BulletTM> bulletIndTMs;
        public List<BulletTM> bulletSciTMs;

        public List<RoleTM> roleEnemyTMs;

        public CardModel() {
            roleTMs = new List<RoleTM>();
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

        public int GetBulletTMsCount() {
            return bulletAgrTMs.Count;
        }

        public int GetBulletIndTMsCount() {
            return bulletIndTMs.Count;
        }

        public int GetBulletSciTMsCount() {
            return bulletSciTMs.Count;
        }
    }
}