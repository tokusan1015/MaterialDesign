using System.ComponentModel.DataAnnotations;

namespace RPUtility
{
    /// <summary>
    /// Int型検証
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class IntValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Int型検証を行います。
        /// </summary>
        /// <param name="value">検証値が設定されます。</param>
        /// <returns>検証結果を返します。</returns>
        public override bool IsValid(object value)
            => Utility.StringUtil.IntParse(
                data: value.ToString()
                ).ToString() == value.ToString();
    }

    /// <summary>
    /// Double型検証
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class DoubleValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Double型検証を行います。
        /// </summary>
        /// <param name="value">検証値が設定されます。</param>
        /// <returns>検証結果を返します。</returns>
        public override bool IsValid(object value)
            => Utility.StringUtil.DoubleParse(
                data: value.ToString()
                ).ToString() == value.ToString();
    }

    /// <summary>
    /// Bool型検証
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class BoolValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Bool型検証を行います。
        /// </summary>
        /// <param name="value">検証値が設定されます。</param>
        /// <returns>検証結果を返します。</returns>
        public override bool IsValid(object value)
            => Utility.StringUtil.BoolParse(
                data: value.ToString()
                ).ToString() == value.ToString();
    }

    /// <summary>
    /// DateTime型検証
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class DateTimeValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// DateTime型検証を行います。
        /// </summary>
        /// <param name="value">検証値が設定されます。</param>
        /// <returns>検証結果を返します。</returns>
        public override bool IsValid(object value)
            => Utility.StringUtil.DateTimeParse(
                data: value.ToString()
                ).ToString() == value.ToString();
    }
}
