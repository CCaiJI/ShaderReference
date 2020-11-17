using System.IO;
using UnityEditor;
using UnityEngine;

namespace MG.MDV
{
    [CustomEditor( typeof( MarkdownAsset ) )]
    public class MarkdownEditorAsset : Editor
    {
        public GUISkin Skin;

        MarkdownViewer mViewer;

        protected void OnEnable()
        {
            var content = ( target as MarkdownAsset ).text;
            var path    = AssetDatabase.GetAssetPath( target );

            string dir =@"Assets/ShaderReference/Plugins/UnityMarkdownViewer/Editor/Skin/MarkdownViewerSkin.guiskin";
            Skin = AssetDatabase.LoadAssetAtPath<GUISkin>(dir);
            Debug.Log(Skin);
            mViewer = new MarkdownViewer( Skin, path, content );
            EditorApplication.update += UpdateRequests;
        }

        protected void OnDisable()
        {
            EditorApplication.update -= UpdateRequests;
            mViewer = null;
        }

        public override bool UseDefaultMargins()
        {
            return false;
        }

        protected override void OnHeaderGUI()
        {
            //base.OnHeaderGUI(); 
        }

        public override void OnInspectorGUI()
        {
            mViewer.Draw();
        }


        void UpdateRequests()
        {
            if( mViewer.Update() )
            {
                Repaint();
            }
        }
    }
}
