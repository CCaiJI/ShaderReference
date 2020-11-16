/**
 * @file         ShaderReferenceEditorWindow.cs
 * @author       Hongwei Li(taecg@qq.com)
 * @created      2018-11-10
 * @updated      2018-12-18
 *
 * @brief        着色器语法参考工具
 */


#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
using Reference.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Reference.ShaderReference
{
    public class ShaderReferenceEditorWindow_fmd : ICWin
    {
        private string[] tabNames;

        private Dictionary<string, List<FMDItem>> _dicInfos = new Dictionary<string, List<FMDItem>>();
        private List<FMDItem> _curShow;
        private int selectedTabID;
        private Dictionary<string, string> dicTexts;

        private string FmdFloder = "Assets/ShaderReference/EditorResources/fmd";

        #region  周期或者初始

        [UnityEditor.MenuItem("Window/Reference/ShaderReferenceWindow_fmd")]
        public static void Open()
        {
            ShaderReferenceEditorWindow_fmd windowCsv = EditorWindow.GetWindow<ShaderReferenceEditorWindow_fmd>();
            windowCsv.titleContent = new GUIContent("Shader");
            windowCsv.Show();
        }

        void OnEnable()
        {
            ReferenceUtil.Init();
            dicTexts = new Dictionary<string, string>();

            tabNames = ReferenceUtil.GetTabNames(".md", new string[] {FmdFloder});

            for (int i = 0; i < tabNames.Length; i++)
            {
                byte[] bytes = UnityEngine.Windows.File.ReadAllBytes($"{FmdFloder}/{tabNames[i]}.md");
                string text = UTF8Encoding.UTF8.GetString(bytes);
                /*var textAsset =
                    AssetDatabase.LoadAssetAtPath<TextAsset>();
                if (textAsset == null)
                {
                    dicTexts[tabNames[i]] = "";
                }
                else
                {
                    dicTexts[tabNames[i]] = textAsset.text;
                }*/
                dicTexts[tabNames[i]] = text;
            }
            
            selectedTabID = EditorPrefs.GetInt("ShaderRef_SeletedIndex_csv", 0);
            FillMDList(selectedTabID);
        }

        private void OnDisable()
        {
            EditorPrefs.SetInt("ShaderRef_SeletedIndex_csv", selectedTabID);
        }

        #endregion

        #region  绘制

        private int tempLast;

        void OnGUI()
        {
            ReferenceUtil.DrawTopBar(this);
            ReferenceUtil.DrawSearchGlobal(SearchHandler);
            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();

            //绘制左侧标签栏
            float _width = 150;
            float _heigth = position.height - 10;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.MaxWidth(_width),
                GUILayout.MinHeight(_heigth));

            tempLast = selectedTabID;
            selectedTabID = GUILayout.SelectionGrid(selectedTabID, tabNames, 1);

            if (tempLast != selectedTabID)
            {
                FillMDList(selectedTabID);
            }

            EditorGUILayout.EndVertical();

            //绘制右侧内容区
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.MinWidth(position.width - _width),
                GUILayout.MinHeight(_heigth));

            DrawReference(_curShow);

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
            Repaint();
        }

        public override void ReOpen()
        {
            this.Close();
            Open();
        }

        private Vector2 scrollPos;

        private void DrawReference(List<FMDItem> infos)
        {
            if (infos != null)
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                foreach (var item in infos)
                {
                    ReferenceUtil.DrawOneContent(item);
                }

                GUILayout.Space(100);

                GUILayout.EndScrollView();
            }
        }

        #endregion

        private void FillMDList(int selectedIndex)
        {
            var key = tabNames[selectedIndex];
            if (!_dicInfos.ContainsKey(key))
            {
                _dicInfos[key] = FMdRead.ReadMd(dicTexts[tabNames[selectedIndex]],FmdFloder);
            }

            _curShow = _dicInfos[key];
        }

        private void SearchHandler(string search)
        {
            StringBuilder result = new StringBuilder();
            foreach (var value in dicTexts)
            {
                if (value.Value.ToLower().Contains(search.ToLower()))
                {
                    selectedTabID = ArrayUtility.IndexOf(tabNames, value.Key);
                    FillMDList(selectedTabID);
                    result.AppendLine("===========");
                    foreach (var item in _dicInfos[value.Key])
                    {
                        if (item.Contains(search))
                        {
                            result.AppendLine(item.ToString());
                        }
                    }
                }
            }

            string resultStr = result.ToString();
            EditorTipWindow.Push("搜索结果：", resultStr);
            Debug.Log(resultStr);
        }

      
    }
}
#endif