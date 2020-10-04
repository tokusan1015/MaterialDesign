using System;
using System.ComponentModel.DataAnnotations;

namespace Utility
{
    /// <summary>
    /// 文字列ユーティリティ
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class StringUtil
    {
        /// <summary>
        /// 文字列型から int 型へ変換します。
        /// 変換できない場合は、def 値を返します。
        /// </summary>
        /// <param name="data">文字列を設定します。</param>
        /// <param name="def">失敗した場合の値を設定します。</param>
        /// <returns>変換結果を返します。</returns>
        public static int IntParse(
            string data,
            int def = 0
            )
        {
            //if (data.Length <= 0) return def;
            return int.TryParse(data, out var o) ? o : def;
        }

        /// <summary>
        /// 文字列型から double 型へ変換します。
        /// 変換できない場合は、def 値を返します。
        /// </summary>
        /// <param name="data">文字列を設定します。</param>
        /// <param name="def">失敗した場合の値を設定します。</param>
        /// <returns>変換結果を返します。</returns>
        public static double DoubleParse(
            string data,
            double def = 0
            )
        {
            //if (data.Length <= 0) return def;
            return double.TryParse(data, out var o) ? o : def;
        }

        /// <summary>
        /// 文字列型から float 型へ変換します。
        /// 変換できない場合は、def 値を返します。
        /// </summary>
        /// <param name="data">文字列を設定します。</param>
        /// <param name="def">失敗した場合の値を設定します。</param>
        /// <returns>変換結果を返します。</returns>
        public static float FloatParse(
            string data,
            float def = 0
            )
        {
            //if (data.Length <= 0) return def;
            return float.TryParse(data, out var o) ? o : def;
        }

        /// <summary>
        /// 文字列型から bool 型へ変換します。
        /// 変換できない場合は、def 値を返します。
        /// </summary>
        /// <param name="data">文字列を設定します。</param>
        /// <param name="def">失敗した場合の値を設定します。</param>
        /// <returns>変換結果を返します。</returns>
        public static bool BoolParse(
            string data,
            bool def = false
            )
        {
            //if (data.Length <= 0) return def;
            return bool.TryParse(data, out var o) ? o : def;
        }

        /// <summary>
        /// *** 未検証 ***
        /// 文字列型から DateTime 型へ変換します。
        /// 変換できない場合は、DateTime.Nowを返します。
        /// *** 未検証 ***
        /// </summary>
        /// <param name="data">文字列を設定します。</param>
        /// <returns>変換結果を返します。</returns>
        public static DateTime DateTimeParse(
            string data,
            IFormatProvider provider = null,
            System.Globalization.DateTimeStyles styles = System.Globalization.DateTimeStyles.None
            )
        {
            //if (data.Length <= 0) return def;
            return DateTime.TryParse(
                s: data,
                provider: provider,
                styles: styles,
                result: out var o
                ) ? o : DateTime.Now;
        }

        /// <summary>
        /// 文字列一覧の中から対応する文字列と
        /// 一致するインデックスを取得します。
        /// 一致しない場合は、-1 を返します。
        /// </summary>
        /// <param name="list">文字列一覧を設定します。</param>
        /// <param name="text">文字列を設定します。</param>
        /// <returns>インデックスを返します。</returns>
        public static int SearchStringListIndex(
            [param: Required]string[] list,
            string text
            )
        {
            for (int i = 0; i < list.Length; i++)
                if (text == list[i]) return i;
            return -1;
        }

        /// <summary>
        /// 連続空白を削除します。
        /// </summary>
        /// <param name="source">文字列を設定します。</param>
        /// <returns>連続空白を削除した文字列を返します。</returns>
        public static string RemoveManySpace(
            string source
            )
        {
            string result = "";
            bool flag = false;     // 直前が' 'の場合true
            foreach (var c in source)
            {
                if (c == ' ')
                {
                    if (!flag)
                    {
                        result += c;
                        flag = true;
                    }
                }
                else
                {
                    result += c;
                    flag = false;
                }
            }

            return result;
        }
    }
}
