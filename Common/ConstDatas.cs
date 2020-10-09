using Reactive.Bindings;
using System.Reflection;

namespace Common
{
    /// <summary>
    /// 共有固定値を表します。
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public static class ConstDatas
    {
        #region 固定値
        /// <summary>
        /// ContentRegion
        /// </summary>
        public static readonly string ContentRegion = "ContentRegion";
        /// <summary>
        /// ReactivePropertyのモード設定
        /// </summary>
        public static readonly ReactivePropertyMode DefaultreactivePropertyMode =
            ReactivePropertyMode.Default 
            | ReactivePropertyMode.IgnoreInitialValidationError;
        /// <summary>
        /// CommonDatasのプロパティのBindingFlags
        /// </summary>
        public static readonly BindingFlags CommonDatasBindingFlags =
            BindingFlags.NonPublic
            | BindingFlags.Instance
            | BindingFlags.DeclaredOnly
            ;
        /// <summary>
        /// ControlInfoのプロパティのBindingFlags
        /// </summary>
        public static readonly BindingFlags ControlInfoBindingFlags =
            BindingFlags.Public
            | BindingFlags.Instance
            | BindingFlags.DeclaredOnly
            ;
        /// <summary>
        /// 入力必須項目です。
        /// </summary>
        public static readonly string InputRequired = "入力必須項目です。";
        #endregion 固定値
    }
}
