using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace KazegamesKit.Editor
{
    public class KazegamesEditor : EditorWindow
    {
        [MenuItem("Tools/Kazegames/New Prototype Project", false, 1)]
        static void CreatePrototypeFolders()
        {
            if(!AssetDatabase.IsValidFolder("Assets/_Prototype"))
            {
                string[] subFolders = new string[]
                {
                    "Animations",
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


        [MenuItem("Tools/Kazegames/Kazegames Kit", false, 0)]
        static void CreateWindow()
        {
            GetWindow<KazegamesEditor>("Kazegames Editor");
        }


        public static SpriteAlignment spriteAlignement = SpriteAlignment.Center;
        public static Vector2 spritePivot = new Vector2(0.5f, 0.5f);


        private void OnGUI()
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField("Spritesheet Slicer");
            spriteAlignement = (SpriteAlignment)EditorGUILayout.EnumPopup("Sprite Alignement", spriteAlignement);
            spritePivot = EditorGUILayout.Vector2Field("Sprite Pivot", spritePivot);

            EditorGUILayout.EndVertical();
        }
    }
}