using Common;
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
        #region Bindingプロパティ
        /// <summary>
        /// 苗字
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "苗字"            
            )]
        public RPUtility.ControlInfo<string> LastName { get; } = new RPUtility.ControlInfo<string>();
        /// <summary>
        /// 名前
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "名前"
            )]
        public RPUtility.ControlInfo<string> FirstName { get; } = new RPUtility.ControlInfo<string>();
        /// <summary>
        /// 年齢
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "年齢"
            )]
        public RPUtility.ControlInfo<DateTime?> Birthday { get; } = new RPUtility.ControlInfo<DateTime?>();
        /// <summary>
        /// 性別
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "性別"
            )]
        public RPUtility.ControlInfo<EnumDatas.Gender> Gender { get; } = new RPUtility.ControlInfo<EnumDatas.Gender>();
        /// <summary>
        /// 国籍
        /// </summary>
        [RPUtility.ControlInfo(
            displeyName: "国籍"
            )]
        public RPUtility.ControlInfo<string> Country { get; } = new ControlInfo<string>();
        /// <summary>
        /// 国籍コンボボックスItemsSource
        /// </summary>
        public ReadOnlyDictionary<string, string> CountryDic { get; set; }
        #endregion Bindingプロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// 引数を追加することによりPrismに対して各インスタンスを要求しています。
        /// CommonDatasは、App.xaml.csのRegisterTypesにて登録されます。
        /// </summary>
        /// <param name="regionManager">リージョンマネージャを設定します。</param>
        /// <param name="commonDatas">共通データを設定します。</param>
        public ViewAViewModel(
            [param: Required]IRegionManager regionManager,
            [param: Required]Common.CommonDatas commonDatas
            ) : base(regionManager: regionManager, commonDatas: commonDatas)
        {
            // 自Viewを設定します。
            this.ViewName = Common.EnumDatas.ViewNames.ViewA.ToString();

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
            this.LastName.SetAll<ViewAViewModel>(
                cibList: this.CIBList,
                onPropertyChangd: this.OnPropertyChanged,
                data: new Utility.DataAndName<ReactiveProperty<string>>()
                {
                    Data = cd.LastName,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => cd.LastName)
                });
            this.FirstName.SetAll<ViewAViewModel>(
                cibList: this.CIBList,
                onPropertyChangd: this.OnPropertyChanged,
                data: new Utility.DataAndName<ReactiveProperty<string>>()
                {
                    Data = cd.FirstName,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => cd.FirstName)
                });
            this.Birthday.SetAll<ViewAViewModel>(
                cibList: this.CIBList,
                onPropertyChangd: this.OnPropertyChanged,
                data: new Utility.DataAndName<ReactiveProperty<DateTime?>>()
                {
                    Data = cd.Birthday,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => cd.Birthday)
                });
            this.Gender.SetAll<ViewAViewModel>(
                cibList: this.CIBList,
                onPropertyChangd: this.OnRadioButtonChanged,
                data: new Utility.DataAndName<ReactiveProperty<EnumDatas.Gender>>()
                {
                    Data = cd.Gender,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => cd.Gender)
                });
            this.Country.SetAll<ViewAViewModel>(
                cibList: this.CIBList,
                onPropertyChangd: this.OnComboChanged,
                data: new Utility.DataAndName<ReactiveProperty<string>>()
                {
                    Data = cd.Country,
                    PropertyName = Utility.ObjectUtil.GetPropertyName(() => cd.Country)
                });
            // データソース設定
            this.CountryDic = new ReadOnlyDictionary<string, string>(cd.CountryDic);
        }
        #endregion コンストラクタ

        #region イベント
        /// <summary>
        /// コンボボックスチェンジイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">送信パラメータ</param>
        private void OnComboChanged(object sender, ControlInfoEventArgs e)
        {
            ;
        }
        /// <summary>
        /// ラジオボタンチェンジイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">送信パラメータ</param>
        private void OnRadioButtonChanged(object sender, ControlInfoEventArgs e)
        {
            ;
        }
        /// <summary>
        /// プロパティチェンジイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">送信パラメータ</param>
        private void OnPropertyChanged(object sender, RPUtility.ControlInfoEventArgs e)
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
        protected override void GetNavigateParameters(object parameters)
        {
            // ParamDatasを受け取ります。
            var param = parameters as Common.ParamDatas;
        }
        #endregion INavigationAware
    }
}
