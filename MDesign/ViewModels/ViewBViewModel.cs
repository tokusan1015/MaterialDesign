using Common;
using Prism.Regions;
using Reactive.Bindings;
using System.ComponentModel.DataAnnotations;

namespace MaterialDesignViews.ViewModels
{
    /// <summary>
    /// ViewB用ViewModel
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class ViewBViewModel : RPUtility.BindableBasePlus
    {
        #region Bindingプロパティ
        /// <summary>
        /// 郵便番号
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "郵便番号"
            )]
        public RPUtility.ControlInfo<string> ZipCode { get; } = new RPUtility.ControlInfo<string>();
        /// <summary>
        /// 都道府県
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "都道府県"
            )]
        public RPUtility.ControlInfo<string> Prefectures { get; } = new RPUtility.ControlInfo<string>();
        /// 市区町村
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "市区町村"
            )]
        public RPUtility.ControlInfo<string> Municipality { get; } = new RPUtility.ControlInfo<string>();
        /// 番地
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "番地"
            )]
        public RPUtility.ControlInfo<string> HouseNumber { get; } = new RPUtility.ControlInfo<string>();
        #endregion Bindingプロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// 引数を追加することによりPrismに対して各インスタンスを要求しています。
        /// CommonDatasは、App.xaml.csのRegisterTypesにて登録されます。
        /// </summary>
        /// <param name="regionManager">リージョンマネージャを設定します。</param>
        /// <param name="commonDatas">共通データを設定します。</param>
        public ViewBViewModel(
            [param: Required]IRegionManager regionManager,
            [param: Required]Common.CommonDatas commonDatas
            ) : base(regionManager: regionManager, commonDatas: commonDatas)
        {
            // 自Viewを設定します。
            this.ViewName = Common.EnumDatas.ViewNames.ViewB.ToString();

            // ActiveViewを渡します。
            commonDatas.SetActiveView(this);

            // 初期化処理を行います。
            this.Initialize();
        }
        /// <summary>
        /// 初期化処理を行います。
        /// </summary>
        private void Initialize()
        {
            // 共通データを受け取ります。
            var cd = this.CommonDatas as CommonDatas;

            // テキストボックス情報を設定します。
            this.ZipCode.SetAll<ViewBViewModel>(
                cibList: this.CIBList,
                onPropertyChangd: this.OnPropertyChanged,
                data: new Utility.DataAndName<ReactiveProperty<string>>()
                {
                    Data = cd.ZipCode,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => cd.ZipCode)
                });
            this.Prefectures.SetAll<ViewBViewModel>(
                cibList: this.CIBList,
                onPropertyChangd: this.OnPropertyChanged,
                data: new Utility.DataAndName<ReactiveProperty<string>>()
                {
                    Data = cd.Prefectures,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => cd.Prefectures)
                });
            this.Municipality.SetAll<ViewBViewModel>(
                cibList: this.CIBList,
                onPropertyChangd: this.OnPropertyChanged,
                data: new Utility.DataAndName<ReactiveProperty<string>>()
                {
                    Data = cd.Municipality,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => cd.Municipality)
                });
            this.HouseNumber.SetAll<ViewBViewModel>(
                cibList: this.CIBList,
                onPropertyChangd: this.OnPropertyChanged,
                data: new Utility.DataAndName<ReactiveProperty<string>>()
                {
                    Data = cd.HouseNumber,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => cd.HouseNumber)
                });
        }
        #endregion コンストラクタ

        #region イベント
        /// <summary>
        /// プロパティチェンジイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">送信パラメータ</param>
        private void OnPropertyChanged(
            object sender,
            RPUtility.ControlInfoEventArgs e
            )
        {
            ;
        }
        #endregion イベント

        #region INavigationAware
        /// <summary>
        /// パラメータを設定します。
        /// 画面遷移前に実行されます。
        /// </summary>
        /// <returns>パラメータオブジェクトを設定します。</returns>
        protected override object SetNavigateParameters()
        {
            // 必要な場合はParamDatasインスタンスを渡します。
            return null;
        }
        /// <summary>
        /// パラメータを取得します。
        /// 画面遷移後に実行されます。
        /// </summary>
        /// <param name="parameters">パラメータオブジェクトを設定します。</param>
        protected override void GetNavigateParameters(
            object parameters
            )
        {
            // ParamDatasとして受け取ります。
            var param = parameters as Common.ParamDatas;
        }
        #endregion INavigationAware
    }
}
