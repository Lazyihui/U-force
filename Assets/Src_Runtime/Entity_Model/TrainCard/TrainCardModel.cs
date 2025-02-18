using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF
{

    public class TrainCardModel
    {
        public int typeID;
        public List<RoleTM> roleTMs;

        public TrainCardModel()
        {
            roleTMs = new List<RoleTM>();
        }

        public void AddRoleTM(RoleTM roleTM)
        {
            roleTMs.Add(roleTM);
        }
    }
}