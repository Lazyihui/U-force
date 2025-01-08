namespace UF {
    public class IDService {
        public int roleID;
        public int BulletID;

        public IDService() {
            roleID = 1;
            BulletID = 1;
        }

        public int PickRoleID() {
            return ++roleID;
        }
        public int PickBulletID() {
            return ++BulletID;
        }



    }
}