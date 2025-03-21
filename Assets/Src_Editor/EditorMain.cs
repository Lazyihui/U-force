using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using GameFunctions;
using GameFunctions.Editors;
using TriInspector;

namespace UF.EditorTool {

    public class EditorMain : MonoBehaviour {

        [Button]
        void GenerateSave() {
            string dir = Path.Combine(Application.dataPath, "Src_Runtime", "Modules_Save", "DTO");
            GFBufferEncoderGenerator.GenModel(dir);
        }
    }
}