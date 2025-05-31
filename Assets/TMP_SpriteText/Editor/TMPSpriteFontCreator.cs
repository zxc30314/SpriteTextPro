using System.IO;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace pruss.Tool.TextMeshPro
{
    public class TMPSpriteFontCreator : EditorWindow
    {
        [MenuItem("Assets/Create/TextMeshPro/Sprite Asset Custom #%F11", false, 100)]
        public static void CustomFontAsset()
        {
            TMP_SpriteAssetMenu.CreateSpriteAsset();
            var targets = Selection.objects;

            if (targets == null)
            {
                Debug.LogWarning("A Font file must first be selected in order to create a Font Asset.");
                return;
            }

            foreach (var target in targets)
            {
                // Make sure the selection is a font file
                if (!target || target.GetType() != typeof(Texture2D))
                {
                    Debug.LogWarning("Selected Object [" + target.name + "] is not a Font file. A Font file must be selected in order to create a Font Asset.", target);
                    continue;
                }

                var filePathWithName = AssetDatabase.GetAssetPath(target);
                var fileNameWithExtension = Path.GetFileName(filePathWithName);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePathWithName);
                var filePath = filePathWithName.Replace(fileNameWithExtension, "");

                var spriteAsset = AssetDatabase.LoadAssetAtPath<TMP_SpriteAsset>(filePath + fileNameWithoutExtension + ".asset");
                CustomFontSprite(spriteAsset);
            }
        }

        private static void CustomFontSprite(TMP_SpriteAsset target)
        {
            foreach (var tmpSpriteGlyph in target.spriteGlyphTable)
            {
                var glyphMetrics = tmpSpriteGlyph.metrics;
                glyphMetrics.horizontalBearingX = 0;
                glyphMetrics.horizontalBearingY = glyphMetrics.height - 1;
                glyphMetrics.horizontalAdvance = glyphMetrics.width + 1;
                tmpSpriteGlyph.metrics = glyphMetrics;
            }

            target.UpdateLookupTables();
        }
    }
}