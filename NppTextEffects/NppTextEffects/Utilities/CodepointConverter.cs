using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TextEffects.Utilities
{

    public static class CodepointConverter
    {

        public static string GetStringFromCodePoints(string codepoints)
        {
            if (string.IsNullOrWhiteSpace(codepoints))
                return string.Empty;

            var lines = codepoints.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var resultLines = new List<string>();

            foreach (var line in lines)
            {
                var hexes = Regex.Split(line, "U\\+", RegexOptions.IgnoreCase);

                var sb = new StringBuilder();
                foreach (var hex in hexes)
                {
                    if (string.IsNullOrEmpty(hex)) continue;
                    var codepoint = Convert.ToInt32(hex, 16);
                    var chstr = Char.ConvertFromUtf32(codepoint);
                    sb.Append(chstr);
                }

                resultLines.Add(sb.ToString());
            }

            // 元の改行コードを保持して結合
            return string.Join(Environment.NewLine, resultLines);
        }


        public static string GetCodePointsFromString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;

            var lines = str.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var resultLines = new List<string>();

            foreach (var line in lines)
            {
                var sb = new StringBuilder();
                var chars = line.ToCharArray();
                for (var i = 0; i < chars.Length; i++)
                {
                    int codepoint;
                    if (Char.IsSurrogate(chars[i]))
                    {
                        // TODO: 2文字目有無やサロゲートペアかの検証が必要
                        codepoint = Char.ConvertToUtf32(chars[i], chars[i + 1]);
                        i++;
                    }
                    else
                    {
                        codepoint = (int)chars[i];
                    }
                    var codepointStr = "U+" + codepoint.ToString("X4"); // 4桁～5桁
                    sb.Append(codepointStr);
                }

                resultLines.Add(sb.ToString());
            }

            // 元の改行コードを保持して結合
            return string.Join(Environment.NewLine, resultLines);
        }

    }
}
