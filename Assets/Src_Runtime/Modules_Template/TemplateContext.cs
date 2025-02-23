using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UF.TemplateInternal {

    public class TemplateContext {

        public Dictionary<int, RoleTM> roles;
        public AsyncOperationHandle rolePtr;

        public Dictionary<int, BulletTM> bullets;
        public AsyncOperationHandle bulletPtr;

        public Dictionary<int, SkillorTM> skills;
        public AsyncOperationHandle skillPtr;

        public Dictionary<int, CaveTM> towers;
        public AsyncOperationHandle towerPtr;

        public Dictionary<int, StageTM> stages;
        public AsyncOperationHandle stagePtr;

        public Dictionary<int, StuffTM> stuffs;
        public AsyncOperationHandle stuffPtr;

        public Dictionary<int, CardTM> cards;
        public AsyncOperationHandle cardPtr;

        public Dictionary<int, TrainCardTM> trainCards;
        public AsyncOperationHandle trainCardPtr;


        public TemplateContext() {
            roles = new Dictionary<int, RoleTM>();
            bullets = new Dictionary<int, BulletTM>();
            skills = new Dictionary<int, SkillorTM>();
            towers = new Dictionary<int, CaveTM>();
            stages = new Dictionary<int, StageTM>();
            stuffs = new Dictionary<int, StuffTM>();
            cards = new Dictionary<int, CardTM>();
            trainCards = new Dictionary<int, TrainCardTM>();
        }


        public void Role_Add(RoleTM role) {
            roles.Add(role.typeID, role);
        }

        public void Bullet_Add(BulletTM bullet) {
            bullets.Add(bullet.typeID, bullet);
        }

        public void Skill_Add(SkillorTM skill) {
            skills.Add(skill.typeID, skill);
        }

        public void Tower_Add(CaveTM tower) {
            towers.Add(tower.typeID, tower);
        }

        public void Stage_Add(StageTM stage) {
            stages.Add(stage.stageID, stage);
        }

        public void Stuff_Add(StuffTM stuff) {
            stuffs.Add(stuff.typeID, stuff);
        }

        public void Card_Add(CardTM card) {
            cards.Add(card.typeID, card);
            
        }

        public void TrainCard_Add(TrainCardTM trainCard) {
            trainCards.Add(trainCard.typeID, trainCard);
        }

    }
}