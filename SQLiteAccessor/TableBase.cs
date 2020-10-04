using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteAccessorBase
{
    /// <summary>
    /// テーブル基本クラス
    /// </summary>
    /// <typeparam name="TTable"></typeparam>
    public class TableBase<TTable> where TTable : class
    {
        #region プロパティ(DB)
        /// <summary>
        /// QueryData
        /// </summary>
        public Utility.QueryData QueryData { get; } = new Utility.QueryData();
        #endregion プロパティ(DB)

        #region Table
        /// <summary>
        /// QueryParamのParamをUpdateします。
        /// </summary>
        public void UpdateQueryDataValue()
        {
            this.QueryData.UpdateQueryDataValue<TTable>(obj: this);
        }
        /// <summary>
        /// テーブル生成用SQL文を生成します。
        /// </summary>
        /// <returns>テーブル生成用SQL文を返します。</returns>
        public string MakeCreateTableString()
        {
            return this.QueryData.MakeSQLiteCreateTableSQL();
        }
        /// <summary>
        /// Insert用のSQL文を生成します。
        /// PrimaryKeyはAUTOINCREMENTである必要があります。
        /// </summary>
        /// <returns>Insert用のSQL文を返します。</returns>
        public string MakeInsertSQL()
        {
            return this.QueryData.MakeInsertSQL();
        }

        /// Update用のSQL文を生成します。
        /// PrimaryKeyはAUTOINCREMENTである必要があります。
        /// </summary>
        /// <param name="where">WHERE句を設定します。</param>
        /// <returns>Update用のSQL文を返します。</returns>
        public string MakeUpdateSQL(string where = "")
        {
            return this.QueryData.MakeUpdateSQL(where: where);
        }
        /// <summary>
        /// Select用のSQL文を生成します。
        /// PrimaryKeyはAUTOINCREMENTである必要があります。
        /// SELECT {columnName[,columnName...]} FROM {tableName} {WHERE} {ORDERBY} {LIMIT} {OFFSET};
        /// WHERE,ORDERBY,LIMIT,OFFSETの文字は付加しないでください。
        /// </summary>
        /// <typeparam name="TTable">テーブル構造定義クラスを設定します。</typeparam>
        /// <param name="where">WHERE句を設定します。空文字は省略されます。</param>
        /// <param name="orderby">ORDERBY句を設定します。空文字は省略されます。</param>
        /// <param name="limit">LIMITの設定を行います。負数は省略されます。</param>
        /// <param name="offset">OFFSETの設定を行います。負数は省略されます。</param>
        /// <returns>Select用のSQL文を返します。</returns>
        public string MakeSelectSQL(
            string where = "",
            string orderby = "",
            int limit = -1,
            int offset = -1
            )
        {
            return this.QueryData.MakeSelectSQL(
                where: where,
                orderby: orderby,
                limit: limit,
                offset: offset
                );
        }

        /// <summary>
        /// Delete用のSQL文を生成します。
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public string MakeDeleteSQL(string where = "")
        {
            return this.QueryData.MakeDeleteSQL(where: where);
        }

        /// <summary>
        /// テーブル構造定義クラスのカラム名を羅列した文字列を生成します。
        /// </summary>
        /// <returns>カラム名を羅列した文字列を返します。</returns>
        public string MakecolumnNames()
        {
            return this.QueryData.MakeColumnNames();
        }

        #endregion Table
    }
}
