using Prism.Commands;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Disposables;
using System.Reflection;

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
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
    public sealed class ControlInfoAttribute : Attribute
    {
        /// <summary>
        /// タイトル
        /// </summary>
        public string DispleyName { get; private set; } = "";
        /// <summary>
        /// コマンド
        /// </summary>
        public string CommandText { get; private set; } = "";
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
        /// <param name="commandText">動作を決めるコマンドを設定します。</param>
        /// <param name="isEnable">コントロールの選択可否を設定します。</param>
        /// <param name="isVisible">コントロールの表示非表示を設定します。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public ControlInfoAttribute(
            string displeyName,
            int groupNo = 0,
            string commandText = "",
            bool isEnable = true,
            bool isVisible = true
            )
        {
            this.DispleyName = displeyName;
            this.GroupNo = groupNo;
            this.CommandText = commandText;
            this.IsEnable = isEnable;
            this.IsVisible = isVisible;
        }
    }
    #endregion ControlInfoAttribute

    /// <summary>
    /// コントロールの共通基底情報
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1012:AbstractTypesShouldNotHaveConstructors")]
    [Utility.Developer(name: "tokusan1015")]
    public abstract class ControlInfoBase
    {
        #region プロパティ
        /// <summary>
        /// プロパティ名を表します。
        /// </summary>
        public string PropertyName { get; set; } = "";
        /// <summary>
        /// タイトル
        /// </summary>
        public ReactivePropertySlim<string> Title { get; } = 
            new ReactivePropertySlim<string>();
        /// <summary>
        /// グループNo.
        /// View単位でグループ化します。
        /// </summary>
        public int GroupNo { get; set; } = -1;
        /// <summary>
        /// コマンド
        /// </summary>
        public string CommandText { get; set; } = string.Empty;
        /// <summary>
        /// 表示非表示
        /// true=表示, false=非表示
        /// </summary>
        public ReactivePropertySlim<bool> IsVisible { get; } =
            new ReactivePropertySlim<bool>();
        /// <summary>
        /// 有効無効
        /// true=有効, false=無効
        /// </summary>
        public ReactivePropertySlim<bool> IsEnable { get; } =
            new ReactivePropertySlim<bool>();
        #endregion プロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ControlInfoBase()
        {

        }
        #endregion コンストラクタ

        #region publicメソッド
        /// <summary>
        /// 対象クラスのプロパティを取得します。
        /// </summary>
        /// <param name="classType">クラスタイプを設定します。</param>
        /// <param name="propertyName">プロパティ名を設定します。</param>
        /// <returns>設定した属性インスタンスが返ります。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected object GetPropertyAttribute(
            [param: Required]Type classType,
            [param: Required]string propertyName
            )
        {
            return Utility.AttributeUtil.GetPropertyAttribute(
                attrType: typeof(ControlInfoAttribute),
                classType: classType,
                propertyName: propertyName,
                bindingAttr: Common.ConstDatas.ControlInfoBindingFlags
                );
        }

        /// <summary>
        /// 指定グループのIsEnableを反転します。
        /// グループ番号が負数の場合は無条件で反転します。
        /// </summary>
        /// <param name="groupNo">グループ番号を設定します。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public void InverseEnable(
            int groupNo = -1
            )
        {
            if (groupNo < 0 || this.GroupNo == groupNo)
                this.IsEnable.Value = !this.IsEnable.Value;
        }
        /// <summary>
        /// 指定グループのIsVisibleを反転します。
        /// </summary>
        /// <param name="groupNo">グループ番号を設定します。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public void InverseVisible(int groupNo = -1)
        {
            if (groupNo < 0 || this.GroupNo == groupNo)
                this.IsVisible.Value = !this.IsVisible.Value;
        }
        #endregion publicメソッド

        #region abstractメソッド
#if DEBUG
        /// <summary>
        /// 値を取得します。
        /// </summary>
        /// <returns>結果を文字列として返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public abstract string GetValue();
