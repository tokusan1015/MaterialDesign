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
    public static class ObjectUtil
    {
        #region GetPropertyName
        /// <summary>
        /// 対象のプロパティ名を取得します。
        /// 対象をラムダ式で指定します。
        /// ex.)
        /// DateTime dt = DateTime.Now;
        /// string name = Utility.ObjectUtil.GetPropertyName(() => dt.Day);
        /// 結果：nameには、"Day"が入ります。
        /// </summary>
        /// <typeparam name="T">対象の型を設定します。</typeparam>
        /// <param name="func">ラムダ式を設定します。</param>
        /// <returns>プロパティ名を返します。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static string GetPropertyName<T>(
            [param: Required]Expression<Func<T>> func
            )
        {
            // nullチェック
            if (func == null)
                throw new ArgumentNullException("func");

            return ((MemberExpression)func.Body).Member.Name;
        }
        #endregion GetPropertyName

        #region GetPropertyValue
        /// <summary>
        /// プロパティ値を取得します。
        /// </summary>
        /// <param name="target">対象オブジェクトを設定します。</param>
        /// <param name="name">プロパティ名を設定します。</param>
        /// <returns>プロパティ値を返します。</returns>
        //public static object GetPropertyValue(
        //    [param: Required]object target,
        //    [param: Required]string name
        //    )
        //{
        //    // nullチェック
        //    if (target == null)
        //        throw new ArgumentNullException("target");
        //    if (name == null)
        //        throw new ArgumentNullException("name");
        //
        //    // プロパティ値を返します。
        //    return target.GetType().GetProperty(name: name)
        //        .GetValue(obj: target, index: null);
        //}
        /// <summary>
        /// プロパティ値を取得します。
        /// </summary>
        /// <param name="target">対象オブジェクトを設定します。</param>
        /// <param name="name">プロパティ名を設定します。</param>
        /// <param name="bindingAttr">BindingFlagsを設定します。</param>
        /// <returns>プロパティ値を返します。</returns>
        public static object GetPropertyValue(
            [param: Required]object target,
            [param: Required]string name,
            BindingFlags bindingAttr
            )
        {
            // nullチェック
            if (target == null)
                throw new ArgumentNullException("target");
            if (name == null)
                throw new ArgumentNullException("name");

            // プロパティ値を返します。
            return target.GetType().GetProperty(name: name, bindingAttr: bindingAttr)
                .GetValue(obj: target, index: null);
        }
        #endregion GetPropertyValue

        #region SetPropertyValue
        /// <summary>
        /// プロパティに値を設定します。
        /// </summary>
        /// <param name="target">対象オブジェクトを設定します。</param>
        /// <param name="name">プロパティ名を設定します。</param>
        /// <param name="value">値(null可)を設定します。</param>
        //public static void SetPropertyValue(
        //    [param: Required]object target,
        //    [param: Required]string name,
        //    object value
        //    )
        //{
        //    // nullチェック
        //    if (target == null)
        //        throw new ArgumentNullException("target");
        //    if (name == null)
        //        throw new ArgumentNullException("name");
        //
        //    // プロパティに値を設定します。
        //    target.GetType().GetProperty(name: name)
        //        .SetValue(obj: target, value: value);
        //}
        /// <summary>
        /// プロパティに値を設定します。
        /// </summary>
        /// <param name="target">対象オブジェクトを設定します。</param>
        /// <param name="name">プロパティ名を設定します。</param>
        /// <param name="value">値(null可)を設定します。</param>
        /// <param name="bindingAttr">BindingFlagsを設定します。</param>
        public static void SetPropertyValue(
            [param: Required]object target,
            [param: Required]string name,
            object value,
            BindingFlags bindingAttr
            )
        {
            // nullチェック
            if (target == null)
                throw new ArgumentNullException("target");
            if (name == null)
                throw new ArgumentNullException("name");

            // プロパティに値を設定します。
            target.GetType().GetProperty(name: name, bindingAttr: bindingAttr)
                .SetValue(obj: target, value: value);
        }
        #endregion SetPropertyValue

        #region GetClassPropertys
        /// <summary>
        /// クラスのプロパティ名一覧を取得します。
        /// </summary>
        /// <param name="typeClass">クラスタイプを設定します。</param>
        /// <returns>プロパティ名一覧を返します。</returns>
        //public static string[] GetClassPropertys(
        //    [param: Required]Type classType
        //    )
        //{
        //    // nullチェック
        //    if (classType == null)
        //        throw new ArgumentNullException("classType");

        //    // プロパティ名一覧を返します。
        //    return classType.GetProperties()
        //        .Select(x => x.Name)
        //        .ToArray();
        //}
        /// <summary>
        /// クラスのプロパティ名一覧を取得します。
        /// </summary>
        /// <param name="typeClass">クラスタイプを設定します。</param>
        /// <param name="bindingAttr">BindingFlagsを設定します。</param>
        /// <returns>プロパティ名一覧を返します。</returns>
        public static string[] GetClassPropertys(
            [param: Required]Type classType,
            BindingFlags bindingAttr
            )
        {
            // nullチェック
            if (classType == null)
                throw new ArgumentNullException("classType");

            // プロパティ名一覧を返します。
            return classType.GetProperties(bindingAttr: bindingAttr)
                .Select(x => x.Name)
                .ToArray();
        }
        #endregion GetClassPropertys

        #region GetClassMethods
        /// <summary>
        /// クラスのメソッド名一覧を取得します。
        /// </summary>
        /// <param name="classType">クラスタイプを設定します。</typeparam>
        /// <returns>メソッド名一覧を返します。</returns>
        //public static string[] GetClassMethods(
        //    [param: Required]Type classType
        //    )
        //{
        //    // nullチェック
        //    if (classType == null)
        //        throw new ArgumentNullException("classType");

        //    return classType.GetMethods()
        //        .Select(x => x.Name)
        //        .ToArray();
        //}
        /// <summary>
        /// クラスのメソッド名一覧を取得します。
        /// </summary>
        /// <param name="classType">クラスタイプを設定します。</typeparam>
        /// <param name="bindingAttr">BindingFlagsを設定します。</param>
        /// <returns>メソッド名一覧を返します。</returns>
        public static string[] GetClassMethods(
            [param: Required]Type classType,
            BindingFlags bindingAttr
            )
        {
            // nullチェック
            if (classType == null)
                throw new ArgumentNullException("classType");

            return classType.GetMethods(bindingAttr: bindingAttr)
                .Select(x => x.Name)
                .ToArray();
        }
        #endregion GetClassMethods

        #region CopyPropertyValues
        /// <summary>
        /// コピー元オブジェクトのプロパティ値を
        /// コピー先オブジェクトのプロパティに設定します。
        /// プロパティ名の一致するもののみを設定します。
        /// </summary>
        /// <param name="source">コピー元</param>
        /// <param name="dest">コピー先</param>
        /// <param name="typeMatchedOnly">対象を同一タイプのみとします。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public static void CopyPropertyValues(
            [param: Required]object source,
            [param: Required]object dest,
            bool typeMatchedOnly = false
            )
        {
            // nullチェック
            if (source == null)
                throw new ArgumentNullException("source");
            if (dest == null)
                throw new ArgumentNullException("dest");

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
        #endregion CopyPropertyValues
    }
}
