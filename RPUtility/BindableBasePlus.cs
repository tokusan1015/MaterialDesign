using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RPUtility
{
    /// <summary>
    /// BindableBaseのラップクラスを表します。
    /// 画面遷移時のパラメータの受け渡しおよび共通データ取得記述を軽減します。
    /// CommonDatas, ControlInfoと連携します。
    /// 継承して使用します。
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public abstract class BindableBasePlus
        : BindableBase, IBindableBasePlus, INavigationAware
        //, INotifyDataErrorInfo
    {
        #region protectedプロパティ
        /// <summary>
        /// ViewName
        /// </summary>
        public string ViewName { get; protected set; }
        /// <summary>
        /// リージョンマネージャを表します。
        /// コンストラクタで取得して設定してください。
        /// </summary>
        protected IRegionManager RegionManager { get; private set; }
        /// <summary>
        /// 共通データオブジェクトを表します。
        /// MainWindowViewModelでインスタンスを生成する必要があります。
        /// </summary>
        protected object CommonDatas { get; private set; }
        /// <summary>
        /// ControlInfoBaseリスト
        /// </summary>
        protected List<RPUtility.ControlInfoBase> CIBList { get; } = new List<RPUtility.ControlInfoBase>();
        #endregion protectedプロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// 引数を追加することによりPrismに対して各インスタンスを要求しています。
        /// CommonDatasは、App.xaml.csのRegisterTypesにて登録されます。
        /// </summary>
        /// <param name="regionManager">リージョンマネージャを設定します。</param>
        /// <param name="commonDatas">共通データを設定します。</param>
        protected BindableBasePlus(
            [param: Required]IRegionManager regionManager,
            [param: Required]object commonDatas
            )
        {
            // リージョンマネージャ、共通データを取得および設定します。
            this.RegionManager = regionManager;
            this.CommonDatas = commonDatas;
        }
        #endregion コンストラクタ

        #region INavigationAware
        /// <summary>
        /// パラメータ(ParamDatas)を受け取ります。
        /// 画面遷移後に呼び出されます。
        /// パラメータキーは、引き渡し側と受け取り側で合わせる必要があります。
        /// </summary>
        /// <param name="navigationContext">ナビゲーションコンテキスト</param>
        public void OnNavigatedTo(
            [param: Required]NavigationContext navigationContext
            )
        {
            // ナビゲーションコンテキストからパラメータを受け取ります。
            var parameters = this.GetNavigateParam(
                navigationContext: navigationContext
                );

            // パラメータが取得可能な場合はGetNavigateParametersを呼び出します。
            if (parameters != null)
                this.GetNavigateParameters(parameters: parameters);
        }
        /// <summary>
        /// パラメータの取得を行います。
        /// 画面遷移後に呼び出されます。
        /// </summary>
        /// <param name="parameters">パラメータを表します。</param>
        protected abstract void GetNavigateParameters(
            [param: Required] object parameters
            );
        /// <summary>
        /// パラメータが存在するか確認します。
        /// パラメータキーは、引き渡し側と受け取り側で合わせる必要があります。
        /// Key: PARAM_KEY_ALL
        /// </summary>
        /// <param name="navigationContext">ナビゲーションコンテキスト</param>
        /// <param name="key">パラメータキーを設定します。</param>
        /// <returns>存在する場合trueを返します。</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            // ナビゲーションコンテキストからパラメータを受け取ります。
            var param = this.GetNavigateParam(
                navigationContext: navigationContext
                );
            return param != null;
        }
        /// <summary>
        /// パラメタを引き渡します。
        /// 画面遷移前に呼び出されます。
        /// パラメータキーは、引き渡し側と受け取り側で合わせる必要があります。
        /// </summary>
        /// <param name="navigationContext">ナビゲーションコンテキスト</param>
        public void OnNavigatedFrom(
            [param: Required]NavigationContext navigationContext
            )
        {
            // パラメータの設定の為にSetNavigateParametersを呼び出します。
            var parameters = this.SetNavigateParameters();

            // パラメータが設定されている場合はパラメータを引き渡します。
            if (parameters != null)
            {
                this.SetNavigateParam(
                    navigationContext: navigationContext,
                    value: parameters
                    );
            }
        }
        /// <summary>
        /// パラメータの設定を行います。
        /// 画面が遷移前に呼び出されます。
        /// </summary>
        protected abstract object SetNavigateParameters();
        #endregion INavigationAware

        #region Navigate
        /// <summary>
        /// パラメータキー
        /// </summary>
        public readonly string PARAM_KEY_ALL = "PARAM_KEY_ALL";

        /// <summary>
        /// Prismで画面遷移を行います。
        /// リージョン名、遷移先名の存在チェックはしていません。
        /// </summary>
        /// <param name="regionName">リージョン名を設定します。</param>
        /// <param name="source">遷移先名を設定します。</param>
        public void Navigate(
            string regionName,
            string target
            )
        {
            // 画面遷移を実行します。
            this.RegionManager.RequestNavigate(
                regionName: regionName,
                source: target
                );
        }

        /// <summary>
        /// Prismでパラメタ付きで画面遷移を行います。
        /// リージョン名、遷移先名の存在チェックはしていません。
        /// パラメータの受取はOnNavigatedToで行います。
        /// </summary>
        /// <param name="regionName">リージョン名を設定します。</param>
        /// <param name="target">遷移先名を設定します。</param>
        /// <param name="parameters">パラメータを設定します。</param>
        public void Navigate(
            string regionName,
            string target,
            NavigationParameters parameters
            )
        {
            if (parameters != null)
            {
                this.RegionManager.RequestNavigate(
                    regionName: regionName,
                    target: target,
                    navigationParameters: parameters
                    );
            }
        }

        /// <summary>
        /// Prismナビゲーション用の引き渡しパラメタを設定します。
        /// OnNavigatedFromで使用します。
        /// </summary>
        /// <param name="navigationContext">ナビゲーションコンテキストを設定します。</param>
        /// <param name="key">パラメータキーを設定します。</param>
        /// <param name="value">パラメータ値を設定します。</param>
        public void SetNavigateParam(
            [param: Required]NavigationContext navigationContext,
            object value
            )
        {
            // nullチェック
            if (value != null)
            {
                // パラメタを設定します。
                navigationContext.Parameters.Add(
                    key: PARAM_KEY_ALL,
                    value: value
                    );
            }
        }

        /// <summary>
        /// Prismナビゲーションコンテキストからパラメタを受け取ります。
        /// OnNavigatedToで使用します。
        /// Key: PARAM_KEY_ALL
        /// </summary>
        /// <param name="navigationContext">ナビゲーションコンテキストを設定します。</param>
        /// <param name="key">パラメータキーを設定します。</param>
        /// <returns>パラメタ値が返ります。</returns>
        public object GetNavigateParam(
            [param: Required]NavigationContext navigationContext
            )
        {
            return navigationContext.Parameters[PARAM_KEY_ALL];
        }
        #endregion Navigate

        #region ControlInfo
        /// <summary>
        /// 対象外以外のボタンのEnableを設定します。
        /// ci が null の場合は全て設定します。
        /// </summary>
        /// <param name="flag">フラグを設定します。</param>
        /// <param name="ci">対象外ControlInfoBaseを設定します。</param>
        public void SetEnables(
            bool flag,
            [param: Required]RPUtility.ControlInfoBase ci
            )
        {
            this.CIBList.ForEach(c =>
            {
                if (!c.Equals(ci))
                    c.IsEnable.Value = flag;
            });
            /*
            foreach (var c in this.CIBList)
            {
                if (!c.Equals(ci))
                    c.IsEnable.Value = flag;
            }
            */
        }
        /// <summary>
        /// 全ControlInfoのIsEnableを設定します。
        /// groupNo が負の場合は全て設定します。
        /// </summary>
        /// <param name="flag">フラグを設定します。</param>
        /// <param name="groupNo">グループNoを設定します。</param>
        public void SetEnables(
            bool flag,
            int groupNo = -1
            )
        {
            this.CIBList.ForEach(c =>
            {
                if (groupNo < 0 || c.GroupNo == groupNo)
                    c.IsEnable.Value = flag;
            });
            /*
            foreach (var c in this.CIBList)
            {
                if (groupNo < 0 || c.GroupNo == groupNo)
                    c.IsEnable.Value = flag;
            }
            */
        }
        /// <summary>
        /// 対象外以外のボタンのEnableを反転します。
        /// ci が null の場合は全て反転します。
        /// </summary>
        /// <param name="ci">対象外ControlInfoBaseを設定します。</param>
        public void InvertEnables(
            [param: Required]RPUtility.ControlInfoBase ci
            )
        {
            /*
            // 末尾の.ToList(),.ToArray()が無いと動きません。
            this.CIBList
                .Where(c => !c.Equals(ci))
                .Select(c => { c.InverseEnable(); return c; })
                .ToArray();
            */
            this.CIBList.ForEach(c =>
            {
                if (!c.Equals(ci))
                    c.InverseEnable();
            });
            /*
            foreach (var c in this.CIBList)
            {
                if (!c.Equals(ci))
                    c.InverseEnable();
            }
            */
        }
        /// <summary>
        /// 対象groupNoのボタンのEnableを反転します。
        /// groupNo が 負の場合は全て反転します。
        /// </summary>
        /// <param name="groupNo">対象groupNoを設定します。</param>
        public void InvertEnables(
            int groupNo
            )
        {
            this.CIBList.ForEach(c =>
            {
                if (groupNo < 0 || c.GroupNo == groupNo)
                    c.InverseEnable();
            });
            /*
            foreach (var c in this.CIBList)
            {
                if (groupNo < 0 || c.GroupNo == groupNo)
                    c.InverseEnable();
            }
            */
        }
        /// <summary>
        /// 対象外以外のボタンのIsVisibleを設定します。
        /// ci が null の場合は全て設定します。
        /// </summary>
        /// <param name="flag">フラグを設定します。</param>
        /// <param name="ci">対象外ControlInfoBaseを設定します。</param>
        public void SetVisibles(
            bool flag,
            [param: Required]RPUtility.ControlInfoBase ci
            )
        {
            this.CIBList.ForEach(c =>
            {
                if (!c.Equals(ci))
                    c.IsVisible.Value = flag;
            });
            /*
            foreach (var c in this.CIBList)
            {
                if (!c.Equals(ci))
                    c.IsVisible.Value = flag;
            }
            */
        }
        /// <summary>
        /// 全ControlInfoのIsVisibleを設定します。
        /// groupNo が負の場合は全て設定します。
        /// </summary>
        /// <param name="flag">フラグを設定します。</param>
        /// <param name="groupNo">グループNoを設定します。</param>
        public void SetVisibles(
            bool flag,
            int groupNo = -1
            )
        {
            this.CIBList.ForEach(c =>
            {
                if (groupNo < 0 || c.GroupNo == groupNo)
                    c.IsVisible.Value = flag;
            });
            /*
            foreach (var c in this.CIBList)
            {
                if (groupNo < 0 || c.GroupNo == groupNo)
                    c.IsVisible.Value = flag;
            }
            */
        }
        /// <summary>
        /// 対象外以外のボタンのVisibleを反転します。
        /// ci が null の場合は全て反転します。
        /// </summary>
        /// <param name="ci">対象外ControlInfoBaseを設定します。</param>
        public void InvertVisible(
            [param: Required]RPUtility.ControlInfoBase ci
            )
        {
            this.CIBList.ForEach(c =>
            {
                if (!c.Equals(ci))
                    c.InverseVisible();
            });
            /*
            foreach (var c in this.CIBList)
            {
                if (!c.Equals(ci))
                    c.InverseVisible();
            }
            */
        }
        /// <summary>
        /// 対象groupNoのボタンのVisibleを反転します。
        /// ci が負の場合は全て反転します。
        /// </summary>
        /// <param name="groupNo">対象groupNoを設定します。</param>
        public void InvertVisible(
            int groupNo
            )
        {
            this.CIBList.ForEach(c =>
            {
                if (groupNo < 0 || c.GroupNo == groupNo)
                    c.InverseVisible();
            });
            /*
            foreach (var c in this.CIBList)
            {
                if (groupNo < 0 || c.GroupNo == groupNo)
                    c.InverseVisible();
            }
            */
        }
        #endregion ControlInfo

        #region Validate
        /// <summary>
        /// CIBListに入力エラーが存在する場合trueを返します。
        /// </summary>
        /// <returns>CIBListに入力エラーが存在する場合trueを返します。</returns>
        public bool HasErrors()
        {
            bool result = false;

            foreach (var cib in this.CIBList)
            {
                result |= cib.HasError();
            }

            return result;
        }
        #endregion Validate

        #region INotifyDataErrorInfo
        /*
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public bool HasErrors => throw new NotImplementedException();


        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }
        */
        #endregion INotifyDataErrorInfo
    }
}
