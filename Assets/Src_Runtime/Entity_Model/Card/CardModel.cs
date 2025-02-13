using System;
using System.Collections;
using System.Collections.Generic;

namespace UF {

    public class CardModel {
        public int typeID;
        public List<RoleTM> roleTMs;
        public List<BulletTM> bulletAgrTMs;
        public List<BulletTM> bulletIndTMs;
        public List<BulletTM> bulletSciTMs;

        public CardModel() {
            roleTMs = new List<RoleTM>();
            bulletAgrTMs = new List<BulletTM>();
            bulletIndTMs = new List<BulletTM>();
            bulletSciTMs = new List<BulletTM>();
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


    }
}