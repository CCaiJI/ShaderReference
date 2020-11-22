using System.IO;
using UnityEditor;
using UnityEngine;

namespace ShaderReference.Editor
{
    public class MenuItem
    {
        [UnityEditor.MenuItem("Window/Reference/打开内置cginc")]
        public static void OpenCginc()
        {
            Debug.Log(EditorApplication.applicationContentsPath);
            OpenDirectory(EditorApplication.applicationContentsPath+"/CGIncludes");
        }


        public static void OpenDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            path = path.Replace("/", "\\");
            if (!Directory.Exists(path))
            {
                Debug.LogError("No Directory: " + path);
                return;
            }

            System.Diagnostics.Process.Start("explorer.exe", path);
        }
    }
}