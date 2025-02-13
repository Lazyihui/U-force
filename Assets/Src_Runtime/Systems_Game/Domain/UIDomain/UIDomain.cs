using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    public static class UIDomain {

        public static void Panel_CardSelection_Open(AppUI appUI, AssetsModule assetsModule, TemplateMoudle templateModule, int stageID, int chapterID) {

            // TODO：放到UIDomain里
            appUI.Panel_StageSelection_Close();

            CardModel card = GameFactory.Card_Create(assetsModule, templateModule, 1);

            appUI.Panel_CardSelection_Open(stageID, chapterID);

            // 第一个
            RoleTM roleTM0 = card.roleTMs[0];
            appUI.Panel_CardDetails_Update(roleTM0.sprite, roleTM0.content);

            for (int i = 0; i < card.roleTMs.Count; i += 1) {
                RoleTM roleTM = card.roleTMs[i];
                appUI.Panel_CardSelection_Add(roleTM.typeID, roleTM.sprite);
            }
            appUI.Panel_SetCardType(CardType.Hero);

            appUI.Panel_WatchTower_Open();
            appUI.Panel_WatchTower_Add(1, null);
            appUI.Panel_WatchTower_Add(2, null);
            appUI.Panel_WatchTower_Add(3, null);
        }

        public static void Update_CardSelectionDetails(int typeID, TemplateMoudle templateMoudle, AppUI appUI) {

            CardType cardType = appUI.Panel_GetCardType();
            if (cardType == CardType.Hero) {
                bool has = templateMoudle.Role_TryGet(typeID, out RoleTM roleTM);

                appUI.Panel_CardDetails_Update(roleTM.sprite, roleTM.content);
                
            } else if (cardType == CardType.Agriculture) {
                appUI.Panel_CardDetails_Update(null, "农业卡");
            } else if (cardType == CardType.Industry) {
                appUI.Panel_CardDetails_Update(null, "工业卡");
            } else if (cardType == CardType.Science) {
                appUI.Panel_CardDetails_Update(null, "科技卡");
            }

        }

    }
}