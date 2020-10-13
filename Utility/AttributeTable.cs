using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Utility
{
    /// <summary>
    /// 属性情報を表します。
    /// </summary>
    public class ColumnAttributeData
    {
        /// <summary>
        /// プロパティ名
        /// </summary>
        public string PropertyName { get; private set; } = "";
        /// <summary>
        /// カラム属性情報
        /// </summary>
        public ColumnAttribute ColumnAttribute { get; private set; } = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="propertyName">プロパティ名を設定します。</param>
        /// <param name="columnAttribute">カラム属性情報を設定します。</param>
        public ColumnAttributeData(
            [param: Required]string propertyName,
            ColumnAttribute columnAttribute
            )
        {
            this.PropertyName = propertyName;
            this.ColumnAttribute = columnAttribute;
        }
    }

    /// <summary>
    /// テーブル属性用ユーティリティ
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public static class AttributeTable
    {
        #region Table

        /// <summary>
        /// TableAttributeからテーブル名を取得します。
        /// </summary>
        /// <param name="tableClassType">テーブル構造定義クラスタイプを設定します。</param>
        /// <returns>テーブル名を返します。</returns>
        public static string GetTableName(
            MemberInfo tableClassType
            )
        {
            // テーブル属性を取得します。
            var tableAttribute = Utility.AttributeUtil
                .GetClassAttribute(tableClassType, typeof(TableAttribute)) as TableAttribute;

            // テーブル名を取得します。
            return tableAttribute.Name;
        }

        /// <summary>
        /// テーブル構造定義クラスのプロパティ名に紐づけられたColumnAttributeを
        /// ICollection(ColumnAttributeData)形式で取得します。
        /// KeyValuePair.Key   : プロパティ名
        /// KeyValuePair.Value : ColumnAttribute
        /// </summary>
        /// <param name="classType">テーブル構造定義クラスタイプを設定します。</param>
        /// <param name="bindingAttr">BindingFlagsを設定します。</param>
        /// <returns>ICollection(ColumnAttributeData)形式で返します。</returns>
        public static ICollection<ColumnAttributeData>
            GetColumnAttributeList(
            [param: Required]Type classType,
            BindingFlags bindingAttr
            )
        {
            var result = new List<ColumnAttributeData>();

            // テーブル構造定義クラスのプロパティ一覧を取得します。
            var props = Utility.ObjectUtil
                .GetClassPropertys(classType: classType, bindingAttr: bindingAttr);

            // 取得した全プロパティ名を処理します。
            foreach (var name in props)
            {
                // プロパティ名に紐づけられたColumnAttributeを取得します。
                var ca = (ColumnAttribute)Utility.AttributeUtil
                    .GetPropertyAttribute(
                        attrType: typeof(ColumnAttribute),
                        classType: classType,
                        propertyName: name.ToString(),
                        bindingAttr: bindingAttr
                        );
                if (ca != null)
                {
                    result.Add(new ColumnAttributeData(
                        propertyName: name,
                        columnAttribute: ca
                        ));
                }
            }

            // ICollection<KeyValuePair<string, ColumnAttribute>>形式で返します。
            return result;
        }

        /// <summary>
        /// テーブル構造定義クラスから情報(カラム名・値)一覧を取得します。
        /// DataPlusを使用している場合です。
        /// </summary>
        /// <param name="TTable">テーブル構造定義クラスタイプを設定します。</param>
        /// <param name="tableClassInstance">テーブル構造定義クラスのインスタンスを設定します。</param>
        /// <param name="bindingAttr">BindingFlagsを設定します。</param>
        /// <returns>カラム情報一覧を返します。</returns>
        public static Utility.ColumnParam GetColumnParam(
            [param: Required]Type classType,
            [param: Required]object tableClassInstance,
            BindingFlags bindingAttr
            )
        {
            // null チェック
            if (classType == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(classType));
            if (tableClassInstance == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(tableClassInstance));

            // テーブル構造定義クラスのColumnAttribute一覧を取得します。
            var calist = Utility.AttributeTable
                .GetColumnAttributeList(classType: classType, bindingAttr: bindingAttr);

            // ColumnParamを生成します。
            var cp = new Utility.ColumnParam();

            // ColumnAttribute一覧で処理します。
            foreach (var ca in calist)
            {
                // カラム名を取得します。
                var columnName = ca.ColumnAttribute.Name;

                // プロパティの値を取得します。
                var value = classType
                    .GetProperty(ca.PropertyName, bindingAttr: bindingAttr)
                    .GetValue(obj: tableClassInstance, index: null);

                // ColumnParamにColumnInfoを追加します。
                cp.Add(columnInfo: new Utility.ColumnInfo()
                {
                    PropertyName = ca.PropertyName,
                    ColumnName = columnName,
                    Dbtype = ca.ColumnAttribute.DbType,
                    IsPrimaryKey = ca.ColumnAttribute.IsPrimaryKey,
                    CanBeNull = ca.ColumnAttribute.CanBeNull,
                    Value = value
                });
            }
            return cp;
        }

        /// <summary>
        /// テーブル構造定義クラスから情報(カラム名)一覧を取得します。
        /// ColumnParamのvalueにはnullを設定します。
        /// </summary>
        /// <param name="TTable">テーブル構造定義クラスタイプを設定します。</param>
        /// <returns>カラム情報一覧を返します。</returns>
        /// <param name="bindingAttr">BindingFlagsを設定します。</param>
        public static Utility.ColumnParam GetColumnParam(
            [param: Required]Type classType,
            BindingFlags bindingAttr
            )
        {
            // テーブル構造定義クラスのColumnAttribute一覧を取得します。
            var calist = Utility.AttributeTable
                .GetColumnAttributeList(classType: classType, bindingAttr: bindingAttr);

            // ColumnParamを生成します。
            var cp = new Utility.ColumnParam();

            // ColumnAttribute一覧で処理します。
            foreach (var ca in calist)
            {
                // カラム名を取得します。
                var columnName = ca.ColumnAttribute.Name;

                // ColumnParamにColumnInfoを追加します。
                cp.Add(columnInfo: new Utility.ColumnInfo()
                {
                    PropertyName = ca.PropertyName,
                    ColumnName = columnName,
                    Dbtype = ca.ColumnAttribute.DbType,
                    IsPrimaryKey = ca.ColumnAttribute.IsPrimaryKey,
                    CanBeNull = ca.ColumnAttribute.CanBeNull,
                    Value = null
                });
            }
            return cp;
        }

        /// <summary>
        /// 対象クラスのプロパティにDataRowの値を設定します。
        /// </summary>
        /// <param name="classType">テーブル構造定義クラスタイプを設定します。</param>
        /// <param name="tableClassInstance">対象のテーブルクラスインスタンスを設定します。</param>
        /// <param name="dataRow">対象に設定するDataRowを設定します。</param>
        /// <param name="bindingAttr">BindingFlagsを設定します。</param>
        public static void SetValuesFromDataRow(
            [param: Required]Type classType,
            [param: Required]object tableClassInstance,
            [param: Required]DataRow dataRow,
            BindingFlags bindingAttr
            )
        {
            // null チェック
            if (tableClassInstance == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(tableClassInstance));
            if (dataRow == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(dataRow));

            // カラム名一覧を取得します。
            var columnNames = Utility.AttributeTable
                .GetColumnAttributeList(classType: classType, bindingAttr: bindingAttr);

            // 対象オブジェクトのプロパティに値を設定します。
            foreach (var n in columnNames)
            {
                var value = dataRow[n.ColumnAttribute.Name];
                Utility.ObjectUtil.SetPropertyValue(
                    target: tableClassInstance,
                    name: n.ColumnAttribute.Name,
                    value: value,
                    bindingAttr: bindingAttr
                    );
            }
        }
        #endregion Table
    }
}
