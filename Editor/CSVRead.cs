using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Reference.ShaderReference;
using TreeEditor;

namespace Reference.Editor
{
    public class MyCSVRead
    {
        public static List<CSVItem> ReadCsv(string csv)
        {
            List<CSVItem> infos = new List<CSVItem>();

            string[] lines = csv.Split(new[] {"\r\n"}, StringSplitOptions.None);

            Queue<char> fuhao = new Queue<char>();

            var properties = typeof(CSVItem).GetProperties();
            var fields = typeof(CSVItem).GetFields();
            string[] patterns = new string[fields.Length + properties.Length];

            StringBuilder builder = new StringBuilder();

            if (lines != null)
            {
                lines[0] = null;
            }


            int index = 0;
            foreach (var line in lines)
            {
                index = 0;
                builder.Clear();
                fuhao.Clear();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                for (int i = 0; i < line.Length; i++)
                {
                    switch (line[i])
                    {
                        case '\"':
                            if (fuhao.Count > 0)
                            {
                                fuhao.Dequeue();
                            }
                            else
                            {
                                fuhao.Enqueue('\"');
                            }

                            break;
                        case ',':
                            if (fuhao.Count > 0)
                            {
                                builder.Append(line[i]);
                            }
                            else
                            {
                                patterns[index] = builder.ToString();
                                index++;
                                builder.Clear();
                            }

                            break;
                        default:
                            builder.Append(line[i]);
                            break;
                    }

                    if (i == line.Length - 1)
                    {
                        patterns[index] = builder.ToString();
                        index = 0;
                        builder.Clear();
                    }
                }


                var info = new CSVItem()
                {
                    H1 = patterns[0],
                    H2 = patterns[1],
                    Description = patterns[2],
                    Url = patterns[3],
                };
                info.Description = ParseCustomFuhao(info.Description);

                infos.Add(info);
            }

            return infos;
        }


        private static string ParseCustomFuhao(string dec)
        {
            dec = dec.Replace(@"\n", "\r\n");

            if (dec.Contains("G<"))
            {
                dec = dec.Replace("G<", "<color=#00FF00>");
                dec = dec.Replace(">G", "</color>"); 
            }

            if (dec.Contains("R<"))
            {
                dec = dec.Replace("R<", "<color=#FF0000>");
                dec = dec.Replace(">R", "</color>");
            }

            return dec;
        }
    }
}