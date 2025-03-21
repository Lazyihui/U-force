using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using GameFunctions;

namespace UF {

    public class SaveCore {
        byte[] writeSlotBuffer;

        const string MOUNT_NAME = "UF";

        public SaveCore() {
            writeSlotBuffer = new byte[1024 * 1024];
        }

        public void Inti() {
        }

        #region   DTO
        // where T : IGFBufferEncoderMessage<T>：约束 T 必须实现 IGFBufferEncoderMessage<T> 接口。
        void SaveDTO<T>(string file, T dto) where T : IGFBufferEncoderMessage<T> {
            int offset = 0;
            dto.WriteTo(writeSlotBuffer, ref offset);
            FileHepler.WriteFileAfterClean(MOUNT_NAME, file, writeSlotBuffer, offset);
        }

        bool TryLoadDTO<T>(string file, out T dto) where T : IGFBufferEncoderMessage<T>, new() {
            if (FileHepler.ExistFile(file)) {
                FileHepler.ReadFile(file, out var readBuffer);
                int offset = 0;
                byte[] newBuffer = new byte[readBuffer.Length * 2];
                // 复制数组
                Array.Copy(readBuffer, newBuffer, readBuffer.Length);
                dto = new T();
                dto.FromBytes(newBuffer, ref offset);
                return true;
            } else {
                dto = default;
                return false;
            }
        }

        #region 读 写 删
        public void Slot_Full_TryLoad(int slotID, out SaveSlotFullDTO dto) {
            dto = new SaveSlotFullDTO();
            bool has;

            has = GameSave_TryLoad(slotID, out dto.gameSaveDTO);
            if (has) {
                dto.gameSaveDTO.slotID = slotID;
            } else {
                dto.gameSaveDTO.Creat(slotID);
            }
        }

        public void Slot_Full_Save(GameEntity save) {

            var dto = save.ToDTO();
            GameSave_Save(dto.gameSaveDTO);

        }

        public void DeleteSlot(int slotID) {
            Delete(File_Slot(slotID));
        }


        #endregion
        #region Slot: GameSaveModel
        void GameSave_Save(in GameSaveDTO dto) {
            string file = GameSave_FilePath(dto.slotID);
            SaveDTO(file, dto);
        }

        bool GameSave_TryLoad(int slotID, out GameSaveDTO save) {
            string file = GameSave_FilePath(slotID);
            return TryLoadDTO(file, out save);
        }

        string GameSave_FilePath(int slot) {
            return FileHepler.CombineDirAndFile(Persistent(), $"save_{slot}.UF");
        }

        #endregion
        // public bool Slot_TryLoad(int slotID, out GameSaveDTO save) {
        //     string file = File_Slot(slotID);
        //     if (FileHepler.ExistFile(file)) {
        //         FileHepler.ReadFile(file, out var readBuffer);
        //         int offset = 0;
        //         byte[] newBuffer = new byte[readBuffer.Length * 2];
        //         // 复制数组
        //         Array.Copy(readBuffer, newBuffer, readBuffer.Length);
        //         save = new GameSaveDTO();
        //         save.FromBytes(newBuffer, ref offset);
        //         return true;
        //     } else {
        //         save = default;
        //         return false;
        //     }
        // }

        #endregion

        #region Delete
        void Delete(string file) {
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