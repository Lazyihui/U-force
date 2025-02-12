using System;
using System.Collections;
using System.Collections.Generic;

namespace UF {

    public class CardModel {
        public int typeID;
        public List<RoleTM> roleTMs;
        public List<BulletTM> bulletTMs;

        public CardModel() {
            roleTMs = new List<RoleTM>();
            bulletTMs = new List<BulletTM>();
        }

        public void AddRoleTM(RoleTM roleTM) {
            roleTMs.Add(roleTM);
        }

        public void AddBulletTM(BulletTM bulletTM) {
            bulletTMs.Add(bulletTM);
        }

    }
}