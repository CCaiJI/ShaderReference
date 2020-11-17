/**
 * @file         ShaderReferenceProperties.cs
 * @author       Hongwei Li(taecg@qq.com)
 * @created      2018-11-17
 * @updated      2018-12-18
 *
 * @brief        绘制相关
 */


using System.Collections;
using System.IO;
using System.Threading.Tasks;
using Reference.Editor;
using UnityEngine.Networking;
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Reference.ShaderReference
{
    public class ReferenceUtil
    {
        private static GUIStyle _BH_Search;
        private static GUIStyle _H1;
        private static GUIStyle _LinkH1;
        private static GUIStyle _LinkH2;
        private static GUIStyle _Tx_Dec;
        private static GUIStyle _style01;
        private static GUIStyle _imgStyle;

        public static void Init()
        {
            //主按钮样式
            _style01 = new GUIStyle();
            _style01.alignment = TextAnchor.MiddleLeft;
            _style01.wordWrap = false;
            _style01.normal.textColor = Color.green;
            _style01.fontStyle = FontStyle.Bold;
            _style01.fontSize = 18;


            _H1 = EditorStyles.toolbar;
            _H1.alignment = TextAnchor.MiddleCenter;
            _H1.fixedHeight = 40;
            _H1.fontSize = 26;


            _LinkH1 = EditorStyles.linkLabel;
            _LinkH1.alignment = TextAnchor.MiddleCenter;
            _LinkH1.fixedHeight = 30;
            _LinkH1.fontSize = 20;

            _LinkH2 = new GUIStyle();
            _LinkH2.fontStyle = FontStyle.Bold;
            _LinkH2.normal.textColor = Color.cyan;
            _LinkH2.alignment = TextAnchor.MiddleLeft;
            _LinkH2.fixedHeight = 20;
            _LinkH2.fontSize = 17;

            _Tx_Dec = new GUIStyle("label");
            _Tx_Dec.wordWrap = true;
            _Tx_Dec.richText = true;
            _Tx_Dec.fontSize = 14;

            _imgStyle = new GUIStyle();
            _imgStyle.alignment = TextAnchor.MiddleCenter;
        }

        public static void DrawTopBar(ICWin window)
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Button("源码", EditorStyles.toolbarButton, GUILayout.Width(100)))
            {
                Application.OpenURL("https://github.com/CCaiJI/Reference.git");
            }

            if (GUILayout.Button("官方图形文档", EditorStyles.toolbarButton, GUILayout.Width(100)))
            {
                Application.OpenURL("file:///G:/UnityDocumentation/Manual/Graphics.html");
            }

            if (GUILayout.Button("重新打开", EditorStyles.toolbarButton, GUILayout.Width(100)))
            {
                window.ReOpen();
            }


            GUILayout.EndHorizontal();
        }


        /// <summary>
        /// 绘制一条内容
        /// </summary>
        /// <param name="str">大标题内容</param>
        /// <param name="message">小标题内容</param>
        public static void DrawOneContent(string str, string message = null)
        {
            //主按钮样式
            GUIStyle style01 = new GUIStyle("label");
            style01.alignment = TextAnchor.MiddleLeft;
            style01.wordWrap = false;
            style01.fontStyle = FontStyle.Bold;
            style01.fontSize = 18;

            //说明样式
            GUIStyle style02 = new GUIStyle("label");
            style02.wordWrap = true;
            style02.richText = true;

            EditorGUILayout.BeginVertical(new GUIStyle("Box"));
            EditorGUILayout.TextArea(str, style01);
            EditorGUILayout.TextArea(message, style02);
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 绘制一条内容
        /// </summary>
        /// <param name="str">大标题内容</param>
        /// <param name="message">小标题内容</param>
        public static void DrawOneContent(CSVItem info)
        {
            if (_H1 == null)
            {
                Init();
            }

            //说明样式

            if (!string.IsNullOrEmpty(info.H1))
            {
                GUILayout.BeginHorizontal();

                if (!string.IsNullOrEmpty(info.Url))
                {
                    if (GUILayout.Button(info.H1, _LinkH1, GUILayout.ExpandWidth(true)))
                    {
                        Application.OpenURL(info.Url);
                    }
                }
                else
                {
                    GUILayout.Label(info.H1, _H1, GUILayout.ExpandWidth(true));
                }

                GUILayout.EndHorizontal();
            }
            else
            {
                //绘制正常的
                EditorGUILayout.BeginVertical(new GUIStyle("Box"));
                EditorGUILayout.TextArea(info.H2, _style01);
                EditorGUILayout.TextArea(info.Description, _Tx_Dec);
                EditorGUILayout.EndVertical();
            }
        }

        /// <summary>
        /// 绘制一条内容
        /// </summary>
        /// <param name="str">大标题内容</param>
        /// <param name="message">小标题内容</param>
        public static void DrawOneContent(FMDItem info)
        {
            if (_H1 == null)
            {
                Init();
            }

            //绘制标题
            if (info.IsImage)
            {
                DrawTex(info);

                return;
            }

            //绘制标题
            if (!string.IsNullOrEmpty(info.H1))
            {
                GUILayout.BeginHorizontal();

                if (!string.IsNullOrEmpty(info.Url))
                {
                    if (GUILayout.Button(info.H1, _LinkH1, GUILayout.ExpandWidth(true)))
                    {
                        Application.OpenURL(info.Url);
                    }
                }
                else
                {
                    GUILayout.Label(info.H1, _H1, GUILayout.ExpandWidth(true));
                }

                GUILayout.EndHorizontal();
            }
            else if (!string.IsNullOrEmpty(info.Url))
            {
                EditorGUILayout.BeginVertical(new GUIStyle("Box"));
                if (GUILayout.Button(info.H2, _LinkH2))
                {
                    Application.OpenURL(info.Url);
                }

                EditorGUILayout.TextArea(info.Description, _Tx_Dec);
                EditorGUILayout.EndVertical();
            }
            else
            {
                //绘制正常的
                EditorGUILayout.BeginVertical(new GUIStyle("Box"));
                EditorGUILayout.TextArea(info.H2, _style01);
                EditorGUILayout.TextArea(info.Description, _Tx_Dec);
                EditorGUILayout.EndVertical();
            }
        }

        private static async void DrawTex(FMDItem info)
        {
            if (info.IsNotLoadUrl)
            {
                info.IsNotLoadUrl = false;
                UnityWebRequest req = UnityWebRequestTexture.GetTexture(info.Url);
                req.SendWebRequest();
                while (!req.isDone)
                {
                    await Task.Delay(1000);
                }
                //  await Task.Delay(2000);

                var tex = req.downloadHandler as DownloadHandlerTexture;
                if (tex != null)
                {
                    info.Texture = tex.texture;
                }

                req.Dispose();
            }
            else if (info.Texture != null)
            {
                var width = 300 * (float) info.Texture.width / info.Texture.height;

                if (GUILayout.Button(info.Texture, _imgStyle, GUILayout.Width(width),
                    GUILayout.Height(300)))
                {
                    EditorTipWindow.Push(info.Texture);
                }
            }
        }

        public static void DrawTitle(string str)
        {
            EditorGUILayout.LabelField(str, EditorStyles.toolbarButton);
        }


        private static string textField = "";

        public static void DrawSearchGlobal(UnityAction<string> unityAction)
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            textField = EditorGUILayout.TextField(textField);
            if (GUILayout.Button("查找", GUILayout.MaxWidth(200)))
            {
                if (!string.IsNullOrEmpty(textField))
                    unityAction.Invoke(textField);
                else
                    Debug.LogError("查找不能为空");
            }

            GUILayout.EndHorizontal();
        }


        public static string[] GetTabNames(string exten, string[] floder)
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

        public static string ParseCustomFuhao(string dec)
        {
            if (string.IsNullOrEmpty(dec))
            {
                return dec;
            }

            dec = dec.Replace(@"\n", "\r\n");
            if (dec.Contains("G{"))
            {
                dec = dec.Replace("G{", "<size=20><i><b><color=#00FF00>");
                dec = dec.Replace("}G", "</color></b></i></size>");
            }

            if (dec.Contains("R{"))
            {
                dec = dec.Replace("R{", "<size=20><i><b><color=#FF0000>");
                dec = dec.Replace("}R", "</color></b></i></size>");
            }

            return dec;
        }
    }
}
#endif