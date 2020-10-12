﻿using Common;
using System;
using Prism.Regions;
using Reactive.Bindings;
using RPUtility;
using System.ComponentModel.DataAnnotations;
using Prism.Ioc;
using Prism.Events;

namespace MaterialDesignViews.ViewModels
{
    /// <summary>
    /// ViewC用ViewModel
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [Utility.Developer(name: "tokusan1015")]
    public class ViewCViewModel : RPUtility.BindableBasePlus
	{
        #region ViewDatas
        /// <summary>
        /// ViewDatas
        /// </summary>
        private Common.ViewCdatas ViewDatas { get; } = new Common.ViewCdatas();
        #endregion ViewDatas

        #region Bindingプロパティ
        /// <summary>
        /// パスボタン
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "…"
            , groupNo: 1
            , commandText: "Click PathButton"
            , isEnable: true
            , isVisible: true
            )]
        public RPUtility.ControlInfo<int> PathButton { get; } = new RPUtility.ControlInfo<int>();
        /// <summary>
        /// 保存パス
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "保存パス"
            , groupNo: 1
            , commandText: "Click SavePath"
            , isEnable: true
            , isVisible: true
            )]
        public RPUtility.ControlInfo<string> SavePath { get; } = new RPUtility.ControlInfo<string>();
        /// <summary>
        /// 備考
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "備考"
            , groupNo: 2
            , commandText: "Click Etc"
            , isEnable: true
            , isVisible: true
            )]
        public RPUtility.ControlInfo<string> Etc { get; } = new RPUtility.ControlInfo<string>();
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
        /// <param name="commonSettings">共通設定を設定します。</param>
        /// <param name="commonDatas">共通データを設定します。</param>
        public ViewCViewModel(
            [param: Required]IContainerExtension container,
            [param: Required]IRegionManager regionManager,
            [param: Required]IEventAggregator eventAggregator,
            [param: Required]Common.CommonSettings commonSettings,
            [param: Required]Common.CommonDatas commonDatas
            ) : base(
                container: container,
                regionManager: regionManager,
                eventAggregator: eventAggregator,
                commonSettings: commonSettings,
                commonDatas: commonDatas,
                // MainViewName, ViewNameを設定します。
                mainViewName: EnumDatas.ViewNames.Main.ToString(),
                viewName: EnumDatas.ViewNames.ViewC.ToString()
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
            // リストをクリア
            this.CibList.Clear();

            // 共通データを取得します。
            var vd = this.ViewDatas;

            // 自身のタイプ取得
            var thisType = this.GetType();

            // ボタン情報を設定します。
            this.PathButton.SetAll(
                viewModelType: thisType,
                cibList: this.CibList,
                onDcommand: this.OnClicked,
                data: new Utility.DataAndName<ReactiveProperty<int>>()
                {
                    Data = null,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => PathButton)
                });

            // テキストボックス情報を設定します。
            this.SavePath.SetAll(
                viewModelType: thisType,
                cibList: this.CibList,
                onPropertyChangd: this.OnPropertyChanged,
                data: new Utility.DataAndName<ReactiveProperty<string>>()
                {
                    Data = vd.SavePath,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => vd.SavePath)
                });
            this.Etc.SetAll(
                viewModelType: thisType,
                cibList: this.CibList,
                onPropertyChangd: this.OnPropertyChanged,
                data: new Utility.DataAndName<ReactiveProperty<string>>()
                {
                    Data = vd.Etc,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => vd.Etc)
                });

            // CommonDataから読み込みます。
            this.CommonDatas.GetViewDatas(this.ViewDatas);
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
        ~ViewCViewModel()
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
        protected override void ReceivedMessage(RPUtility.IEventParam eventParam)
        {
            // メッセージ受信処理を記述します。
        }
        /// <summary>
        /// ボタンクリックイベント
        /// </summary>
        /// <param name="sender">送信元を設定します。</param>
        /// <param name="e">送信パラメータを設定します。</param>
        private void OnClicked(object sender, ControlInfoEventArgs e)
        {
            // フォルダダイアログ生成します。
            using (var dlg = new Utility.FileDialog())
            {
                // フォルダダイアログ表示します。
                if (dlg.Show())
                {
                    // フォルダパスを保存します。
                    this.SavePath.Data.Value = dlg.FileName;
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
        private void OnPropertyChanged(
            object sender,
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
                    Console.WriteLine($"{ci.Title} : {ci.Data.Value}");
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
        public override void GetNavigateParameters(
            object parameters
            )
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
