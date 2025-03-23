using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    public static class UIDomain {

        public static void Panel_CardSelection_Open(AppUI appUI, AssetsModule assetsModule, TemplateMoudle templateModule) {

            int stageID = appUI.playerSaveData.GetStageCurID();
            int chapterID = appUI.playerSaveData.GetChapterCurID();
            appUI.Panel_StageSelection_Close();

            CardModel spawnCard = GameFactory.Card_Create(assetsModule, templateModule, stageID);
            appUI.Set_CardModel(spawnCard);
            CardModel card = appUI.Get_CardModel();

            // 把怪物的typeID存下来
            var playerSaveData = appUI.playerSaveData;
            playerSaveData.ClearEnemyTypeID();
            for (int i = 0; i < card.roleEnemyTMs.Count; i += 1) {
                RoleTM roleTM = card.roleEnemyTMs[i];
                playerSaveData.EnemyTypeIDs.Add(roleTM.typeID);
            }

            // 记录卡槽数
            playerSaveData.Set_slotMax(card.slotMax);

            appUI.Panel_CardSelection_Open(stageID, card.stageCurType);// 打开卡牌选择界面
            appUI.Panel_CardSelection_AddCardSelected(playerSaveData.slotMax);// 放卡片的地方

            appUI.Panel_SetCardType(CardType.Hero);

            // 第一个
            RoleTM roleTM0 = card.roleTMs[0];
            appUI.Panel_CardDetails_Update(roleTM0.sprite, roleTM0.feature, roleTM0.typeID);

            for (int i = 0; i < card.roleTMs.Count; i += 1) {
                RoleTM roleTM = card.roleTMs[i];
                appUI.Panel_CardSelection_AddCard(roleTM.typeID, roleTM.sprite);
            }

            appUI.Panel_WatchTower_Open();
            for (int i = 0; i < card.roleEnemyTMs.Count; i += 1) {
                RoleTM roleTM = card.roleEnemyTMs[i];
                appUI.Panel_WatchTower_Add(roleTM.typeID, roleTM.sprite);
            }

            int indCount = card.GetBulletIndTMsCount();
            if (indCount == 0) {
                appUI.Panel_CardSelected_IndButton(false);
            }

            int agrCount = card.GetBulletAgrTMsCount();
            if (agrCount == 0) {
                appUI.Panel_CardSelected_AgrButton(false);
            }

            int sciCount = card.GetBulletSciTMsCount();
            if (sciCount == 0) {
                appUI.Panel_CardSelected_SciButton(false);
            }

        }

        // 卡片更新
        public static void Update_CardSelectionDetails(int typeID, TemplateMoudle templateMoudle, AppUI appUI) {
            // 确认按钮打开

            CardType cardType = appUI.Panel_GetCardType();
            if (cardType == CardType.Hero) {
                templateMoudle.Role_TryGet(typeID, out RoleTM roleTM);
                appUI.Panel_CardDetails_Update(roleTM.sprite, roleTM.feature, roleTM.typeID);

            } else if (cardType == CardType.Agriculture) {
                templateMoudle.Bullet_TryGet(typeID, out BulletTM bulletTM);
                appUI.Panel_CardDetails_Update(bulletTM.sprite, bulletTM.content, bulletTM.typeID);

            } else if (cardType == CardType.Industry) {
                templateMoudle.Bullet_TryGet(typeID, out BulletTM bulletTM);
                appUI.Panel_CardDetails_Update(bulletTM.sprite, bulletTM.content, bulletTM.typeID);

            } else if (cardType == CardType.Science) {
                templateMoudle.Bullet_TryGet(typeID, out BulletTM bulletTM);
                appUI.Panel_CardDetails_Update(bulletTM.sprite, bulletTM.content, bulletTM.typeID);

            }
        }

        public static void Panel_OneCardSelection_Update(AppUI appUI, CardType type) {
            appUI.Panel_SetCardType(type);

            // 1.先清空卡片选择界面 
            appUI.Panel_OneCardSelection_Clear();
            var selectedInFormation = appUI.selectedInFormation;

            // 2.展示卡片
            CardType cardType = appUI.Panel_GetCardType();
            var cardModel = appUI.Get_CardModel();

            if (cardType == CardType.Hero) {
                for (int i = 0; i < cardModel.roleTMs.Count; i += 1) {
                    RoleTM roleTM = cardModel.roleTMs[i];
                    appUI.Panel_CardSelection_AddCard(roleTM.typeID, roleTM.sprite);

                    // 更新右边详情页
                    RoleTM roleTM0 = cardModel.roleTMs[0];
                    appUI.Panel_CardDetails_Update(roleTM0.sprite, roleTM0.feature, roleTM0.typeID);


                }
            } else if (cardType == CardType.Agriculture) {
                for (int i = 0; i < cardModel.bulletAgrTMs.Count; i += 1) {
                    BulletTM bulletTM = cardModel.bulletAgrTMs[i];
                    appUI.Panel_CardSelection_AddCard(bulletTM.typeID, bulletTM.sprite);

                    BulletTM bulletTM0 = cardModel.bulletAgrTMs[0];
                    appUI.Panel_CardDetails_Update(bulletTM0.sprite, bulletTM0.content, bulletTM0.typeID);
                }
            } else if (cardType == CardType.Industry) {
                for (int i = 0; i < cardModel.bulletIndTMs.Count; i += 1) {
                    BulletTM bulletTM = cardModel.bulletIndTMs[i];
                    appUI.Panel_CardSelection_AddCard(bulletTM.typeID, bulletTM.sprite);

                    BulletTM bulletTM0 = cardModel.bulletIndTMs[0];
                    appUI.Panel_CardDetails_Update(bulletTM0.sprite, bulletTM0.content, bulletTM0.typeID);
                }
            } else if (cardType == CardType.Science) {

                for (int i = 0; i < cardModel.bulletSciTMs.Count; i += 1) {
                    BulletTM bulletTM = cardModel.bulletSciTMs[i];
                    appUI.Panel_CardSelection_AddCard(bulletTM.typeID, bulletTM.sprite);

                    BulletTM bulletTM0 = cardModel.bulletSciTMs[0];
                    appUI.Panel_CardDetails_Update(bulletTM0.sprite, bulletTM0.content, bulletTM0.typeID);
                }
            }

            // 3.看那些卡片被选择了

            if (selectedInFormation.roleSelectedTypeID != -1) {
                appUI.Panel_OneCardSelection_Lock(selectedInFormation.roleSelectedTypeID);
                // appUI.Panel_CardDetailsBtn_Lock();
            }

            if (selectedInFormation.scienceTypeID != -1) {
                appUI.Panel_OneCardSelection_Lock(selectedInFormation.scienceTypeID);
                // appUI.Panel_CardDetailsBtn_Lock();
            }

            appUI.Panel_CardSelected_TakeAll(out List<int> selectedList);
            for (int i = 0; i < selectedList.Count; i += 1) {
                int typeID = selectedList[i];
                appUI.Panel_OneCardSelection_Lock(typeID);
                // appUI.Panel_CardDetailsBtn_Lock();
            }



        }

        public static void Panel_SelectedCardAddToSlot(AppUI appUI, TemplateMoudle templateMoudle, int typeID) {

            var cardType = appUI.Panel_GetCardType();

            if (cardType == CardType.Hero) {
                appUI.Panel_OneCardSelection_UnLock(appUI.selectedInFormation.roleSelectedTypeID);
                appUI.selectedInFormation.roleSelectedTypeID = typeID;

                templateMoudle.Role_TryGet(typeID, out RoleTM roleTM);
                appUI.Panel_CardSelection_SetHero(roleTM.sprite);

            } else if (cardType == CardType.Agriculture) {

                templateMoudle.Bullet_TryGet(typeID, out BulletTM bulletTM);
                appUI.Panel_CardSelection_SetProp(typeID, bulletTM.sprite);

            } else if (cardType == CardType.Industry) {

                templateMoudle.Bullet_TryGet(typeID, out BulletTM bulletTM);
                appUI.Panel_CardSelection_SetProp(typeID, bulletTM.sprite);

            } else if (cardType == CardType.Science) {
                // 锁定按钮
                appUI.Panel_OneCardSelection_UnLock(appUI.selectedInFormation.scienceTypeID);

                // 记录刚刚选着的TypeID
                appUI.selectedInFormation.scienceTypeID = typeID;

                // 改变图片的样式
                templateMoudle.Bullet_TryGet(typeID, out BulletTM bulletTM);
                appUI.Panel_CardSelection_SetScience(bulletTM.sprite);

            }
        }

        public static void Panel_WatchTower_GetDetail(int typeID, TemplateMoudle templateMoudle, AppUI appUI) {
            templateMoudle.Role_TryGet(typeID, out RoleTM roleTM);

            string skill = "";
            for (int i = 0; i < roleTM.skillorSoArray.Length; i += 1) {
                skill += roleTM.skillorSoArray[i].tm.typeName + "\n";
            }
            appUI.Panel_WatchDetail_Open(typeID);
        }

        public static void Panel_StageSelectionEnter(AppUI appUI) {
            appUI.Panel_Login_Close();
            appUI.Panel_CardSelection_Close();
            appUI.Panel_WatchTower_Close();
            // 1.清空之前的数据
            appUI.Reset();

            int curChapterID = appUI.playerSaveData.GetChapterCurID();
            int unLockChapterID = appUI.playerSaveData.GetUnLockChapterID();//解锁关卡
            int unLockStageCount = appUI.playerSaveData.unLockStageCount;//解锁关卡数量
            int starAllCount = appUI.playerSaveData.GetstarAllCount();

            appUI.Panel_StageSelection_Open(curChapterID, unLockChapterID);
            appUI.Panel_StageSelection_Select(InputDeviceType.Keyboard, curChapterID, 2);
            appUI.Panel_StageSelection_SetStarNum(starAllCount);

            for (int i = 0; i < unLockStageCount; i += 1) {
                int stageID = i + 1;
                int multiple = curChapterID - 1;
                int stageid = stageID + multiple * 10;
                int starCount = appUI.playerSaveData.GetStagePassedStar(stageid * curChapterID);

                bool isPrassed = appUI.playerSaveData.IsStageReached(stageid * curChapterID);
                appUI.Panel_StageSelectionOneStage_Add(stageid, isPrassed, starCount);
            }

        }

        public static void Panel_StageSelection_ChangeChapter(AppUI appUI, int chapter, int maxChapter) {
            appUI.Panel_StageSelection_Select(InputDeviceType.Keyboard, chapter, maxChapter);

            int unLockChapterID = appUI.playerSaveData.GetUnLockChapterID();
            int unLockStageCount = appUI.playerSaveData.unLockStageCount;

            for (int i = 0; i < unLockStageCount; i += 1) {
                int stageID = i + 1;
                int multiple = chapter - 1;
                int stageid = stageID + multiple * 10;

                int starCount = appUI.playerSaveData.GetStagePassedStar(stageid * chapter);
                bool isPrassed = appUI.playerSaveData.IsStageReached(stageid * chapter);

                appUI.Panel_StageSelectionOneStage_Add(stageid, isPrassed, starCount);
            }
        }

        #region  Tick
        // 计算进入游戏Button是否可用
        public static void isEnterGameButton(AppUI appUI) {

            appUI.Panel_CardSelected_TakeAll(out List<int> typeIDs);

            int stageID = appUI.playerSaveData.GetStageCurID();
            int roleTypeID = appUI.selectedInFormation.roleSelectedTypeID;
            int sciType = appUI.selectedInFormation.scienceTypeID;
            int slotMax = appUI.playerSaveData.GetSlotMax();
            var card = appUI.Get_CardModel();
            if (card == null) {
                return;
            }

            int propCount = card.GetPropTMsCount();
            if (propCount == 0) {
                slotMax = -2;
                appUI.Panel_CardSelected_AgrButton(false);
                appUI.Panel_CardSelected_IndButton(false);
                appUI.Panel_CardSelected_PropTxtActive(true);
            }

            // 科技
            int sciCount = card.GetBulletSciTMsCount();
            if (sciCount == 0) {
                sciType = -2;
                appUI.Panel_CardSelected_SciButton(false);
                appUI.Panel_CardSelected_SciTxtActive(true);
            }

            if (typeIDs.Count < slotMax || stageID == -1 || roleTypeID == -1 || sciType == -1) {
                appUI.Panel_CardSelected_EnterGame(false);
            } else {
                appUI.Panel_CardSelected_EnterGame(true);
            }
        }
        // 计算单个card是否可选择
        public static void isOneCardSelectedButton(AppUI appUI) {
            CardType cardType = appUI.Panel_GetCardType();

            if (cardType == CardType.Hero) {
            }

            if (cardType == CardType.Agriculture) {
                appUI.Panel_CardSelected_TakeAll(out List<int> typeIDs);
                if (typeIDs.Count >= appUI.playerSaveData.GetSlotMax()) {
                    // appUI.Panel_CardDetailsBtn_Lock();
                    return;
                }
            }

            if (cardType == CardType.Industry) {
                appUI.Panel_CardSelected_TakeAll(out List<int> typeIDs);
                if (typeIDs.Count >= appUI.playerSaveData.GetSlotMax()) {
                    // appUI.Panel_CardDetailsBtn_Lock();
                    return;
                }
            }

            if (cardType == CardType.Science) {
            }
        }

        #endregion

        #region  Role
        public static void HUD_Progressbar_Update(AppUI ui, RoleAttributeComponent attrCom, float gameWaves) {
            int hp = attrCom.GetHp();
            int maxHp = attrCom.GetHpMax();
            ui.HUD_Progressbar_Update(gameWaves, hp, maxHp);//更新进度条
        }

        #endregion

        #region Train
        public static void Panel_Training_Select_Open(AppUI appUI, AssetsModule assetsModule, TemplateMoudle templateModule) {
            appUI.Panel_Login_Close();
            appUI.Panel_Training_Select_Open();

            CardModel model = GameFactory.Card_Create(assetsModule, templateModule, 500);
            var cards = model.roleTMs;

            for (int i = 0; i < cards.Count; i += 1) {
                var card = cards[i];
                appUI.Panel_Training_Select_Add(card.typeID, card.sprite, card.typeName);
            }

            appUI.Set_CardModel(model);
        }

        public static void Panel_Training_Select_AddPlayer(AppUI appUI, TemplateMoudle templateModule) {
            CardModel model = appUI.Get_CardModel();
            var cards = model.aiTms;

            for (int i = 0; i < cards.Count; i += 1) {
                var card = cards[i];
                appUI.Panel_Training_Select_Add(card.typeID, card.sprite, card.typeName);
            }
        }

        // public static void Panel_Training_SelectedCardAdd(AppUI appUI, TemplateMoudle templateModule, int typeID)
        // {
        //     var model = templateModule.TrainCard_TryGet(1, out TrainCardTM tm);
        // }
        #endregion

        #region 图鉴
        public static void Panel_Illustrated_Open(AppUI appUI, TemplateMoudle templateMoudle) {
            appUI.Panel_Illustrated_Open();

            var EnemyTypeIDs = appUI.playerSaveData.EnemyTypeIDs;
            int len = EnemyTypeIDs.Count;
            for (int i = 0; i < len; i += 1) {
                int typeID = EnemyTypeIDs[i];
                var role = templateMoudle.Role_TryGet(typeID, out var tm);

                appUI.Panel_Illustrated_Add(typeID, tm.sprite, true);
            }
        }

        public static void Panel_Eliminate_Illustrated_Open(AppUI appUI, TemplateMoudle templateMoudle) {
            appUI.Panel_Illustrated_Open();

            var stageID = appUI.playerSaveData.GetStageCurID();
            templateMoudle.Stage_TryGet(stageID, out var tm);

            var roles = tm.roleSpawns;
            var stuffs = tm.stuffSpawns;

            for (int i = 0; i < roles.Length; i += 1) {
                var role = roles[i];
                var roleTM = role.so.tm;

                if (appUI.Panel_Illustrated_TryGet(roleTM.typeID)) { continue; }

                appUI.Panel_Illustrated_Add(roleTM.typeID, roleTM.sprite, true);
            }

            for (int i = 0; i < stuffs.Length; i += 1) {
                var stuff = stuffs[i];
                var stuffTM = stuff.so.tm;

                if (appUI.Panel_Illustrated_TryGet(stuffTM.typeID)) { continue; }

                appUI.Panel_Illustrated_Add(stuffTM.typeID, stuffTM.sprite, false);
            }
        }

        public static void Panel_TrainIllustrated_Open(AppUI appUI, TemplateMoudle templateMoudle) {
            appUI.Panel_TrainIllustrated_Open();

            templateMoudle.Stage_TryGet(500, out StageTM tm);
            var props = tm.gameTM.sceneProps;
            int len = props.Length;

            for (int i = 0; i < len; i += 1) {
                var prop = props[i];
                appUI.Panel_TrainIllustrated_Add(prop.tm.typeID, prop.tm.sprite);
            }
        }
        #endregion

        #region 瞭望塔
        public static void Panel_WatchTower_View_Open(AppUI appUI, TemplateMoudle templateMoudle) {
            appUI.Panel_Illustrated_Open();

            CardModel card = appUI.Get_CardModel();
            for (int i = 0; i < card.roleEnemyTMs.Count; i += 1) {
                RoleTM tm = card.roleEnemyTMs[i];
                appUI.Panel_Illustrated_Add(tm.typeID, tm.sprite, true);
            }
        }
        #endregion

    }
}