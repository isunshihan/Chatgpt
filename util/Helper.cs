using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using TinyPinyin;

namespace Chatgpt
{
    public static class Helper
    {

        public static int ConvertChineseToArabic(string input)
        {
            // 判断输入的字符串是否为阿拉伯数字
            if (Regex.IsMatch(input, @"^\d+$"))
            {
                return int.Parse(input);
            }

            // 判断输入的字符串是否为汉字
            if (Regex.IsMatch(input, @"^[\u4e00-\u9fa5]+$"))
            {
                Dictionary<char, int> chineseToArabicMap = new Dictionary<char, int>
            {
                {'零', 0},
                {'一', 1},
                {'二', 2},
                {'三', 3},
                {'四', 4},
                {'五', 5},
                {'六', 6},
                {'七', 7},
                {'八', 8},
                {'九', 9},
                {'十', 10}
            };

                int result = 0;
                int temp = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    int value = chineseToArabicMap[input[i]];
                    if (value >= 10)
                    {
                        if (temp == 0) temp = 1;
                        result += temp * value;
                        temp = 0;
                    }
                    else
                    {
                        temp = value;
                    }
                }
                result += temp;
                return result;
            }

            return 0;
        }

        public static string GetPinyin(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in str)
            {
                if (!char.IsWhiteSpace(item))
                {
                    if (PinyinHelper.IsChinese(item))
                    {
                        sb.Append(PinyinHelper.GetPinyin(item));
                    }
                }
            }
            return sb.ToString().ToLower();
        }
    }
}
