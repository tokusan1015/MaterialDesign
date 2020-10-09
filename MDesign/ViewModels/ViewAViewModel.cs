using Common;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using Reactive.Bindings;
using RPUtility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MaterialDesignViews.ViewModels
{
    /// <summary>
    /// ViewA用ViewModel
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class ViewAViewModel : RPUtility.BindableBasePlus
    {
        #region ViewDatas
        /// <summary>
        /// ViewDatas
        /// </summary>
        private Common.ViewAdatas ViewDatas { get; } = new Common.ViewAdatas();
        #endregion ViewDatas

        #region Bindingプロパティ
        /// <summary>
        /// 苗字
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "苗字"
            , groupNo: 1
            , commandText: "Click LastName"
            , isEnable: true
            , isVisible: true
            )]
        public RPUtility.ControlInfo<string> LastName { get; } = new RPUtility.ControlInfo<string>();
        /// <summary>
        /// 名前
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "名前"
            , groupNo: 1
            , commandText: "Click FirstName"
            , isEnable: true
            , isVisible: true
            )]
        public RPUtility.ControlInfo<string> FirstName { get; } = new RPUtility.ControlInfo<string>();
        /// <summary>
        /// 年齢
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "年齢"
            , groupNo: 2
            , commandText: "Click Birthday"
            , isEnable: true
            , isVisible: true
            )]
        public RPUtility.ControlInfo<DateTime?> Birthday { get; } = new RPUtility.ControlInfo<DateTime?>();
        /// <summary>
        /// 性別
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "性別"
            , groupNo: 3
            , commandText: "Click Gender"
            , isEnable: true
            , isVisible: true
            )]
        public RPUtility.ControlInfo<EnumDatas.Gender> Gender { get; } = new RPUtility.ControlInfo<EnumDatas.Gender>();
        /// <summary>
        /// 国籍
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "国籍"
            , groupNo: 4
            , commandText: "Click Country"
            , isEnable: true
            , isVisible: true
            )]
        public RPUtility.ControlInfo<string> Country { get; } = new ControlInfo<string>();
        /// <summary>
        /// 国籍コンボボックスItemsSource
        /// </summary>
        private IReadOnlyDictionary<string, string> _CountryDic = null;
        public IReadOnlyDictionary<string, string> CountryDic
        {
            get { return this._CountryDic; }
            set
            {
                if (this._CountryDic != value)
                    SetProperty(ref this._CountryDic, value);
            }
        }
        #endregion Bindingプロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// 引数を追加することによりPrismに対して各インスタンスを要求しています。
        /// CommonDatasは、App.xaml.csのRegisterTypesにて登録されます。
        /// </summary>
        /// <param name="container">拡張コンテナを設定します。</param>
        /// <param name="regionManager">リージョンマネージャを設定します。</param>
        /// <param name="eventAggregator">イベントアグリゲータを設定します。</param>
        /// <param name="commonDatas">共通データを設定します。</param>
        public ViewAViewModel(
            [param: Required]IContainerExtension container,
            [param: Required]IRegionManager regionManager,
            [param: Required]IEventAggregator eventAggregator,
            [param: Required]Common.CommonDatas commonDatas
            ) : base(
                container: container, 
                regionManager: regionManager, 
                eventAggregator: eventAggregator,
                commonDatas: commonDatas,
                // MainViewName, ViewNameを設定します。
                mainViewName: EnumDatas.ViewNames.Main.ToString(),
                viewName: EnumDatas.ViewNames.ViewA.ToString()
                )
        {
            // 初期化処理を行います。
            this.Initialize();
        }
        /// <summary>
        /// 初期化処理を表します。
        /// </summary>
        private void Initialize()
        {
            // コンポーネントの初期化処理を行います。
            this.InitializeComponent();
        }
        /// <summary>
        /// コンポーネントの初期化処理を行います。
        /// </summary>
        private void InitializeComponent()
        {
            // ViewIdを設定します。
            this.ViewId = (int)Common.EnumDatas.ViewNames.ViewA;

            // リストをクリア
            this.CibList.Clear();

            // 共通データを受け取ります。
            var vd = this.ViewDatas;

            // 自身のタイプ取得
            var thisType = this.GetType();

            // テキストボックス情報を設定します。
            this.LastName.SetAll(
                viewModelType: thisType,
                cibList: this.CibList,
                onPropertyChangd: this.OnPropertyChanged,
                data: new Utility.DataAndName<ReactiveProperty<string>>()
                {
                    Data = vd.LastName,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => vd.LastName)
                });
            this.FirstName.SetAll(
                viewModelType: thisType,
                cibList: this.CibList,
                onPropertyChangd: this.OnPropertyChanged,
                data: new Utility.DataAndName<ReactiveProperty<string>>()
                {
                    Data = vd.FirstName,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => vd.FirstName)
                });
            this.Birthday.SetAll(
                viewModelType: thisType,
                cibList: this.CibList,
                onPropertyChangd: this.OnPropertyChanged,
                data: new Utility.DataAndName<ReactiveProperty<DateTime?>>()
                {
                    Data = vd.Birthday,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => vd.Birthday)
                });
            this.Gender.SetAll(
                viewModelType: thisType,
                cibList: this.CibList,
                onPropertyChangd: this.OnRadioButtonChanged,
                data: new Utility.DataAndName<ReactiveProperty<EnumDatas.Gender>>()
                {
                    Data = vd.Gender,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => vd.Gender)
                });
            this.Country.SetAll(
                viewModelType: thisType,
                cibList: this.CibList,
                onPropertyChangd: this.OnComboChanged,
                data: new Utility.DataAndName<ReactiveProperty<string>>()
                {
                    Data = vd.Country,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => vd.Country)
                });
        }
        #endregion コンストラクタ

        #region Loaded
        /// <summary>
        /// ロード完了イベント
        /// </summary>
        protected override void Loaded()
        {
            // CommonDataから読み込みます。
            this.CommonDatas.GetViewDatas(this.ViewDatas);

            // データソース設定
            this.CountryDic = this.ViewDatas.CountryDic;

            // メッセージ送受信開始
            this.MessageManager.Start = true;

            // 初期化完了フラグ設定
            this.InitiazaizuEnd = true;
        }
        #endregion Loaded

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~ViewAViewModel()
        {
            this.CibList.Clear();
            this.ViewDatas.Dispose();
        }
        #endregion デストラクタ

        #region メソッド
        /// <summary>
        /// CommonDatasにデータを保存します。
        /// </summary>
        protected override void SaveCommonDatas()
        {
            // CommonDatasにデータを保存します。
            this.CommonDatas.SetViewDatas(this.ViewDatas);
        }
        #endregion メソッド

        #region イベント
        /// <summary>
        /// メッセージ受信イベント
        /// </summary>
        /// <param name="message">受信メッセージ</param>
        protected override void ReceivedMessage(RPUtility.MessageSend messageSend)
        {
            // メッセージ受信処理を記述します。
        }
        /// <summary>
        /// コンボボックスチェンジイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">送信パラメータ</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)")]
        private void OnComboChanged(
            object sender,
            ControlInfoEventArgs e
            )
        {
            // nullチェック
            if (sender != null)
            {
                // sender確認
                if (sender is ControlInfo<string> ci)
                {
#if DEBUG
                    Console.WriteLine($"{this.ViewName} : {ci.Title} : {ci.Data.Value}");
#endif
                    // 入力項目の検証を行います。
                    this.CheckValidation(save: true);
                }
            }
        }
        /// <summary>
        /// ラジオボタンチェンジイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">送信パラメータ</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)")]
        private void OnRadioButtonChanged(object sender,
            ControlInfoEventArgs e
            )
        {
            // nullチェック
            if (sender != null)
            {
                // sender確認
                if (sender is ControlInfo<EnumDatas.Gender> ci)
                {
#if DEBUG
                    Console.WriteLine($"{this.ViewName} : {ci.Title} : {ci.Data.Value}");
#endif
                    // 入力項目の検証を行います。
                    this.CheckValidation(save: true);
                }
            }
        }
        /// <summary>
        /// プロパティチェンジイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">送信パラメータ</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:ローカライズされるパラメーターとしてリテラルを渡さない", MessageId = "System.Console.WriteLine(System.String)")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)")]
        private void OnPropertyChanged(object sender,
            RPUtility.ControlInfoEventArgs e
            )
        {
            // nullチェック
            if (sender != null)
            {
                // sender確認
                if (sender is ControlInfo<string> ci)
                {
#if DEBUG
                    Console.WriteLine($"{this.ViewName} : {ci.Title}: {ci.Data.Value}");
#endif
                    // 入力項目の検証を行います。
                    this.CheckValidation(save: true);
                }
            }
        }
        #endregion イベント

        #region INavigationAware
        /// <summary>
        /// パラメータを設定します。
        /// 画面遷移前に実行されます。
        /// </summary>
        /// <returns>パラメータオブジェクトを設定します。</returns>
        public override object SetNavigateParameters()
        {
            // 必要な場合はParamDatasインスタンスを渡します。
            return null;
        }
        /// <summary>
        /// パラメータを取得します。
        /// 画面遷移後に実行されます。
        /// </summary>
        /// <param name="parameters">パラメータオブジェクトを設定します。</param>
        public override void GetNavigateParameters(object parameters)
        {
            ;
        }

        /// <summary>
        /// 画面遷移前に呼び出されます。
        /// 遷移の実行許可確認を行います。
        /// </summary>
        /// <returns>画面遷移可能な場合trueを返します。</returns>
        public override bool IsNavigate()
        {
            // 入力項目の検証を行います。
            return this.CheckValidation(save: true);
        }

        /// <summary>
        /// ナビゲーションが完了したことを知らせます。
        /// </summary>
        /// <param name="result">ナビゲーション結果が返ります。</param>
        public override void NavigationComplete(NavigationResult result)
        {
            ;
        }
        #endregion INavigationAware
    }
}
