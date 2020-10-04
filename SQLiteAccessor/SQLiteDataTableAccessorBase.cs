using System;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SQLite;

namespace SQLiteAccessorBase
{
    /// <summary>
    /// SQLiteDataTableAccessor
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class SQLiteDataTableAccessorBase : SQLiteAccessBase
    {
        #region プロパティ
        /// <summary>
        /// 共通エラーメッセージ
        /// </summary>
        private const string ERROR = "SQLiteDataTableAccessorBase内でエラーが発生しました。";
        /// <summary>
        /// テーブル名
        /// </summary>
        protected string TableName { get; private set; }
        /// <summary>
        /// データテーブル
        /// </summary>
        protected DataTable DataTable { get; private set; } = null;
        /// <summary>
        /// データアダプタ
        /// </summary>
        protected SQLiteDataAdapter DataAdapter { get; private set; } = null;
        #endregion プロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataSource">データソースを設定します。</param>
        public SQLiteDataTableAccessorBase(
            string dataSource = ""
            ) : base(dataSource: dataSource)
        {
            ;
        }
        #endregion コンストラクタ

        #region 破棄処理
        /// <summary>
        /// 破棄処理
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            // 破棄処理
            this.ConnectionClose();
            this.Dispose_DataTable();
            this.Dispose_DataAdapter();

            base.Dispose(disposing);
        }
        /// <summary>
        /// データテーブル開放
        /// </summary>
        private void Dispose_DataTable()
        {
            if (this.DataTable != null)
            {
                this.DataTable.Clear();
                this.DataTable.Dispose();
                this.DataTable = null;
            }
        }
        /// <summary>
        /// データアダプタ開放
        /// </summary>
        private void Dispose_DataAdapter()
        {
            if (this.DataAdapter != null)
            {
                this.DataAdapter.Dispose();
                this.DataAdapter = null;
            }
        }
        #endregion 破棄処理

        #region メソッド
        /// <summary>
        /// データテーブルを生成します。
        /// </summary>
        private void CreateDataTable()
        {
            // データテーブルを破棄します。
            this.Dispose_DataTable();

            this.DataTable = new DataTable();
        }

        /// <summary>
        /// DataTableにデータを設定します。
        /// データを取得するSQL文を設定してください。
        /// 尚、"{0}"は、テーブル名固定となります。
        /// </summary>
        /// <param name="sqlString">データを取得するSQL文を設定します。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:SQL クエリのセキュリティ脆弱性を確認")]
        public void SetDataTable<TTable>(
            string sqlString = @"SELECT * FROM {0};"
            ) where TTable : class
        {
            // データアダプタを開放します。
            this.Dispose_DataAdapter();

            // データテーブルを開放します。
            this.Dispose_DataTable();

            // テーブル名を取得します。
            this.TableName = Utility.AttributeUtil
                .GetClassAttribute<TableAttribute>(typeof(TTable)).Name;

            // SQL文を生成します。
            var sql = string.Format(sqlString, this.TableName);

            try
            {
                // データテーブルを生成します。
                this.DataTable = new DataTable();

                // データアダプタを設定します。
                this.DataAdapter =
                    new SQLiteDataAdapter(sql, this.Connection);

                // データアダプタとデータテーブルを紐づけます。
                this.DataAdapter.Fill(this.DataTable);
            }
            catch (Exception ex)
            {
                // 破棄処理
                this.Dispose();
                // 例外を発生します。
                throw new SQLiteAccessorException(
                    message: ERROR,
                    innerException: ex
                    );
            }
        }
        #endregion メソッド
    }
}
