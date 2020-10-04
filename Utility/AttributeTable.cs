using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Utility
{
    /// <summary>
    /// テーブル属性用ユーティリティ
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class AttributeTable
    {
        #region Table
        /// <summary>
        /// テーブル構造定義クラスのテーブル名を取得します。
        /// </summary>
        /// <typeparam name="TTable">テーブル構造定義クラス</typeparam>
        /// <returns>テーブル名を返します。</returns>
        public static string GetTableAttributeName<TTable>(
            ) where TTable : class
        {
            // テーブル構造定義クラスのテーブル名を取得します。
            return Utility.AttributeUtil
                .GetClassAttribute<TableAttribute>(typeof(TTable)).Name;
        }

        /// <summary>
        /// テーブル構造定義クラスのプロパティ名に紐づけられたColumnAttributeを
        /// ICollection(KeyValuePair(string, ColumnAttribute))形式で取得します。
        /// KeyValuePair.Key   : プロパティ名
        /// KeyValuePair.Value : ColumnAttribute
        /// </summary>
        /// <typeparam name="TTable">テーブル構造定義クラスを設定します。</typeparam>
        /// <returns>ICollection(KeyValuePair(string, ColumnAttribute))形式で返します。</returns>
        public static ICollection<KeyValuePair<string, ColumnAttribute>>
            GetColumnAttributeList<TTable>(
            ) where TTable : class
        {
            var result = new List<KeyValuePair<string, ColumnAttribute>>();

            // テーブル構造定義クラスのプロパティ一覧を取得します。
            var props = Utility.ObjectUtil.GetClassPropertys<TTable>();

            // 取得した全プロパティ名を処理します。
            foreach (var name in props)
            {
                // プロパティ名に紐づけられたColumnAttributeを取得します。
                var ca = Utility.AttributeUtil
                    .GetPropertyAttribute<ColumnAttribute>(
                    classType: typeof(TTable),
                    propertyName: name.ToString()
                    );
                if (ca != null)
                {
                    result.Add(new KeyValuePair<string, ColumnAttribute>(key: name, value: ca));
                }
            }

            // ICollection<KeyValuePair<string, ColumnAttribute>>形式で返します。
            return result;
        }

        /// <summary>
        /// テーブル構造定義クラスから情報(カラム名・値)一覧を取得します。
        /// </summary>
        /// <typeparam name="TTable">テーブル構造定義クラスを設定します。</typeparam>
        /// <param name="obj">テーブル構造定義クラスのインスタンスを設定します。</param>
        /// <returns>カラム情報一覧を返します。</returns>
        public static Utility.ColumnParam GetColumnParam<TTable>(
            [param: Required]object obj
            ) where TTable : class
        {
            // テーブル構造定義クラスのColumnAttribute一覧を取得します。
            var calist = Utility.AttributeTable.GetColumnAttributeList<TTable>();

            // ColumnParamを生成します。
            var cp = new Utility.ColumnParam();

            // ColumnAttribute一覧で処理します。
            foreach (var ca in calist)
            {
                // カラム名を取得します。
                var columnName = ca.Value.Name;

                // プロパティの値を取得します。
                var value = typeof(TTable)
                    .GetProperty(ca.Key)
                    .GetValue(obj: obj, index: null);

                // ColumnParamにColumnInfoを追加します。
                cp.Add(columnInfo: new Utility.ColumnInfo()
                {
                    PropertyName = ca.Key,
                    ColumnName = columnName,
                    DbType = ca.Value.DbType,
                    IsPrimaryKey = ca.Value.IsPrimaryKey,
                    CanBeNull = ca.Value.CanBeNull,
                    Value = value
                });
            }
            return cp;
        }

        /// <summary>
        /// テーブル構造定義クラスから情報(カラム名)一覧を取得します。
        /// ColumnParamのvalueにはnullを設定します。
        /// </summary>
        /// <typeparam name="TTable">テーブル構造定義クラスを設定します。</typeparam>
        /// <returns>カラム情報一覧を返します。</returns>
        public static Utility.ColumnParam GetColumnParam<TTable>(
            ) where TTable : class
        {
            // テーブル構造定義クラスのColumnAttribute一覧を取得します。
            var calist = Utility.AttributeTable.GetColumnAttributeList<TTable>();

            // ColumnParamを生成します。
            var cp = new Utility.ColumnParam();

            // ColumnAttribute一覧で処理します。
            foreach (var ca in calist)
            {
                // カラム名を取得します。
                var columnName = ca.Value.Name;

                // ColumnParamにColumnInfoを追加します。
                cp.Add(columnInfo: new Utility.ColumnInfo()
                {
                    PropertyName = ca.Key,
                    ColumnName = columnName,
                    DbType = ca.Value.DbType,
                    IsPrimaryKey = ca.Value.IsPrimaryKey,
                    CanBeNull = ca.Value.CanBeNull,
                    Value = null
                });
            }
            return cp;
        }

        /// <summary>
        /// 対象クラスのプロパティにDataRowの値を設定します。
        /// </summary>
        /// <typeparam name="TTable">テーブル構造定義クラスを設定します。</typeparam>
        /// <param name="obj">対象のオブジェクトを設定します。</param>
        /// <param name="dataRow">対象に設定するDataRowを設定します。</param>
        public static void SetValuesFromDataRow<TTable>(
            [param: Required]object obj,
            [param: Required]DataRow dataRow
            ) where TTable : class
        {
            // カラム名一覧を取得します。
            var columnNames = Utility.AttributeTable
                .GetColumnAttributeList<TTable>();

            // 対象オブジェクトのプロパティに値を設定します。
            foreach (var n in columnNames)
            {
                var value = dataRow[n.Value.Name];
                Utility.ObjectUtil.SetPropertyValue(
                    obj: obj,
                    name: n.Value.Name,
                    value: value
                    );
            }
        }
        #endregion Table
    }
}
