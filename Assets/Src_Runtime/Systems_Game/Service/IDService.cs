namespace UF {
    public class IDService {
        public int roleID;
        public int BulletID;
        public int SkillID;

        public int TowerID;

        public int StuffID;
        public int PropID;

        // 唯一 看看之后要不要改
        public int mapID;
        public IDService() {
            roleID = 1;
            BulletID = 1;
            SkillID = 1;
            TowerID = 1;
            StuffID = 1;
            mapID = 1;
            PropID = 1;
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
        public int PickStuffID() {
            return ++StuffID;
        }
        public int PickMapID() {
            return ++mapID;
        }

        public int PickPropID() {
            return ++PropID;
        }
    }
}