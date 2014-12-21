using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace Dithering
{
    public class DitheringEditor : EditorWindow
    {
        [MenuItem("Assets/Dithering/Gamma RGBA")]
        static void DitherOnGammaRGBA()
        {
            DuplicateAndEditSelectedTexture2D(x =>
                {
                    var color32s = x.GetPixels32();
                    ErrorDiffusionDithering.Dither<GammaRgba, Color32, Color16>(
                        color32s, x.width, ErrorDiffusion.FloydSteinberg);
                    x.SetPixels32(color32s);
                }, TextureImporterFormat.Automatic16bit);
        }

        [MenuItem("Assets/Dithering/Linear sRGB-A")]
        static void DitherOnLinearSRGBA()
        {
            DuplicateAndEditSelectedTexture2D(x =>
                {
                    var color32s = x.GetPixels32();
                    ErrorDiffusionDithering.Dither<LinearSrgba, Color32, Color16>(
                        color32s, x.width, ErrorDiffusion.FloydSteinberg);
                    x.SetPixels32(color32s);
                }, TextureImporterFormat.Automatic16bit);
        }


        static void DuplicateAndEditSelectedTexture2D(System.Action<Texture2D> edit, TextureImporterFormat format)
        {
            var texture = Selection.activeObject as Texture2D;
            var path = AssetDatabase.GetAssetPath(texture);
            var result = new Texture2D(0, 0, TextureFormat.ARGB32, false);
            result.LoadImage(File.ReadAllBytes(path));
            edit(result);
            var newPath = AssetDatabase.GenerateUniqueAssetPath(path);
            File.WriteAllBytes(newPath, result.EncodeToPNG());
            Object.DestroyImmediate(result);
            AssetDatabase.ImportAsset(newPath, ImportAssetOptions.ForceSynchronousImport);
            TextureImporter srcIn = AssetImporter.GetAtPath(path) as TextureImporter;
            TextureImporter dstIn = AssetImporter.GetAtPath(newPath) as TextureImporter;
            var settings = new TextureImporterSettings();
            srcIn.ReadTextureSettings(settings);
            dstIn.SetTextureSettings(settings);
            dstIn.textureType = srcIn.textureType;
            dstIn.textureFormat = format;
            AssetDatabase.WriteImportSettingsIfDirty(newPath);
            AssetDatabase.ImportAsset(newPath, ImportAssetOptions.ForceSynchronousImport);
        }
    }
}