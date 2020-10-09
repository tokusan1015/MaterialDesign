using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

[assembly: CLSCompliant(true)]
namespace SQLiteAccessorBase
{
    /// <summary>
    /// SQLiteConnectionBase
    /// </summary>
    public class SQLiteAccessBase : IDisposable
    {
        #region プロパティ
        /// <summary>
        /// データソース
        /// </summary>
        protected string DataSource { get; private set; } = @"./SQLiteDB.db";
        /// <summary>
        /// 接続文字列
        /// </summary>
        protected SQLiteConnectionStringBuilder ConnectionString { get; private set; } = null;
        /// <summary>
        /// コネクション
        /// </summary>
        protected SQLiteConnection Connection { get; set; } = null;
        #endregion プロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DataSource">データソースを設定します。</param>
        public SQLiteAccessBase(
            [param: Required]string dataSource
            )
        {
            // nullチェック
            if (dataSource == null)
                throw new ArgumentNullException("dataSource");

            // データソースを設定します。
            if (dataSource.Length > 0)
                this.DataSource = dataSource;

            // 接続文字列を生成します。
            this.ConnectionString =
                new SQLiteConnectionStringBuilder { DataSource = this.DataSource };

            // コネクションを確立します。
            this.Connection = new SQLiteConnection(
                 connectionString: this.ConnectionString.ToString()
                 );
        }
        #endregion コンストラクタ

        #region 破棄処理
        /// <summary>
        /// 破棄処理
        /// </summary>
        private void Dispose_Connection()
        {
            // コネクションが生成されている場合
            if (this.Connection != null)
            {
                // コネクションを切断します。
                if (this.Connection.State != System.Data.ConnectionState.Closed)
                {
                    this.Connection.Close();
                }
                // コネクションを破棄します。
                this.Connection.Dispose();
                // コネクションにnullを設定します。
                this.Connection = null;
            }
        }

        /// <summary>
        /// コネクションを切断します。
        /// Dispose処理の最初に実行してください。
        /// </summary>
        protected void ConnectionClose()
        {
            // コネクションのnull確認
            if (this.Connection != null)
            {
                // コネクションを切断します。
                if (this.Connection.State != System.Data.ConnectionState.Closed)
                {
                    this.Connection.Close();
                }
            }
        }
        #endregion 破棄処理

        #region コネクション
        /// <summary>
        /// コネクションがNullの場合例外を発生します。
        /// </summary>
        private void ConnectionIsNull()
        {
            if (this.Connection == null)
                throw new SQLiteAccessorException("Connection is null.");
        }

        /// <summary>
        /// コネクションを接続します。
        /// </summary>
        public void Open()
        {
            // コネクションがnullか調べます。
            this.ConnectionIsNull();

            // コネクションが破壊している場合
            if (this.IsBroken())
            {
                this.Close();
            }

            // コネクションが切断している場合
            if (this.IsClosed())
            {
                // コネクションを接続します。
                this.Connection.Open();
            }
        }

        /// <summary>
        /// コネクションを切断します。
        /// </summary>
        public void Close()
        {
            // コネクションがnullか調べます。
            this.ConnectionIsNull();

            // コネクションが接続している場合
            if (this.IsOpen())
            {
                // コネクションを切断します。
                this.Connection.Close();
            }
        }
        #endregion コネクション

        #region 接続状態
        /// <summary>
        /// コネクションが接続状態であるか調べます。
        /// </summary>
        /// <returns>接続状態の場合trueを返します。</returns>
        public bool IsOpen()
        {
            return this.Connection.State == System.Data.ConnectionState.Open
                || this.Connection.State == System.Data.ConnectionState.Connecting
                || this.Connection.State == System.Data.ConnectionState.Executing
                || this.Connection.State == System.Data.ConnectionState.Fetching;
        }

        /// <summary>
        /// コネクションが切断状態であるか調べます。
        /// </summary>
        /// <returns>切断状態の場合trueを返します。</returns>
        public bool IsClosed()
        {
            return this.Connection.State == System.Data.ConnectionState.Closed;
        }

        /// <summary>
        /// コネクションが破壊状態であるか調べます。
        /// 破壊状態の場合、Close() => Open() で回復します。
        /// </summary>
        /// <returns>接続状態が破壊の場合trueを返します。</returns>
        public bool IsBroken()
        {
            return this.Connection.State == System.Data.ConnectionState.Broken;
        }

        /// <summary>
        /// Execute可能か調べます。
        /// 不可能な場合は、例外が発生します。
        /// </summary>
        /// <returns>可能な場合はtrueを返します。</returns>
        public void CheckExecuteConnection()
        {
            // コネクションがnullか調べます。
            this.ConnectionIsNull();

            // コネクションが切断または破壊か調べます。
            if (this.IsClosed() || this.IsBroken())
            {
                throw new SQLiteAccessorException("Connection is Close or Broken.");
            }
        }
        #endregion 接続状態

        #region SQLiteCommand
        /// <summary>
        /// SQLiteCommandにQueryDataを設定します。
        /// </summary>
        /// <param name="cmd">SQLiteCommandを設定します。</param>
        /// <param name="queryData">QueryDataを設定します。</param>
        /// <returns>処理件数を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:SQL クエリのセキュリティ脆弱性を確認")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.IndexOf(System.String)")]
        protected void SetSQLiteCommandToQueryData(
            [param: Required]SQLiteCommand cmd,
            [param: Required]Utility.QueryData queryData
            )
        {
            // nullチェック
            if (cmd == null)
                throw new ArgumentNullException("cmd");
            if (queryData == null)
                throw new ArgumentNullException("queryData");

            // パラメータクリア
            cmd.Parameters.Clear();

            // パラメータを設定します。
            if (queryData.Param != null)
            {
                foreach (var item in queryData.Param)
                {
                    // クエリにKeyが存在した場合は、パラメータを設定します。   
                    if (queryData.Query.IndexOf(item.ParamColumnName) > 0)
                        cmd.Parameters.Add(new SQLiteParameter(item.ParamColumnName, item.Value));
                }
            }

            // クエリを設定します。
            cmd.CommandText = queryData.Query;
        }
        #endregion SQLiteCommand

        #region ExecuteQuery
        #endregion ExecuteQuery

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)。
                    this.Dispose_Connection();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~SQLiteConnectionBase() {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
