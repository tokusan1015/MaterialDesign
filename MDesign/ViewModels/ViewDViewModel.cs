using Common;
using System;
using Prism.Regions;
using System.ComponentModel.DataAnnotations;
using Prism.Ioc;
using Prism.Events;

namespace MaterialDesignViews.ViewModels
{
    /// <summary>
    /// ViewD用ViewModel
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class ViewDViewModel : RPUtility.BindableBasePlus
    {
        #region ViewDatas
        /// <summary>
        /// ViewDatas
        /// </summary>
        //private Common.ViewDdatas ViewDatas { get; } = new Common.ViewDdatas();
        #endregion ViewDatas

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
        public ViewDViewModel(
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
                viewName: EnumDatas.ViewNames.ViewD.ToString()
                )
        {
            // コンポーネントの初期化処理を行います。
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
        #endregion コンストラクタ

        #region Loaded
        /// <summary>
        /// ロード完了イベント
        /// </summary>
        protected override void Loaded()
        {
            throw new NotImplementedException();

            // CommonDataから読み込みます。
            //this.CommonDatas.GetViewDatas(this.ViewDatas);

            // メッセージ送受信開始
            //this.StartMessage = true;

            // 初期化完了フラグ設定
            //this.InitiazaizuEnd = true;
        }
        /// <summary>
        /// コンポーネントの初期化処理を行います。
        /// </summary>
        private void InitializeComponent()
        {
            // リストをクリア
            this.CibList.Clear();
        }
        #endregion Loaded

        #region メソッド
        /// <summary>
        /// CommonDatasにデータを保存します。
        /// </summary>
        protected override void SaveCommonDatas()
        {
            throw new NotImplementedException();
            // CommonDatasにデータを保存します。
            //this.CommonDatas.SetViewDatas(this.ViewDatas);
        }
        #endregion メソッド

        #region デストラクタ
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~ViewDViewModel()
        {
            this.CibList.Clear();
            //this.ViewDatas.Dispose();
        }
        #endregion デストラクタ

        #region イベント
        /// <summary>
        /// メッセージ受信イベント
        /// </summary>
        /// <param name="message">受信メッセージ</param>
        protected override void ReceivedMessage(RPUtility.MessageSend messageSend)
        {
            // メッセージ受信処理を記述します。
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
            [param: Required]object parameters
            )
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
