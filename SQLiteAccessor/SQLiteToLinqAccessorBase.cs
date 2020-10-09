using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Data.SQLite;
using System.Linq;

namespace SQLiteAccessorBase
{
    /// <summary>
    /// SQLiteToLinqAccessorBase
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class SQLiteToLinqAccessorBase : SQLiteAccessBase
    {
        #region プロパティ
        /// <summary>
        /// 共通エラーメッセージ
        /// </summary>
        private const string ERROR = "SQLiteToLinqAccessorBase内でエラーが発生しました。";
        #endregion プロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// デフォルトデータソース：./SQLiteDB.db
        /// </summary>
        /// <param name="dataSource">データソースを設定します。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public SQLiteToLinqAccessorBase(
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
        /// <param name="disposing">破棄フラグ</param>
        protected override void Dispose(bool disposing)
        {
            this.ConnectionClose();

            base.Dispose(disposing);
        }
        #endregion 破棄処理


        #region バージョン取得
        /// <summary>
        /// SQLiteのバージョンを取得します。
        /// コネクションを接続してから呼び出してください。
        /// </summary>
        /// <returns>バージョン情報を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public string GetVersion()
        {

            // SQLiteのバージョンを取得するSQL文
            const string SqlVersion = "select sqlite_version()";

            // 戻り値(バージョン情報が入ります。)
            string result = "";

            try
            {
                // ここにデータベース処理コードを書く
                using (SQLiteCommand cmd = new SQLiteCommand(this.Connection))
                {
                    cmd.CommandText = SqlVersion;
                    result = cmd.ExecuteScalar().ToString();
                }

                return result;
            }
            catch (Exception ex)
            {
                // 破棄処理
                this.Dispose();
                // 例外発生
                throw new SQLiteAccessorException(
                    message: ERROR,
                    innerException: ex
                    );
            }
        }
        #endregion バージョン取得

        #region テーブル生成
        /// <summary>
        /// テーブルを生成します。
        /// コネクションを接続してから呼び出してください。
        /// </summary>
        /// <param name="queryData">QueryDataを設定します。</param>
        /// <returns>処理件数を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public int CreateTable(
            [param: Required]Utility.QueryData queryData,
            bool makeSql = true
            )
        {
            // nullチェック
            if (queryData == null)
                throw new ArgumentNullException("queryData");

            try
            {
                int result = -1;

                // コマンドを生成します。
                using (SQLiteCommand cmd = new SQLiteCommand(this.Connection))
                {
                    // SQL文を自動生成します。
                    if (makeSql) queryData.MakeSqliteCreateTableSql();

                    // SQLiteCommandにQueryDataを設定します。
                    this.SetSQLiteCommandToQueryData(cmd: cmd, queryData: queryData);

                    // クエリを実行します。
                    result = cmd.ExecuteNonQuery();
                }

                return result;
            }
            catch (Exception ex)
            {
                // 破棄処理
                this.Dispose();
                // 例外発生
                throw new SQLiteAccessorException(
                    message: ERROR,
                    innerException: ex
                    );
            }
        }
        #endregion テーブル作成

