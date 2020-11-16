using System;
using UnityEditor;
using UnityEngine;

namespace Reference.Editor
{
    public class EditorTipWindow : EditorWindow
    {
        private static string _message;
        private static GUIStyle textArea;

        private static Texture _tex;

        public static void Push(Texture tex)
        {
            _tex = tex;
            EditorTipWindow editorTipWindow = GetWindow<EditorTipWindow>();
            editorTipWindow.minSize = new Vector2(_tex.width, _tex.height);
            editorTipWindow.Show();
        }

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
            _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Height(500));
            if (_tex != null)
            {
                GUILayout.Label(_tex, GUILayout.Width(_tex.width), GUILayout.Height(_tex.height));
            }

            if (!string.IsNullOrEmpty(_message))
            {
                GUILayout.TextArea(_message, textArea);
            }

            GUILayout.EndScrollView();
            if (GUILayout.Button("OK"))
            {
                Close();
            }
        }
    }
}