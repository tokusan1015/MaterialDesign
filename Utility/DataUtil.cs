using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace Utility
{
    #region QueryData
    /// <summary>
    /// クエリとして引き渡すデータリストを表します。
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class QueryData
    {
        #region プロパティ
        /// <summary>
        /// テーブル名を表します。
        /// </summary>
        public string TableName { get; private set; } = "";
        private string _query = "";
        /// <summary>
        /// クエリ
        /// 置き換えたいカラム名に"@"を付加して"@Data"等とします。
        /// </summary>
        public string Query => this._query;
        private ColumnParam _param = null;
        /// <summary>
        /// パラメータ
        /// 置き換えたいカラム名に"@"を付加して"@Data"等とします。
        /// </summary>
        public ColumnParam Param => this._param;
        #endregion プロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tableName">テーブル名を設定します。</param>
        public QueryData()
        {

        }
        /// <summary>
        /// QueryDataを生成します。
        /// </summary>
        /// <param name="query">クエリを設定します。</param>
        private QueryData(
            string query
            )
        {
            this._query = query;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tableName">テーブル名を設定します。</param>
        /// <param name="param">パラメータ(ColumnParams)を設定します。</param>
        private QueryData(
            string query,
            ColumnParam param
            )
        {
            this._query = query;
            this._param = param;
        }
        #endregion コンストラクタ

        #region メソッド
        /// <summary>
        /// パラメータ一覧をクリアします。
        /// </summary>
        public void Dispose_Param()
        {
            if (this._param != null)
            {
                this._param.Clear();
                this._param = null;
            }
        }

        /// <summary>
        /// パラメータを生成します。
        /// テーブルクラスオブジェクトを設定してください。
        /// </summary>
        /// <param name="obj">テーブルクラスオブジェクトを設定します。</param>
        public void MakeParamater<TTable>(
            object obj
            ) where TTable : class
        {
            // テーブル名が設定されていない場合
            if (this.TableName.Length <= 0)
            {
                // テーブル名を取得・設定します。
                this.TableName = Utility.AttributeTable.GetTableAttributeName<TTable>();
            }

            // パラメータが設定されていない場合
            if (this._param == null)
            {
                // パラメータを取得・設定します。
                this._param = Utility.AttributeTable.GetColumnParam<TTable>(obj: obj);
            }
        }
        /// <summary>
        /// QueryDataのValueをUpdateします。
        /// </summary>
        public void UpdateQueryDataValue<TTable>(object obj) where TTable : class
        {
            // パラメータが設定されていない場合は再設定します。
            if (this._param == null)
            {
                // パラメータを取得・設定します。
                this._param = Utility.AttributeTable.GetColumnParam<TTable>(obj: obj);
            }

            // 値を再設定します。
            foreach (var p in this._param)
            {
                this._param[p.PropertyName].Value = 
                    Utility.ObjectUtil.GetPropertyValue(obj: obj, p.PropertyName);
            }
        }
        #endregion メソッド

        #region MakeSQL_SQLlite
        /// <summary>
        /// テーブル構造定義クラスのカラム名を羅列した文字列を生成します。
        /// </summary>
        /// <param name="obj">対象のオブジェクトを設定します。</param>
        /// <returns>カラム名を羅列した文字列を返します。</returns>
        public string MakeColumnNames()
        {
            // Paramがnullの場合例外を発生します。
            if (this._param == null)
                throw new ArgumentNullException("Param");

            string result = "";
            var calist = this._param.Select(x => x.ColumnName).ToArray();

            // ","区切りで連結して返します。
            result = Utility.StringUtil.RemoveManySpace(
                string.Join(
                    separator: ", ",
                    value: calist
                    ));

            return result;
        }

        /// <summary>
        /// SQLiteテーブル生成用SQL文を生成します。
        /// 生成されたSQL文は、Queryにも保存されます。
        /// </summary>
        /// <returns>生成したSQL文を返します。</returns>
        public string MakeSQLiteCreateTableSQL()
        {
            // Paramがnullの場合例外を発生します。
            if (this._param == null)
                throw new ArgumentNullException("Param");

            // テーブル生成用SQL文字列
            var sb = new StringBuilder();
            sb.Append("CREATE TABLE IF NOT EXISTS ");

            // テーブル構造定義クラスのテーブル名を生成します。
            sb.Append("[" + this.TableName + "](");

            // 全パラメータに対して処理を行います。
            var columns = new List<string>();
            foreach (var columnInfo in this.Param)
            {
                if (columnInfo.Value != null)
                {
                    var dt = columnInfo.ConvertDbType;
                    var pk = columnInfo.IsPrimaryKey ? "PRIMARY KEY" : "";
                    var nn = !columnInfo.CanBeNull ? "NOT NULL" : "";
                    columns.Add($"[{columnInfo.ColumnName}] {dt} {pk} {nn}");
                }
            }
            sb.Append(string.Join(", ", columns));
            sb.Append(");");

            this._query = Utility.StringUtil.RemoveManySpace(sb.ToString());

            return this._query;
        }

        /// <summary>
        /// Select用のSQL文を生成します。
        /// SELECT {columnName[,columnName...]} FROM {tableName} {WHERE} {ORDERBY} {LIMIT} {OFFSET};
        /// WHERE,ORDERBY,LIMIT,OFFSETの文字は付加しないでください。
        /// 生成されたSQL文は、Queryにも保存されます。
        /// </summary>
        /// <param name="where">WHERE句を設定します。空文字は省略されます。</param>
        /// <param name="orderby">ORDERBY句を設定します。空文字は省略されます。</param>
        /// <param name="limit">LIMITの設定を行います。負数は省略されます。</param>
        /// <param name="offset">OFFSETの設定を行います。負数は省略されます。</param>
        /// <returns>生成したSQL文を返します。</returns>
        public string MakeSelectSQL(
            string where = "",
            string orderby = "",
            int limit = -1,
            int offset = -1
            )
        {
            // Paramがnullの場合例外を発生します。
            if (this._param == null)
                throw new ArgumentNullException("Param");

            // SELECT {columnName[,columnName...]} FROM {tableName} {WHERE} {ORDERBY} {LIMIT} {OFFSET};
            const string SELECT = @"SELECT {1} FROM {0} {2} {3} {4} {5};";

            string wh = "";
            string ob = "";
            string li = "";
            string of = "";

            if (where.Length > 0) wh = $"WHERE {where}";
            if (orderby.Length > 0) ob = $"ORDER BY {orderby}";
            if (limit >= 0) li = $"LIMIT {limit}";
            if (offset >= 0) of = $"OFFSET {offset}";

            this._query = Utility.StringUtil.RemoveManySpace(string.Format(
                SELECT,
                this.TableName,         // {0}
                this.MakeColumnNames(), // {1}
                wh,                     // {2}
                ob,                     // {3}
                li,                     // {4}
                of                      // {5}
                ));

            return this._query;
        }

        /// <summary>
        /// Insert用のSQL文を生成します。
        /// 生成されたSQL文は、Queryにも保存されます。
        /// </summary>
        /// <returns>生成したSQL文を返します。</returns>
        public string MakeInsertSQL()
        {
            // Paramがnullの場合例外を発生します。
            if (this._param == null)
                throw new ArgumentNullException("Param");

            const string INSERT = @"INSERT INTO {0}({1})VALUES({2});";

            // 全ColumnAttributeに対して処理を行います。
            var columns = new List<string>();   // {1}
            var values = new List<string>();    // {2}
            foreach (var ci in this._param)
            {
                // AUTOINCREMENT対応
                // PrimaryKeyでない場合のみ処理を行います。
                if (!ci.IsPrimaryKey)
                {
                    // カラム名を追加します。
                    columns.Add(ci.ColumnName);
                    values.Add(ci.ParamColumnName);
                }
            }

            // Insert文を生成します。
            this._query = Utility.StringUtil.RemoveManySpace(string.Format(
                INSERT,
                this.TableName,             // {0}
                string.Join(", ", columns), // {1}
                string.Join(", ", values)   // {2}
                ));

            return this._query;
        }

        /// <summary>
        /// Update用のSQL文を生成します。
        /// whereを設定しない場合はPrimaryKeyの直値が設定されます。(例.Id = @Id)
        /// 生成されたSQL文は、Queryにも保存されます。
        /// </summary>
        /// <param name="where">where句を設定します。</param>
        /// <returns>生成したSQL文を返します。</returns>
        public string MakeUpdateSQL(
            string where = ""
            )
        {
            // Paramがnullの場合例外を発生します。
            if (this._param == null)
                throw new ArgumentNullException("Param");

            // UPDATE テーブル名 SET 列名1=値1[,列名n=値n] WHERE 条件
            const string UPDATE = @"UPDATE {0} SET {1} WHERE {2};";

            // 全カラム情報に対して処理を行います。
            var columns = new List<string>();
            foreach (var columnInfo in this._param)
            {
                // PrimaryKeyで処理を変えます。
                if (columnInfo.IsPrimaryKey)
                {
                    // whereが設定されていない場合のみ設定します。
                    if (where.Length <= 0)
                    {
                        // "columnName = @columnName" の形式で保存します。
                        where = $"{columnInfo.ColumnName} = {columnInfo.ParamColumnName}";
                    }
                }
                else
                {
                    // "columnName = @columnName" の形式でAddします。
                    columns.Add($"{columnInfo.ColumnName} = {columnInfo.ParamColumnName}");
                }
            }

            // Update文を生成します。
            this._query = Utility.StringUtil.RemoveManySpace(string.Format(
                UPDATE,
                this.TableName,             // {0}
                string.Join(", ", columns), // {1}
                where                       // {2}
                ));

            return this._query;
        }

        /// <summary>
        /// Delete用のSQL文を生成します。
        /// whereを設定しない場合はPrimaryKeyの直値が設定されます。(例.Id = @Id)
        /// 生成されたSQL文は、Queryにも保存されます。
        /// </summary>
        /// <param name="where">where句を設定します。</param>
        /// <returns>生成したSQL文を返します。</returns>
        public string MakeDeleteSQL(
            string where = ""
            )
        {
            // Paramがnullの場合例外を発生します。
            if (this._param == null)
                throw new ArgumentNullException("Param");

            // UPDATE テーブル名 SET 列名1=値1[,列名n=値n] WHERE 条件
            const string DELETE = @"DELETE FROM {0} WHERE {1};";

            // WHERE句が設定されていない場合のみ処理します。
            if (where.Length <= 0)
            {
                // where句を生成します。
                where = this._param
                    .Where(ci => ci.IsPrimaryKey)
                    .Select(ci =>
                    {
                        // PrimaryKeyをＷHERE句として使用します。
                        return $"{ci.ColumnName} = {ci.ParamColumnName}";
                    }).FirstOrDefault();
            }

            // Delete文を生成します。
            this._query = Utility.StringUtil.RemoveManySpace(string.Format(
                    DELETE,
                    this.TableName, // {0}
                    where           // {1}
                    ));

            return this._query;
        }
        #endregion MakeSQL_SQLlite
    }
    #endregion QueryData

    #region ColumnParams
    /// <summary>
    /// DBデータ用のカラム名とパラメータの一覧を管理するコレクションを表します。
    /// </summary>
    [Utility.Developer("tokusan1015")]
    public class ColumnParam : IEnumerable<ColumnInfo>
    {
        #region プロパティ
        /// <summary>
        /// 管理用ColumnInfoリスト
        /// </summary>
        private List<ColumnInfo> ColumnInfos { get; } =  new List<ColumnInfo>();
        /// <summary>
        /// インデクサー
        /// </summary>
        /// <param name="columnName">カラム名を設定します。</param>
        /// <returns>キーに対応するKeyValuePairを返します。</returns>        
        public ColumnInfo this[string columnName]
            => this.GetColumnInfo(columnName: columnName);
        /// <summary>
        /// 格納されているColumnInfoの数を取得します。
        /// </summary>
        public int Count
            => this.ColumnInfos.Count;
        #endregion プロパティ

        #region メソッド
        /// <summary>
        /// コレクションにColumnInfoを追加します。
        /// </summary>
        /// <param name="columnInfo">ColumnInfoを設定します。</param>
        public void Add(ColumnInfo columnInfo)
        {
            this.ColumnInfos.Add(item: columnInfo);
        }
        /// <summary>
        /// コレクションをクリアします。
        /// </summary>
        public void Clear()
        {
            this.ColumnInfos.Clear();
        }
        /// <summary>
        /// 指定したカラム名がコレクションに存在するか調べます。
        /// </summary>
        /// <param name="columnName">カラム名を設定します。</param>
        /// <returns>存在する場合trueを返します。</returns>
        public bool ContainsKey(string columnName)
        {
            return this.ColumnInfos.Any(x => x.ColumnName == columnName);
        }
        /// <summary>
        /// 指定したカラム名に対応するColumnInfoを取得します。
        /// カラム名が存在しない場合はnullを返します。
        /// </summary>
        /// <param name="columnName">カラム名を指定します。</param>
        /// <returns>ColumnInfoを返します。</returns>
        public ColumnInfo GetColumnInfo(string columnName)
        {
            return this.ColumnInfos
                .Where(x => x.ColumnName == columnName)
                .FirstOrDefault();
        }
        /// <summary>
        /// 指定したカラム名をコレクションから削除します。
        /// </summary>
        /// <param name="columnName">カラム名を設定します。</param>
        /// <returns>成功した場合trueを返します。</returns>
        public bool Remove(string columnName)
        {
            return this.ColumnInfos
                .Remove(this.GetColumnInfo(columnName: columnName));
        }
        /// <summary>
        /// 反復処理する列挙子を返します。
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.ColumnInfos.GetEnumerator();
        }
        /// <summary>
        /// 反復処理する列挙子を返します。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ColumnInfo> GetEnumerator()
        {
            return this.ColumnInfos.GetEnumerator();
        }
        #endregion メソッド
    }
    #endregion ColumnParams

    #region ColumnInfo
    /// <summary>
    /// カラム情報クラス
    /// </summary>
    public class ColumnInfo
    {
        #region 固定値
        /// <summary>
        /// パラメータデータでカラム名の先頭に付加する文字を表します。
        /// </summary>
        const char COLUMN_HEAD = '@';
        #endregion 固定値

        #region プロパティ
        /// <summary>
        /// DbType変換リスト
        /// </summary>
        public ICollection<KeyValuePair<string, string>> ConvertList { get; private set; } = null;
        /// <summary>
        /// クラスのプロパティ名
        /// </summary>
        public string PropertyName { get; set; } 
        /// <summary>
        /// テーブルのカラム名
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// カラムのDBタイプ
        /// </summary>
        public string DbType { get; set; }
        /// <summary>
        /// 変換DBタイプ
        /// </summary>
        public string ConvertDbType => this.DbTypeConverter(this.DbType);
        /// <summary>
        /// カラムのPrimaryKeyフラグ
        /// </summary>
        public bool IsPrimaryKey { get; set; }
        /// <summary>
        /// カラムのNull許可フラグ
        /// </summary>
        public bool CanBeNull { get; set; }
        /// <summary>
        /// カラムの値オブジェクト
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// パラメータ用のカラム名(カラム名の先頭に'@'が付加されます)
        /// </summary>
        public string ParamColumnName => COLUMN_HEAD + this.ColumnName;
        #endregion プロパティ

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// convertListは、DbTypeのstring => string の変換を行います。
        /// convertListがnullの場合は、SQLite用のコンバータがセットされます。
        /// </summary>
        public ColumnInfo(
            ICollection<KeyValuePair<string, string>> convertList = null
            )
        {
            // 指定されない場合は、自動生成します。
            if (convertList == null)
            {
                this.MakeConvertList();
            }
        }
        #endregion コンストラクタ

        #region メソッド
        /// <summary>
        /// コンバータリストを使用してDbTypeを変換します。
        /// </summary>
        /// <param name="dbType">DbTypeを設定します。</param>
        /// <returns>コンバータリストを使用して変換したDbTypeを返します。</returns>
        public string DbTypeConverter(string dbType)
        {
            var result = this.ConvertList
                .Where(x => x.Key == dbType)
                .Select(x => x.Value)
                .FirstOrDefault();
            if (result.Length <= 0) throw new ArgumentOutOfRangeException("dbType");

            return result;
        }

        /// <summary>
        /// SQLite用ConvertListを生成します。。
        /// </summary>
        private void MakeConvertList()
        {
            this.ConvertList = new List<KeyValuePair<string, string>>
            {
                // INTEGER
                new KeyValuePair<string, string>(key: "INTEGER", value: "INTEGER"),
                new KeyValuePair<string, string>(key: "INT", value: "INTEGER"),
                new KeyValuePair<string, string>(key: "TINYINT", value: "INTEGER"),
                new KeyValuePair<string, string>(key: "SMALLINT", value: "INTEGER"),
                new KeyValuePair<string, string>(key: "MEDIUMINT", value: "INTEGER"),
                new KeyValuePair<string, string>(key: "BIGINT", value: "INTEGER"),
                // TEXT
                new KeyValuePair<string, string>(key: "TEXT", value: "TEXT"),
                new KeyValuePair<string, string>(key: "NVARCHAR", value: "TEXT"),
                new KeyValuePair<string, string>(key: "VARCHAR", value: "TEXT"),
                new KeyValuePair<string, string>(key: "CHARACTER", value: "TEXT"),
                new KeyValuePair<string, string>(key: "CLOB", value: "TEXT"),
                // REAL
                new KeyValuePair<string, string>(key: "REAL", value: "REAL"),
                new KeyValuePair<string, string>(key: "DOUBLE", value: "REAL"),
                new KeyValuePair<string, string>(key: "FLOAT", value: "REAL"),
                // NUMERIC
                new KeyValuePair<string, string>(key: "NUMERIC", value: "NUMERIC"),
                new KeyValuePair<string, string>(key: "DECIMAL", value: "NUMERIC"),
                new KeyValuePair<string, string>(key: "BOOLEAN", value: "NUMERIC"),
                new KeyValuePair<string, string>(key: "DATE", value: "NUMERIC"),
                new KeyValuePair<string, string>(key: "DATETIME", value: "NUMERIC")
            };
        }
        #endregion メソッド
    }
    #endregion ColumnInfo

    #region DataAndName
    /// <summary>
    /// プロパティ名付きのデータを表します。
    /// ex.)
    /// var data = new DataAndName()
    /// { 
    ///     Data = value,
    ///     PropertyName = Utility.ObjectUtil.GetPropertyName(() => value)
    /// }
    /// </summary>
    /// <typeparam name="T">型を設定します。</typeparam>
    [Utility.Developer(name: "tokusan1015")]
    public class DataAndName<T> where T : class
    {
        /// <summary>
        /// データ
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// プロパティ名
        /// </summary>
        public string PropertyName { get; set; }
    }
    #endregion DataAndName

    #region DataPlus
    /// <summary>
    /// バックアップ付きデータクラスを表します。
    /// </summary>
    /// <typeparam name="TType">型を設定します。</typeparam>
    [Utility.Developer(name: "tokusan1015")]
    public class DataPlus<TType> where TType : IComparable
    {
        /// <summary>
        /// データ
        /// </summary>
        private TType _data { get; set; }
        /// <summary>
        /// 初期データ
        /// </summary>
        private TType _original { get; set; }
        /// <summary>
        /// オリジナルデータ
        /// </summary>
        public TType Original
        {
            get { return this._original; }
        }
        /// <summary>
        /// データを返します。
        /// </summary>
        public TType Value
        {
            get { return this._data; }
            set
            {
                if (this._data.CompareTo(value) != 0)
                {
                    this._data = value;
                }
            }
        }
        /// <summary>
        /// 修正が有った場合trueを返します。
        /// </summary>
        public bool IsChanged
        {
            get { return this._data.CompareTo(this._original) != 0; }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="original">オリジナルデータ</param>
        public DataPlus(TType original)
        {
            this._original = original;
            this._data = original;
        }
    }
    #endregion DataPlus
}
