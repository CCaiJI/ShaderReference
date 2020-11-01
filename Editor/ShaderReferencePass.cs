/**
 * @file         ShaderReferencePass.cs
 * @author       Hongwei Li(taecg@qq.com)
 * @created      2018-12-18
 * @updated      2018-12-18
 *
 * @brief        SubShader中Pass的内容
 */

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace taecg.tools.shaderReference
{
    public class ShaderReferencePass : EditorWindow
    {
        #region 数据成员
        private Vector2 scrollPos;
        #endregion

        public void DrawMainGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            ShaderReferenceUtil.DrawTitle("Tags");
            ShaderReferenceUtil.DrawOneContent("Tags { \"TagName1\" = \"Value1\" \"TagName2\" = \"Value2\" }", "Tag的语法结构，通过Tags{}来表示需要添加的标识,大括号内可以添加多组Tag（所以才叫Tags嘛）,名称（TagName）和值（Value）是成对成对出现的，并且全部用字符串表示。");

            ShaderReferenceUtil.DrawTitle("Queue");
            ShaderReferenceUtil.DrawOneContent("Queue", "渲染队列直接影响性能中的重复绘制，合理的队列可极大的提升渲染效率。\n渲染队列数目小于2500的对象都被认为是不透明的物体（从前往后渲染），高于2500的被认为是半透明物体（从后往前渲染）。\n\"Queue\" = \"Geometry+1\" 可通过在值后加数字的方式来改变队列。");
            ShaderReferenceUtil.DrawOneContent("\"Queue\" = \"Background\"", "值为1000，此队列的对象最先进行渲染。");
            ShaderReferenceUtil.DrawOneContent("\"Queue\" = \"Geometry\"", "默认值，值为2000，通常用于不透明对象，比如场景中的物件与角色等。");
            ShaderReferenceUtil.DrawOneContent("\"Queue\" = \"AlphaTest\"", "值为2450，要么完全透明要么完全不透明，多用于利用贴图来实现边缘透明的效果，也就是美术常说的透贴。");
            ShaderReferenceUtil.DrawOneContent("\"Queue\" = \"Transparent\"", "值为3000，常用于半透明对象，渲染时从后往前进行渲染，建议需要混合的对象放入此队列。");
            ShaderReferenceUtil.DrawOneContent("\"Queue\" = \"Overlay\"", "值为4000,此渲染队列用于叠加效果。最后渲染的东西应该放在这里（例如镜头光晕等）。");

            EditorGUILayout.EndScrollView();
        }
    }
}
#endif