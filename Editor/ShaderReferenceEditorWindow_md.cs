/**
 * @file         ShaderReferenceEditorWindow.cs
 * @author       Hongwei Li(taecg@qq.com)
 * @created      2018-11-10
 * @updated      2018-12-18
 *
 * @brief        着色器语法参考工具
 */


using System.IO;
using MG.MDV;
#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
using Reference.Editor;
using UnityEditor;
using UnityEngine;

namespace Reference.ShaderReference
{
    public class ShaderReferenceEditorWindow_md : ICWin
    {
        private string[] tabNames;

        private int selectedTabID;
        private Dictionary<string, string> dicTexts;

        public GUISkin Skin;
        MarkdownViewer mViewer;

        private string MdFullPath(string tabName) => $"Assets/Reference/EditorResources/md/{tabName}.md";
        private string MdFloder = "Assets/Reference/EditorResources/md";
        private string SkinPath = $"Assets/Reference/EditorResources/md/MarkdownViewerSkin.guiskin";


        #region  周期或者初始

        [UnityEditor.MenuItem("Window/Reference/ShaderReferenceWindow_md")]
        public static void Open()
        {
            ShaderReferenceEditorWindow_md window = EditorWindow.GetWindow<ShaderReferenceEditorWindow_md>();
            window.titleContent = new GUIContent("Shader");
            window.minSize = new Vector2(500, 500);
            window.Show();
        }

        void OnEnable()
        {
            ReferenceUtil.Init();
            dicTexts = new Dictionary<string, string>();

            Skin =
                AssetDatabase.LoadAssetAtPath<GUISkin>(SkinPath);

            tabNames = ReferenceUtil.GetTabNames(".md", new string[] {MdFloder});

            for (int i = 0; i < tabNames.Length; i++)
            {
                var textAsset =
                    AssetDatabase.LoadAssetAtPath<TextAsset>(MdFullPath(tabNames[i]));

                if (textAsset == null)
                {
                    dicTexts[tabNames[i]] = "";
                }
                else
                {
                    dicTexts[tabNames[i]] = textAsset.text;
                }
            }


            selectedTabID = EditorPrefs.GetInt("ShaderRef_SeletedIndex_md", 0);
            FillMd(selectedTabID);
        }

        private void OnDisable()
        {
            EditorPrefs.SetInt("ShaderRef_SeletedIndex_md", selectedTabID);
        }
        
        public override void ReOpen()
        {
            this.Close();
            Open();
        }

        #endregion

        #region  绘制

        private int tempLast;

        void OnGUI()
        {
            ReferenceUtil.DrawTopBar(this);
            ReferenceUtil.DrawSearchGlobal(SearchHandler);
            GUILayout.Space(10);


            //绘制左侧标签栏
            float _width = 150;
            float _heigth = position.height - 10;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.MaxWidth(_width),
                GUILayout.MinHeight(_heigth));

            tempLast = selectedTabID;
            selectedTabID = GUILayout.SelectionGrid(selectedTabID, tabNames, 1);

            if (tempLast != selectedTabID)
            {
                FillMd(selectedTabID);
            }

            EditorGUILayout.EndVertical();


            DrawMd();
            //  Repaint();
        }


        private void DrawMd()
        {
            mViewer.Draw(new Rect(160, 30, Screen.width - 150, Screen.height),
                new Rect(110, 30, Screen.width - 100, Screen.height));
        }

        #endregion

        private void FillMd(int selectedIndex)
        {
            string name = tabNames[selectedIndex];
            var content = dicTexts[name];

            var path = MdFullPath(name);

            mViewer = new MarkdownViewer(Skin, path, content);
            EditorApplication.update += delegate
            {
                if (mViewer != null && mViewer.Update())
                {
                    Repaint();
                }
            };
        }


        private void SearchHandler(string search)
        {
            StringBuilder result = new StringBuilder();
            foreach (var value in dicTexts)
            {
                if (value.Value.Contains(search))
                {
                    result.AppendLine($"【{value.Key}】包含查找");
                }
            }

            Debug.Log(result.ToString());
        }
    }
}
#endif