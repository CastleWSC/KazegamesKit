using UnityEngine;
using UnityEditor;
using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace KazegamesKit.Editor
{
    public class SpritesheetSlicerXML 
    {
        [MenuItem("CONTEXT/TextureImporter/Spritesheet Slicer using XML")]
        static void SliceCmd(MenuCommand cmd)
        {
            TextureImporter importer = cmd.context as TextureImporter;

            Slice(importer, 
                AssetDatabase.LoadMainAssetAtPath(Path.ChangeExtension(importer.assetPath, ".xml")) as TextAsset, 
                new Vector2(0.5f, 0f));
        }

        [MenuItem("CONTEXT/TextureImporter/Spritesheet Slicer using XML", true)]
        static bool ValidateSlicedCmd(MenuCommand cmd)
        {
            bool isValidate = true;

            // texture
            TextureImporter importer = cmd.context as TextureImporter;
            isValidate &= importer != null;

            // xml
            string xmlPath = Path.ChangeExtension(importer.assetPath, ".xml");
            isValidate &= AssetDatabase.GetMainAssetTypeAtPath(xmlPath) != null;
            
            return isValidate;
        }

        private static void Slice(TextureImporter importer, TextAsset xml, Vector2 pivot)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml.text);

            XmlElement root = xmlDoc.DocumentElement;
            if(root.Name == "TextureAtlas")
            {
                Texture2D tex = AssetDatabase.LoadMainAssetAtPath(importer.assetPath) as Texture2D;
                int texHeight = tex.height;

                List<SpriteMetaData> metaDatas = new List<SpriteMetaData>();

                foreach(XmlNode node in root.ChildNodes)
                {
                    if(node.Name == "SubTexture")
                    {
                        try
                        {
                            int w = Convert.ToInt32(node.Attributes["width"].Value);
                            int h = Convert.ToInt32(node.Attributes["height"].Value);
                            int x = Convert.ToInt32(node.Attributes["x"].Value);
                            int y = texHeight - (h + Convert.ToInt32(node.Attributes["y"].Value));

                            SpriteMetaData smd = new SpriteMetaData()
                            {
                                alignment = (int)KazegamesEditor.spriteAlignement,
                                border = new Vector4(),
                                name = node.Attributes["name"].Value,
                                pivot = GetSpritePivot(KazegamesEditor.spriteAlignement),
                                rect = new Rect(x, y, w, h)
                            };

                            metaDatas.Add(smd);
                        }
                        catch(Exception e)
                        {
                            Debug.LogException(e);
                            return;
                        }
                    }
                }

                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Multiple;
                importer.spritesheet = metaDatas.ToArray();

                EditorUtility.SetDirty(importer);

                try
                {
                    AssetDatabase.StartAssetEditing();
                    AssetDatabase.ImportAsset(importer.assetPath);
                }
                finally
                {
                    AssetDatabase.StopAssetEditing();
                }
            }
            else
            {
                Debug.LogError($"[SpritesheetSlicerXML] XML format is invalide, root= {root.Name}.");
                return;
            }
        }

        private static Vector2 GetSpritePivot(SpriteAlignment alignment)
        {
            switch (alignment)
            {
                case SpriteAlignment.Center:
                    return new Vector2(.5f, .5f);

                case SpriteAlignment.TopLeft:
                    return new Vector2(0f, 1f);
                case SpriteAlignment.TopCenter:
                    return new Vector2(.5f, 1f);
                case SpriteAlignment.TopRight:
                    return new Vector2(1f, 1f);

                case SpriteAlignment.LeftCenter:
                    return new Vector2(0f, .5f);
                case SpriteAlignment.RightCenter:
                    return new Vector2(1f, .5f);

                case SpriteAlignment.BottomLeft:
                    return new Vector2(0f, 0f);
                case SpriteAlignment.BottomCenter:
                    return new Vector2(.5f, 0f);
                case SpriteAlignment.BottomRight:
                    return new Vector2(1f, 0f);

            }

            return KazegamesEditor.spritePivot;
        }
    }
}
