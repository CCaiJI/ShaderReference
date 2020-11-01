using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Reference.ShaderReference;

namespace Reference.Editor
{
    public class FMdRead
    {
        private static string Pattern_HuaKuohao = @"\[(\w|\W){1,}\]";
        private static string Pattern_Kuohao = @"\((\w|\W){1,}\)";

        /// <summary>
        /// ^：行开头开始匹配,表示 非开头无法匹配
        /// \(: 这个匹配的就是（
        /// \s：匹配空格
        /// #：匹配#
        /// (|):逻辑符号()，或者
        /// {1}：数量. 
        /// </summary>
        private static Dictionary<EPattern, FMDMatch> _patterns = new Dictionary<EPattern, FMDMatch>()
        {
            {EPattern.EH1, new FMDMatch(@"^#{1}\s", MatchH)},
            {EPattern.EH2, new FMDMatch(@"^#{2,}\s", MatchH)},
            {EPattern.EDec1, new FMDMatch(@"^-{1}\s", MatchDec1)},
            {EPattern.EDec2, new FMDMatch(@"^\s{1,}-{1}\s", MatchDec2)},
            {EPattern.EUrl, new FMDMatch($"{Pattern_HuaKuohao}{Pattern_Kuohao}", MatchUrl)},
            {EPattern.EUrlH, new FMDMatch($"{Pattern_HuaKuohao}{Pattern_Kuohao}", MatchUrlH)},
        };

        private static FMDItem TempItem;

        public static List<FMDItem> ReadMd(string fmd)
        {
            List<FMDItem> infos = new List<FMDItem>();
            string[] lines = fmd.Split(new[] {"\r\n"}, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (!string.IsNullOrEmpty(line) && !line.StartsWith("//"))
                {
                    TempItem = new FMDItem();

                    TempItem.Url = _patterns[EPattern.EUrl].Match(line);
                    bool isHasUrl = !string.IsNullOrEmpty(TempItem.Url);

                    string h1 = _patterns[EPattern.EH1].Match(line);

                    if (!string.IsNullOrEmpty(h1) && isHasUrl)
                    {
                        TempItem.H1 = _patterns[EPattern.EUrlH].Match(line);
                    }
                    else if (!string.IsNullOrEmpty(h1))
                    {
                        TempItem.H1 = h1;
                    }


                    if (string.IsNullOrEmpty(TempItem.H1))
                    {
                        if (isHasUrl)
                        {
                            TempItem.H2 = _patterns[EPattern.EUrlH].Match(line);
                        }
                        else
                        {
                            TempItem.H2 = _patterns[EPattern.EH2].Match(line);
                        }

                        bool isNext = true;
                        int index = i + 1;
                        while (isNext)
                        {
                            if (index < lines.Length)
                            {
                                string dec2 = _patterns[EPattern.EDec1].Match(lines[index]);
                                if (!string.IsNullOrEmpty(dec2))
                                {
                                    if (string.IsNullOrEmpty(TempItem.Description))
                                    {
                                        TempItem.Description = $"  {dec2}";
                                    }
                                    else
                                    {
                                        TempItem.Description = $"{TempItem.Description}\r\n  {dec2}";
                                    }

                                    index++;
                                }
                                else
                                {
                                    isNext = false;
                                }
                            }
                            else
                            {
                                isNext = false;
                            }
                        }

                        i = index - 1;
                    }

                    if (!TempItem.IsNull())
                        infos.Add(TempItem);
                }
            }

            return infos;
        }


        #region  匹配辅助

        struct FMDMatch
        {
            public string Pattern;
            private Func<string, string, string> Func;

            public FMDMatch(string pattern, Func<string, string, string> func)
            {
                Pattern = pattern;
                Func = func;
            }

            public string Match(string line)
            {
                return Func(line, Pattern);
            }
        }


        enum EPattern //目的在于减少类写的太多
        {
            EH1,
            EH2,
            EUrlH, //#  [参考文档](httP://....)  = 参考文档
            EDec1,
            EDec2,
            EUrl,
        }


        private static string MatchH(string line, string pattern)
        {
            var temp = Regex.Split(line, pattern);
            if (temp != null && temp.Length > 1)
            {
                return temp[1].Trim();
            }

            return null;
        }

        private static string MatchDec1(string line, string pattern)
        {
            var temp = Regex.Split(line, pattern);
            if (temp != null && temp.Length > 1)
            {
                return temp[1].Trim();
            }

            return null;
        }

        private static string MatchDec2(string line, string pattern)
        {
            var temp = Regex.Split(line, pattern);
            if (temp != null && temp.Length > 1)
            {
                return temp[1].Trim();
            }

            return null;
        }

        private static string MatchUrl(string line, string pattern)
        {
            //  var temp = Regex.Match(line, @"\[");
            var temp = Regex.Match(line, pattern);
            if (temp != null && !string.IsNullOrEmpty(temp.Value))
            {
                var match = Regex.Match(temp.Value, Pattern_HuaKuohao);
                line = temp.Value.Substring(match.Length + 1, temp.Length - match.Length - 2);
                
                return line;
            }

            return null;
        }

        private static string MatchUrlH(string line, string pattern)
        {
            //  var temp = Regex.Match(line, @"\[");
            var temp = Regex.Match(line, pattern);
            if (temp != null && !string.IsNullOrEmpty(temp.Value))
            {
                string h = Regex.Match(temp.Value, Pattern_HuaKuohao).Value;
                h = h.Substring(1, h.Length - 2);
                return h;
            }

            return null;
        }

        #endregion


        private static FMDItem Match(string line)
        {
            FMDItem item = new FMDItem();
            item.Url = _patterns[EPattern.EUrl].Match(line);
            item.H1 = _patterns[EPattern.EH1].Match(line);

            if (!string.IsNullOrEmpty(item.Url) && !string.IsNullOrEmpty(item.H1))
                item.H1 = _patterns[EPattern.EUrlH].Match(line);

            item.H2 = _patterns[EPattern.EH2].Match(line);
            item.Description = _patterns[EPattern.EDec1].Match(line);
            return item;
        }
    }
}