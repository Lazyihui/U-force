using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    public static class UIDomain {

        public static void Panel_CardSelection_Open(LoginSystemContext ctx, AppUI appUI, AssetsModule assetsModule, TemplateMoudle templateModule, int stageID, int chapterID) {

            // TODO：放到UIDomain里
            appUI.Panel_StageSelection_Close();

            ctx.cardModel = GameFactory.Card_Create(assetsModule, templateModule, 1);
            CardModel card = ctx.Get_CardModel();

            appUI.Panel_CardSelection_Open(stageID, chapterID);// 打开卡牌选择界面

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
                templateMoudle.Role_TryGet(typeID, out RoleTM roleTM);
                appUI.Panel_CardDetails_Update(roleTM.sprite, roleTM.content);

            } else if (cardType == CardType.Agriculture) {
                templateMoudle.Bullet_TryGet(typeID, out BulletTM bulletTM);
                appUI.Panel_CardDetails_Update(bulletTM.sprite, bulletTM.content);

            } else if (cardType == CardType.Industry) {
                templateMoudle.Bullet_TryGet(typeID, out BulletTM bulletTM);
                appUI.Panel_CardDetails_Update(bulletTM.sprite, bulletTM.content);


            } else if (cardType == CardType.Science) {
                templateMoudle.Bullet_TryGet(typeID, out BulletTM bulletTM);
                appUI.Panel_CardDetails_Update(bulletTM.sprite, bulletTM.content);

            }

        }

    }
}