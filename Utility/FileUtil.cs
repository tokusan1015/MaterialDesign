using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Utility
{
    /// <summary>
    /// ファイルユーティリティ
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public static class FileUtil
    {
        /// <summary>
        /// 文字列一覧をファイルに書き込みます。
        /// </summary>
        /// <param name="list">文字列一覧を設定します。</param>
        /// <param name="filePath">出力ファイルパスを設定します。</param>
        public static void FileWriteAllText(
            [param: Required]IReadOnlyCollection<string> list,
            string filePath
            )
        {
            // nullチェック
            if (list == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(list));

            try
            {
                // 文字列一覧をファイルに書き込みます。
                File.WriteAllLines(
                    path: filePath,
                    contents: list.OrderBy(x => x)
                    );
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ファイルから文字列を読込みます。
        /// 文字列一覧として返します。
        /// </summary>
        /// <param name="filePath">ファイルパスを設定します。</param>
        /// <returns>文字列一覧を返します。</returns>
        public static IReadOnlyCollection<string> FileReadAllText(
            string filePath
            )
        {
            try
            {
                // ファイルから文字列を読込み返します。
                var data = File.ReadAllLines(filePath).OrderBy(x => x);
                return new List<string>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
