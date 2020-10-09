using MaterialDesign.Views;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MaterialDesign
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// メインウィンドウを生成します。
        /// MainWindowとMainWindowViewModelは自動的に紐づけられます。
        /// (〇〇と〇〇ViewModelを紐づけます。名前付けに注意しましょう。)
        /// </summary>
        /// <returns>Windowを返します。</returns>
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        /// <summary>
        /// 共通データを登録します。
        /// </summary>
        /// <param name="containerRegistry">コンテナレジストリを設定します。</param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 共通データ(CommonDatas)を登録します。
            containerRegistry.RegisterInstance<Common.CommonDatas>(new Common.CommonDatas());
        }

        /// <summary>
        /// モジュールを登録します。
        /// </summary>
        /// <param name="moduleCatalog">モジュールカタログを設定します。</param>
        protected override void ConfigureModuleCatalog(
            IModuleCatalog moduleCatalog
            )
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            // モジュールを登録します。
            moduleCatalog.AddModule<MaterialDesignViews.MDesignModule>();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public App()
        {
            // 各種例外を購読します。
            // メッセージボックスが表示される場合はプログラムの改修が必要となります。
#if DEBUG
            // マネージコード内で例外がスローされると最初に必ず発生します。（.NET 4.0より）
            AppDomain.CurrentDomain.FirstChanceException
              += App_FirstChanceException;

            // UIスレッドで実行されているコードで処理されない場合に発生します。（.NET 3.0より）
            this.DispatcherUnhandledException
                += App_DispatcherUnhandledException;

            // バックグラウンドタスク内で処理されない場合に発生します。（.NET 4.0より）
            TaskScheduler.UnobservedTaskException
              += App_UnobservedTaskException;

            // 例外が処理されない場合発生します。（.NET 1.0より）
            AppDomain.CurrentDomain.UnhandledException
              += App_UnhandledException;
#endif
        }

        #region Exception
        /// <summary>
        /// マネージコード内の例外発生イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">送信パラメータ</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:ローカライズされるパラメーターとしてリテラルを渡さない", MessageId = "System.Windows.MessageBox.Show(System.String,System.String,System.Windows.MessageBoxButton,System.Windows.MessageBoxImage)")]
        private void App_FirstChanceException(
            object sender, 
            FirstChanceExceptionEventArgs e
            )
        {
            if (e.Exception == null || e.Exception?.TargetSite == null)
            {
                MessageBox.Show("System.Exceptionとして扱えない例外です。");
                return;
            }

            // メッセージ取得
            var targetSiteName = e.Exception.TargetSite.Name;
            var message = e.Exception.Message;

            // メッセージボックス表示して続行します。
            MessageBox.Show(
                messageBoxText:
                    $"例外が{targetSiteName}で発生しました。\n"
                    + $"エラーメッセージ：{message}\n"
                    + "プログラムを続行します。",
                caption: "FirstChanceException",
                button: MessageBoxButton.OK, 
                icon: MessageBoxImage.Information
                );
        }
        /// <summary>
        /// UIスレッド内の例外発生イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">送信パラメータ</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:ローカライズされるパラメーターとしてリテラルを渡さない", MessageId = "System.Windows.MessageBox.Show(System.String,System.String,System.Windows.MessageBoxButton,System.Windows.MessageBoxImage)")]
        private void App_DispatcherUnhandledException(
            object sender, 
            DispatcherUnhandledExceptionEventArgs e
            )
        {
            // メッセージ取得
            var targetSiteName = e.Exception.TargetSite.Name;
            var message = e.Exception.Message;

            // メッセージボックスを表示して続行判断を仰ぎます。
            e.Handled = MessageBox.Show(
                messageBoxText:
                    $"例外が{targetSiteName}で発生しました。\n"
                    + $"エラーメッセージ：{message}\n"
                    + "プログラムを続行しますか？",
                caption: "DispatcherUnhandledException",
                button: MessageBoxButton.YesNo,
                icon: MessageBoxImage.Warning
                ) == MessageBoxResult.Yes ? true : false;
        }

        /// <summary>
        /// バックグラウンドタスク内の例外発生イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">送信パラメータ</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:ローカライズされるパラメーターとしてリテラルを渡さない", MessageId = "System.Windows.MessageBox.Show(System.String,System.String,System.Windows.MessageBoxButton,System.Windows.MessageBoxImage)")]
        private void App_UnobservedTaskException(
            object sender,
            UnobservedTaskExceptionEventArgs e
            )
        {
            // ExceptionObjectを取得します。
            var targetSiteName = e.Exception.InnerException.TargetSite.Name;
            var message = e.Exception.InnerException.Message;

            // メッセージボックスを表示して続行判断を仰ぎます。
            if (MessageBox.Show(
                messageBoxText:
                    $"例外がバックグラウンドタスクの{targetSiteName}で発生しました。\n"
                    + $"エラーメッセージ：{message}\n"
                    + "プログラムを続行しますか？",
                caption: "UnobservedTaskException",
                button: MessageBoxButton.YesNo,
                icon: MessageBoxImage.Warning
                ) == MessageBoxResult.Yes)
                e.SetObserved();
        }

        /// <summary>
        /// 例外が処理されない場合に発生するイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">送信パラメータ</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:ローカライズされるパラメーターとしてリテラルを渡さない", MessageId = "System.Windows.MessageBox.Show(System.String,System.String,System.Windows.MessageBoxButton,System.Windows.MessageBoxImage)")]
        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // ExceptionObjectを取得します。
            if (!(e.ExceptionObject is Exception exception))
            {
                MessageBox.Show("System.Exceptionとして扱えない例外です。");
                return;
            }

            // メッセージ取得
            var targetSiteName = exception.TargetSite.Name;
            var message = exception.Message;

            // メッセージボックスを表示してプログラムを終了します。
            MessageBox.Show(
                messageBoxText:
                    $"例外が{targetSiteName}で発生しました。\n"
                    + $"エラーメッセージ：{message}\n"
                    + "プログラムは終了します。",
                caption: "UnhandledException",
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Stop);
            Environment.Exit(0);
        }
        #endregion Exception 
    }
}
