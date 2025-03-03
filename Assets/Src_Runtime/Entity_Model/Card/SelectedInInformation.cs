using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    public class SelectedInFormation {
        public int roleSelectedTypeID;
        public int scienceTypeID;
        public List<int> typeIDs;

        public SelectedInFormation() {
            roleSelectedTypeID = -1;
            scienceTypeID = -1;
            typeIDs = new List<int>();
        }

        public void Reset() {
            roleSelectedTypeID = -1;
            scienceTypeID = -1;
            typeIDs.Clear();
        }

    }
}