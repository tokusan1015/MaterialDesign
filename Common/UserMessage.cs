using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Common
{
    /// <summary>
    /// メッセージ列挙型
    /// </summary>
    public enum UserMessage
    {
        不明なエラー,
    }

    /// <summary>
    /// ユーザーメッセージを表示するクラスです。
    /// </summary>
    public class UserMessageBox : Utility.MessageBoxBase
    {
        /// <summary>
        /// ユーザーメッセージを表示します。
        /// </summary>
        /// <param name="userMessage">ユーザーメッセージを設定します。</param>
        /// <returns>MessageBoxResultを返します。</returns>
        public MessageBoxResult Show(
            UserMessage userMessage
            )
        {
            return this.ShowUserMessage<UserMessage>(userMessage);
        }
    }
}
