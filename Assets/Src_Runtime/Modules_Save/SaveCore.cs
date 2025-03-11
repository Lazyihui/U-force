using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

        // 保存存档
        public void Slot_Save(GameSaveModel model) {
            string file = File_Slot(model.slotID);

            SaveDTO save;
            save.slotID = model.slotID;

            save.stage_Passed = new List<int>();
            foreach (var int32 in model.stage_Passed) {
                save.stage_Passed.Add(int32);
                Debug.Log("已经通关的关卡: " + int32);
            }

            save.stage_PassedStar = new List<int>();
            foreach (var int32 in model.stage_PassedStar.Values) {
                save.stage_PassedStar.Add(int32);
            }

            save.stage_UnLock = new List<int>();
            foreach (var int32 in model.stage_UnLock) {
                save.stage_UnLock.Add(int32);
            }

            save.unLockChapterID = model.unLockChapterID;
            save.starAllCount = model.starAllCount;

            int offset = 0;
            int size = save.GetSizeForWrite();
            // 如果缓冲区不够大，重新分配
            if (writeSlotBuffer.Length < size) {
                writeSlotBuffer = new byte[size];
            }
            // 写入数据
            save.WriteTo(writeSlotBuffer, ref offset);
            // 写入文件
            FileHepler.WriteFileAfterClean(MOUNT_NAME, file, writeSlotBuffer, offset);

            Debug.Log("Slot saved: " + size);
        }

        // 读取存 是否有存档SlotID
        public bool Slot_Has(int slotID) {
            string file = File_Slot(slotID);
            return FileHepler.ExistFile(file);
        }

        public bool Slot_TryLoad(int slotID, out SaveDTO save) {
            string file = File_Slot(slotID);
            if (FileHepler.ExistFile(file)) {
                FileHepler.ReadFile(file, out var readBuffer);
                int offset = 0;
                byte[] newBuffer = new byte[readBuffer.Length * 2];
                // 复制数组
                Array.Copy(readBuffer, newBuffer, readBuffer.Length);
                save = new SaveDTO();
                save.FromBytes(newBuffer, ref offset);
                Debug.Log("Slot loaded: " + slotID);
                return true;
            } else {
                save = default;
                Debug.LogError("Slot not found: " + slotID);
                return false;
            }

        }
        // 合并存档
        public SaveDTO CombineRightToLeft(SaveDTO a, SaveDTO b) {
            b.stage_Passed.ForEach(id => {
                a.stage_Passed.Add(id);
            });
            b.stage_PassedStar.ForEach(id => {
                a.stage_PassedStar.Add(id);
            });
            b.stage_UnLock.ForEach(id => {
                a.stage_UnLock.Add(id);
            });
            return a;
        }

        // 删除存档
        public void Slot_Delete(int slotID) {
            string file = File_Slot(slotID);
            if (FileHepler.ExistFile(file)) {
                FileHepler.DeleteFile(MOUNT_NAME, file);
            }
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
        #region Clear
        public static void UNSAFE_ClearSave() {
            {
                string dir = Persistent();
                if (Directory.Exists(dir)) {
                    Directory.Delete(dir, true);
                    Debug.Log("Cur Save cleared: " + dir);
                }
            }
        }
        #endregion
    }
}