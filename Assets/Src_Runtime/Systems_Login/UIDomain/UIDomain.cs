using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    public static class UIDomain {

        public static void Panel_CardSelection_Open(AppUI appUI, AssetsModule assetsModule, TemplateMoudle templateModule, int stageID, int chapterID) {

            appUI.Panel_StageSelection_Close();

            appUI.cardModel = GameFactory.Card_Create(assetsModule, templateModule, 1);
            CardModel card = appUI.Get_CardModel();

            appUI.Panel_CardSelection_Open(stageID, chapterID);// 打开卡牌选择界面
            appUI.Panel_CardSelection_AddCardSelected(4);// 放卡片的地方

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

        public static void Update_CardSelectionDetails(int typeID, TemplateMoudle templateMoudle, AppUI appUI) {

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
            //先清空卡片选择界面 
            appUI.Panel_OneCardSelection_Clear();

            CardType cardType = appUI.Panel_GetCardType();
            var cardModel = appUI.Get_CardModel();

            if (cardType == CardType.Hero) {
                for (int i = 0; i < cardModel.roleTMs.Count; i += 1) {
                    RoleTM roleTM = cardModel.roleTMs[i];
                    appUI.Panel_CardSelection_AddCard(roleTM.typeID, roleTM.sprite);
                }
            } else if (cardType == CardType.Agriculture) {
                for (int i = 0; i < cardModel.bulletAgrTMs.Count; i += 1) {
                    BulletTM bulletTM = cardModel.bulletAgrTMs[i];
                    appUI.Panel_CardSelection_AddCard(bulletTM.typeID, bulletTM.sprite);
                }
            } else if (cardType == CardType.Industry) {
                for (int i = 0; i < cardModel.bulletIndTMs.Count; i += 1) {
                    BulletTM bulletTM = cardModel.bulletIndTMs[i];
                    appUI.Panel_CardSelection_AddCard(bulletTM.typeID, bulletTM.sprite);
                }
            } else if (cardType == CardType.Science) {
                for (int i = 0; i < cardModel.bulletSciTMs.Count; i += 1) {
                    BulletTM bulletTM = cardModel.bulletSciTMs[i];
                    appUI.Panel_CardSelection_AddCard(bulletTM.typeID, bulletTM.sprite);
                }
            }

        }

        public static void Panel_SelectedCardAddToSlot(AppUI appUI, TemplateMoudle templateMoudle, int typeID) {

            var cardType = appUI.Panel_GetCardType();

            if (cardType == CardType.Hero) {
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

        public static void Panel_StageSelectionEnter(AppUI appUI, int stageID, int chapterID) {
            appUI.Panel_StageSelection_Select(InputDeviceType.Keyboard, 1, 5);

            appUI.Panel_StageSelection_Open(1, 1);

            appUI.Panel_StageSelectionOneStage_Add(1, false, 0);
            appUI.Panel_StageSelectionOneStage_Add(2, false, 0);
            appUI.Panel_StageSelectionOneStage_Add(3, false, 0);

        }

        public static void Panel_Training_Select_Open(AppUI appUI, AssetsModule assetsModule, TemplateMoudle templateModule)
        {
            appUI.Panel_Training_Select_Open();

            TrainCardModel model = GameFactory.TrainCard_Create(assetsModule, templateModule, 1);
            var cards = model.roleTMs;

            for (int i = 0; i < cards.Count; i += 1)
            {
                var card = cards[i];
                appUI.Panel_Training_Select_Add(card.typeID, card.sprite, card.typeName);
            }
        }
    }
}