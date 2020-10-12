using Common;
using MaterialDesignModels;
using Prism.Ioc;
using Prism.Regions;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using System.Linq;
using System.Data.Linq;
using Prism.Events;
using System.Reflection;

namespace MaterialDesign.ViewModels
{
    /// <summary>
    /// MainWindowViewModel
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [Utility.Developer(name: "tokusan1015")]
    public class MainWindowViewModel : RPUtility.BindableBasePlus
    {
        #region Bindingプロパティ
        /// <summary>
        /// ウィンドウタイトルを表します。
        /// </summary>
        public ReactivePropertySlim<string> WindowTitle { get; } =
            new ReactivePropertySlim<string>(initialValue: "MaterialDesign");
        /// <summary>
        /// メインタイトルを表します。
        /// </summary>
        public ReactivePropertySlim<string> MainTitle { get; } =
            new ReactivePropertySlim<string>(initialValue: "メイン画面");
        /// <summary>
        /// 画面タイトル
        /// </summary>
        public ReactivePropertySlim<string> ViewTitle { get; } =
            new ReactivePropertySlim<string>();
        /// <summary>
        /// ボタン1情報(Dataは使用しないのでint型にしています。)
        /// displeyName, commandTextはViewモジュール側でのの設定が優先されます。
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "ボタン１"
            , groupNo: 1
            , commandText: "Move ViewA"
            , isEnable: true
            , isVisible: true
            )]
        public RPUtility.ControlInfo<int> BtnInfo1 { get; } = new RPUtility.ControlInfo<int>();
        /// <summary>
        /// ボタン2情報(Dataは使用しないのでint型にしています。)
        /// displeyName, commandTextはViewモジュール側でのの設定が優先されます。
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "ボタン２"
            , groupNo: 1
            , commandText: "Move ViewB"
            , isEnable: true
            , isVisible: true
            )]
        public RPUtility.ControlInfo<int> BtnInfo2 { get; } = new RPUtility.ControlInfo<int>();
        /// <summary>
        /// ボタン3情報(Dataは使用しないのでint型にしています。)
        /// displeyName, commandTextはViewモジュール側でのの設定が優先されます。
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "ボタン３"
            , groupNo: 2
            , commandText: "Move ViewC"
            , isEnable: true
            , isVisible: true
            )]
        public RPUtility.ControlInfo<int> BtnInfo3 { get; } =new RPUtility.ControlInfo<int>();
        /// <summary>
        /// ボタン4情報(Dataは使用しないのでint型にしています。)
        /// displeyName, commandTextはViewモジュール側でのの設定が優先されます。
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "ボタン４"
            , groupNo: 2
            , commandText: "Move ViewD"
            , isEnable: false
            , isVisible: false
            )]
        public RPUtility.ControlInfo<int> BtnInfo4 { get; } = new RPUtility.ControlInfo<int>();
        /// <summary>
        /// ボタン5情報(Dataは使用しないのでint型にしています。)
        /// displeyName, commandTextはViewモジュール側でのの設定が優先されます。
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "ボタン５"
            , groupNo: 3
            , commandText: "InverseEnable"
            , isEnable: true
            , isVisible: true
            )]
        public RPUtility.ControlInfo<int> BtnInfo5 { get; } = new RPUtility.ControlInfo<int>();
        /// <summary>
        /// ボタン6情報(Dataは使用しないのでint型にしています。)
        /// 終了ボタンとして予約されています。
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "終了"
            , groupNo:4
            , commandText: "ExitApp"
            , isEnable: true
            , isVisible: true
            )]
        public RPUtility.ControlInfo<int> BtnInfo6 { get; } = new RPUtility.ControlInfo<int>();
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
        public MainWindowViewModel(
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
                // MainViewName,ViewNameを設定します。
                mainViewName: EnumDatas.ViewNames.Main.ToString(),
                viewName: EnumDatas.ViewNames.Main.ToString()
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
        /// コンポーネントの初期化処理を表します。
        /// </summary>
        private void InitializeComponent()
        {
            // 自身のタイプ取得
            var thisType = this.GetType();

            // BtnInfo1の情報を設定します。
            this.BtnInfo1.SetAll(
                viewModelType: thisType,
                cibList: this.CibList,
                onDcommand: this.OnBtnInfo_Clicked,
                data: new Utility.DataAndName<ReactiveProperty<int>>()
                {
                    Data = null,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => BtnInfo1)
                });
            // BtnInfo2の情報を設定します。
            this.BtnInfo2.SetAll(
                viewModelType: thisType,
                cibList: this.CibList,
                onDcommand: this.OnBtnInfo_Clicked,
                data: new Utility.DataAndName<ReactiveProperty<int>>()
                {
                    Data = null,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => BtnInfo2)
                });
            // BtnInfo3の情報を設定します。
            this.BtnInfo3.SetAll(
                viewModelType: thisType,
                cibList: this.CibList,
                onDcommand: this.OnBtnInfo_Clicked,
                data: new Utility.DataAndName<ReactiveProperty<int>>()
                {
                    Data = null,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => BtnInfo3)
                });
            // BtnInfo4の情報を設定します。
            this.BtnInfo4.SetAll(
                viewModelType: thisType,
                cibList: this.CibList,
                onDcommand: this.OnBtnInfo_Clicked,
                data: new Utility.DataAndName<ReactiveProperty<int>>()
                {
                    Data = null,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => BtnInfo4)
                });
            // BtnInfo5の情報を設定します。
            this.BtnInfo5.SetAll(
                viewModelType: thisType,
                cibList: this.CibList,
                onDcommand: this.OnBtnInfo_Clicked,
                data: new Utility.DataAndName<ReactiveProperty<int>>()
                {
                    Data = null,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => BtnInfo5)
                });
            // BtnInfo6の情報を設定します。
            this.BtnInfo6.SetAll(
                viewModelType: thisType,
                cibList: this.CibList,
                onDcommand: this.OnBtnInfo_Clicked,
                data: new Utility.DataAndName<ReactiveProperty<int>>()
                {
                    Data = null,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => BtnInfo6)
                });
        }
        #endregion コンストラクタ

        #region Loaded
        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">送信パラメータ</param>
        protected override void Loaded()
        {
            // リージョンを設定します。
            this.SetRegion(this.RegionManager.Regions[Common.ConstDatas.ContentRegion]);

            // Viewの初期化処理を行います。
            this.InitializeViews();

            // 初期画面を設定します。
            this.ChangeShowView(EnumDatas.ViewNames.ViewA.ToString());
            //RegionManager.RegisterViewWithRegion(
            //    Common.ConstDatas.ContentRegion,
            //    typeof(MaterialDesignViews.Views.ViewA)
            //    );

            // データを取得します。
            var da = new DataAccess();
            da.LoadCommonDatas(this.CommonDatas);

            // メッセージ受信開始
            this.MessageManager.Start = true;

            // 初期化完了フラグ設定
            this.InitiazaizuEnd = true;
        }

        /// <summary>
        /// Viewの初期化処理を表します。
        /// </summary>
        private void InitializeViews()
        {
            // RegionおよびViewsに登録します。
            this.AddRegionAndViews<MaterialDesignViews.Views.ViewA>(
                viewName: EnumDatas.ViewNames.ViewA.ToString());
            this.AddRegionAndViews<MaterialDesignViews.Views.ViewB>(
                viewName: EnumDatas.ViewNames.ViewB.ToString());
            this.AddRegionAndViews<MaterialDesignViews.Views.ViewC>(
                viewName: EnumDatas.ViewNames.ViewC.ToString());
        }
        /// <summary>
        /// 共通設定を読み込み処理を表します。
        /// </summary>
        /// <param name="viewName">View名を設定します。</param>
        private void LoadStartSettings(
            [param: Required]string viewName
            )
        {
            // タイトル設定
            this.LoadSettingsToMainTitle();

            // ViewTitle設定
            this.LoadSettingToViewTitle(viewName: viewName);

            // ボタン設定
            this.LoadSettingsToButtonInfo(viewName: viewName);

        }
        /// <summary>
        /// 共通設定からMainTitleを取得し設定します。
        /// </summary>
        private void LoadSettingsToMainTitle(
            )
        {
            if (this.CommonSettings.MainTitle != null)
            {
                // タイトル設定
                this.MainTitle.Value = this.CommonSettings.MainTitle;
            }
        }
        /// <summary>
        /// 共通設定からViewTitleを取得し設定します。
        /// </summary>
        /// <param name="viewName">View名を設定します。</param>
        private void LoadSettingToViewTitle(
            [param: Required]string viewName
            )
        {
            // nullチェック
            if (viewName == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " + nameof(viewName));

            // ViewTitle設定
            var vt = this.CommonSettings.ViewTitle
                .Where(x => x.Key == viewName).Select(x => x.Value).FirstOrDefault();
            if (vt != null)
            {
                this.ViewTitle.Value = vt;
            }
        }
        /// <summary>
        /// 共通設定からButtonInfoを取得設定します。
        /// </summary>
        /// <param name="viewName">View名を設定します。</param>
        private void LoadSettingsToButtonInfo(
            [param: Required]string viewName
            )
        {
            // nullチェック
            if (viewName == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " + nameof(viewName));

            // BottonInfo設定
            foreach (var bi in this.CibList)
            {
                var btnInfo = this.CommonSettings.ButtonInfo
                    .Where(x => x.Key == bi.PropertyName)
                    .Select(x => x.Value)
                    .FirstOrDefault();
                if (btnInfo != null)
                {
                    if (btnInfo.ButtonCommand != null) bi.CommandText = btnInfo.ButtonCommand;
                    if (btnInfo.ButtonTitle != null) bi.Title.Value = btnInfo.ButtonTitle;
                    if (btnInfo.IsEnable != null) bi.IsEnable.Value = (bool)btnInfo.IsEnable;
                    if (btnInfo.IsVisible != null) bi.IsVisible.Value = (bool)btnInfo.IsVisible;
                }
            }
        }
        #endregion Loaded

        #region イベント
        /// <summary>
        /// メッセージ受信イベント
        /// 自分宛のメッセージのみ受信します。
        /// </summary>
        /// <param name="message">受信メッセージ</param>
        protected override void ReceivedMessage(
            [param: Required]RPUtility.IEventParam eventParam
            )
        {
            // nullチェック
            if (eventParam == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(eventParam));

            // クラス名を取得します。
            var name = eventParam.EventType.Name;

            // 受信メッセージを処理します。
            switch (name)
            {
                case "MessageInfoSend":
                    this.ProcessingMessageInfoSend(eventParam: eventParam);
                    break;
                default:
                    throw new InvalidOperationException(MethodBase.GetCurrentMethod().Name + " : " +$"ReceivedMessage:{name}");
            }
        }
        /// <summary>
        /// InputErrorSendに対する処理を表します。
        /// コメント：BindableBasePlusに入れるべきか？
        /// </summary>
        /// <param name="eventParam">IEventParamを設定します。</param>
        private void ProcessingMessageInfoSend(
            [param: Required]RPUtility.IEventParam eventParam
            )
        {
            // nullチェック
            if (eventParam == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(eventParam));

            // IEventParamをキャストします。
            var param = eventParam as RPUtility.MessageInfoSend;

            // コマンドに対応する処理を行います。 
            switch (param.Command)
            {
                case EnumDatas.MassageInfo.LoadSetting:
                    // 共通設定を読み込みます。
                    if (Utility.BooleanUtili.IsExecuteAtOnce())
                    {
                        this.LoadStartSettings(EnumDatas.ViewNames.ViewA.ToString());
                    }
                    break;
                case EnumDatas.MassageInfo.InputError:
                    // InputErrorの場合はボタンを使用禁止にします。
                    this.SetEnables(false, -1);
                    break;
                case EnumDatas.MassageInfo.NoInputError:
                    // NoInputErrorの場合はボタンを使用許可にします。
                    this.SetEnables(true, -1);
                    break;
                case EnumDatas.MassageInfo.Message:
                    // Messageの場合は実装していません。
                    throw new NotImplementedException();
                default:
                    // 不明の場合は例外を発生します。
                    throw new InvalidOperationException(MethodBase.GetCurrentMethod().Name + " : " +nameof(param.Command));
            }
        }

        /// <summary>
        /// BtnInfoボタンクリックイベント
        /// 全てのボタン押下がここに来るように設定しています。
        /// </summary>
        /// <param name="sender">送信元が設定されます。</param>
        /// <param name="e">送信パラメータが設定されます。</param>
        private void OnBtnInfo_Clicked(object sender, RPUtility.ControlInfoEventArgs e)
        {
            // クリックイベント処理を記述します。
            // 各データを取得します。
            var ci = (sender as RPUtility.ControlInfo<int>);
            var (Command, ViewName) = Utility.StringUtil.AnalyseCommand(e.Param as string);

            // 画面遷移可能かチェックします。遷移可能な場合trueとなります。
            var mv = Utility.EnumUtil.EnumIsDefined(typeof(EnumDatas.ViewNames), ViewName);

            // コマンド解析
            switch (Command)
            {
                case "Move":
                    // 画面遷移可能かチェックします。
                    // mv：移動可能なViewである事(EnumDatas.ViewNamesに存在している)
                    // コマンドのViewName： 異なるViewである事
                    if (mv && ViewName != this.ActiveViewName)
                    {
                        // ViewTitleを設定します。
                        this.LoadSettingToViewTitle(ViewName);

                        // コマンドのViewNameを表示します。
                        this.ChangeShowView(ViewName);

                        // 対象画面へ遷移します。
                        //this.Navigate(
                        //    regionName: Common.ConstDatas.ContentRegion,
                        //    target: ViewName);
                    }
                    break;
                case "InverseEnable":
                    // 該当ボタンのEnableを反転します。
                    this.InvertEnables(1);
                    break;
                case "ExitApp":
                    // アプリケーションを終了します。
                    this.ExitApplication();
                    break;
                default:
                    throw new InvalidOperationException(MethodBase.GetCurrentMethod().Name + " : " +$"存在しないコマンドが指定されました。Command={Command}");
            }
        }
        #endregion イベント

        #region メソッド
        /// <summary>
        /// CommonDatasにデータを保存します。
        /// </summary>
        /// <param name="cd">CommonDatasを設定します。</param>
        protected override void SaveCommonDatas()
        {

        }
        /// <summary>
        /// アプリケーション終了処理
        /// </summary>
        private void ExitApplication()
        {
            // データ保存
            var da = new MaterialDesignModels.DataAccess();
            da.SaveCommonDatas(this.CommonDatas);

            // アプリケーションを終了します。
            System.Windows.Application.Current.Shutdown();
        }
        #endregion メソッド

        #region BindableBasePlus
        /// <summary>
        /// パラメータを設定します。
        /// </summary>
        /// <returns>パラメータオブジェクトを設定します。</returns>
        public override object SetNavigateParameters()
        {
            // 必要な場合はParamDatasインスタンスを渡します。
            return null;
        }
        /// <summary>
        /// パラメータを取得します。
        /// </summary>
        /// <param name="parameters">パラメータオブジェクトを設定します。</param>
        public override void GetNavigateParameters(object parameters)
        {
            // ParamDatasとして受け取ります。
            //var param = parameters as Common.ParamDatas;
        }

        /// <summary>
        /// 画面遷移前に呼び出されます。
        /// 遷移の実行許可確認を行います。
        /// </summary>
        /// <returns>画面遷移可能な場合trueを返します。</returns>
        public override bool IsNavigate()
        {
            return false;
        }

        /// <summary>
        /// ナビゲーションが完了したことを知らせます。
        /// </summary>
        /// <param name="result">ナビゲーション結果が返ります。</param>
        public override void NavigationComplete(NavigationResult result)
        {
            ;
        }
        #endregion BindableBasePlus
    }
}
