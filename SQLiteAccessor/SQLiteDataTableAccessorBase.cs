using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SQLite;
using System.Globalization;
using System.Reflection;

namespace SQLiteAccessorBase
{
    /// <summary>
    /// SQLiteDataTableAccessor
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class SQLiteDataTableAccessorBase : SQLiteAccessBase, IDisposable
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
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
        /// <param name="disposing">disposeフラグ</param>
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

            this.DataTable = new DataTable
            {
                Locale = CultureInfo.InvariantCulture
            };
        }

        /// <summary>
        /// DataTableにデータを設定します。
        /// データを取得するSQL文を設定してください。
        /// 尚、"{0}"は、テーブル名固定となります。
        /// </summary>
        /// <param name="tableClassType">テーブル構造定義クラスタイプを設定します。</param>
        /// <param name="sqlText">データを取得するSQL文を設定します。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:SQL クエリのセキュリティ脆弱性を確認")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public void SetDataTable(
            [param: Required]MemberInfo tableClassType,
            string sqlText = @"SELECT * FROM {0};"
            )
        {
            // nullチェック
            if (tableClassType == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(tableClassType));
            if (sqlText == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(sqlText));

            // データアダプタを開放します。
            this.Dispose_DataAdapter();

            // データテーブルを開放します。
            this.Dispose_DataTable();

            // テーブル名を取得します。
            this.TableName = Utility.AttributeTable
                .GetTableName(tableClassType: tableClassType);

            // SQL文を生成します。
            var sql = string.Format(sqlText, this.TableName);

            try
            {
                // データテーブルを生成します。
                this.DataTable = new DataTable
                {
                    Locale = CultureInfo.InvariantCulture
                };

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
