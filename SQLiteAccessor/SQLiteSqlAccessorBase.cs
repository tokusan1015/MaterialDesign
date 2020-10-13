using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SQLite;
using System.Reflection;

namespace SQLiteAccessorBase
{
    /// <summary>
    /// SQLiteSqlAccessorBase
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class SQLiteSqlAccessorBase : SQLiteAccessBase
    {
        #region プロパティ
        /// <summary>
        /// ERRORメッセージ
        /// </summary>
        private readonly string ERROR = "SQLiteSqlAccessorBaseで例外が発生しました。";
        /// <summary>
        /// SQLiteDataReader
        /// </summary>
        public SQLiteDataReader DataReader { get; private set; } = null;
        #endregion プロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DataSource">データソースを設定します。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public SQLiteSqlAccessorBase(
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
            this.ConnectionClose();
            this.Dispose_DataReader();

            base.Dispose(disposing);
        }
        /// <summary>
        /// DataReader破棄処理
        /// </summary>
        private void Dispose_DataReader()
        {
            if (this.DataReader != null)
            {
                this.DataReader.Close();
                this.DataReader = null;
            }
        }
        #endregion 破棄処理

        #region メソッド
        /// <summary>
        /// SQLを発行します。
        /// INSERT, UPDATE, DELETE
        /// コネクションを接続後に呼び出してください。
        /// </summary>
        /// <param name="query">クエリデータリストを設定します。</param>
        /// <returns>処理件数を返します。</returns>
        public int ExecuteNonQuery(
            [param: Required]ICollection<Utility.QueryData> queryDatas
            ) 
        {
            // nullチェック
            if (queryDatas == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(queryDatas));

            // 処理可能か調べます。
            this.CheckExecuteConnection();

            int result = 0;

            // トランザクション
            SQLiteTransaction transaction = null;

            // SQLiteコマンドを生成します。
            using (var cmd = new SQLiteCommand(this.Connection))
            {
                // トランザクションを開始します。
                transaction = this.Connection.BeginTransaction();

                try
                {
                    // queryDatasの件数分処理します。
                    foreach (var queryData in queryDatas)
                    {
                        // コマンドにQueryDataを設定します。
                        this.SetSQLiteCommandToQueryData(cmd: cmd, queryData: queryData);

                        // クエリを実行します。
                        result += cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // トランザクションをロールバックします。
                    transaction.Rollback();
                    // 破棄処理
                    this.Dispose();
                    // 例外を発生します。
                    throw new SQLiteAccessorException(
                        message: $"レコード挿入,削除,更新に失敗しました。\n",
                        innerException: ex
                        );
                }
                // トランザクションをコミットします。
                transaction.Commit();
            }
            // 処理した件数を返します。
            return result;
        }
        /// <summary>
        /// SQLを発行します。
        /// SELECT
        /// コネクションの接続後に呼び出してください。
        /// </summary>
        /// <param name="queryData">クエリデータを設定します。</param>
        public void ExecuteQuery(
            [param: Required]Utility.QueryData queryData
            )
        {
            // 処理可能か調べます。
            this.CheckExecuteConnection();

            // SQLiteコマンドを生成します。
            using (var cmd = new SQLiteCommand(this.Connection))
            {
                try
                {
                    // SQLiteCommandにQueryDataを設定します。
                    this.SetSQLiteCommandToQueryData(cmd: cmd, queryData: queryData);

                    // クエリを実行します。
                    this.DataReader = cmd.ExecuteReader();
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
        }
        #endregion メソッド
    }
}
