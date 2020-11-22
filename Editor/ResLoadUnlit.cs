using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ShaderReference.Editor
{
    public class ResLoadUnlit
    {
        public const string MarkDownViewerSkinPath =
            @"Assets\ShaderReference\EditorResources\MarkdownViewerSkin.guiskin";

        private const string pkgName = "com.actionf.shaderrefrence";
        public static string _relativePath { get; private set; }

        static ResLoadUnlit()
        {
            _relativePath = GetPackageRelativePath()+"/EditorResources";
        }

        public static Dictionary<string, string> LoadTextAsset(string floder, string suffix)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string[] filePaths = Directory.GetFiles($"{_relativePath}/{floder}");
            string tempPath;
            string extenTemp;
            string fileName;
            for (int i = 0; i < filePaths.Length; i++)
            {
                tempPath = filePaths[i];
                extenTemp = Path.GetExtension(tempPath);
                if (extenTemp == suffix)
                {
                    fileName = Path.GetFileNameWithoutExtension(tempPath);
                    TextAsset textAsset = GetAssetInPackageByFull<TextAsset>(tempPath);
                    dic.Add(fileName, textAsset.text);
                }
            }

            return dic;
        }

        private static string[] GetTabNames(string exten, string[] floder)
        {
            var texGuids =
                AssetDatabase.FindAssets("t:Object", floder);

            List<string> tempTab = new List<string>();
            for (int i = 0;
                i < texGuids.Length;
                i++)
            {
                string tempPath = AssetDatabase.GUIDToAssetPath(texGuids[i]);
                if (Path.GetExtension(tempPath) == exten)
                {
                    tempTab.Add(Path.GetFileNameWithoutExtension(path: tempPath));
                }
            }

            return tempTab.ToArray();
        }

        public static T GetAssetInPackageByRelative<T>(string pkgRelativePath) where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>($"{_relativePath}/{pkgRelativePath}");
        }

        public static T GetAssetInPackageByFull<T>(string fullPath) where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }

        private static string GetPackageRelativePath()
        {
            // 搜索潜在的软件包位置
            string packagePath = Path.GetFullPath($"Packages/{pkgName}");
            if (Directory.Exists(packagePath))
            {
                return $"Packages/{pkgName}";
            }

            packagePath = Path.GetFullPath("Assets/..");
            if (Directory.Exists(packagePath))
            {
                // 搜索本地开发默认位置
                if (Directory.Exists(packagePath + $"/Assets/Packages/{pkgName}/EditorResources"))
                {
                    return $"Assets/Packages/{pkgName}";
                }

                // 搜索本地常规软件包默认位置
                if (Directory.Exists(packagePath + "/Assets/ShaderReference/EditorResources"))
                {
                    return "Assets/ShaderReference";
                }

                // 搜索潜在位置
                string[] matchingPaths = Directory.GetDirectories(packagePath, "ShaderReference", SearchOption.AllDirectories);
                packagePath = ValidateLocation(matchingPaths, packagePath);
                if (packagePath != null) return packagePath;
            }

            return null;
        }

        /// <summary>
        /// Method to validate the location of the asset folder by making sure the GUISkins folder exists.
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        private static string ValidateLocation(string[] paths, string projectPath)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                // Check if any of the matching directories contain a GUISkins directory.
                if (Directory.Exists(paths[i] + "/EditorResources"))
                {
                    string folderPath = paths[i].Replace(projectPath, "");
                    folderPath = folderPath.TrimStart('\\', '/');
                    return folderPath;
                }
            }

            return null;
        }
    }
}
