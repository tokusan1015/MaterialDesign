using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Utility
{
    /// <summary>
    /// ユーザーメッセージ基本クラス
    /// </summary>
    public class MessageBoxBase
    {
        /// <summary>
        /// キャプション
        /// </summary>
        protected string Caption { get; set; } = "Warning";
        /// <summary>
        /// ボタン
        /// </summary>
        protected MessageBoxButton Button { get; set; } = MessageBoxButton.OK;
        /// <summary>
        /// アイコン
        /// </summary>
        protected MessageBoxImage Icon { get; set; } = MessageBoxImage.Warning;
        /// <summary>
        /// デフォルトボタン
        /// </summary>
        protected MessageBoxResult DefaultResult { get; set; } = MessageBoxResult.OK;
        /// <summary>
        /// オプション
        /// </summary>
        protected MessageBoxOptions Option { get; set; } = MessageBoxOptions.None;

        /// <summary>
        /// ユーザーメッセージを表示します。
        /// </summary>
        /// <typeparam name="TEnumMessage">メッセージ用列挙型を設定します。</typeparam>
        /// <param name="enumMessage">メッセージを設定します。</param>
        protected MessageBoxResult ShowUserMessage<TEnumMessage>(
            TEnumMessage enumMessage
            ) where TEnumMessage : Enum
        {
            return MessageBox.Show(
                messageBoxText: enumMessage.ToString(),
                caption: this.Caption,
                button: this.Button,
                icon: this.Icon,
                defaultResult: this.DefaultResult,
                options: this.Option
                );
        }
    }
}
