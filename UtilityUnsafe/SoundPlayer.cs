using System.Timers;
using WMPLib;

namespace Utility
{
    /// <summary>
    /// WindowsMediaPlayerラップクラス
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class SoundPlayer
    {
        /// <summary>
        /// 内部ステータスを表します。
        /// </summary>
        public enum EnumStatus
        {
            UnKnown,
            Ready,
            Play,
            Stop,
        }

        /// <summary>
        /// メディアプレイヤーを表します。
        /// </summary>
        private readonly WindowsMediaPlayer _mediaPlayer = new WindowsMediaPlayer();

        /// <summary>
        /// タイマーを表します。
        /// </summary>
        private readonly Timer _timer = new Timer();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="soundFile">サウンドファイルパスを設定します。</param>
        /// <param name="loop">繰り返しフラグを設定します。</param>
        public SoundPlayer(
            string soundFile = "",
            bool loop = false
            )
        {
            // イベント追加
            this._timer.Stop();
            this._timer.Interval = 300;
            this._timer.Elapsed += OnTimer;

            // ステータス変更
            this.Status = EnumStatus.Ready;

            // サウンドファイル読込
            if (soundFile.Length > 0)
                this.SetSound(soundFile: soundFile);
            this.Loop = loop;
        }


        #region イベント
        /// <summary>
        /// タイマーイベントを受け取ります。
        /// </summary>
        /// <param name="sender">呼び出し元を設定します。</param>
        /// <param name="e">イベントパラメタを設定します。</param>
        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            if (this._mediaPlayer.playState == WMPPlayState.wmppsStopped
                && this.Status == EnumStatus.Play
                && this.Loop
                )
            {
                this._mediaPlayer.controls.play();
            }
        }
        #endregion イベント

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~SoundPlayer()
        {
            this._mediaPlayer.controls.stop();
        }

        #region プロパティ
        /// <summary>
        /// ステータス
        /// </summary>
        public EnumStatus Status { get; private set; } = EnumStatus.UnKnown;

        /// <summary>
        /// プレイヤーステータス
        /// </summary>
        public string MPStatus
        {
            get
            {
                return this._mediaPlayer.status;
            }
        }

        /// <summary>
        /// ループ再生
        /// </summary>
        public bool Loop { get; set; } = false;

        /// <summary>
        /// ボリューム
        /// </summary>
        public int Volume
        {
            get { return this._mediaPlayer.settings.volume; }
            set { this._mediaPlayer.settings.volume = value; }
        }

        /// <summary>
        /// ミュート
        /// </summary>
        public bool Mute
        {
            get { return this._mediaPlayer.settings.mute; }
            set { this._mediaPlayer.settings.mute = value; }
        }

        /// <summary>
        /// シーク
        /// </summary>
        public double Seek
        {
            get { return this._mediaPlayer.controls.currentPosition; }
            set { this._mediaPlayer.controls.currentPosition = value; }
        }

        /// <summary>
        /// 再生時間
        /// ステータスが再生中になると取得可能です。
        /// </summary>
        public double Duration
        {
            get { return this._mediaPlayer.currentMedia.duration; }
        }
        #endregion プロパティ

        #region publid メソッド
        /// <summary>
        /// 再生するサウンドファイルを設定します。
        /// </summary>
        /// <param name="soundFile">サウンドファイルのパスを設定します。</param>
        public void SetSound(
            string soundFile
            )
        {
            this._mediaPlayer.URL = soundFile;
            this.Stop();
        }
        /// <summary>
        /// サウンドの再生を開始します。
        /// </summary>
        public void Play()
        {
            this._mediaPlayer.controls.play();
            this._timer.Start();
            this.Status = EnumStatus.Play;
        }

        /// <summary>
        /// サウンドの再生を停止します。
        /// </summary>
        public void Stop()
        {
            this.Loop = false;
            this._timer.Stop();
            this._mediaPlayer.controls.stop();
            this.Status = EnumStatus.Stop;
        }

        /// <summary>
        /// サウンドの再生を一時停止します。
        /// </summary>
        public void Pause()
        {
            this._timer.Stop();
            this._mediaPlayer.controls.pause();
            this.Status = EnumStatus.Stop;
        }
        #endregion public メソッド
    }
}
