using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Disposables;

namespace Common
{
    /// <summary>
    /// ViewB用のデータを表します。
    /// *******************************************************
    /// View,ViewModelでのプログラム上でのデータ編集は禁止です。
    /// *******************************************************
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public sealed class ViewBdatas : BindableBase, IDisposable
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

        #region ViewB(住所)
        /// <summary>
        /// 郵便番号
        /// </summary>
        [DisplayName("郵便番号")]
        [Required(ErrorMessage = "入力必須項目です。")]
        [RegularExpression(@"[0-9]{7}",
            ErrorMessage = "ハイフン(-)を除いた半角数字７文字で入力してください。")]
        public ReactiveProperty<string> ZipCode { get; private set; }
        /// <summary>
        /// 都道府県
        /// </summary>
        [DisplayName("都道府県")]
        [Required(ErrorMessage = "入力必須項目です。")]
        [RegularExpression(@"[^\x01-\x7E]{1,5}",
            ErrorMessage = "全角文字１～５文字以内で入力してください。")]
        public ReactiveProperty<string> Prefectures { get; private set; }
        /// <summary>
        /// 市区町村
        /// </summary>
        [DisplayName("市区町村")]
        [Required(ErrorMessage = "入力必須項目です。")]
        [RegularExpression(@"[^\x01-\x7E]{1,10}",
            ErrorMessage = "全角文字１～１０文字以内で入力してください。")]
        public ReactiveProperty<string> Municipality { get; private set; }
        /// <summary>
        /// 番地
        /// </summary>
        [DisplayName("番地")]
        [Required(ErrorMessage = "入力必須項目です。")]
        [RegularExpression(@"[^\x01-\x7E]{1,20}",
            ErrorMessage = "全角文字２０文字以内で入力してください。")]
        public ReactiveProperty<string> HouseNumber { get; private set; }
        #endregion ViewB(住所)

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ViewBdatas()
        {
            this.Initialize();
        }
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>Disposableに追加されたオブジェクト数を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄")]
        private int Initialize()
        {
            // 6)郵便番号
            this.ZipCode = new ReactiveProperty<string>(mode: Common.ConstDatas.DefaultreactivePropertyMode)
                //.SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? ConstDatas.InputRequired : null)    // 空文字の時はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.ZipCode)
                .AddTo(this.Disposable);

            // 7)都道府県
            this.Prefectures = new ReactiveProperty<string>(mode: Common.ConstDatas.DefaultreactivePropertyMode)
                //.SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? ConstDatas.InputRequired : null)    // 空文字の時はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.Prefectures)
                .AddTo(this.Disposable);

            // 8)市区町村
            this.Municipality = new ReactiveProperty<string>(mode: Common.ConstDatas.DefaultreactivePropertyMode)
                //.SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? ConstDatas.InputRequired : null)    // 空文字の時はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.Municipality)
                .AddTo(this.Disposable);

            // 9)番地
            this.HouseNumber = new ReactiveProperty<string>(mode: Common.ConstDatas.DefaultreactivePropertyMode)
                //.SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? ConstDatas.InputRequired : null)    // 空文字の時はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.HouseNumber)
                .AddTo(this.Disposable);

            return this.Disposable.Count;
        }
        #endregion コンストラクタ

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "<Disposable>k__BackingField")]
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
        // ~ViewBdatas() {
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