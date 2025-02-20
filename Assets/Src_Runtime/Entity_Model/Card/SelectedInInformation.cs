using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    public class SelectedInFormation {

        public int stageID;
        public int chapterID;
        public int roleSelectedTypeID;
        public int scienceTypeID;

        public SelectedInFormation() {
            stageID = -1;
            chapterID = -1;
            roleSelectedTypeID = -1;
            scienceTypeID = -1;
        }
    }
}