using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Disposables;

namespace Common
{
    /// <summary>
    /// 全Viewでの共通データを表します。
    /// *******************************************************
    /// View,ViewModelでのプログラム上でのデータ編集は禁止です。
    /// *******************************************************
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public sealed class CommonDatas : IDisposable
    {
        #region Common
        /// <summary>
        /// アクティブViewを表します。
        /// </summary>
        public RPUtility.BindableBasePlus ActiveView { get; private set; } = null;
        #endregion Common

        #region ItemsSource
        /// <summary>
        /// 国名コンボボックス(ItemsSource)
        /// </summary>
        public Dictionary<string, string> CountryDic { get; private set; } = new Dictionary<string, string>();
        /// <summary>
        /// 破棄処理用
        /// </summary>
        private CompositeDisposable Disposable { get; set; } = new CompositeDisposable();
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
        public CommonDatas()
        {
            try
            {
                this.Initialize();
            }
            catch (Exception)
            {
                this.Dispose();
            }
        }
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>Disposableに追加されたオブジェクト数を返します。</returns>
        private int Initialize()
        {
            // Validation設定
            // 1)名前
            this.LastName = new ReactiveProperty<string>()
                //.SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? ConstDatas.InputRequired : null)    // 空文字の時はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.LastName);
            this.Disposable.Add(this.LastName);

            // 2)苗字
            this.FirstName = new ReactiveProperty<string>()
                //.SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? ConstDatas.InputRequired : null)    // 空文字の時はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.FirstName);
            this.Disposable.Add(this.FirstName);

            // 3)誕生日
            this.Birthday = new ReactiveProperty<DateTime?>()
                .SetValidateNotifyError(x => x == null ? ConstDatas.InputRequired : null)                         // nullの場合はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.Birthday);
            this.Disposable.Add(this.Birthday);

            // 4)性別
            this.Gender = new ReactiveProperty<EnumDatas.Gender>()
                .SetValidateAttribute(() => this.Gender);
            this.Disposable.Add(this.Gender);

            // 5)国籍
            this.Country = new ReactiveProperty<string>()
                .SetValidateAttribute(() => this.Country);
            this.Disposable.Add(this.Country);

            // 6)郵便番号
            this.ZipCode = new ReactiveProperty<string>()
                //.SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? ConstDatas.InputRequired : null)    // 空文字の時はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.ZipCode);
            this.Disposable.Add(this.ZipCode);

            // 7)都道府県
            this.Prefectures = new ReactiveProperty<string>()
                //.SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? ConstDatas.InputRequired : null)    // 空文字の時はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.Prefectures);
            this.Disposable.Add(this.Prefectures);

            // 8)市区町村
            this.Municipality = new ReactiveProperty<string>()
                //.SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? ConstDatas.InputRequired : null)    // 空文字の時はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.Municipality);
            this.Disposable.Add(this.Municipality);

            // 9)番地
            this.HouseNumber = new ReactiveProperty<string>()
                //.SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? ConstDatas.InputRequired : null)    // 空文字の時はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.HouseNumber);
            this.Disposable.Add(this.HouseNumber);

            // 10)保存パス
            this.SavePath = new ReactiveProperty<string>()
                //.SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? ConstDatas.InputRequired : null)    // 空文字の時はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.SavePath);
            this.Disposable.Add(this.SavePath);

            // 11)備考
            this.Etc = new ReactiveProperty<string>()
                //.SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? ConstDatas.InputRequired : null)    // 空文字の時はエラーにします。(CommonDatasで設定)
                .SetValidateAttribute(() => this.Etc);
            this.Disposable.Add(this.Etc);

            return this.Disposable.Count;
        }
        #endregion コンストラクタ

        #region 破棄処理
        /// <summary>
        /// 破棄処理
        /// </summary>
        private void Dispose_RP()
        {
            if (this.Disposable != null)
            {
                this.Disposable.Dispose();
                this.Disposable = null;
            }
            if (this.ActiveView != null)
            {
                this.ActiveView = null;
            }
        }
        #endregion 破棄処理

        #region メソッド
        /// <summary>
        /// アクティブViewを設定します。
        /// </summary>
        /// <param name="bindableBasePlus">対象のアクティブViewを設定します。</param>
        public void SetActiveView(
            RPUtility.BindableBasePlus bindableBasePlus
            )
        {
            this.ActiveView = bindableBasePlus;
        }
        #endregion メソッド

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
                    this.Dispose_RP();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~CommonDatas() {
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

    /// <summary>
    /// コンボボックス用Item
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class ComboItem
    {
        /// <summary>
        /// Idを表します。
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Keyを表します。
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Valueを表します。
        /// </summary>
        public string Value { get; set; }
    }
}