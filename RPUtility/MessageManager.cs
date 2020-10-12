using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RPUtility
{
    /// <summary>
    /// PrismでViewからMainViewに対してメッセージを送受信するクラスです。
    /// CommonEventで送受信を行います。
    /// 送信先名を"ALL"にすると全Viewに対して送信します。
    /// Messageには、"コマンド"を設定します。
    /// コマンドは列挙型で宣言しておきます。
    /// 送信先と送信元が同じ場合でも送信は実行されます。
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
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
        /// 入力ステータス送信コマンドを表します。
        /// 入力ステータスを文字列で設定します。
        /// Common.EnumDatas.InputStatus
        /// 追加するイベントがある場合DelegeteCommandを増やして下さい。
        /// </summary>
        public DelegateCommand InputStatusSendCommand { get; } = null;

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
        /// XALMとBindingする用
        /// </summary>
        private string _message = "MainView ViewA NoInputError";
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
            [param: Required]IEventAggregator eventAggregator,
            [param: Required]Action<RPUtility.IEventParam> receivedMessage,
            [param: Required]string mainViewName,
            [param: Required]string viewName
            ) 
        {
            // nullチェック
            this.EventAggregator = eventAggregator ?? throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(eventAggregator));
            if (receivedMessage == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(receivedMessage));
            if (mainViewName == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(mainViewName));
            if (viewName == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(viewName));

            // 文字数チェック
            if (mainViewName.Trim().Length < 1) throw new InvalidOperationException(MethodBase.GetCurrentMethod().Name + " : " +nameof(mainViewName) + " is Empty");
            if (viewName.Trim().Length < 1) throw new InvalidOperationException(MethodBase.GetCurrentMethod().Name + " : " +nameof(viewName) + " is Empty");

            // 内部保存
            this.MainViewName = mainViewName;
            this.ViewName = viewName;

            // メッセージ送信・受信を設定します。
            this.InputStatusSendCommand = new DelegateCommand(this.InputStatusSend);
            this.EventAggregator.GetEvent<CommonEvent>()
                .Subscribe(
                    action: receivedMessage,
                    threadOption: ThreadOption.PublisherThread,
                    keepSubscriberReferenceAlive: false,
                    // 送信先でフィルターしています。
                    filter: (filter) => filter.Reciever.Contains(this.ViewName)
                    );
        }

        #region 送信
        /// <summary>
        /// メッセージをマスターに送信します。
        /// XAMLからの送信用です。
        /// 追加するイベントがある場合は増やしてください。
        /// ***************************************
        /// プログラムからはコールしないでください。
        /// ***************************************
        /// </summary>
        private void InputStatusSend()
        {
            // メッセージ生成
            var eventParam = new MessageInfoSend()
            {
                Sender = this.ViewName,
                Reciever = this.MainViewName,
                Command = Common.EnumDatas.MassageInfo.Message,
                Message = this.Message
            };

            // メッセージ送信
            this.SendMessage(eventParam: eventParam);
        }
        /// <summary>
        /// メッセージを送信先に送信します。
        /// プログラム上からはこちらを使用します。
        /// </summary>
        /// <param name="reciever">送信先を設定します。</param>
        /// <param name="messageCommand">メッセージコマンドを設定します。</param>
        /// <param name="message">必要ならばメッセージを設定します。</param>
        public void SendMessage<TEventParam>(
            [param: Required]TEventParam eventParam
            ) where TEventParam : IEventParam
        {
            // nullチェック
            if (eventParam == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(eventParam));

            // メッセージ送信チェック
            if (!this.Start) return;

            // メッセージ送信
            this.EventAggregator
                .GetEvent<CommonEvent>()
                .Publish(payload: eventParam);
        }
        #endregion 送信
    }
}