        #region レコードの挿入
        /// <summary>
        /// テーブル構造定義クラスを使用してレコードの挿入を行います。
        /// コネクションを接続してから呼び出してください。
        /// </summary>
        /// <typeparam name="TTable">テーブル構造定義クラスを設定します。</typeparam>
        /// <param name="insertList">テーブル構造定義クラスリストを設定します。</param>
        public void Insert<TTable>(
            [param: Required]IReadOnlyCollection<TTable> insertList
            ) where TTable : class
        {
            try
            {
                // ここにデータベース処理コードを書く
                using (DataContext context = new DataContext(this.Connection))
                {
                    Table<TTable> table = context.GetTable<TTable>();

                    table.InsertAllOnSubmit(insertList);
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                // 破棄処理
                this.Dispose();
                // 例外発生
                throw new SQLiteAccessorException(
                    message: ERROR,
                    innerException: ex
                    );
            }
        }
        /// <summary>
        /// SQL文を使用してレコードの挿入を行います。
        /// Linq to SQLite でAUTOINCREMENTの不具合対応の為、
        /// InsertのみSQL文で処理します。
        /// </summary>
        /// <param name="queryData">QueryDataを設定します。</param>
        /// <param name="makeSql">SQL文の自動生成を設定します。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public void InsertSql(
            [param: Required]IEnumerable<Utility.QueryData> queryDatas,
            bool makeSql = true
            )
        {
            // nullチェック
            if (queryDatas == null)
                throw new ArgumentNullException("queryDatas");

            try
            {
                // トランザクション
                SQLiteTransaction transaction = null;

                // ここにデータベース処理コードを書く
                using (SQLiteCommand cmd = new SQLiteCommand(this.Connection))
                {
                    // トランザクションを開始します。
                    transaction = this.Connection.BeginTransaction();

                    try
                    {
                        foreach (var queryData in queryDatas)
                        {
                            // SQLを自動生成します。
                            if (makeSql) queryData.MakeInsertSql();

                            // SQLiteCommandにQueryDataを設定します。
                            this.SetSQLiteCommandToQueryData(cmd: cmd, queryData: queryData);

                            // クエリを実行します。
                            var result = cmd.ExecuteNonQuery();

                            // データ更新できない場合
                            if (result != 1)
                            {
                                // ロールバックします。
                                transaction.Rollback();
                                throw new InvalidOperationException($"レコード挿入に失敗しました。\n SQL={queryData.Query}\n");
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // トランザクションが有効な場合
                        if (transaction != null)
                        {
                            // ロールバックします。
                            transaction.Rollback();
                        }
                        throw;
                    }
                }

                // コミットします。
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // 破棄処理
                this.Dispose();
                // 例外発生
                throw new SQLiteAccessorException(
                    message: ERROR,
                    innerException: ex
                    );
            }
        }
        #endregion レコードの挿入

        #region レコードの参照
        /// <summary>
        /// テーブル構造定義クラスを使用してレコードの参照を行います。
        /// </summary>
        /// <typeparam name="TTable">テーブル構造定義クラスを設定します。</typeparam>
        /// <param name="action">レコードの検索関数(Linq to SQLite)を設定します。</param>
        /// <returns>結果をIQueryable形式で返します。</returns>
        public void Select<TTable>(
            [param: Required]Action<Table<TTable>> action
            ) where TTable : class
        {
            // nullチェック
            if (action == null)
                throw new ArgumentNullException("action");

            try
            {
                // ここにデータベース処理コードを書く
                using (DataContext context = new DataContext(this.Connection))
                {
                    // オブジェクトのコレクションを返します。
                    Table<TTable> table = context.GetTable<TTable>();

                    // Selectを実行します。
                    action(table);                        
                }
            }
            catch (Exception ex)
            {
                // 破棄処理
                this.Dispose();
                // 例外発生
                throw new SQLiteAccessorException(
                    message: ERROR,
                    innerException: ex
                    );
            }
        }
        #endregion レコードの参照

        #region レコードの更新
        /// <summary>
        /// テーブル構造定義クラスを使用してレコードの更新を行います。
        /// </summary>
        /// <typeparam name="TTable">テーブル構造定義クラスを設定します。</typeparam>
        /// <param name="action">レコードの更新関数を設定します。</param>
        public void Update<TTable>(
            [param: Required]Action<Table<TTable>> action
            ) where TTable : class
        {
            // nullチェック
            if (action == null)
                throw new ArgumentNullException("action");

            try
            {
                // ここにデータベース処理コードを書く
                using (DataContext context = new DataContext(this.Connection))
                {
                    Table<TTable> table = context.GetTable<TTable>();

                    // 対象データを更新します。
                    action(table);
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                // 破棄処理
                this.Dispose();
                // 例外発生
                throw new SQLiteAccessorException(
                    message: ERROR,
                    innerException: ex
                    );
            }
        }
        #endregion レコードの更新

        #region レコードの削除
        /// <summary>
        ///  テーブル構造定義クラスを使用してレコードの削除を行います。
        ///  レコードの検索を行いその結果を渡します。
        /// </summary>
        /// <typeparam name="TTable">テーブル構造定義クラスを設定します。</typeparam>
        /// <param name="func">レコードの削除関数を設定します。</param>
        public void Delete<TTable>(
            [param: Required]Func<Table<TTable>, IQueryable<TTable>> func
            ) where TTable : class
        {
            // nullチェック
            if (func == null)
                throw new ArgumentNullException("func");

            try
            {
                // ここにデータベース処理コードを書く
                using (DataContext context = new DataContext(this.Connection))
                {
                    Table<TTable> table = context.GetTable<TTable>();
                    var deleteList = func(table);
                    table.DeleteAllOnSubmit(deleteList);
                }
            }
            catch (Exception ex)
            {
                // 破棄処理
                this.Dispose();
                // 例外発生
                throw new SQLiteAccessorException(
                    message: ERROR,
                    innerException: ex
                    );
            }
        }
        #endregion レコードの削除
    }
}
