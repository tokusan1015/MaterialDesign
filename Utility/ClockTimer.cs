using System;
using System.Timers;

namespace Utility
{
    /// <summary>
    /// クロックタイマー
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class ClockTimer : IDisposable
    {
        #region privateプロパティ
        /// <summary>
        /// タイマー
        /// </summary>
        private Timer _Timer { get; set; } = new Timer(interval: 1000);
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
        public ClockTimer()
        {
            // イベント発生を停止します。
            this._Timer.Enabled = false;
        }
        #endregion コンストラクタ

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
        public void Unsubscribe(
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

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "<_Timer>k__BackingField")]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)。
                    // タイマーを停止します。
                    this._Timer.Stop();
                    // タイマーを開放します。
                    this._Timer.Close();
                    this._Timer.Dispose();
                    this._Timer = null;
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~ClockTimer() {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
