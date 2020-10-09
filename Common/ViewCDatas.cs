using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Disposables;

namespace Common
{
    /// <summary>
    /// ViewC用データを表します。
    /// *******************************************************
    /// View,ViewModelでのプログラム上でのデータ編集は禁止です。
    /// *******************************************************
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public sealed class ViewCdatas : BindableBase, IDisposable
    {
        #region Disposable
        /// <summary>
        /// 破棄処理用
        /// </summary>
        private CompositeDisposable Disposable { get; set; } = new CompositeDisposable();
        #endregion Disposable

        #region Key
        /// <summary>
        /// データのKeyを表します。
        /// Keyを使用してDBとのやり取りを行います。
        /// </summary>
        public int Key { get; set; } = 0;
        #endregion Key

        #region ViewC(設定)
        /// <summary>
        /// 保存パス
        /// </summary>
        [DisplayName("保存パス")]
        [Required(ErrorMessage = "入力必須です。")]
        public ReactiveProperty<string> SavePath { get; private set; }
        /// <summary>
        /// 備考
        /// </summary>
        [DisplayName("備考")]
        [RegularExpression(@"[^\x01-\x7E]{0,20}",
            ErrorMessage = "全角文字０～２０文字以内で入力してください。")]
        public ReactiveProperty<string> Etc { get; private set; }
        #endregion ViewC(設定)

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ViewCdatas()
        {
            this.Initialize();
        }
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>Disposableに追加されたオブジェクト数を返します。</returns>
        private int Initialize()
        {
            // Validation設定
            // 10)保存パス
            this.SavePath = new ReactiveProperty<string>(mode: Common.ConstDatas.DefaultreactivePropertyMode)
                //.SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? ConstDatas.InputRequired : null)    // 空文字の時はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.SavePath);
            this.Disposable.Add(this.SavePath);

            // 11)備考
            this.Etc = new ReactiveProperty<string>(mode: Common.ConstDatas.DefaultreactivePropertyMode)
                //.SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? ConstDatas.InputRequired : null)    // 空文字の時はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.Etc);
            this.Disposable.Add(this.Etc);

            return this.Disposable.Count;
        }
        #endregion コンストラクタ

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)。
                    this.Disposable.Dispose();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~ViewCdatas() {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}