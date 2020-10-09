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
    /// ViewA用のデータを表します。
    /// *******************************************************
    /// View,ViewModelでのプログラム上でのデータ編集は禁止です。
    /// *******************************************************
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public sealed class ViewAdatas : BindableBase, IDisposable
    {
        #region Disposable
        /// <summary>
        /// ReactiveProperty破棄処理用
        /// </summary>
        private CompositeDisposable Disposable { get; set; } = new CompositeDisposable();
        #endregion Disposable

        #region ItemsSource
        /// <summary>
        /// 国名コンボボックス(ItemsSource)
        /// </summary>
        public IReadOnlyDictionary<string, string> CountryDic { get; set; } = null;
        #endregion ItemsSource

        #region Key
        /// <summary>
        /// データのKeyを表します。
        /// Keyを使用してDBとのやり取りを行います。
        /// </summary>
        public int Key { get; set; } = 0;
        #endregion Key

        #region ViewA(氏名)
        /// <summary>
        /// 苗字
        /// </summary>
        [DisplayName("苗字")]
        [Required(ErrorMessage = "入力必須項目です。")]
        [RegularExpression(@"[^\x01-\x7E]{1,5}",
            ErrorMessage = "全角文字１～５文字以内で入力してください。")]
        public ReactiveProperty<string> LastName { get; private set; }
        /// <summary>
        /// 名前
        /// </summary>
        [DisplayName("名前")]
        [Required(ErrorMessage = "入力必須項目です。")]
        [RegularExpression(@"[^\x01-\x7E]{1,5}",
            ErrorMessage = "全角文字１～５文字以内で入力してください。")]
        public ReactiveProperty<string> FirstName { get; private set; }
        /// <summary>
        /// 生年月日
        /// Nullチェックはコンストラクタで設定しています。
        /// DatePickerで検証実施済みの為、追加検証はしません。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [DisplayName("誕生日")]
        public ReactiveProperty<DateTime?> Birthday { get; private set; }
        /// <summary>
        /// 性別
        /// </summary>
        [DisplayName("性別")]
        public ReactiveProperty<EnumDatas.Gender> Gender { get; private set; }
        /// <summary>
        /// 国籍
        /// </summary>
        [DisplayName("国籍")]
        [Required(ErrorMessage = "入力必須項目です。")]
        public ReactiveProperty<string> Country { get; private set; }
        #endregion ViewA(氏名)

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ViewAdatas()
        {
            // 初期化処理
            this.Initialize();
        }
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>Disposableに追加されたオブジェクト数を返します。</returns>
        private int Initialize()
        {
            // Validation設定
            // 1)名前
            this.LastName = new ReactiveProperty<string>(mode: Common.ConstDatas.DefaultreactivePropertyMode)
                //.SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? ConstDatas.InputRequired : null)    // 空文字の時はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.LastName)
                .AddTo(this.Disposable);

            // 2)苗字
            this.FirstName = new ReactiveProperty<string>(mode: Common.ConstDatas.DefaultreactivePropertyMode)
                //.SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? ConstDatas.InputRequired : null)    // 空文字の時はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.FirstName)
                .AddTo(this.Disposable);

            // 3)誕生日
            this.Birthday = new ReactiveProperty<DateTime?>(mode: Common.ConstDatas.DefaultreactivePropertyMode)
                .SetValidateNotifyError(x => x == null ? ConstDatas.InputRequired : null)                         // nullの場合はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.Birthday)
                .AddTo(this.Disposable);

            // 4)性別
            this.Gender = new ReactiveProperty<EnumDatas.Gender>(mode: Common.ConstDatas.DefaultreactivePropertyMode)
                .SetValidateAttribute(() => this.Gender)
                .AddTo(this.Disposable);

            // 5)国籍
            this.Country = new ReactiveProperty<string>(mode: Common.ConstDatas.DefaultreactivePropertyMode)
                .SetValidateAttribute(() => this.Country)
                .AddTo(this.Disposable);

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
        // ~ViewAdatas() {
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