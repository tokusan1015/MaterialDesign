using MaterialDesignViews.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.ComponentModel.DataAnnotations;

namespace MaterialDesignViews
{
    /// <summary>
    /// モジュール
    /// 画面(ユーザコントロール)を管理します。
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class MDesignModule : IModule
    {
        /// <summary>
        /// 初期化処理
        /// BaseSystemより呼び出されます。
        /// </summary>
        /// <param name="containerProvider">登録先コンテナレジストリを設定します。</param>
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        /// <summary>
        /// 画面用ユーザコントロール登録処理
        /// BaseSystemより呼び出されます。
        /// 画面遷移は画面用ユーザコントロールを切り替える事により行います。
        /// </summary>
        /// <param name="containerRegistry">登録先コンテナレジストリを設定します。</param>
        public void RegisterTypes(
            [param: Required]IContainerRegistry containerRegistry
            )
        {
            // 画面遷移画面用ユーザコントロール追加
            containerRegistry.RegisterForNavigation<ViewA>();
            containerRegistry.RegisterForNavigation<ViewB>();
            containerRegistry.RegisterForNavigation<ViewC>();
        }
    }
}