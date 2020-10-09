using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Utility
{
    /// <summary>
    /// サウンドプレイヤー
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public sealed class SoundPlayerEx
    {
        /// <summary>
        /// ステータス
        /// </summary>
        public enum Mode
        {
            Unknown,    // 不明
            Stoped,     // 停止中
            Playing,    // 再生中
        }

        /// <summary>
        /// サウンドプレイヤーオブジェクト
        /// </summary>
        public SoundPlayer SoundPlayer { get; private set; } = null;

        /// <summary>
        /// サウンドファイル名
        /// </summary>
        public string SoundFileName { get; private set; } = "./default.wav";

        /// <summary>
        /// ステータス
        /// </summary>
        public Mode Status { get; private set; } = SoundPlayerEx.Mode.Unknown;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="soundFileName">再生するサウンドファイル(.wav)を設定します。</param>
        public SoundPlayerEx(
            string soundFileName
            )
        {
            this.SoundFileName = soundFileName;
        }

        /// <summary>
        /// サウンドファイルを開きます。
        /// </summary>
        public void OpenSoundFile(
            )
        {
            if (this.SoundPlayer == null)
            {
                this.SoundPlayer = new SoundPlayer(
                    soundLocation: this.SoundFileName
                    );
                this.Status = Mode.Stoped;
            }
            else
            {
                new SoundPlayerMessage().Show(
                    enumMessage: SoundPlayerMessage.EnumMessage.ファイルが開かれていません
                    );
            }
        }

        /// <summary>
        /// サウンドファイルを再生します。
        /// </summary>
        public void Play()
        {
            // モード変更チェック
            if (this.CheckMode(Mode.Playing))
            {
                this.SoundPlayer.Play();
                this.Status = Mode.Playing;
            }
        }

        /// <summary>
        /// サウンドファイルをループ再生します。
        /// </summary>
        public void PlayLoop()
        {
            // モード変更チェック
            if (this.CheckMode(Mode.Playing))
            {
                // サウンドファイルを再生します。
                this.SoundPlayer.PlayLooping();
                this.Status = Mode.Playing;
            }
        }

        /// <summary>
        /// サウンドファイルを停止します。
        /// </summary>
        public void Stop()
        {
            // モード変更チェック
            if (this.CheckMode(Mode.Stoped))
            {
                // サウンドファイルを再生します。
                this.SoundPlayer.Stop();
                this.Status = Mode.Stoped;
            }
        }

        /// <summary>
        /// モードチェックを行います。
        /// 問題がある場合はメッセージを表示します。
        /// </summary>
        /// <param name="mode">処理モードを設定します。</param>
        /// <returns>問題がない場合はtrueが返ります。</returns>
        private bool CheckMode(
            Mode mode
            )
        {
            // SoundPlayerのnullチェック
            if (this.SoundPlayer == null)
            {
                new SoundPlayerMessage().Show(
                    enumMessage: SoundPlayerMessage.EnumMessage.ファイルが開かれていません
                    );
                return false;
            }

            // 再生チェック
            if (mode == Mode.Playing && this.Status == Mode.Playing)
            {
                new SoundPlayerMessage().Show(
                    enumMessage: SoundPlayerMessage.EnumMessage.再生中です
                    );
                return false;
            }

            // 停止チェック
            if (mode == Mode.Stoped && this.Status == Mode.Stoped)
            {
                new SoundPlayerMessage().Show(
                    enumMessage: SoundPlayerMessage.EnumMessage.停止中です
                    );
                return false;
            }

            return true;
        }

        /// <summary>
        /// 破棄処理
        /// </summary>
        ~SoundPlayerEx()
        {
            // 破棄処理を実行します。
            if (this.SoundPlayer != null)
            {
                this.SoundPlayer.Stop();
                this.SoundPlayer.Dispose();
                this.SoundPlayer = null;
            }
        }
    }

    /// <summary>
    /// メッセージを表示するクラスを表します。
    /// </summary>
    public class SoundPlayerMessage : Utility.MessageBoxBase
    {
        /// <summary>
        /// メッセージ
        /// </summary>
        public enum EnumMessage
        {
            ファイルが開かれていません,
            再生中です,
            停止中です,
        }

        /// <summary>
        /// ユーザーメッセージを表示します。
        /// </summary>
        /// <param name="enumMessage">メッセージを設定します。</param>
        /// <returns>MessageBoxResultを返します。</returns>
        public MessageBoxResult Show(
            EnumMessage enumMessage
            )
        {
            return this.ShowUserMessage<EnumMessage>(enumMessage: enumMessage);
        }
    }
}
