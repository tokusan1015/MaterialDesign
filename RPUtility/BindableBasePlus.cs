using System;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.ObjectModel;
using Common;
using Prism.Ioc;
using System.Windows.Controls;  // assembly:PresentationCore, PresentationFramework
using Prism.Commands;
using Prism.Events;
using System.Reflection;

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
        : BindableBase, INavigationAware, IRegionMemberLifetime    //, INotifyDataErrorInfo
    {
        #region DelegateCommand
        /// <summary>
        /// ロード完了イベント(LoadedCommand)
        /// </summary>
        private DelegateCommand _LoadedCommand;
        public DelegateCommand LoadedCommand =>
            _LoadedCommand = _LoadedCommand ?? new DelegateCommand(Loaded);
        protected abstract void Loaded();
        #endregion DelegateCommand

        #region プロパティ
        /// <summary>
        /// 初期化終了フラグ
        /// </summary>
        public bool InitiazaizuEnd { get; set; } = false;

        #region Prism
        /// <summary>
        /// 画面をキャッシュに保持・破棄フラグを表します。
        /// trueにするとキャッシュに保持されます。
        /// </summary>
        public bool KeepAlive => true;
        /// <summary>
        /// 拡張コンテナを表します。
        /// コンストラクタで取得設定してください。
        /// </summary>
        protected IContainerExtension Container { get; } = null;
        /// <summary>
        /// リージョンマネージャを表します。
        /// コンストラクタで取得して設定してください。
        /// </summary>
        protected IRegionManager RegionManager { get; } = null;
        /// <summary>
        /// イベントアグリゲータを表します。
        /// </summary>
        protected IEventAggregator EventAggregator { get; } = null;
        /// <summary>
        /// リージョンを表します。
        /// </summary>
        protected IRegion Region { get; private set; } = null;
        #endregion Prism

        #region メッセージ
        /// <summary>
        /// メッセージマネージャを表します。
        /// </summary>
        public MessageManager MessageManager { get; } = null;
        #endregion メッセージ

        #region MainView/View共通
        /// <summary>
        /// MainView用ActiveViewName
        /// </summary>
        public string ActiveViewName { get; protected set; } = "";
        /// <summary>
        /// ViewId
        /// 識別用Id
        /// </summary>
        public int ViewId { get; set; } = -1;
        /// <summary>
        /// 識別用MainViewName
        /// </summary>
        public string MainViewName { get; } = "";
        /// <summary>
        /// 識別用ViewName
        /// </summary>
        public string ViewName { get; } = "";
        /// <summary>
        /// View一覧
        /// </summary>
        protected Dictionary<string, ViewInfo> Views { get; } =
            new Dictionary<string, ViewInfo>();
        #endregion MainView/View共通

        #region 共通データ
        /// <summary>
        /// 共通設定を表します。
        /// </summary>
        protected Common.CommonSettings CommonSettings { get; private set; } = null;
        /// <summary>
        /// 共通データを表します。
        /// </summary>
        protected Common.CommonDatas CommonDatas { get; private set; } = null;
        #endregion 共通データ

        #region ControlInfo
        /// <summary>
        /// ControlInfoBaseリスト
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        protected List<RPUtility.ControlInfoBase> CibList { get; } = new List<RPUtility.ControlInfoBase>();
        #endregion　ControlInfo

        #endregion プロパティ

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
        /// <param name="mainViewName">MainViewNameを設定します。</param>
        /// <param name="viewName">ViewNameを設定します。</param>
        protected BindableBasePlus(
            [param: Required]IContainerExtension container,
            [param: Required]IRegionManager regionManager,
            [param: Required]IEventAggregator eventAggregator,
            [param: Required]object commonSettings,
            [param: Required]object commonDatas,
            [param: Required]string mainViewName,
            [param: Required]string viewName
            )
        {
            // 拡張コンテナ、リージョンマネージャ、共通データの取得および設定をします。
            this.Container = container ?? throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(container));
            this.RegionManager = regionManager ?? throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(regionManager));
            this.EventAggregator = eventAggregator ?? throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(eventAggregator));
            this.MainViewName = mainViewName ?? throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(mainViewName));
            this.ViewName = viewName ?? throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(viewName));
            // objectからCommonSettingsにキャストして設定します。
            var cs = commonSettings ?? throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " + nameof(commonSettings));
            this.CommonSettings = cs as Common.CommonSettings;
            // objectからCommonDatasにキャストして設定します。
            var cd = commonDatas ?? throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(commonDatas));
            this.CommonDatas = cd as Common.CommonDatas;

            // メッセージマネージャを設定します。
            this.MessageManager = new MessageManager(
                eventAggregator: eventAggregator,
                receivedMessage: this.ReceivedMessage,
                mainViewName: this.MainViewName,
                viewName: this.ViewName
                );
        }
        #endregion コンストラクタ

        #region リージョン
        /// <summary>
        /// リージョンを設定します。
        /// Loadedイベントで設定してください。
        /// </summary>
        /// <param name="region">リージョンを設定します。</param>
        protected void SetRegion(
            [param: Required]IRegion region
            )
        {
            this.Region = region ?? throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(region));
        }
        /// <summary>
        /// リージョンおよびViewsにViewを登録します。
        /// </summary>
        /// <typeparam name="TView">登録するViewの型を設定します。</typeparam>
        /// <param name="viewName">呼び出し名称を設定します。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected void AddRegionAndViews<TView>(
            [param: Required]string viewName
            ) where TView : UserControl
        {
            // nullチェック
            if (viewName == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(viewName));

            // Viewを取得・登録します。
            var view = this.Container.Resolve<TView>();
            this.Region.Add(view);

            // Viewsに登録します。
            this.Views.Add(
                key: viewName,
                value: new RPUtility.ViewInfo()
                {
                    Value = view
                });
        }
        #endregion リージョン

        #region CommonDatas
        /// <summary>
        /// CommonDatasへデータを保存します。
        /// 保存指示はメッセージにて配信されます。
        /// </summary>
        protected abstract void SaveCommonDatas();
        #endregion CommonDatas

        #region メッセージ
        /// <summary>
        /// メッセージを受信します。
        /// 自分宛のメッセージのみ受信します。
        /// </summary>
        /// <param name="message">受信メッセージ</param>
        protected abstract void ReceivedMessage(RPUtility.IEventParam eventParam);
        #endregion メッセージ

        #region View Activation, Deactivation
        /// <summary>
        /// 対象のViewを表示します。
        /// 表示中のViewを非表示にします。
        /// </summary>
        /// <param name="viewName">ViewNameを設定します。</param>
        public void ChangeShowView(string viewName)
        {
            // 初期値の場合は
            if (this.ActiveViewName.Length <= 0)
            {
                // 現在のViewを非表示にします。
                if (this.Views.ContainsKey(this.ActiveViewName))
                {
                    var uc = this.Views[this.ActiveViewName];
                    this.Region.Deactivate(uc.Value);
                }
            }

            // viewを表示にします。
            if (this.Views.ContainsKey(viewName))
            {
                var uc = this.Views[viewName];
                this.Region.Activate(uc.Value);
                this.ActiveViewName = viewName;
            }
        }
        #endregion View Activation, Deactivation

        #region INavigationAware
        #region OnNavigatedTo
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
        public abstract void GetNavigateParameters(
            [param: Required] object parameters
            );
        #endregion OnNavigatedTo

        #region IsNavigationTarget
        /// <summary>
        /// 移動可能であるかを確認します。
        /// パラメータが存在するかを確認します。
        /// パラメータキーは、引き渡し側と受け取り側で合わせる必要があります。
        /// Key: PARAM_KEY_ALL
        /// </summary>
        /// <param name="navigationContext">ナビゲーションコンテキスト</param>
        /// <param name="key">パラメータキーを設定します。</param>
        /// <returns>存在する場合trueを返します。</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            // 遷移可能フラグ
            var result = false;

            // 画面遷移の実行許可確認を行います。
            var isNavigate = IsNavigate();

            // ナビゲーションコンテキストからパラメータを受け取ります。
            var param = this.GetNavigateParam(
                navigationContext: navigationContext
                );

            // 遷移可能フラグ(必要な場合は、'&&'で連結してください。)
            result = isNavigate && param != null
                // ここに '&& xxxx'で連結します。
                ;

            return result;
        }
        /// <summary>
        /// 画面遷移前に呼び出されます。
        /// 遷移の実行許可確認を行います。
        /// </summary>
        /// <returns>画面遷移可能な場合trueを返します。</returns>
        public abstract bool IsNavigate();
        #endregion IsNavigationTarget

        #region OnNavigatedFrom
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
        /// <returns>パラメータを設定します。</returns>
        public abstract object SetNavigateParameters();
        #endregion OnNavigatedFrom
        #endregion INavigationAware

        #region Navigation
        /// <summary>
        /// パラメータキー
        /// </summary>
        private readonly string PARAM_KEY_ALL = "PARAM_KEY_ALL";

        #region Navigate
        /// <summary>
        /// Prismで画面遷移を行います。
        /// リージョン名、遷移先名の存在チェックはしていません。
        /// </summary>
        /// <param name="regionName">リージョン名を設定します。</param>
        /// <param name="source">遷移先名を設定します。</param>
        public void Navigate(
            [param: Required]string regionName,
            [param: Required]string target
            )
        {
            // nullチェック
            if (regionName == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(regionName));
            if (target == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(target));

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
            [param: Required]string regionName,
            [param: Required]string target,
            NavigationParameters parameters
            )
        {
            // nullチェック
            if (regionName == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(regionName));
            if (target == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(target));

            if (parameters != null)
            {
                this.RegionManager.RequestNavigate(
                    regionName: regionName,
                    target: target,
                    navigationCallback: this.NavigationComplete,
                    navigationParameters: parameters
                    );
            }
        }
        /// <summary>
        /// ナビゲーションが完了したことを知らせます。
        /// </summary>
        /// <param name="result">ナビゲーション結果が返ります。</param>
        public abstract void NavigationComplete(NavigationResult result);
        #endregion Navigate

        #region NavigateParam
        /// <summary>
        /// Prismナビゲーション用の引き渡しパラメタを設定します。
        /// OnNavigatedFromで使用します。
        /// </summary>
        /// <param name="navigationContext">ナビゲーションコンテキストを設定します。</param>
        /// <param name="key">パラメータキーを設定します。</param>
        /// <param name="value">パラメータ値を設定します。</param>
        public void SetNavigateParam(
            [param: Required]NavigationContext navigationContext,
            [param: Required]object value
            )
        {
            // nullチェック
            if (navigationContext == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(navigationContext));

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
            // nullチェック
            if (navigationContext == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + " : " +nameof(navigationContext));

            return navigationContext.Parameters[PARAM_KEY_ALL];
        }
        #endregion NavigateParam
        #endregion Navigation

        #region ControlInfo
        /// <summary>
        /// 対象外以外のボタンのEnableを設定します。
        /// ci が null の場合は全て設定します。
        /// </summary>
        /// <param name="enable">フラグを設定します。</param>
        /// <param name="ci">対象外ControlInfoBaseを設定します。</param>
        public void SetEnables(
            bool enable,
            [param: Required]RPUtility.ControlInfoBase ci
            )
        {
            this.CibList.ForEach(c =>
            {
                if (!c.Equals(ci))
                    c.IsEnable.Value = enable;
            });
        }
        /// <summary>
        /// 全ControlInfoのIsEnableを設定します。
        /// groupNo が負の場合は全て設定します。
        /// </summary>
        /// <param name="enable">フラグを設定します。</param>
        /// <param name="groupNo">グループNoを設定します。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public void SetEnables(
            bool enable,
            int groupNo = -1
            )
        {
            this.CibList.ForEach(c =>
            {
                if (groupNo < 0 || c.GroupNo == groupNo)
                    c.IsEnable.Value = enable;
            });
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
            this.CibList.ForEach(c =>
            {
                if (!c.Equals(ci))
                    c.InverseEnable();
            });
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
            this.CibList.ForEach(c =>
            {
                if (groupNo < 0 || c.GroupNo == groupNo)
                    c.InverseEnable();
            });
        }
        /// <summary>
        /// 対象外以外のボタンのIsVisibleを設定します。
        /// ci が null の場合は全て設定します。
        /// </summary>
        /// <param name="visible">フラグを設定します。</param>
        /// <param name="ci">対象外ControlInfoBaseを設定します。</param>
        public void SetVisibles(
            bool visible,
            [param: Required]RPUtility.ControlInfoBase ci
            )
        {
            this.CibList.ForEach(c =>
            {
                if (!c.Equals(ci))
                    c.IsVisible.Value = visible;
            });
        }
        /// <summary>
        /// 全ControlInfoのIsVisibleを設定します。
        /// groupNo が負の場合は全て設定します。
        /// </summary>
        /// <param name="visible">フラグを設定します。</param>
        /// <param name="groupNo">グループNoを設定します。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public void SetVisibles(
            bool visible,
            int groupNo = -1
            )
        {
            this.CibList.ForEach(c =>
            {
                if (groupNo < 0 || c.GroupNo == groupNo)
                    c.IsVisible.Value = visible;
            });
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
            this.CibList.ForEach(c =>
            {
                if (!c.Equals(ci))
                    c.InverseVisible();
            });
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
            this.CibList.ForEach(c =>
            {
                if (groupNo < 0 || c.GroupNo == groupNo)
                    c.InverseVisible();
            });
        }
        #endregion ControlInfo

        #region Validate
        /// <summary>
        /// 入力項目の検証を行います。
        /// エラーが存在する場合trueを返します。
        /// </summary>
        /// <param name="save">trueの場合エラーが無い場合に保存します。</param>
        /// <returns>エラーが存在する場合trueを返します。</returns>
        public bool CheckValidation(
            bool save
            )
        {
            // 初期化完了チェック
            if (!this.InitiazaizuEnd) return false;
            if (!this.MessageManager.Start) return false;

            // エラーチェック
            var result = this.HasErrors();

            // エラーが無ければ
            if (!result && save)
            {
                // データ保存
                this.SaveCommonDatas();
            }

            // メッセージ送信
            var command = result ?
                EnumDatas.MassageInfo.InputError
                : EnumDatas.MassageInfo.NoInputError;
            var eventParam = new MessageInfoSend()
            {
                Reciever = this.MainViewName,
                Sender = this.ViewName,
                Command = command,
                Message = ""
            };
            this.MessageManager.SendMessage(eventParam: eventParam);

            return result;
        }

        /// <summary>
        /// CIBListに入力エラーが存在する場合trueを返します。
        /// </summary>
        /// <returns>CIBListに入力エラーが存在する場合trueを返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:ローカライズされるパラメーターとしてリテラルを渡さない", MessageId = "System.Console.WriteLine(System.String)")]
        private bool HasErrors()
        {
            bool result = false;

            foreach (var cib in this.CibList)
            {
                var err = cib.HasError();
#if DEBUG
                if (err)
                {
                    // エラー有り
                    Console.WriteLine($"{this.ViewName} : Input Error {cib.PropertyName} = {cib.GetValue()}");
                }
#endif
                result |= err;
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

    /// <summary>
    /// View情報クラス
    /// </summary>
    public class ViewInfo
    {
        /// <summary>
        /// Viewを表します。
        /// </summary>
        public UserControl Value { get; set; } = null;
        /// <summary>
        /// 入力エラーを表します。
        /// </summary>
        public bool InputError { get; set; } = true;
    }
}
