using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    public class SaveCore {
        byte[] writeSlotBuffer;

        const string MOUNT_NAME = "UF";

        public SaveCore() {
            writeSlotBuffer = new byte[1024 * 1024];
        }

        public void Inti() {
        }

        #region  Binding

        // 这里面的内容是保存绑定的内容 （暂时没看懂）
        public bool Binding_TryLoad(InputModule input) {
            string file = File_InputBinding();
            if (FileHepler.ExistFile(file)) {
                string json = FileHepler.ReadAllText_UTF8(file);
                if (!string.IsNullOrEmpty(json)) {
                    Debug.Log("Binding loaded: " + json);
                    input.FromJson(json);
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }

        public void Binding_Save(InputModule input) {
            string file = File_InputBinding();
            FileHepler.WriteAllText_UTF8(MOUNT_NAME, file, input.ToJson());
        }

        #endregion

        #region SlotGame

        public void Slot_Save(GameSaveModel playerSaveDate) {

        }


        #endregion

        // 保存游戏内容
        string File_Slot(int slot) {
            return FileHepler.CombineDirAndFile(Persistent(), $"save_{0}.UF");
        }

        // 保存绑定
        string File_InputBinding() {
            return FileHepler.CombineDirAndFile(Persistent(), "input_binding.cfg");
        }

        // 获取持久化数据存储路径。
        static string Persistent() {
            return FileHepler.GetPersistentDataPath(MOUNT_NAME);
        }

    }
}