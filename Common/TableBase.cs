using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// テーブル基本クラス
    /// </summary>
    public class TableBase
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
        /// <param name="bindingAttr">BindingFlagsを設定します。</param>
        public void UpdateQueryDataValue(
            BindingFlags bindingAttr
            )
        {
            this.QueryData.UpdateQueryDataValue(
                tableClassInstance: this,
                bindingAttr: bindingAttr
                );
        }
        /// <summary>
        /// テーブル生成用SQL文を生成します。
        /// </summary>
        /// <returns>テーブル生成用SQL文を返します。</returns>
        public string MakeCreateTableString()
        {
            return this.QueryData.MakeSqliteCreateTableSql();
        }
        /// <summary>
        /// Insert用のSQL文を生成します。
        /// PrimaryKeyはAUTOINCREMENTである必要があります。
        /// </summary>
        /// <returns>Insert用のSQL文を返します。</returns>
        public string MakeInsertSql()
        {
            return this.QueryData.MakeInsertSql();
        }

        /// Update用のSQL文を生成します。
        /// PrimaryKeyはAUTOINCREMENTである必要があります。
        /// </summary>
        /// <param name="where">WHERE句を設定します。</param>
        /// <returns>Update用のSQL文を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public string MakeUpdateSql(
            [param: Required]string where = ""
            )
        {
            // nullチェック
            if (where == null)
                throw new ArgumentNullException("where");

            return this.QueryData.MakeUpdateSql(where: where);
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public string MakeSelectSql(
            [param: Required]string where = "",
            [param: Required]string orderby = "",
            int limit = -1,
            int offset = -1
            )
        {
            return this.QueryData.MakeSelectSql(
                where: where,
                orderby: orderby,
                limit: limit,
                offset: offset
                );
        }

        /// <summary>
        /// Delete用のSQL文を生成します。
        /// </summary>
        /// <param name="where">ＷHERE句を設定します。WHERE文字を入れないで下さい。</param>
        /// <returns>生成したSQL文を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public string MakeDeleteSql(
            [param: Required]string where = ""
            )
        {
            // nullチェック
            if (where == null)
                throw new ArgumentNullException("where");

            return this.QueryData.MakeDeleteSql(where: where);
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
