using Common;
using Prism.Regions;
using Reactive.Bindings;
using RPUtility;
using System.ComponentModel.DataAnnotations;

namespace MaterialDesignViews.ViewModels
{
    /// <summary>
    /// ViewC用ViewModel
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class ViewCViewModel : RPUtility.BindableBasePlus
	{
        #region Bindingプロパティ
        /// <summary>
        /// パスボタン
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "…"
            )]
        public RPUtility.ControlInfo<int> PathButton { get; } = new RPUtility.ControlInfo<int>();
        /// <summary>
        /// 保存パス
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "保存パス"
            )]
        public RPUtility.ControlInfo<string> SavePath { get; } = new RPUtility.ControlInfo<string>();
        /// <summary>
        /// 備考
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "備考"
            )]
        public RPUtility.ControlInfo<string> Etc { get; } = new RPUtility.ControlInfo<string>();
        #endregion Bindingプロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// 引数を追加することによりPrismに対して各インスタンスを要求しています。
        /// CommonDatasは、App.xaml.csのRegisterTypesにて登録されます。
        /// </summary>
        /// <param name="regionManager">リージョンマネージャを設定します。</param>
        /// <param name="commonDatas">共通データを設定します。</param>
        public ViewCViewModel(
            [param: Required]IRegionManager regionManager,
            [param: Required]Common.CommonDatas commonDatas
            ) : base(
                regionManager: regionManager,
                commonDatas: commonDatas
                )
        {
            // 自Viewを設定します。
            this.ViewName = Common.EnumDatas.ViewNames.ViewC.ToString();

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

            // ボタン情報を設定します。
            this.PathButton.SetAll<ViewCViewModel>(
                cibList: this.CIBList,
                onClicked: this.OnClicked,
                data: new Utility.DataAndName<ReactiveProperty<int>>()
                {
                    Data = null,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => PathButton)
                });

            // テキストボックス情報を設定します。
            this.SavePath.SetAll<ViewCViewModel>(
                cibList: this.CIBList,
                onPropertyChangd: this.OnPropertyChanged,
                data: new Utility.DataAndName<ReactiveProperty<string>>()
                {
                    Data = cd.SavePath,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => cd.SavePath)
                });
            this.Etc.SetAll<ViewCViewModel>(
                cibList: this.CIBList,
                onPropertyChangd: this.OnPropertyChanged,
                data: new Utility.DataAndName<ReactiveProperty<string>>()
                {
                    Data = cd.Etc,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => cd.Etc)
                });
        }

        #endregion コンストラクタ

        #region イベント
        /// <summary>
        /// ボタンクリックイベント
        /// </summary>
        /// <param name="sender">送信元を設定します。</param>
        /// <param name="e">送信パラメータを設定します。</param>
        private void OnClicked(object sender, ControlInfoEventArgs e)
        {
            // フォルダダイアログ生成します。
            var dlg = new Utility.FileDialog();

            // フォルダダイアログ表示します。
            if (dlg.Show())
            {
                // フォルダパスを保存します。
                this.SavePath.Data.Value = dlg.FileName;
            }
        }
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
