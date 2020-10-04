using System;
using System.Timers;

namespace Utility
{
    /// <summary>
    /// クロックタイマー
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class ClockTimer
    {
        #region privateプロパティ
        /// <summary>
        /// タイマー
        /// </summary>
        private Timer _Timer { get; set; }
        #endregion privateプロパティ

        #region publicプロパティ
        /// <summary>
        /// イベント発生中を表します。
        /// </summary>
        public bool IsRunning
        {
            get { return this._Timer.Enabled; }

        }
        /// <summary>
        /// 開始日時
        /// </summary>
        public DateTime StartTime { get; } = DateTime.Now;
        #endregion publicプロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="interval">イベント間隔(ms)を設定します。</param>
        public ClockTimer(
            int interval = 1000
            )
        {
            // タイマーを生成します。
            this._Timer = new Timer(interval: interval)
            {
                // イベント発生を停止します。
                Enabled = false
            };
        }
        #endregion コンストラクタ

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~ClockTimer()
        {
            // タイマーを停止します。
            this._Timer.Stop();
            // タイマーを開放します。
            this._Timer.Close();
            this._Timer.Dispose();
            this._Timer = null;
        }
        #endregion デストラクタ

        #region publicメソッド
        /// <summary>
        /// イベント発生の購読を開始します。
        /// </summary>
        /// <param name="eventHandler">イベントハンドラを設定します。</param>
        public void Subscribe(
            ElapsedEventHandler eventHandler
            )
        {
            if (eventHandler != null)
                this._Timer.Elapsed += eventHandler;
        }
        /// <summary>
        /// イベント発生の購読を終了します。
        /// </summary>
        /// <param name="action">イベントハンドラを設定します。</param>
        public void UnSubscribe(
            ElapsedEventHandler eventHandler
            )
        {
            if (eventHandler != null)
                this._Timer.Elapsed -= eventHandler;
        }
        /// <summary>
        /// イベント発生を開始します。
        /// </summary>
        public void Start()
        {
            if (!this.IsRunning)
                this._Timer.Enabled = true;
        }
        /// <summary>
        /// イベント発生を停止します。
        /// </summary>
        public void Stop()
        {
            if (this.IsRunning)
                this._Timer.Enabled = false;
        }
        /// <summary>
        /// イベントの発生・停止を反転します。
        /// </summary>
        public void Invert()
        {
            this._Timer.Enabled = !this._Timer.Enabled;
        }
        #endregion publicメソッド
    }
}