#endif
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
        #region Bindingコマンド
        /// <summary>
        /// Bindingコマンド(Click)
        /// *** publicでないと動きません。***
        /// </summary>
        public DelegateCommand Dcommand { get; set; }
        #endregion Bindingコマンド

        #region イベントハンドラ
        /// <summary>
        /// コマンドイベントハンドラ
        /// </summary>
        private event EventHandler<ControlInfoEventArgs> OnDcommand;
        /// <summary>
        /// プロパティチェンジイベントハンドラ
        /// </summary>
        private event EventHandler<ControlInfoEventArgs> OnPropertyChanged;
        #endregion イベントハンドラ

        #region プロパティ
        #endregion プロパティ

        #region Bindingプロパティ
        /// <summary>
        /// データ
        /// </summary>
        public ReactiveProperty<T> Data { get; private set; } = null;
        #endregion Bindingプロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ControlInfo()
        {
            //　nullの場合のみ生成
            if (this.Dcommand == null)
            {
                // Bindingコマンド設定
                this.Dcommand = new DelegateCommand(() =>
                {
                    // クリックイベント発行
                    this.OnDcommand?.Invoke(sender: this, e: new ControlInfoEventArgs(param: this.CommandText));
                });
            }
        }
        #endregion コンストラクタ

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
        /// <param name="viewModelType">属性を取得するクラスの型を指定します。</param>
        /// <param name="cibList">CIBListを設定します。</param>
        /// <param name="data">データを設定します。</param>
        /// <param name="onDcommand">コマンドEventHandlerを設定します。</param>
        /// <param name="onPropertyChangd">プロパティチェンジEventHandlerを設定します。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public void SetAll(
            [param: Required]Type viewModelType,
            [param: Required]List<ControlInfoBase> cibList,
            [param: Required]Utility.DataAndName<ReactiveProperty<T>> data,
            EventHandler<ControlInfoEventArgs> onDcommand = null,
            EventHandler<ControlInfoEventArgs> onPropertyChangd = null
            )
        {
            // 引数チェック
            if (cibList == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(cibList));
            if (data == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(data));

            // プロパティ名を保存します。
            this.PropertyName = data.PropertyName;
           
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
                        e: new ControlInfoEventArgs(param: this.Data.Value.ToString())
                        );
                });
            }

            // タイトルを設定します。
            this.Title.Value = (this.GetPropertyAttribute(
                    classType: viewModelType,
                    propertyName: this.PropertyName
                    ) as ControlInfoAttribute).DispleyName;

            // コマンドを設定します。
            this.CommandText = (this.GetPropertyAttribute(
                    classType: viewModelType,
                    propertyName: this.PropertyName
                    ) as ControlInfoAttribute).CommandText;

            // グループ番号を設定します。
            this.GroupNo = (this.GetPropertyAttribute(
                    classType: viewModelType,
                    propertyName: this.PropertyName
                    ) as ControlInfoAttribute).GroupNo;

            // 選択可否を設定します。
            this.IsEnable.Value = (this.GetPropertyAttribute(
                    classType: viewModelType,
                    propertyName: this.PropertyName
                    ) as ControlInfoAttribute).IsEnable;

            // 選択可否を設定します。
            this.IsVisible.Value = (this.GetPropertyAttribute(
                    classType: viewModelType,
                    propertyName: this.PropertyName
                    ) as ControlInfoAttribute).IsVisible;

            // クリックイベントを設定します。
            if (onDcommand != null) this.OnDcommand += onDcommand;

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
#if DEBUG
        /// <summary>
        /// 値を文字列として返します。
        /// エラー用
        /// </summary>
        /// <returns></returns>
        public override string GetValue()
        {
            if (this.Data != null)
            {
                if (this.Data.Value != null)
                {
                    return this.Data.Value.ToString();
                }
            }
            return string.Empty;
        }
#endif
        #endregion publicメソッド
    }
}
