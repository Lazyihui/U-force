namespace UF {
    public class IDService {
        public int roleID;
        public int BulletID;
        public int SkillID;

        public int TowerID;
        public IDService() {
            roleID = 1;
            BulletID = 1;
            SkillID = 1;
            TowerID = 1;
        }

        public int PickRoleID() {
            return ++roleID;
        }
        public int PickBulletID() {
            return ++BulletID;
        }
        public int PickSkillorID() {
            return ++SkillID;
        }
        public int PickTowerID() {
            return ++TowerID;
        }
    }
}