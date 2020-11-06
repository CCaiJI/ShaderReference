using System;
using UnityEditor;
using UnityEngine;

namespace Reference.Editor
{
    public class EditorTipWindow : EditorWindow
    {
        private static string _message;
        private static GUIStyle textArea;

        public static void Push(string title, string message)
        {
            textArea = EditorStyles.textArea;
            textArea.richText = true;
            textArea.fontSize = 14;


            _message = message;
            EditorTipWindow editorTipWindow = GetWindow<EditorTipWindow>();
            editorTipWindow.minSize = new Vector2(600, 0);

            editorTipWindow.titleContent.text = title;
            editorTipWindow.Show();
        }

        private Vector2 _scrollPos;

        private void OnGUI()
        {
            _scrollPos = GUILayout.BeginScrollView(_scrollPos,GUILayout.Height(500));
          
            GUILayout.TextArea(_message, textArea);
            GUILayout.EndScrollView();
            if (GUILayout.Button("OK"))
            {
                Close();
            }
        }
    }
}