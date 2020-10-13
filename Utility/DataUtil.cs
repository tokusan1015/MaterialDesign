using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Linq;
using System.Linq;
using System.Reflection;
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
        public QueryData(
            [param: Required]string query
            )
        {
            this._query = query ?? throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(query));
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tableName">テーブル名を設定します。</param>
        /// <param name="param">パラメータ(ColumnParams)を設定します。</param>
        public QueryData(
            [param: Required]string query,
            [param: Required]ColumnParam param
            )
        {
            this._query = query ?? throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(query));
            this._param = param ?? throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(param));
        }
        #endregion コンストラクタ

        #region メソッド
        /// <summary>
        /// パラメータを生成します。
        /// </summary>
        /// <param name="tableClassInstance">テーブルクラスのインスタンスを設定します。</param>
        /// <param name="bindingAttr">BindingFlagsを設定します。</param>
        public void MakeParamater(
            [param: Required]object tableClassInstance,
            BindingFlags bindingAttr
            )
        {
            // nullチェック
            if (tableClassInstance == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(tableClassInstance));

            // テーブル名が設定されていない場合
            if (this.TableName.Length <= 0)
            {
                // テーブル名を取得・設定します。
                this.TableName = Utility.AttributeTable.GetTableName(
                    tableClassType: tableClassInstance.GetType()
                    );
            }

            // パラメータが設定されていない場合
            if (this._param == null)
            {
                // パラメータを取得・設定します。
                this._param = Utility.AttributeTable
                    .GetColumnParam(
                        classType: tableClassInstance.GetType(),
                        tableClassInstance: tableClassInstance,
                        bindingAttr: bindingAttr
                        );
            }
        }
        /// <summary>
        /// QueryDataのValueをUpdateします。
        /// </summary>
        /// <param name="tableClassInstance">テーブル構造定義クラスのインスタンスを設定します。</param>
        /// <param name="bindingAttr">BindingFlagsを設定します。</param>
        public void UpdateQueryDataValue(
            [param: Required]object tableClassInstance,
            BindingFlags bindingAttr
            )
        {
            // nullチェック
            if (tableClassInstance == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(tableClassInstance));

            // パラメータが設定されていない場合は再設定します。
            if (this._param == null)
            {
                // パラメータを取得・設定します。
                this._param = Utility.AttributeTable
                    .GetColumnParam(
                        classType: tableClassInstance.GetType(),
                        tableClassInstance: tableClassInstance,
                        bindingAttr: bindingAttr
                        );
            }

            // 値を再設定します。
            foreach (var p in this._param)
            {
                this._param[p.PropertyName].Value = 
                    Utility.ObjectUtil.GetPropertyValue(
                        target: tableClassInstance,
                        name: p.PropertyName,
                        bindingAttr: bindingAttr
                        );
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
                throw new InvalidOperationException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +"_param is null.");

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
        public string MakeSqliteCreateTableSql()
        {
            // Paramがnullの場合例外を発生します。
            if (this._param == null)
                throw new InvalidOperationException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +"_param is null.");

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
                    var dt = columnInfo.ConvertDbtype;
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
        /// <param name="limit">LIMITの設定を行います。負数の場合は省略されます。</param>
        /// <param name="offset">OFFSETの設定を行います。負数び場合は省略されます。</param>
        /// <returns>生成したSQL文を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object[])")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
        public string MakeSelectSql(
            [param: Required]string where = "",
            [param: Required]string orderby = "",
            int limit = -1,
            int offset = -1
            )
        {
            // nullチェック
            if (this._param == null)
                throw new InvalidOperationException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +"_param is null.");
            if (where == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(where));
            if (orderby == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(orderby));

            // SELECT {columnName[,columnName...]} FROM {tableName} {WHERE} {ORDERBY} {LIMIT} {OFFSET};
            const string SELECT = @"SELECT {1} FROM {0} {2} {3} {4} {5};";

            string wh = "";
            string ob = "";
            string li = "";
            string of = "";

            if (where.Length > 0)
            {
                wh = $"WHERE {where}";
            }
            if (orderby.Length > 0)
            {
                ob = $"ORDER BY {orderby}";
            }
            if (limit >= 0)
            {
                li = $"LIMIT {limit}";
            }
            if (offset >= 0)
            {
                of = $"OFFSET {offset}";
            }

            this._query = Utility.StringUtil.RemoveManySpace(
                string.Format(
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object,System.Object)")]
        public string MakeInsertSql()
        {
            // Paramがnullの場合例外を発生します。
            if (this._param == null)
                throw new InvalidOperationException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +"_param is null.");

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object,System.Object)")]
        public string MakeUpdateSql(
            [param: Required]string where = ""
            )
        {
            // nullチェック
            if (this._param == null)
                throw new InvalidOperationException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(this._param) + " is null.");
            if (where == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(where));

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
        /// <param name="where">where句を設定します。WHERE文字を入れないで下さい。</param>
        /// <returns>生成したSQL文を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)")]
        public string MakeDeleteSql(
            [param: Required]string where = ""
            )
        {
            // nullチェック
            if (this._param == null)
                throw new InvalidOperationException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(this._param) + " is null.");
            if (where == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(where));

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
        public void Add(
            [param: Required]ColumnInfo columnInfo
            )
        {
            // nullチェック
            if (columnInfo == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(columnInfo));

            // columnInfoを追加します。
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
        public bool ContainsKey(
            [param: Required]string columnName
            )
        {
            // nullチェック
            if (columnName == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(columnName));

            // 存在チェックを行い結果を返します。
            return this.ColumnInfos.Any(x => x.ColumnName == columnName);
        }
        /// <summary>
        /// 指定したカラム名に対応するColumnInfoを取得します。
        /// カラム名が存在しない場合はnullを返します。
        /// </summary>
        /// <param name="columnName">カラム名を指定します。</param>
        /// <returns>ColumnInfoを返します。</returns>
        public ColumnInfo GetColumnInfo(
            [param: Required]string columnName
            )
        {
            // nullチェック
            if (columnName == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(columnName));

            // カラム名に対応するColumnIndoを返します。
            return this.ColumnInfos
                .Where(x => x.ColumnName == columnName)
                .FirstOrDefault();
        }
        /// <summary>
        /// 指定したカラム名をコレクションから削除します。
        /// </summary>
        /// <param name="columnName">カラム名を設定します。</param>
        /// <returns>成功した場合trueを返します。</returns>
        public bool Remove(
            [param: Required]string columnName
            )
        {
            // nullチェック
            if (columnName == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(columnName));

            // 指定したカラム名をコレクションから削除し結果を返します。
            return this.ColumnInfos
                .Remove(this.GetColumnInfo(columnName: columnName));
        }
        /// <summary>
        /// 反復処理する列挙子を返します。
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            // 反復処理する列挙子を返します。
            return this.ColumnInfos.GetEnumerator();
        }
        /// <summary>
        /// 反復処理する列挙子を返します。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ColumnInfo> GetEnumerator()
        {
            // 反復処理する列挙子を返します。
            return this.ColumnInfos.GetEnumerator();
        }
        #endregion メソッド
    }
    #endregion ColumnParams

    #region ConvertTextData
    /// <summary>
    /// 旧名称から新名称に変換する辞書クラスです。
    /// </summary>
    public class ConvertDictionary
    {
        /// <summary>
        /// 文字変換辞書データクラス
        /// </summary>
        class ConvertData
        {
            /// <summary>
            /// 旧名称
            /// </summary>
            public string OldName { get; private set; }
            /// <summary>
            /// 新名称
            /// </summary>
            public string NewName { get; private set; }
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="oldName">旧名称</param>
            /// <param name="newName">新名称</param>
            public ConvertData(
                [param: Required]string oldName,
                [param: Required]string newName
                )
            {
                this.OldName = oldName ?? throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(oldName));
                this.NewName = newName ?? throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(newName));
            }
        }

        #region プロパティ
        /// <summary>
        /// 辞書データ
        /// </summary>
        private List<ConvertData> TextDatas { get; } = new List<ConvertData>();
        #endregion プロパティ

        /// <summary>
        /// 辞書に変換データを追加します。
        /// </summary>
        /// <param name="oldName">旧名称</param>
        /// <param name="newName">新名称</param>
        public void Add(
            [param: Required]string oldName,
            [param: Required]string newName
            )
        {
            // nullチェック
            if (oldName == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(oldName));
            if (oldName.Length <= 0)
                throw new ArgumentException(nameof(oldName) + " is empty.");
            if (newName == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(newName));
            if (newName.Length <= 0)
                throw new ArgumentException(nameof(newName) + " is empty.");

            // 存在チェックを行います。
            if (ContainsOldName(oldName: oldName))
                throw new ArithmeticException(nameof(oldName) + " already exists.");

            // データを辞書に追加します。
            this.TextDatas.Add(
                item: new ConvertData(oldName: oldName, newName: newName)
                );
        }

        /// <summary>
        /// 辞書に旧名称が存在するか調べます。
        /// </summary>
        /// <param name="oldName">旧名称</param>
        /// <returns>存在する場合trueを返します。</returns>
        public bool ContainsOldName(
            [param: Required]string oldName
            )
        {
            // nullチェック
            if (oldName == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(oldName));
            if (oldName.Length <=0)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(oldName));

            // 存在チェックを行い結果を返します。
            return this.TextDatas
                .Any(x => x.OldName == oldName);
        }

        /// <summary>
        /// 旧名称を新名称に変換します。
        /// </summary>
        /// <param name="oldName">旧名称を設定します。</param>
        /// <returns>新名称を返します。</returns>
        public string Convert(
            [param: Required]string oldName 
            )
        {
            // nullチェック
            if (oldName == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(oldName));
            if (oldName.Length <= 0)
                throw new InvalidOperationException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(oldName) + " is empty.");

            // 変換を行い結果を返します。
            return this.TextDatas
                .Where(x => x.OldName == oldName)
                .Select(x => x.NewName)
                .FirstOrDefault();
        }
    }
    #endregion ConvertTextData

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
        public ConvertDictionary ConvertDictionary { get; private set; } = null;
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
        public string Dbtype { get; set; }
        /// <summary>
        /// 変換DBタイプ
        /// </summary>
        public string ConvertDbtype => this.DbtypeConverter(this.Dbtype);
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public ColumnInfo(
            ConvertDictionary convertList = null
            )
        {
            // 指定されない場合は、自動生成します。
            if (convertList == null)
            {
                this.MakeConvertList();
            }
            else
            {
                this.ConvertDictionary = convertList;
            }

        }
        #endregion コンストラクタ

        #region メソッド
        /// <summary>
        /// 変換辞書を使用してDbTypeを変換します。
        /// </summary>
        /// <param name="dbType">DbTypeを設定します。</param>
        /// <returns>変換辞書を使用して変換したDbTypeを返します。</returns>
        public string DbtypeConverter(
            [param: Required]string dbType
            )
        {
            // nullチェック
            if (dbType == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(dbType));

            var result = this.ConvertDictionary.Convert(dbType);
            if (result == null)
                throw new ArgumentOutOfRangeException("dbType");

            return result;
        }

        /// <summary>
        /// SQLite用ConvertListを生成します。。
        /// </summary>
        private void MakeConvertList()
        {
            // 変換リストを生成します。
            this.ConvertDictionary = new ConvertDictionary();

            // INTEGER
            this.ConvertDictionary.Add(oldName: "INTEGER", newName: "INTEGER");
            this.ConvertDictionary.Add(oldName: "INT", newName: "INTEGER");
            this.ConvertDictionary.Add(oldName: "TINYINT", newName: "INTEGER");
            this.ConvertDictionary.Add(oldName: "SMALLINT", newName: "INTEGER");
            this.ConvertDictionary.Add(oldName: "MEDIUMINT", newName: "INTEGER");
            this.ConvertDictionary.Add(oldName: "BIGINT", newName: "INTEGER");
            // TEXT
            this.ConvertDictionary.Add(oldName: "TEXT", newName: "TEXT");
            this.ConvertDictionary.Add(oldName: "NVARCHAR", newName: "TEXT");
            this.ConvertDictionary.Add(oldName: "VARCHAR", newName: "TEXT");
            this.ConvertDictionary.Add(oldName: "CHARACTER", newName: "TEXT");
            this.ConvertDictionary.Add(oldName: "CLOB", newName: "TEXT");
            // REAL
            this.ConvertDictionary.Add(oldName: "REAL", newName: "REAL");
            this.ConvertDictionary.Add(oldName: "DOUBLE", newName: "REAL");
            this.ConvertDictionary.Add(oldName: "FLOAT", newName: "REAL");
            // NUMERIC
            this.ConvertDictionary.Add(oldName: "NUMERIC", newName: "NUMERIC");
            this.ConvertDictionary.Add(oldName: "DECIMAL", newName: "NUMERIC");
            this.ConvertDictionary.Add(oldName: "BOOLEAN", newName: "NUMERIC");
            this.ConvertDictionary.Add(oldName: "DATE", newName: "NUMERIC");
            this.ConvertDictionary.Add(oldName: "DATETIME", newName: "NUMERIC");
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
        /// 設定値を表します。
        /// </summary>
        private TType _value { get; set; }
        /// <summary>
        /// オリジナル値を表します。
        /// </summary>
        private TType _original { get; set; }
        /// <summary>
        /// オリジナル値を取得します。
        /// </summary>
        public TType Original
        {
            get { return this._original; }
        }
        /// <summary>
        /// 設定値を取得/設定します。
        /// </summary>
        public TType Value
        {
            get { return this._value; }
            set
            {
                if (this._value.CompareTo(value) != 0)
                {
                    this._value = value;
                }
            }
        }
        /// <summary>
        /// 変更が有った場合trueを返します。
        /// </summary>
        public bool IsChanged
        {
            get { return this._value.CompareTo(this._original) != 0; }
        }
        /// <summary>
        /// オリジナルデータを設定します。
        /// 同時にValueも変更されます。
        /// </summary>
        /// <param name="originalData">オリジナルデータを設定します。</param>
        public void SetOriginalData(
            [param: Required]TType originalData
            )
        {
            // nullチェック
            if (originalData == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(originalData));

            this._original = originalData;
            this._value = originalData;
        }
    }
    #endregion DataPlus

    #region DataWithValid
    /// <summary>
    /// 有効付きデータを表します。
    /// 無効データの取得は例外が発生します。
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class DataWithValid<TData>
    {
        private TData _Value;
        /// <summary>
        /// データを表します。
        /// </summary>
        public TData Value
        {
            get
            {
                // 無効データの参照は例外発生となります。
                if (!this.Valid) throw new InvalidOperationException("Value is invalid.");
                return this._Value;
            }
            set
            {
                // 無効データの設定は例外発生となります。
                if (!this.Valid) throw new InvalidOperationException("Value is invalid.");
                this._Value = value;
            }
        }
        /// <summary>
        /// データの有効無効をあらわします。
        /// 有効の場合trueに設定します。
        /// 変更はSetValid(), SetInvalid()メソッドを使用してください。
        /// </summary>
        public bool Valid { get; private set; } = true;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">値を設定します。</param>
        public DataWithValid(
            TData value
            )
        {
            // nullチェック
            if (value == null) throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA + nameof(value));

            this._Value = value;
        }
        /// <summary>
        /// 値を有効にします。
        /// </summary>
        public void SetValid()
        {
            this.Valid = true;
        }
        /// <summary>
        /// 値を無効にします。
        /// </summary>
        public void SetInvalid()
        {
            this.Valid = false;
        }
    }
    #endregion DataValid
}
