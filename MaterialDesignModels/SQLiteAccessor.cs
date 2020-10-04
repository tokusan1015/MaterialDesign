namespace MaterialDesignModels
{
    /// <summary>
    /// SQLiteAccess
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class SQLiteAccessor : SQLiteAccessorBase.SQLiteSqlAccessorBase
    {
        /// <summary>
        /// 共通エラーメッセージ
        /// </summary>
        private const string ERR_MESSAGE = "SQLiteAccess内でエラーが発生しました。";

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataSource">データソース</param>
        public SQLiteAccessor(
            string dataSource = ""
            ) : base(dataSource)
        {
        }
        #endregion コンストラクト

        #region レコードの検索
        /// <summary>
        /// レコード検索の例
        /// </summary>
        /// <returns>検索結果を返します。</returns>
        public void Select_Example()
        {

        }
        #endregion レコードの検索
    }
}
