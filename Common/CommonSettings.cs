using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 共通設定を表します。
    /// Viewデータ(初期表示ViewのViewDatas)で設定され、MainViewで使用されます。
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class CommonSettings
    {
        #region MainViewSettings
        /// <summary>
        /// Mainタイトルを表します。
        /// </summary>
        public string MainTitle { get; set; }
        /// <summary>
        /// View切替ボタンのタイトルを表します。
        /// ボタン数はMainViewで管理します。
        /// key:View名, value:ボタンタイトル
        /// </summary>
        public IReadOnlyCollection<KeyValuePair<string, ButtonInfo>> ButtonInfo { get; set; } 
        /// <summary>
        /// ViewTitleを表します。
        /// key:View名, value:サブタイトル
        /// </summary>
        public IReadOnlyCollection<KeyValuePair<string, string>> ViewTitle { get; set; }
        #endregion MainViewSettings
    }
    /// <summary>
    /// ボタン情報を表します。
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class ButtonInfo
    {
        /// <summary>
        /// ボタン名称
        /// </summary>
        public string PropertyName { get; set; } = null;
        /// <summary>
        /// ボタン表示名
        /// </summary>
        public string ButtonTitle { get; set; } = null;
        /// <summary>
        /// ボタンコマンド
        /// </summary>
        public string ButtonCommand { get; set; } = null;
        /// <summary>
        /// Enableフラグ
        /// </summary>
        public bool? IsEnable { get; set; } = null;
        /// <summary>
        /// Visibleフラグ
        /// </summary>
        public bool? IsVisible { get; set; } = null;
    }
}
