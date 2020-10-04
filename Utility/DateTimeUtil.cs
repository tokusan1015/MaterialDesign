using System;
using System.ComponentModel.DataAnnotations;

namespace Utility
{
    /// <summary>
    /// DateTimeユーティリティ
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class DateTimeUtil
    {
        /// <summary>
        /// 対象日での年齢を計算します。
        /// age = (targetDay - birthday) / 10000
        /// 但し、targetDayとbirthdayは、yyyyMMdd形式とします。
        /// </summary>
        /// <param name="birthday">誕生日を設定します。</param>
        /// <param name="targetDay">対象日を設定します。nullの場合は本日とします。</param>
        /// <returns>年齢を返します。</returns>
        public static int CalculateAge(
            [param: Required]DateTime birthday,
            DateTime? targetDay = null
            )
        {
            // フォーマット
            const string format = "yyyyMMdd";

            // 誕生日をyyyyMMdd形式のintで取得する。
            var bd = StringUtil.IntParse(birthday.ToString(format));

            // 対象日をyyyyMMdd形式のintで取得する。
            var td = StringUtil.IntParse(
                targetDay == null ? DateTime.Now.ToString(format) : targetDay?.ToString(format));

            // 年齢を計算して返します。
            return (td - bd) / 10000;
        }
    }
}
