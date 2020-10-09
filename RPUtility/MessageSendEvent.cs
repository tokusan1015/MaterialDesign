using Prism.Events;

namespace RPUtility
{
    /// <summary>
    /// Prism用メッセージ通信イベントクラスを表します。
    /// 空クラスでなければなりません。
    /// </summary>
    public class MessageSendEvent : PubSubEvent<MessageSend>
    {
    }
    /// <summary>
    /// 送信メッセージ情報クラスを表します。
    /// </summary>
    public class MessageSend
    {
        /// <summary>
        /// 送信元を表します。
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// 送信先を表します。
        /// </summary>
        public string Reciever { get; set; }
        /// <summary>
        /// コマンドを表します。
        /// </summary>
        public Common.EnumDatas.MessageCommand Command { get; set; }
        /// <summary>
        /// メッセージを表します。
        /// 必要であれば追加します。
        /// </summary>
        public string Message { get; set; }
    }
}
