using Prism.Events;

namespace RPUtility
{
    /// <summary>
    /// Prism用メッセージ通信クラスを表します。
    /// 空クラスでなければなりません。
    /// </summary>
    public class MessageSendEvent : PubSubEvent<string>
    {
    }
}
