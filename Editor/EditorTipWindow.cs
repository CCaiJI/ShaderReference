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

        private void OnGUI()
        {
            GUILayout.TextArea(_message, textArea);

            if (GUILayout.Button("OK"))
            {
                Close();
            }
        }
    }
}