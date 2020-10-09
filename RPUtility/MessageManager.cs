using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPUtility
{
    /// <summary>
    /// PrismでViewからMainViewに対してメッセージを送受信するクラスです。
    /// 送信先名を"ALL"にすると全Viewに対して送信します。
    /// Messageには、"コマンド"を設定します。
    /// コマンドは列挙型で宣言しておきます。
    /// 送信先と送信元が同じ場合でも送信は実行されます。
    /// </summary>
    public class MessageManager : BindableBase
    {
        /// <summary>
        /// 全員宛文字列
        /// </summary>
        const string SEND_ALL = "ALL";

        /// <summary>
        /// 送信フォーマット
        /// </summary>
        const string SEND_FORMAT = "{0} {1} {2}";

        /// <summary>
        /// イベントアグリゲータを表します。
        /// </summary>
        private IEventAggregator EventAggregator { get; } = null;

        /// <summary>
        /// メッセージコマンドを表します。
        /// "送信先 送信元 コマンド"形式で設定します。
        /// </summary>
        public DelegateCommand SendMessageCommand { get; } = null;

        /// <summary>
        /// MainViewの名前を表します。
        /// </summary>
        public string MainViewName { get; } = "";

        /// <summary>
        /// 自身のViewの名前を表します。
        /// </summary>
        public string ViewName { get; } = "";

        /// <summary>
        /// 送信するメッセージを表します。
        /// </summary>
        private string _message = "Message to Send";
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        /// <summary>
        /// 送受信開始フラグを表します。
        /// </summary>
        public bool Start { get; set; } = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="eventAggregator">イベントアグリゲータを設定します。</param>
        /// <param name="receivedMessage">受信するメソッドを設定します。</param>
        /// <param name="mainViewName">MainView名を設定します。</param>
        /// <param name="viewName">自身のViewNameを設定します。</param>
        public MessageManager(
            IEventAggregator eventAggregator,
            Action<RPUtility.MessageSend> receivedMessage,
            string mainViewName,
            string viewName
            )
        {
            // nullチェック
            this.EventAggregator = eventAggregator ?? throw new ArgumentNullException("eventAggregator");
            if (receivedMessage == null) throw new ArgumentNullException("receivedMessage");
            if (mainViewName == null) throw new ArgumentNullException("mainViewName");
            if (viewName == null) throw new ArgumentNullException("viewName");

            // 文字数チェック
            if (mainViewName.Trim().Length < 1) throw new InvalidOperationException("mainViewName is Empty");
            if (viewName.Trim().Length < 1) throw new InvalidOperationException("viewName is Empty");

            // 内部保存
            this.MainViewName = mainViewName;
            this.ViewName = viewName;

            // メッセージ送信・受信を設定します。
            this.SendMessageCommand = new DelegateCommand(this.SendMessage);
            this.EventAggregator.GetEvent<RPUtility.MessageSendEvent>()
                .Subscribe(
                    action: receivedMessage,
                    threadOption: ThreadOption.PublisherThread,
                    keepSubscriberReferenceAlive: false,
                    filter: (filter) => filter.Reciever.Contains(this.ViewName)
                    );
        }

        #region 送信
        /// <summary>
        /// メッセージをマスターに送信します。
        /// </summary>
        public void SendMessage()
        {
            // メッセージ送信チェック
            if (!this.Start) return;

            // メッセージ生成
            var sendMessage = new RPUtility.MessageSend()
            {
                Sender = this.ViewName,
                Reciever = this.MainViewName,
                Command = Common.EnumDatas.MessageCommand.Message,
                Message = this.Message
            };

            // メッセージ送信
            this.EventAggregator
                .GetEvent<RPUtility.MessageSendEvent>()
                .Publish(payload: sendMessage);
        }
        /// <summary>
        /// メッセージをマスターに送信します。
        /// </summary>
        /// <param name="messageCommand">メッセージコマンドを設定します。</param>
        /// <param name="message">必要ならばメッセージを設定します。</param>
        public void SendMessage(
            Common.EnumDatas.MessageCommand messageCommand,
            string message = ""
            )
        {
            // メッセージ送信チェック
            if (!this.Start) return;

            // メッセージ生成
            var sendMessage = new RPUtility.MessageSend()
            {
                Sender = this.ViewName,
                Reciever = this.MainViewName,
                Command = messageCommand,
                Message = message
            };

            // メッセージ送信
            this.EventAggregator
                .GetEvent<RPUtility.MessageSendEvent>()
                .Publish(payload: sendMessage);
        }
        #endregion 送信
    }
}
