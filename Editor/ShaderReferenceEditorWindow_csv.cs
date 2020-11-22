/**
 * @file         ShaderReferenceEditorWindow.cs
 * @author       Hongwei Li(taecg@qq.com)
 * @created      2018-11-10
 * @updated      2018-12-18
 *
 * @brief        着色器语法参考工具
 */


using ShaderReference.Editor;
using File = System.IO.File;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ShaderReference.Editor;
using UnityEditor;
using UnityEngine;

namespace Reference.ShaderReference
{
    public class ShaderReferenceEditorWindow_csv : ICWin
    {
        private string[] tabNames;

        private Dictionary<string, List<CSVItem>> _dicInfos = new Dictionary<string, List<CSVItem>>();
        private List<CSVItem> _curShow;
        private int selectedTabID;
        private Dictionary<string, string> dicTexts;

        private string CsvFloder = "csv";

        #region  周期或者初始

       // [MenuItem("Window/Reference/ShaderReferenceWindow_csv")]
        public static void Open()
        {
            ShaderReferenceEditorWindow_csv windowCsv = EditorWindow.GetWindow<ShaderReferenceEditorWindow_csv>();
            windowCsv.titleContent = new GUIContent("Shader");
            windowCsv.Show();
        }

        void OnEnable()
        {
            ReferenceUtil.Init();
            dicTexts = new Dictionary<string, string>();

            dicTexts= ResLoadUnlit.LoadTextAsset(CsvFloder, ".csv");
            tabNames=new string[dicTexts.Count];
            dicTexts.Keys.CopyTo(tabNames, 0);


            selectedTabID = EditorPrefs.GetInt("ShaderRef_SeletedIndex_csv", 0);
            FillCSVList(selectedTabID);
        }

        private void OnDisable()
        {
            EditorPrefs.SetInt("ShaderRef_SeletedIndex_csv", selectedTabID);
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
                FillCSVList(selectedTabID);
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


        private Vector2 scrollPos;

        private void DrawReference(List<CSVItem> infos)
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

        private void FillCSVList(int selectedIndex)
        {
            var key = tabNames[selectedIndex];
            if (!_dicInfos.ContainsKey(key))
            {
                _dicInfos[key] = MyCSVRead.ReadCsv(dicTexts[tabNames[selectedIndex]]);
            }

            
            _curShow = _dicInfos[key];
        }

        private void SearchHandler(string search)
        {
            StringBuilder result = new StringBuilder();
            foreach (var value in dicTexts)
            {
                if (value.Value.Contains(search))
                {
                    selectedTabID = ArrayUtility.IndexOf(tabNames, value.Key);
                    
                    FillCSVList(selectedTabID);
                    
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