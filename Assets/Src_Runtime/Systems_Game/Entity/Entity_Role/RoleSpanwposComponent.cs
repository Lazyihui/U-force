using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    public class RoleSpawnerPosComponent {

        public Vector2 posPlayer;
        public Vector2 rotationPlayer;

        public List<Vector2> posListMinion;
        public List<Vector2> rotationListMinion;

        public RoleSpawnerPosComponent() {
            posPlayer = new Vector2(0, 0);
            rotationPlayer = new Vector2(0, 0);

            // TODO:后面从配置文件读取TM
            posListMinion = new List<Vector2>{
                new Vector2(16, 9),
                new Vector2(16, 6.5f),
                new Vector2(16, 4),
                new Vector2(16, 1.5f),
                new Vector2(16, -1),
                new Vector2(16, -3.5f),
            };

            rotationListMinion = new List<Vector2>();
        }

        public Vector2 GetRandomPosMinion() {
            int randomIndex = UnityEngine.Random.Range(0, posListMinion.Count);//[0, posListMinion.Count)
            return posListMinion[randomIndex];
        }

    }
}
