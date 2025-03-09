using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    public class SaveCore {
        byte[] writeSlotBuffer;

        const string MOUNT_NAME = "UF";

        public SaveCore() {
            writeSlotBuffer = new byte[1024*1024];
        }

        public void Inti(){
            
        }
    }
}