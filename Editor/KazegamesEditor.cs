using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace KazegamesKit.Editor
{
    public class KazegamesEditor 
    {
        [MenuItem("Tools/Kazegames/Create Prototype Folders")]
        static void CreatePrototypeFolders()
        {
            if(!AssetDatabase.IsValidFolder("Assets/_Prototype"))
            {
                string[] subFolders = new string[]
                {
                    "Scripts",
                    "Scenes",
                    "Textures",
                    "Materials",
                    "Fonts",
                    "Prefabs"
                };


                string root = AssetDatabase.CreateFolder("Assets", "_Prototype");
                
                for(int i=0; i<subFolders.Length; i++)
                {
                    AssetDatabase.CreateFolder("Assets/_Prototype", subFolders[i]);
                }

                AssetDatabase.Refresh();
            }
        }
    }
}