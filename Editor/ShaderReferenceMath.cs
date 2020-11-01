/**
 * @file         ShaderReferenceMath.cs
 * @author       Hongwei Li(taecg@qq.com)
 * @created      2018-11-16
 * @updated      2018-11-16
 *
 * @brief        数学运算相关
 */

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace taecg.tools.shaderReference
{
    public class ShaderReferenceMath : EditorWindow
    {
        #region 数据成员
        private Vector2 scrollPos;
        #endregion

        public void DrawMainGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            ShaderReferenceUtil.DrawTitle("Math");
            ShaderReferenceUtil.DrawOneContent("dot (a,b)", "点乘，a和b必须为三维向量或者四维向量,其计算结果是两个向量夹角的余弦值，相当于a.x*b.x+a.y*b.y+a.z*b.z\na和b的位置无所谓前后，结果都是一样的\n");
            ShaderReferenceUtil.DrawOneContent("smoothstep (min,max,x)", "如果 x 比min 小，返回 0\n如果 x 比max 大，返回 1\n如果 x 处于范围 [min，max]中，则返回 0 和 1 之间的值(按值在min和max间的比例).");
            EditorGUILayout.EndScrollView();
        }
    }
}
#endif