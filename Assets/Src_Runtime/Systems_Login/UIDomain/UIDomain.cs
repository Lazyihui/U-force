using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    public static class UIDomain {

        public static void Panel_CardSelection_Open(AppUI appUI, AssetsModule assetsModule, TemplateMoudle templateModule) {
            int stageID = appUI.playerSaveData.GetStageCurID();
            int chapterID = appUI.playerSaveData.GetChapterCurID();
            int slotMax = appUI.playerSaveData.GetSlotMax();

            appUI.Panel_StageSelection_Close();

            CardModel spawnCard = GameFactory.Card_Create(assetsModule, templateModule, 1);
            appUI.Set_CardModel(spawnCard);
            CardModel card = appUI.Get_CardModel();

            appUI.Panel_CardSelection_Open(stageID, chapterID);// 打开卡牌选择界面

            // TODO:最大的卡槽数量
            appUI.Panel_CardSelection_AddCardSelected(1);// 放卡片的地方

            // 第一个
            RoleTM roleTM0 = card.roleTMs[0];
            appUI.Panel_CardDetails_Update(roleTM0.sprite, roleTM0.content, roleTM0.typeID);

            for (int i = 0; i < card.roleTMs.Count; i += 1) {
                RoleTM roleTM = card.roleTMs[i];
                appUI.Panel_CardSelection_AddCard(roleTM.typeID, roleTM.sprite);
            }
            appUI.Panel_SetCardType(CardType.Hero);


            appUI.Panel_WatchTower_Open();
            for (int i = 0; i < card.roleEnemyTMs.Count; i += 1) {
                RoleTM roleTM = card.roleEnemyTMs[i];
                appUI.Panel_WatchTower_Add(roleTM.typeID, roleTM.sprite);
            }
        }

        // 卡片更新
        public static void Update_CardSelectionDetails(int typeID, TemplateMoudle templateMoudle, AppUI appUI) {

            bool isSlotFull = appUI.playerSaveData.isSlotFull;
            Debug.Log("isSlotFull" + isSlotFull);

            // 如果卡槽满了
            if (isSlotFull) {
                appUI.Panel_CardDetailsBtn_Lock();
                appUI.Panel_Notice_Open("卡槽已满 请开始游戏");
            }

            CardType cardType = appUI.Panel_GetCardType();
            if (cardType == CardType.Hero) {
                templateMoudle.Role_TryGet(typeID, out RoleTM roleTM);
                appUI.Panel_CardDetails_Update(roleTM.sprite, roleTM.content, roleTM.typeID);

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

        public static void Panel_OneCardSelection_Update(AppUI appUI) {
            //1.先清空卡片选择界面 
            appUI.Panel_OneCardSelection_Clear();
            var selectedInFormation = appUI.selectedInFormation;

            // 3.展示卡片
            CardType cardType = appUI.Panel_GetCardType();
            var cardModel = appUI.Get_CardModel();

            if (cardType == CardType.Hero) {
                for (int i = 0; i < cardModel.roleTMs.Count; i += 1) {
                    RoleTM roleTM = cardModel.roleTMs[i];
                    appUI.Panel_CardSelection_AddCard(roleTM.typeID, roleTM.sprite);

                    // 更新右边详情页
                    RoleTM roleTM0 = cardModel.roleTMs[0];
                    appUI.Panel_CardDetails_Update(roleTM0.sprite, roleTM0.content, roleTM0.typeID);


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
                appUI.Panel_CardDetailsBtn_Lock();
            }

            if (selectedInFormation.scienceTypeID != -1) {
                appUI.Panel_OneCardSelection_Lock(selectedInFormation.scienceTypeID);
                appUI.Panel_CardDetailsBtn_Lock();
            }

            appUI.Panel_CardSelected_TakeAll(out List<int> selectedList);
            for (int i = 0; i < selectedList.Count; i += 1) {
                int typeID = selectedList[i];
                appUI.Panel_OneCardSelection_Lock(typeID);
                appUI.Panel_CardDetailsBtn_Lock();
            }



        }

        public static void Panel_SelectedCardAddToSlot(AppUI appUI, TemplateMoudle templateMoudle, int typeID) {

            var cardType = appUI.Panel_GetCardType();

            bool isSlotFull = appUI.playerSaveData.isSlotFull;
            Debug.Log("isSlotFull" + isSlotFull);

            // 如果卡槽满了
            if (isSlotFull) {
                appUI.Panel_CardDetailsBtn_Lock();
            }

            if (cardType == CardType.Hero) {
                appUI.Panel_OneCardSelection_UnLock(appUI.selectedInFormation.roleSelectedTypeID);
                appUI.selectedInFormation.roleSelectedTypeID = typeID;

                templateMoudle.Role_TryGet(typeID, out RoleTM roleTM);
                appUI.Panel_CardSelection_SetHero(roleTM.sprite);

            } else if (cardType == CardType.Agriculture) {

                templateMoudle.Bullet_TryGet(typeID, out BulletTM bulletTM);
                appUI.playerSaveData.isSlotFull = appUI.Panel_CardSelection_SetProp(typeID, bulletTM.sprite);

            } else if (cardType == CardType.Industry) {

                templateMoudle.Bullet_TryGet(typeID, out BulletTM bulletTM);
                appUI.playerSaveData.isSlotFull = appUI.Panel_CardSelection_SetProp(typeID, bulletTM.sprite);

            } else if (cardType == CardType.Science) {
                appUI.Panel_OneCardSelection_UnLock(appUI.selectedInFormation.scienceTypeID);
                appUI.selectedInFormation.scienceTypeID = typeID;

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
            appUI.Panel_WatchDetail_Open(roleTM.sprite, roleTM.typeName, roleTM.content, roleTM.hp, skill, roleTM.restraint);
        }

        public static void Panel_StageSelectionEnter(AppUI appUI) {

            int curChapterID = appUI.playerSaveData.GetChapterCurID();
            int unLockChapterID = appUI.playerSaveData.GetUnLockChapterID();
            int unLockStageCount = appUI.playerSaveData.GetUnLockStageCount();

            appUI.Panel_StageSelection_Open(curChapterID, unLockChapterID);
            appUI.Panel_StageSelection_Select(InputDeviceType.Keyboard, curChapterID, unLockChapterID);


            for (int i = 0; i < unLockStageCount; i += 1) {
                int stageID = i + 1;
                int multiple = curChapterID - 1;
                int stageid = stageID + multiple * 10;
                int starCount = appUI.playerSaveData.GetStagePassedStar(stageid * curChapterID);
                bool isPrassed = appUI.playerSaveData.IsStagePassed(stageid * curChapterID);

                appUI.Panel_StageSelectionOneStage_Add(stageid, isPrassed, starCount);
            }

        }

        public static void Panel_StageSelection_ChangeChapter(AppUI appUI, int chapter, int maxChapter) {
            appUI.Panel_StageSelection_Select(InputDeviceType.Keyboard, chapter, maxChapter);

            int unLockChapterID = appUI.playerSaveData.GetUnLockChapterID();
            int unLockStageCount = appUI.playerSaveData.GetUnLockStageCount();

            for (int i = 0; i < unLockStageCount; i += 1) {
                int stageID = i + 1;
                int multiple = chapter - 1;
                int stageid = stageID + multiple * 10;

                int starCount = appUI.playerSaveData.GetStagePassedStar(stageid * chapter);
                bool isPrassed = appUI.playerSaveData.IsStagePassed(stageid * chapter);

                appUI.Panel_StageSelectionOneStage_Add(stageid, isPrassed, starCount);
            }
        }


        #region  Role
        public static void HUD_Progressbar_Update(AppUI ui, RoleAttributeComponent attrCom, float gameWaves) {
            int hp = attrCom.GetHp();
            int maxHp = attrCom.GetHpMax();
            ui.HUD_Progressbar_Update(gameWaves, hp, maxHp);//更新进度条
        }

        #endregion

        #region Train
        public static void Panel_Training_Select_Open(AppUI appUI, AssetsModule assetsModule, TemplateMoudle templateModule) {
            appUI.Panel_Training_Select_Open();

            TrainCardModel model = GameFactory.TrainCard_Create(assetsModule, templateModule, 1);
            var cards = model.roleTMs;

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
    }
}