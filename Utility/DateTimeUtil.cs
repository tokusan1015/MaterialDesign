using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Utility
{
    /// <summary>
    /// DateTimeユーティリティ
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public static class DateTimeUtil
    {
        /// <summary>
        /// 対象日での年齢を計算します。
        /// age = (targetDay - birthday) / 10000
        /// 但し、targetDayとbirthdayは、yyyyMMdd形式とします。
        /// </summary>
        /// <param name="birthday">誕生日を設定します。</param>
        /// <param name="targetDay">対象日を設定します。nullの場合は本日とします。</param>
        /// <returns>年齢を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public static int CalculateAge(
            [param: Required]DateTime birthday,
            DateTime? targetDay = null
            )
        {
            // フォーマット
            const string format = "yyyyMMdd";

            // 誕生日をyyyyMMdd形式のintで取得する。
            var bd = StringUtil.IntdataParse(birthday.ToString(format, provider: CultureInfo.CurrentCulture));

            // 対象日をyyyyMMdd形式のintで取得する。
            var td = StringUtil.IntdataParse(
                targetDay == null ? 
                    DateTime.Now.ToString(format: format, provider: CultureInfo.CurrentCulture)
                    : targetDay?.ToString(format: format, provider: CultureInfo.CurrentCulture)
                    );

            // 年齢を計算して返します。
            return (td - bd) / 10000;
        }
    }
}
