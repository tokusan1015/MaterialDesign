using System;
using System.ComponentModel.DataAnnotations;

namespace Utility
{
    /// <summary>
    /// 文字列ユーティリティ
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public static class StringUtil
    {
        /// <summary>
        /// 文字列型から int 型へ変換します。
        /// 変換できない場合は、def 値を返します。
        /// </summary>
        /// <param name="value">文字列を設定します。</param>
        /// <param name="def">失敗した場合の値を設定します。</param>
        /// <returns>変換結果を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public static int IntdataParse(
            [param: Required]string value,
            int def = 0
            )
        {
            // nullチェック
            if (value == null)
                throw new ArgumentNullException("value");

            //if (data.Length <= 0) return def;
            return int.TryParse(value, out var o) ? o : def;
        }

        /// <summary>
        /// 文字列型から double 型へ変換します。
        /// 変換できない場合は、def 値を返します。
        /// </summary>
        /// <param name="value">文字列を設定します。</param>
        /// <param name="def">失敗した場合の値を設定します。</param>
        /// <returns>変換結果を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public static double DoubledataParse(
            [param: Required]string value,
            double def = 0
            )
        {
            // nullチェック
            if (value == null)
                throw new ArgumentNullException("value");

            //if (data.Length <= 0) return def;
            return double.TryParse(value, out var o) ? o : def;
        }

        /// <summary>
        /// 文字列型から float 型へ変換します。
        /// 変換できない場合は、def 値を返します。
        /// </summary>
        /// <param name="value">文字列を設定します。</param>
        /// <param name="def">失敗した場合の値を設定します。</param>
        /// <returns>変換結果を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public static float FloatdataParse(
            [param: Required]string value,
            float def = 0
            )
        {
            // nullチェック
            if (value == null)
                throw new ArgumentNullException("value");

            //if (data.Length <= 0) return def;
            return float.TryParse(value, out var o) ? o : def;
        }

        /// <summary>
        /// 文字列型から bool 型へ変換します。
        /// 変換できない場合は、def 値を返します。
        /// </summary>
        /// <param name="value">文字列を設定します。</param>
        /// <param name="def">失敗した場合の値を設定します。</param>
        /// <returns>変換結果を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public static bool BooleanParse(
            [param: Required]string value,
            bool def = false
            )
        {
            // nullチェック
            if (value == null)
                throw new ArgumentNullException("value");

            //if (data.Length <= 0) return def;
            return bool.TryParse(value: value, result: out var o) ? o : def;
        }

        /// <summary>
        /// 文字列型から DateTime 型へ変換します。
        /// 変換できない場合は、例外を発生します。
        /// </summary>
        /// <param name="value">文字列を設定します。</param>
        /// <param name="provider">プロバイダを設定します。</param>
        /// <param name="styles">スタイルを設定します。</param>
        /// <returns>変換結果を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public static DateTime DateTimeParse(
            [param: Required]string value,
            IFormatProvider provider = null,
            System.Globalization.DateTimeStyles styles = System.Globalization.DateTimeStyles.None
            )
        {
            // nullチェック
            if (value == null)
                throw new ArgumentNullException("value");

            return DateTime.TryParse(
                s: value,
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
            [param: Required]string text
            )
        {
            // nullチェック
            if (list == null)
                throw new ArgumentNullException("list");
            if (text == null)
                throw new ArgumentNullException("text");

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
            [param: Required]string source
            )
        {
            // nullチェック
            if (source == null)
                throw new ArgumentNullException("source");

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
        /// <summary>
        /// コマンドを分解します。
        /// </summary>
        /// <param name="command">コマンド文字列</param>
        /// <returns>(Command, ViewName)形式で返します。</returns>
        public static (string Command, string ViewName) AnalyseCommand(
            [param: Required]string command
            )
        {
            // nullチェック
            if (command == null)
                throw new ArgumentNullException("command");

            var s = command.Split(' ');
            string Command = "";
            string ViewName = "";

            if (s.Length > 0) Command = s[0];
            if (s.Length > 1) ViewName = s[1];

            return (Command, ViewName);
        }
    }
}
