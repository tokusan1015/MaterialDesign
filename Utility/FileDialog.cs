using System;
using MWAPICP = Microsoft.WindowsAPICodePack;

namespace Utility
{
    /// <summary>
    /// フォルダーダイアログを表示してパスを取得します。
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class FileDialog : IDisposable
    {
        #region プロパティ
        /// <summary>
        /// ファイルダイアログを表します。
        /// </summary>
        private MWAPICP::Dialogs.CommonOpenFileDialog _dlg { get; set; } = null;
        /// <summary>
        /// ファイル名(フルパス)を表します。
        /// </summary>
        public string FileName { get; private set; } = "./";
        #endregion プロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileDialog(
            )
        {
            this._dlg = new MWAPICP.Dialogs.CommonOpenFileDialog();
        }
        #endregion コンストラクタ

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~FileDialog()
        {
            this.Dispose(false);
        }
        /// <summary>
        /// 破棄処理
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 破棄処理
        /// 破棄フラグ）
        /// デストラクタ：false
        /// Dispose()：true, GC.SuppressFinalize(this);
        /// Dispose(bool)：破棄処理を行います。
        /// </summary>
        /// <param name="disposing">破棄フラグを設定します。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._dlg != null)
                {
                    this._dlg.Dispose();
                    this._dlg = null;
                }
            }
        }
        #endregion デストラクタ

        #region メソッド
        /// <summary>
        /// フォルダダイアログを表示します。
        /// </summary>
        /// <param name="title">タイトルを設定します。</param>
        /// <param name="initialDirectory">初期ディレクトリを設定します。</param>
        /// <param name="isFolderPicker">フォルダ選択(true)とファイル選択(false)を切り替えます。</param>
        /// <returns>成功した場合はtrueを返します。</returns>
        public bool Show(
            string title = "フォルダを選択してください",
            string initialDirectory = "./",
            bool isFolderPicker = true
            )
        {
            // フォルダ選択に設定します。(ファイル選択の場合はfalseに設定します。)
            this._dlg.IsFolderPicker = isFolderPicker;
            
            // タイトルを設定します。
            this._dlg.Title = title;

            // 初期ディレクトリを設定します。
            this._dlg.InitialDirectory = initialDirectory;

            // ダイアログを表示します。
            if (this._dlg.ShowDialog() == MWAPICP::Dialogs.CommonFileDialogResult.Ok)
            {
                // フォルダパスを保存します。
                this.FileName = this._dlg.FileName;

                return true;
            }

            return false;
        }
        #endregion メソッド
    }
}
