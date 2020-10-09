using System;
using System.ComponentModel.DataAnnotations;

namespace RPUtility
{
    /// <summary>
    /// Int型検証
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class IntValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Int型検証を行います。
        /// </summary>
        /// <param name="value">検証値が設定されます。</param>
        /// <returns>検証結果を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Int32.ToString")]
        public override bool IsValid(
            [param: Required]object value
            )
        {            
            // nullチェック
            if (value == null)
                throw new ArgumentNullException("value");

            return Utility.StringUtil.IntdataParse(
                value: value.ToString()
                ).ToString() == value.ToString();
        }
    }

    /// <summary>
    /// Double型検証
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DoubleValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Double型検証を行います。
        /// </summary>
        /// <param name="value">検証値が設定されます。</param>
        /// <returns>検証結果を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Double.ToString")]
        public override bool IsValid(
            [param: Required]object value
            )
        {
            // nullチェック
            if (value == null)
                throw new ArgumentNullException("value");

            return Utility.StringUtil.DoubledataParse(
                value: value.ToString()
                ).ToString() == value.ToString();
        }
    }

    /// <summary>
    /// Bool型検証
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class BoolValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Bool型検証を行います。
        /// </summary>
        /// <param name="value">検証値が設定されます。</param>
        /// <returns>検証結果を返します。</returns>
        public override bool IsValid(
            [param: Required]object value
            )
        {
            // nullチェック
            if (value == null)
                throw new ArgumentNullException("value");

            return Utility.StringUtil.BooleanParse(
                value: value.ToString()
                ).ToString() == value.ToString();
        }
    }

    /// <summary>
    /// DateTime型検証
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DateTimeValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// DateTime型検証を行います。
        /// </summary>
        /// <param name="value">検証値が設定されます。</param>
        /// <returns>検証結果を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.DateTime.ToString")]
        public override bool IsValid(
            [param: Required]object value
            )
        {
            // nullチェック
            if (value == null)
                throw new ArgumentNullException("value");

            return Utility.StringUtil.DateTimeParse(
                value: value.ToString()
                ).ToString() == value.ToString();
        }
    }
}
