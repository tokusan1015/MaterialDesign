using Common;
using MaterialDesignModels;
using Prism.Regions;
using Reactive.Bindings;
using System;
using System.ComponentModel.DataAnnotations;

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
            new ReactivePropertySlim<string>(initialValue: "管理画面");
        /// <summary>
        /// 画面タイトル
        /// </summary>
        public ReactivePropertySlim<string> ViewTitle { get; } =
            new ReactivePropertySlim<string>();
        /// <summary>
        /// ボタン1情報(Dataは使用しないのでint型にしています。)
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "氏名",
            groupNo: 1,
            command: "Move ViewA"
            )]
        public RPUtility.ControlInfo<int> BtnInfo1 { get; } = new RPUtility.ControlInfo<int>();
        /// <summary>
        /// ボタン2情報(Dataは使用しないのでint型にしています。)
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "住所",
            groupNo: 1,
            command: "Move ViewB"
            )]
        public RPUtility.ControlInfo<int> BtnInfo2 { get; } = new RPUtility.ControlInfo<int>();
        /// <summary>
        /// ボタン3情報(Dataは使用しないのでint型にしています。)
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "設定",
            groupNo: 2,
            command: "Move ViewC"
            )]
        public RPUtility.ControlInfo<int> BtnInfo3 { get; } =new RPUtility.ControlInfo<int>();
        /// <summary>
        /// ボタン4情報(Dataは使用しないのでint型にしています。)
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "一覧",
            groupNo: 2,
            command: "Move ViewD",
            isEnable: false,
            isVisible: false
            )]
        public RPUtility.ControlInfo<int> BtnInfo4 { get; } = new RPUtility.ControlInfo<int>();
        /// <summary>
        /// ボタン5情報(Dataは使用しないのでint型にしています。)
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "反転",
            groupNo: 3,
            command: "InverseEnable"
            )]
        public RPUtility.ControlInfo<int> BtnInfo5 { get; } = new RPUtility.ControlInfo<int>();
        /// <summary>
        /// ボタン6情報(Dataは使用しないのでint型にしています。)
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "終了",
            groupNo:4,
            command: "ExitApp"
            )]
        public RPUtility.ControlInfo<int> BtnInfo6 { get; } = new RPUtility.ControlInfo<int>();
        #endregion Bindingプロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// 引数を追加することによりPrismに対して各インスタンスを要求しています。
        /// CommonDatasは、App.xaml.csのRegisterTypesにて登録されます。
        /// </summary>
        /// <param name="regionManager">リージョンマネージャを設定します。</param>
        /// <param name="commonDatas">共通データを設定します。</param>
        public MainWindowViewModel(
            [param: Required]IRegionManager regionManager,
            [param: Required]Common.CommonDatas commonDatas
            ) : base(regionManager: regionManager, commonDatas: commonDatas)
        {
            // 初期化処理を行います。
            this.Initialize();

            // データを取得します。
            var da = new DataAccess();
            var result = da.LoadCommonDatas(this.CommonDatas as CommonDatas);
        }
        /// <summary>
        /// 初期化処理を表します。
        /// </summary>
        private void Initialize()
        {
            // ReactiveProperty初期化処理を行います。
            this.InitializeReactiveProperty();

            // 初期ViewTitleを設定します。
            this.ViewTitle.Value = EnumDatas.ButtonNames.氏名.ToString();

            // 初期画面を設定します。
            RegionManager.RegisterViewWithRegion(
                Common.ConstDatas.ContentRegion,
                typeof(MaterialDesignViews.Views.ViewA)
                );
        }

        /// <summary>
        /// ReactivePropertyの初期化処理を表します。
        /// </summary>
        private void InitializeReactiveProperty()
        { 
            // BtnInfo1の情報を設定します。
            this.BtnInfo1.SetAll<MainWindowViewModel>(
                cibList: this.CIBList,
                onClicked: this.OnBtnInfo_Clicked,
                data: new Utility.DataAndName<ReactiveProperty<int>>()
                {
                    Data = null,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => BtnInfo1)
                });
            // BtnInfo2の情報を設定します。
            this.BtnInfo2.SetAll<MainWindowViewModel>(
                cibList: this.CIBList,
                onClicked: this.OnBtnInfo_Clicked,
                data: new Utility.DataAndName<ReactiveProperty<int>>()
                {
                    Data = null,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => BtnInfo2)
                });
            // BtnInfo3の情報を設定します。
            this.BtnInfo3.SetAll<MainWindowViewModel>(
                cibList: this.CIBList,
                onClicked: this.OnBtnInfo_Clicked,
                data: new Utility.DataAndName<ReactiveProperty<int>>()
                {
                    Data = null,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => BtnInfo3)
                });
            // BtnInfo4の情報を設定します。
            this.BtnInfo4.SetAll<MainWindowViewModel>(
                cibList: this.CIBList,
                onClicked: this.OnBtnInfo_Clicked,
                isEnable: false,
                isVisible: false,
                data: new Utility.DataAndName<ReactiveProperty<int>>()
                {
                    Data = null,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => BtnInfo4)
                });
            // BtnInfo5の情報を設定します。
            this.BtnInfo5.SetAll<MainWindowViewModel>(
                cibList: this.CIBList,
                onClicked: this.OnBtnInfo_Clicked,
                data: new Utility.DataAndName<ReactiveProperty<int>>()
                {
                    Data = null,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => BtnInfo5)
                });
            // BtnInfo6の情報を設定します。
            this.BtnInfo6.SetAll<MainWindowViewModel>(
                cibList: this.CIBList,
                onClicked: this.OnBtnInfo_Clicked,
                data: new Utility.DataAndName<ReactiveProperty<int>>()
                {
                    Data = null,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => BtnInfo6)
                });
        }
        #endregion コンストラクタ

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~MainWindowViewModel()
        {
            this.Dispose_RP();
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// ReactivePropertyの破棄処理
        /// </summary>
        private void Dispose_RP()
        {
            this.WindowTitle.Dispose();
            this.MainTitle.Dispose();
            this.ViewTitle.Dispose();
        }
        #endregion デストラクタ

        #region イベント
        /// <summary>
        /// BtnInfoボタンクリックイベント
        /// 全てのボタン押下がここに来るように設定しています。
        /// </summary>
        /// <param name="sender">送信元が設定されます。</param>
        /// <param name="e">送信パラメータが設定されます。</param>
        private void OnBtnInfo_Clicked(object sender, RPUtility.ControlInfoEventArgs e)
        {
            // 各データを取得します。
            var cd = this.CommonDatas as CommonDatas;
            var ci = (sender as RPUtility.ControlInfo<int>);
            var command = this.AnalyseCommand(e.Param as string);

            // 画面遷移可能かチェックします。遷移可能な場合trueとなります。
            var mv = Utility.EnumUtil.Find(typeof(EnumDatas.ViewNames), command.ViewName);

            // 押下されたボタンのenumを取得します。
            var bn = (EnumDatas.ButtonNames)Utility.EnumUtil.EnumParse(
                typeof(EnumDatas.ButtonNames),
                ci.Title.Value
                );

            // エラー検証を行います。エラーがない場合trueとなります。
            var av = cd.ActiveView as RPUtility.BindableBasePlus;
            var va = !av.HasErrors();

            // コマンド解析
            switch (command.Command)
            {
                case "Move":
                    // 画面遷移可能かチェックします。
                    // mv：移動可能なViewである事(EnumDatas.ViewNamesに存在している)
                    // va：エラーが無い事(エラーがない場合true)
                    // rn != av.ViewName： 異なるViewである事
                    if (mv && va && command.ViewName != av.ViewName)
                    {
                        // ViewTitleを設定します。
                        this.ViewTitle.Value = bn.ToString();
                        // 対象画面へ遷移します。
                        this.Navigate(
                            regionName: Common.ConstDatas.ContentRegion,
                            target: command.ViewName);
                    }
                    break;
                case "InverseEnable":
                    // 該当ボタンのEnableを反転します。
                    this.InvertEnables(1);
                    break;
                case "ExitApp":
                    // エラーが存在しない場合アプリケーションを終了します。
                    if (va)
                    {
                        // アプリケーションを終了します。
                        this.ExitApplication();
                    }
                    break;
                default:
                    throw new System.Exception($"存在しないコマンドが指定されました。Command={command.Command}");
            }
        }
        #endregion イベント

        #region メソッド
        /// <summary>
        /// コマンドを分解します。
        /// </summary>
        /// <param name="command">コマンド文字列</param>
        /// <returns>(Command, ViewName)形式で返します。</returns>
        private (string Command, string ViewName) AnalyseCommand(
            string command
            )
        {
            var s = command.Split(' ');
            string Command = "";
            string ViewName = "";

            if (s.Length > 0) Command = s[0];
            if (s.Length > 1) ViewName = s[1];

            return (Command, ViewName);
        }
        /// <summary>
        /// アプリケーション終了処理
        /// </summary>
        private void ExitApplication()
        {
            // データ保存
            var da = new DataAccess();
            var result = da.SaveCommonDatas(
                cd: this.CommonDatas as CommonDatas
                );

            // アプリケーションを終了します。
            System.Windows.Application.Current.Shutdown();
        }
        #endregion メソッド

        #region BindableBasePlus
        /// <summary>
        /// パラメータを設定します。
        /// </summary>
        /// <returns>パラメータオブジェクトを設定します。</returns>
        protected override object SetNavigateParameters()
        {
            // 必要な場合はParamDatasインスタンスを渡します。
            return null;
        }
        /// <summary>
        /// パラメータを取得します。
        /// </summary>
        /// <param name="parameters">パラメータオブジェクトを設定します。</param>
        protected override void GetNavigateParameters(object parameters)
        {
            // ParamDatasとして受け取ります。
            var param = parameters as Common.ParamDatas;
        }
        #endregion BindableBasePlus
    }
}
