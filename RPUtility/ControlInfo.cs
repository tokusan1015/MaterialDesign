using Prism.Commands;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Disposables;

namespace RPUtility
{
    /// <summary>
    /// ControlInfoEventArgs
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class ControlInfoEventArgs : EventArgs
    {
        /// <summary>
        /// オブジェクトパラメータを表します。
        /// </summary>
        public object Param { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="objectParam">オブジェクトパラメータを設定します。</param>
        public ControlInfoEventArgs(
            object param = null
            )
        {
            this.Param = param;
        }
    }

    #region ControlInfoAttribute
    /// <summary>
    /// ボタン属性を表します。
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    [AttributeUsage(
        AttributeTargets.Property,
        AllowMultiple = false,
        Inherited = false)]
    public class ControlInfoAttribute : Attribute
    {
        /// <summary>
        /// タイトル
        /// </summary>
        public string DispleyName { get; private set; } = "";
        /// <summary>
        /// コマンド
        /// </summary>
        public string Command { get; private set; } = "";
        /// <summary>
        /// グループ番号
        /// </summary>
        public int GroupNo { get; private set; } = 0;
        /// <summary>
        /// IsEnable
        /// </summary>
        public bool IsEnable { get; private set; } = true;
        /// <summary>
        /// IsVisible
        /// </summary>
        public bool IsVisible { get; private set; } = true;

        /// <summary>
        /// コンストラクタ
        /// コマンド例
        /// 画面遷移  : "Move [遷移先]"
        /// アプリ終了: "Exit"
        /// </summary>
        /// <param name="displeyName">表示するタイトルを設定します。</param>
        /// <param name="groupNo">グループ番号を設定します。</param>
        /// <param name="command">動作を決めるコマンドを設定します。</param>
        /// <param name="isEnable">コントロールの選択可否を設定します。</param>
        /// <param name="isVisible">コントロールの表示非表示を設定します。</param>
        public ControlInfoAttribute(
            string displeyName,
            int groupNo = 0,
            string command = "",
            bool isEnable = true,
            bool isVisible = true
            )
        {
            this.DispleyName = displeyName;
            this.GroupNo = groupNo;
            this.Command = command;
            this.IsEnable = isEnable;
            this.IsVisible = isVisible;
        }
    }
    #endregion ControlInfoAttribute

    /// <summary>
    /// コントロールの共通基底情報
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public abstract class ControlInfoBase
    {
        #region イベントハンドラ
        /// <summary>
        /// クリックイベントハンドラ
        /// </summary>
        protected event EventHandler<ControlInfoEventArgs> OnClicked;
        #endregion イベントハンドラ

        #region Bindingコマンド
        /// <summary>
        /// Bindingコマンド(Click)
        /// </summary>
        public DelegateCommand Clicked { get; protected set; }
        #endregion Bindingコマンド

        #region プロパティ
        /// <summary>
        /// グループNo.
        /// View単位でグループ化します。
        /// </summary>
        public int GroupNo { get; protected set; } = -1;
        /// <summary>
        /// コマンド
        /// </summary>
        public string Command { get; protected set; } = string.Empty;
        /// <summary>
        /// 表示非表示
        /// true=表示, false=非表示
        /// </summary>
        public ReactivePropertySlim<bool> IsVisible { get; private set; } = null;
        /// <summary>
        /// 有効無効
        /// true=有効, false=無効
        /// </summary>
        public ReactivePropertySlim<bool> IsEnable { get; private set; } = null;
        #endregion プロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ControlInfoBase()
        {
            this.IsEnable = new ReactivePropertySlim<bool>();
            this.IsVisible = new ReactivePropertySlim<bool>();

            // Bindingコマンド設定
            this.Clicked = new DelegateCommand(() =>
            {
                // クリックイベント発行
                this.OnClicked?.Invoke(sender: this, e: new ControlInfoEventArgs(param: this.Command));
            });
        }
        #endregion コンストラクタ

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~ControlInfoBase()
        {
            if (this.IsEnable != null)
            {
                this.IsEnable.Dispose();
                this.IsEnable = null;
            }
            if (this.IsVisible != null)
            {
                this.IsVisible.Dispose();
                this.IsVisible = null;
            }
        }
        #endregion デストラクタ

        #region publicメソッド
        /// <summary>
        /// 指定グループのIsEnableを反転します。
        /// グループ番号が負数の場合は無条件で反転します。
        /// </summary>
        /// <param name="groupNo">グループ番号を設定します。</param>
        public void InverseEnable(int groupNo = -1)
        {
            if (groupNo < 0 || this.GroupNo == groupNo)
                this.IsEnable.Value = !this.IsEnable.Value;
        }
        /// <summary>
        /// 指定グループのIsVisibleを反転します。
        /// </summary>
        /// <param name="groupNo">グループ番号を設定します。</param>
        public void InverseVisible(int groupNo = -1)
        {
            if (groupNo < 0 || this.GroupNo == groupNo)
                this.IsVisible.Value = !this.IsVisible.Value;
        }
        #endregion publicメソッド

        #region abstractメソッド
        /// <summary>
        /// 入力エラーが存在する場合trueを返します。
        /// 必要であるならば検証項目を追加します。
        /// </summary>
        /// <returns>エラーが存在する場合trueを返します。</returns>
        public abstract bool HasError();

        #endregion abstractメソッド
    }

    /// <summary>
    /// コントロールの共通情報
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public sealed class ControlInfo<T> : ControlInfoBase
    {
        #region イベントハンドラ
        /// <summary>
        /// プロパティチェンジイベントハンドラ
        /// </summary>
        public event EventHandler<ControlInfoEventArgs> OnPropertyChanged;
        #endregion イベントハンドラ

        #region Bindingプロパティ
        /// <summary>
        /// データ
        /// </summary>
        public ReactiveProperty<T> Data { get; private set; } = null;
        /// <summary>
        /// タイトル
        /// </summary>
        public ReactivePropertySlim<string> Title { get; private set; } = null;
        #endregion Bindingプロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ControlInfo()
        {
            this.Title = new ReactivePropertySlim<string>();
        }
        #endregion コンストラクタ

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~ControlInfo()
        {
            this.Dispose_RP();
            GC.SuppressFinalize(obj: this);
        }
        /// <summary>
        /// 破棄処理
        /// </summary>
        private void Dispose_RP()
        {
            if (this.Data != null)
            {
                this.Data.Dispose();
                this.Data = null;
            }
            if (this.Title != null)
            {
                this.Title.Dispose();
                this.Title = null;
            }
        }
        #endregion デストラクタ

        #region publicメソッド
        /// <summary>
        /// 全パラメータを設定します。
        /// dataへの設定方法)
        /// data: new DataAndName()
        /// { 
        ///     Data = プロパティ,
        ///     PropertyName = Utility.ObjectUtil.GetPropertyName(() => プロパティ)
        /// }
        /// タイトル)
        /// dataのDisplayName属性のTitleが設定されます。
        /// cibListは、自動的に追加されます。
        /// </summary>
        /// <typeparam name="TViewModel">属性を取得する型を指定します。</typeparam>
        /// <param name="cibList">CIBListを設定します。</param>
        /// <param name="data">データを設定します。</param>
        /// <param name="onClicked">クリックEventHandlerを設定します。</param>
        /// <param name="onPropertyChangd">プロパティチェンジEventHandlerを設定します。</param>
        /// <param name="isEnable">有効無効を設定します。</param>
        /// <param name="isVisible">表示非表示を設定します。</param>
        public void SetAll<TViewModel>(
            [param: Required]List<ControlInfoBase> cibList,
            [param: Required]Utility.DataAndName<ReactiveProperty<T>> data,
            EventHandler<ControlInfoEventArgs> onClicked = null,
            EventHandler<ControlInfoEventArgs> onPropertyChangd = null,
            bool isEnable = true,
            bool isVisible = true
            ) where TViewModel : class
        {
            // 引数チェック
            if (cibList == null || data == null)
                throw new ArgumentNullException("cibListまたはdataは、nullにすることはできません。");

            if (data.Data != null)
            {
                // プロパティ設定
                this.Data = data.Data;
                // プロパティチェンジイベント購読
                this.Data.Subscribe(_ =>
                {
                    // プロパティチェンジイベント発行
                    this.OnPropertyChanged?.Invoke(
                        sender: this,
                        e: new ControlInfoEventArgs(param: this.Data.ToString())
                        );
                });
            }

            // タイトルを設定します。
            this.Title.Value = Utility.AttributeUtil
                .GetPropertyAttribute<ControlInfoAttribute>(
                    classType: typeof(TViewModel),
                    propertyName: data.PropertyName
                    ).DispleyName;

            // コマンドを設定します。
            this.Command = Utility.AttributeUtil
                .GetPropertyAttribute<ControlInfoAttribute>(
                    classType: typeof(TViewModel),
                    propertyName: data.PropertyName
                    ).Command;

            // グループ番号を設定します。
            this.GroupNo = Utility.AttributeUtil
                .GetPropertyAttribute<ControlInfoAttribute>(
                    classType: typeof(TViewModel),
                    propertyName: data.PropertyName
                    ).GroupNo;

            // 選択可否を設定します。
            this.IsEnable.Value = Utility.AttributeUtil
                .GetPropertyAttribute<ControlInfoAttribute>(
                    classType: typeof(TViewModel),
                    propertyName: data.PropertyName
                    ).IsEnable;

            // 選択可否を設定します。
            this.IsVisible.Value = Utility.AttributeUtil
                .GetPropertyAttribute<ControlInfoAttribute>(
                    classType: typeof(TViewModel),
                    propertyName: data.PropertyName
                    ).IsVisible;

            // クリックイベントを設定します。
            if (onClicked != null) this.OnClicked += onClicked;

            // プロパティ変更イベントを設定します。
            if (onPropertyChangd != null) this.OnPropertyChanged += onPropertyChangd;

            // コントロールリストを設定します。
            if (cibList != null) cibList.Add(this);
        }
        /// <summary>
        /// 入力エラーが存在する場合trueを返します。
        /// 必要であるならば検証項目を追加します。
        /// </summary>
        /// <returns>エラーが存在する場合trueを返します。</returns>
        public override bool HasError()
        {
            return this.Data != null ? this.Data.HasErrors : false;
        }
        #endregion publicメソッド
    }
}
