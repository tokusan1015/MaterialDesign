using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Utility
{
    /// <summary>
    /// Objectユーティリティ
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class ObjectUtil
    {
        /// <summary>
        /// 対象のプロパティ名を取得します。
        /// 対象をラムダ式で指定します。
        /// ex.)
        /// DateTime dt = DateTime.Now;
        /// string name = Utility.ObjectUtil.GetPropertyName(() => dt.Day);
        /// 結果：nameには、"Day"が入ります。
        /// </summary>
        /// <typeparam name="T">対象の型を設定します。</typeparam>
        /// <param name="e">ラムダ式を設定します。</param>
        /// <returns>プロパティ名を返します。</returns>
        public static string GetPropertyName<T>(Expression<Func<T>> e)
        {
            return ((MemberExpression)e.Body).Member.Name;
        }

        /// <summary>
        /// プロパティ値を取得します。
        /// </summary>
        /// <param name="obj">対象オブジェクトを設定します。</param>
        /// <param name="name">プロパティ名を設定します。</param>
        /// <returns>プロパティ値を返します。</returns>
        public static object GetPropertyValue(
            object obj,
            string name
            )
        {
            // プロパティ値を返します。
            return obj.GetType().GetProperty(name: name)
                .GetValue(obj: obj, index: null);
        }

        /// <summary>
        /// プロパティに値を設定します。
        /// </summary>
        /// <param name="obj">対象オブジェクトを設定します。</param>
        /// <param name="name">プロパティ名を設定します。</param>
        /// <param name="value">値を設定します。</param>
        public static void SetPropertyValue(
            object obj,
            string name,
            object value
            )
        {
            // プロパティに値を設定します。
            obj.GetType().GetProperty(name: name)
                .SetValue(obj: obj, value: value);
        }

        /// <summary>
        /// クラスのプロパティ名一覧を取得します。
        /// </summary>
        /// <param name="typeClass">クラスタイプを設定します。</param>
        /// <returns>プロパティ名一覧を返します。</returns>
        public static string[] GetClassPropertys<TClass>(
            ) where TClass : class
        {
            // プロパティ名一覧を返します。
            return typeof(TClass).GetProperties()
                .Select(x => x.Name)
                .ToArray();
        }

        /// <summary>
        /// クラスのメソッド名一覧を取得します。
        /// </summary>
        /// <returns>メソッド名一覧を返します。</returns>
        public static string[] GetClassMethods<TClass>(
            ) where TClass : class
        {
            return typeof(TClass).GetMethods()
                .Select(x => x.Name)
                .ToArray();
        }

        /// <summary>
        /// コピー元オブジェクトのプロパティ値を
        /// コピー先オブジェクトのプロパティに設定します。
        /// プロパティ名の一致するもののみを設定します。
        /// </summary>
        /// <param name="source">コピー元</param>
        /// <param name="dest">コピー先</param>
        /// <param name="typeMatchedOnly">対象を同一タイプのみとします。</param>
        public static void CopyPropertyValues(
            [param: Required]object source,
            [param: Required]object dest,
            bool typeMatchedOnly = false
            )
        {
            // タイプを取得します。
            var sType = source.GetType();
            var dType = dest.GetType();
            
            // 同一タイプ検証が必要である場合
            if (typeMatchedOnly)
            {
                // 同一タイプ検証を行います。
                if (sType.Name != dType.Name)
                    throw new ArgumentException(
                        $"コピー元({sType.Name})とコピー先({dType.Name})は同じクラスである必要があります。");
            }

            // コピー元のメンバーリストを取得します。
            var sMembers = sType.GetMembers(bindingAttr:
                BindingFlags.Public
                | BindingFlags.Instance
                | BindingFlags.DeclaredOnly
                );

            // コピー先のメンバーリストを取得します。
            var dMembers = dType.GetMembers(bindingAttr:
                BindingFlags.Public
                | BindingFlags.Instance
                | BindingFlags.DeclaredOnly
                );
            
            // コピー名称リストを生成します。
            var nameList = new List<string>();
            foreach (var m in dMembers)
            {
                //プロパティのみリストに追加します。
                if (m.MemberType == MemberTypes.Property)
                {
                    nameList.Add(item: m.Name);
                }
            }

            // 同名プロパティの値をコピーします。
            foreach (var m in sMembers)
            {
                if (m.MemberType == MemberTypes.Property)
                {
                    //同名プパティであるか確認します。
                    if (nameList.Contains(item: m.Name))
                    {
                        // コピー元プロパティ値を取得します。
                        var sValue = sType
                            .GetProperty(name: m.Name)
                            .GetValue(obj: source, index: null);

                        // コピー先プロパティに値を設定します。
                        dType.GetProperty(name: m.Name)
                            .SetValue(obj: dest, value: sValue);
                    }
                }
            }
        }
    }
}
