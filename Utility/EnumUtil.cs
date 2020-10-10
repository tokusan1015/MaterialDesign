using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Utility
{
    /// <summary>
    /// 列挙型ユーティリティ
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public static class EnumUtil
    {
        /// <summary>
        /// 列挙型定義テキスト一覧を取得します。
        /// </summary>
        /// <param name="enumType">列挙型タイプを設定します。</param>
        /// <returns>定義テキスト一覧を返します。</returns>
        public static string[] GetEnumNames(
            [param: Required]Type enumType
            )
        {
            // nullチェック
            if (enumType == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(enumType));

            // 列挙型定義テキスト一覧を返します。
            return Enum.GetNames(enumType);
        }

        /// <summary>
        /// 列挙型値一覧を取得します。
        /// </summary>
        /// <param name="enumType">列挙型タイプを設定します。</param>
        /// <returns>列挙型定数一覧を返します。</returns>
        public static Array GetEnumValues(
            [param: Required]Type enumType
            )
        {
            // nullチェック
            if (enumType == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(enumType));

            // 列挙型定数一覧を返します
            return Enum.GetValues(enumType);
        }

        /// <summary>
        /// 文字列を列挙型に変換します。
        /// 例) var e = (eMode)EnumUtil.EnumParse(typeof(eMode), "mode_1");
        ///     eModeは列挙型名、mode_1は定義テキスト
        /// </summary>
        /// <param name="enumType">列挙型Typeを設定します。</param>
        /// <param name="value">定義テキストを設定します。</param>
        /// <returns>列挙型をオブジェクト型で返します。</returns>
        public static object EnumParse(
            [param: Required]Type enumType,
            string value
            )
        {
            // nullチェック
            if (enumType == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(enumType));
            if (value == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(value));

            return Enum.Parse(
                enumType: enumType,
                value: value
                );
        }

        /// <summary>
        /// 文字列が列挙型に存在するか評価します。
        /// 存在する場合trueを返します。
        /// </summary>
        /// <param name="enumType">列挙型のタイプを設定します。</param>
        /// <param name="value">評価する文字列を設定します。</param>
        /// <returns>存在する場合trueを返します。</returns>
        public static bool EnumIsDefined(
            [param: Required]Type enumType,
            string value
            )
        {
            // nullチェック
            if (enumType == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(enumType));
            if (value == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(value));

            return Enum.IsDefined(
                enumType: enumType,
                value: value
                );
        }
    }
}
