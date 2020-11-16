using UnityEngine;

namespace Reference.ShaderReference
{
    public class FMDItem
    {
        public bool IsImage;
        public string H1;
        public string H2;
        public string Url;
        public string Description;
        public Texture Texture;
        public bool IsNotLoadUrl=true;

        public bool Contains(string search)
        {
            search=search.ToLower();

            if ((!string.IsNullOrEmpty(H1) && H1.ToLower().Contains(search)) ||
                (!string.IsNullOrEmpty(H2) && H2.ToLower().Contains(search)) ||
                (!string.IsNullOrEmpty(Description) && Description.ToLower().Contains(search)))
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(H1))
            {
                return $"<color=#FF0000>小标题:\n{H2}</color>\n描述：\n{Description}";
            }
            else
            {
                return $"大标题：\n{H1}\n<color=#FF0000>小标题:\n{H2}</color>\n描述：\n{Description}";
            }
        }

        public bool IsNull()
        {
            return string.IsNullOrEmpty(H1) && string.IsNullOrEmpty(H2);
        }
    }
}