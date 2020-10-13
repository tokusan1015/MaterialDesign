using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Utility
{
    /// <summary>
    /// 属性ユーティリティ
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public static class AttributeUtil
    {
        #region 共通
        #region GetClassAttribute
        /// <summary>
        /// 対象クラスの属性インスタンスを取得します。
        /// </summary>
        /// <param name="classType">対象のクラスタイプを設定します。</param>
        /// <param name="attrType">属性タイプを設定します。</param>
        /// <returns>Attributeを返します。</returns>
        public static object GetClassAttribute(
            [param: Required]MemberInfo classType,
            [param: Required]Type attrType
            )
        {
            // nullチェック
            if (classType == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(classType));
            if (attrType == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(attrType));

            return classType.GetCustomAttributes(attrType, false).FirstOrDefault();
        }
        #endregion GetClassAttribute

        #region GetPropertyAttribute
        /// <summary>
        /// 対象クラスのプロパティを取得します。
        /// </summary>
        /// <param name="attrType">属性タイプを設定します。</param>
        /// <param name="classType">クラスタイプを設定します。</param>
        /// <param name="propertyName">プロパティ名を設定します。</param>
        /// <returns>設定した属性インスタンスが返ります。</returns>
        //public static object GetPropertyAttribute(
        //    [param: Required]Type attrType,
        //    [param: Required]Type classType,
        //    [param: Required]string propertyName
        //    )
        //{
        //    // null チェック
        //    if (attrType == null)
        //        throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(attrType");
        //    if (classType == null)
        //        throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(classType");
        //    if (propertyName == null)
        //        throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(propertyName");
        //
        //    PropertyInfo propertyInfo = classType.GetProperty(
        //        name: propertyName
        //        );
        //
        //    return propertyInfo?.GetCustomAttributes(attrType, false).FirstOrDefault();
        //}
        /// <summary>
        /// 対象クラスのプロパティを取得します。
        /// </summary>
        /// <param name="attrType">属性タイプを設定します。</param>
        /// <param name="classType">クラスタイプを設定します。</param>
        /// <param name="propertyName">プロパティ名を設定します。</param>
        /// <param name="bindingAttr">BindingFlagsを設定します。</param>
        /// <returns>設定した属性インスタンスが返ります。</returns>
        public static object GetPropertyAttribute(
            [param: Required]Type attrType,
            [param: Required]Type classType,
            [param: Required]string propertyName,
            BindingFlags bindingAttr
            )
        {
            // null チェック
            if (attrType == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(attrType));
            if (classType == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(classType));
            if (propertyName == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(propertyName));

            PropertyInfo propertyInfo = classType.GetProperty(
                name: propertyName,
                bindingAttr: bindingAttr
                );

            return propertyInfo?.GetCustomAttributes(attrType, false).FirstOrDefault();
        }
        #endregion GetPropertyAttribute

        #region GetMethodAttribute
        /// <summary>
        /// 対象クラスのメソッドを取得します。
        /// </summary>
        /// <param name="attrType">属性タイプを設定します。</param>
        /// <param name="classType">クラスタイプを設定します。</param>
        /// <param name="methodName">メソッド名を設定します。</param>
        /// <returns>設定した属性インスタンスが返ります。</returns>
        //public static object GetMethodAttribute(
        //    [param: Required]Type attrType,
        //    [param: Required]Type classType,
        //    [param: Required]string methodName
        //    )
        //{
        //    // nullチェック
        //    if (attrType == null)
        //        throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(attrType");
        //    if (classType == null)
        //        throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(classType");
        //    if (methodName == null)
        //        throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(methodName");
        //
        //    MethodInfo methodInfo = classType.GetMethod(
        //        name: methodName
        //        );
        //
        //    return methodInfo?.GetCustomAttributes(attrType, false).FirstOrDefault();
        //}
        /// <summary>
        /// 対象クラスのメソッドを取得します。
        /// </summary>
        /// <param name="attrType">属性タイプを設定します。</param>
        /// <param name="classType">クラスタイプを設定します。</param>
        /// <param name="methodName">メソッド名を設定します。</param>
        /// <param name="bindingAttr">BindingFlagsを設定します。</param>
        /// <returns>設定した属性インスタンスが返ります。</returns>
        public static object GetMethodAttribute(
            [param: Required]Type attrType,
            [param: Required]Type classType,
            [param: Required]string methodName,
            BindingFlags bindingAttr
            )
        {
            // nullチェック
            if (attrType == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(attrType));
            if (classType == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(classType));
            if (methodName == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(methodName));

            MethodInfo methodInfo = classType.GetMethod(
                name: methodName,
                bindingAttr: bindingAttr
                );

            return methodInfo?.GetCustomAttributes(attrType, false).FirstOrDefault();
        }
        #endregion GetMethodAttribute
        #endregion 共通
    }
}
