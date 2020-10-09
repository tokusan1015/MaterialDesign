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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public SQLiteAccessor(
            string dataSource = ""
            ) : base(dataSource)
        {

        }
        #endregion コンストラクト

        #region レコードの検索
        #endregion レコードの検索
    }
}
