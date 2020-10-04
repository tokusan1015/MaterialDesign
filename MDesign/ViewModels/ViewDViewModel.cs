using Common;
using Prism.Regions;
using System.ComponentModel.DataAnnotations;

namespace MaterialDesignViews.ViewModels
{
    /// <summary>
    /// ViewD用ViewModel
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class ViewDViewModel : RPUtility.BindableBasePlus
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// 引数を追加することによりPrismに対して各インスタンスを要求しています。
        /// CommonDatasは、App.xaml.csのRegisterTypesにて登録されます。
        /// </summary>
        /// <param name="regionManager">リージョンマネージャを設定します。</param>
        /// <param name="commonDatas">共通データを設定します。</param>
        public ViewDViewModel(
            [param: Required]IRegionManager regionManager,
            [param: Required]Common.CommonDatas commonDatas
            ) : base(
                regionManager: regionManager,
                commonDatas: commonDatas
                )
        {
            // 自Viewを設定します。
            this.ViewName = Common.EnumDatas.ViewNames.ViewD.ToString();

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
            // 共通データを取得します。
            var cd = this.CommonDatas as CommonDatas;

        }
        #endregion コンストラクタ

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
            [param: Required]object parameters
            )
        {
            // ParamDatasとして受け取ります。
            var param = parameters as Common.ParamDatas;
        }
        #endregion INavigationAware
    }
}
