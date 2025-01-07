namespace UF {
    public class IDService {
        public int roleID;

        public IDService() {
            roleID = 1;
        }

         public int PickRoleID() {
            return ++roleID;
        }

    }
}