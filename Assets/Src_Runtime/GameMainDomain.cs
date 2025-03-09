using System;
using System.Collections.Generic;
using GameFunctions;

namespace UF {

    public static class GameMainDomain {

        public static void Game_Load(GameSaveModel model, SaveDTO save) {
            model.slotID = save.slotID;

            model.stage_Passed = new HashSet<int>();
            foreach (Int32 u32 in save.stage_Passed) {
                model.stage_Passed.Add(u32);
            }

            model.stage_PassedStar = new Dictionary<int, int>();
            for (int i = 0; i < save.stage_PassedStar.Count; i++) {
                int value = save.stage_PassedStar[i];
                model.stage_PassedStar.Add(i, value);
            }

            model.stage_UnLock = new HashSet<int>();
            foreach (Int32 u32 in save.stage_UnLock) {
                model.stage_UnLock.Add(u32);
            }

            model.unLockStageCount = save.unLockStageCount;
            model.unLockChapterID = save.unLockChapterID;
            model.starAllCount = save.starAllCount;
        }

        //  public static void Game_Load(GameSaveModel model, in Saving.SaveDTO save) {

        //     model.slotID = save.slotID;
        //     model.version = save.version;

        //     model.map_passedMaps = new HashSet<StageSignature>();
        //     foreach (var u32 in save.map_passedMaps) {
        //         model.map_passedMaps.Add(u32);
        //     }
        //     model.map_totalMaps = new HashSet<StageSignature>();
        //     foreach (var u32 in save.map_totalMaps) {
        //         model.map_totalMaps.Add(u32);
        //     }
        //     model.map_stage = new StageSignature(save.map_chapter, save.map_stage, save.map_subStage);
        //     model.map_entranceIndex = save.map_entranceIndex;

        //     foreach (uint id in save.map_reachedMaps) {
        //         model.Map_Reached_Add(id);
        //     }

        // }
    }
}