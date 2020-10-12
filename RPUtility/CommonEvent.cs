using Prism.Events;
using System;

namespace RPUtility
{
    /// <summary>
    /// Prism用メッセージ送信イベントクラスを表します。
    /// MessageManager専用となります。
    /// 空クラスにしてください。
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class CommonEvent : PubSubEvent<IEventParam>
    {
    }
    /// <summary>
    /// フィルター用イベントパラメータインターフェース
    /// </summary>
    public interface IEventParam
    {
        /// <summary>
        /// 自身のタイプを取得します。
        /// フィルターに利用しています。
        /// </summary>
        Type EventType { get; }
        /// <summary>
        /// 送信先を表します。
        /// フィルターに利用しています。
        /// </summary>
        string Reciever { get; set; }
    }
    /// <summary>
    /// 送信情報クラスを表します。
    /// このクラスインスタンスを送信します。
    /// イベントの種類を増やす場合はクラスを作成します。
    /// IEventParamを必ず継承してください。
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class MessageInfoSend : IEventParam
    {
        /// <summary>
        /// 自身のタイプを取得します。
        /// </summary>
        public Type EventType => this.GetType();
        /// <summary>
        /// 送信先を表します。
        /// フィルターに利用しています。
        /// </summary>
        public string Reciever { get; set; }
        /// <summary>
        /// 送信元を表します。
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// コマンドを表します。
        /// View送信の場合は、Messageになります。
        /// </summary>
        public Common.EnumDatas.MassageInfo Command { get; set; }
        /// <summary>
        /// メッセージを表します。
        /// ViewのSendMessageCommandで使用されます。
        /// </summary>
        public string Message { get; set; }
    }
}
