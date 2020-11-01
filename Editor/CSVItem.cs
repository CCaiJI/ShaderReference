namespace Reference.ShaderReference
{
    public struct CSVItem
    {
        public string H1;
        public string H2;
        public string Url;
        public string Description;

        public bool Contains(string search)
        {
            if (H1.Contains(search) || H2.Contains(search) || Description.Contains(search))
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(H1))
            {
                
                return $"<color=#FF0000>{H2}</color>\n{Description}";
            }
            else
                return $"{H1}\n<color=#FF0000>{H2}</color>\n{Description}";
        }
    }
}