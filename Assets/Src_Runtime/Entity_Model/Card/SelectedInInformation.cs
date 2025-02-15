using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    public class SelectedInFormation {

        public int stageID;
        public int chapterID;
        public int roleSelectedTypeID;
        public int scienceTypeID;
        public int[] propTypeID;

        public SelectedInFormation() {
            stageID = 0;
            chapterID = 0;
            roleSelectedTypeID = 0;
            scienceTypeID = 0;
        }
    }
}