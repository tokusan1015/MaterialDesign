using System;

[assembly: CLSCompliant(true)]
namespace Utility
{
    #region DeveloperAttribute
    /// <summary>
    /// クラスの開発者と所属を記録する属性を表します。
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    [AttributeUsage(
        AttributeTargets.Class,
        AllowMultiple = true,
        Inherited = false)]
    public sealed class DeveloperAttribute : Attribute
    {
        /// <summary>
        /// 開発者名
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">開発者名を設定します。</param>
        public DeveloperAttribute(
            string name
            )
        {
            this.Name = name;
        }
    }
    #endregion DeveloperAttribute

    #region ButtonAttribute
    /// <summary>
    /// ボタン属性を表します。
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    [AttributeUsage(
        AttributeTargets.Property,
        AllowMultiple = false,
        Inherited = false)]
    public sealed class ButtonAttribute : Attribute
    {
        /// <summary>
        /// タイトル
        /// </summary>
        public string ButtonTitle { get; private set; } = "";
        /// <summary>
        /// コマンド
        /// </summary>
        public string ButtonCommand { get; private set; } = "";
        /// <summary>
        /// コンストラクタ
        /// コマンド例
        /// 画面遷移  : "Move [遷移先]"
        /// アプリ終了: "Exit"
        /// </summary>
        /// <param name="buttonCommand">ボタンの動作を決めるコマンドを設定します。</param>
        /// <param name="transitionName">遷移先Viewの名称を設定します。</param>
        /// <param name="buttonTitle">ボタンに表示するタイトルを設定します。</param>
        public ButtonAttribute(
            string buttonTitle,
            string buttonCommand
            )
        {
            this.ButtonCommand = buttonCommand;
            this.ButtonTitle = buttonTitle;
        }
    }
    #endregion ButtonAttribute
}
