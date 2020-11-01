/**
 * @file         ShaderReferenceMath.cs
 * @author       Hongwei Li(taecg@qq.com)
 * @created      2018-12-05
 * @updated      2018-12-12
 *
 * @brief        杂项，一些算法技巧
 */

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace taecg.tools.shaderReference
{
    public class ShaderReferenceMiscellaneous : EditorWindow
    {
        #region 数据成员
        private Vector2 scrollPos;
        #endregion

        public void DrawMainGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            ShaderReferenceUtil.DrawTitle("UV");
            ShaderReferenceUtil.DrawOneContent("UV重映射到中心位置", "float2 centerUV = uv * 2 - 1\n将UV值重映射为(-1,-1)~(1,1)，也就是将UV的中心点从左下角移动到中间位置。");
            ShaderReferenceUtil.DrawOneContent("画圆", "float circle = smoothstep(_Radius, (_Radius + _CircleFade), length(uv * 2 - 1));\n利用UV来画圆，通过_Radius来调节大小，_CircleFade来调节边缘虚化程序。");
            ShaderReferenceUtil.DrawOneContent("画矩形", "float2 centerUV = abs(i.uv.xy * 2 - 1);\n"
            + "float rectangleX = smoothstep(_Width, (_Width + _RectangleFade), centerUV.x);\n"
            + "float rectangleY = smoothstep(_Heigth, (_Heigth + _RectangleFade), centerUV.y);\n"
            + "float rectangleClamp = clamp((rectangleX + rectangleY), 0.0, 1.0);\n"
            + "利用UV来画矩形，_Width调节宽度，_Height调节高度，_RectangleFade调节边缘虚化度。");

            ShaderReferenceUtil.DrawTitle("顶点");
            ShaderReferenceUtil.DrawOneContent("模型中心点坐标", "float3 objCenterPos = mul( unity_ObjectToWorld, float4( 0, 0, 0, 1 ) ).xyz;\n"
            + "在Shader中获取当前模型的中心点，其实就是将(0,0,0)点从本地转换到世界空间坐标下即可，在制作对象从下往之类的效果时常用到。");
            EditorGUILayout.EndScrollView();
        }
    }
}
#endif