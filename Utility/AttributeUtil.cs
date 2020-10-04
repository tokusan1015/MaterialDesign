using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Utility
{
    /// <summary>
    /// 属性ユーティリティ
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class AttributeUtil
    {
        #region 共通
        /// <summary>
        /// 対象クラスの属性インスタンスを取得します。
        /// </summary>
        /// <param name="classType">対象のクラスタイプを設定します。</param>
        /// <returns>Attributeを返します。</returns>
        public static TAttribute GetClassAttribute<TAttribute>(
            [param: Required]Type classType
            ) where TAttribute : Attribute
        {
            var attrType = typeof(TAttribute);
            return (TAttribute)classType
                .GetCustomAttributes(attrType, false).FirstOrDefault();
        }
        /// <summary>
        /// 対象クラスのプロパティを取得します。
        /// </summary>
        /// <typeparam name="TAttribute">属性クラスを設定します。</typeparam>
        /// <param name="classType">クラスタイプを設定します。</param>
        /// <param name="propertyName">プロパティ名を設定します。</param>
        /// <param name="useAttr">BindingFlagsの使用の可否を設定します。</param>
        /// <param name="bindingAttr">BindingFlagsを設定します。</param>
        /// <returns>設定した属性インスタンスが返ります。</returns>
        public static TAttribute GetPropertyAttribute<TAttribute>(
            [param: Required]Type classType,
            string propertyName,
            bool useAttr = false,
            System.Reflection.BindingFlags bindingAttr = 
                System.Reflection.BindingFlags.Public 
                | System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Static
                | System.Reflection.BindingFlags.GetProperty
                | System.Reflection.BindingFlags.SetProperty
                | System.Reflection.BindingFlags.Instance
            ) where TAttribute : Attribute
        {
            var attrType = typeof(TAttribute);
            System.Reflection.PropertyInfo propertyInfo = null;

            if (useAttr)
            {
                propertyInfo = classType.GetProperty(
                    name: propertyName,
                    bindingAttr: bindingAttr
                    );
            }
            else
            {
                propertyInfo = classType.GetProperty(
                    name: propertyName
                    );
            }

            if (propertyInfo == null)
                throw new Exception($"propertyName : '{propertyName}'");

            return (TAttribute)propertyInfo.GetCustomAttributes(attrType, false).FirstOrDefault();
        }
        /// <summary>
        /// 対象クラスのメソッドを取得します。
        /// </summary>
        /// <typeparam name="TAttribute">属性クラスを設定します。</typeparam>
        /// <param name="classType">クラスタイプを設定します。</param>
        /// <param name="methodName">メソッド名を設定します。</param>
        /// <param name="useAttr">BindingFlagsの使用の可否を設定します。</param>
        /// <param name="bindingAttr">BindingFlagsを設定します。</param>
        /// <returns>設定した属性インスタンスが返ります。</returns>
        public static TAttribute GetMethodAttribute<TAttribute>(
            [param: Required]Type classType,
            string methodName,
            bool useAttr = false,
            System.Reflection.BindingFlags bindingAttr =
                System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Static
                | System.Reflection.BindingFlags.GetProperty
                | System.Reflection.BindingFlags.SetProperty
                | System.Reflection.BindingFlags.Instance
            ) where TAttribute : Attribute
        {
            var attrType = typeof(TAttribute);
            System.Reflection.MethodInfo methodInfo = null;

            if (useAttr)
            {
                methodInfo = classType.GetMethod(
                    name: methodName,
                    bindingAttr: bindingAttr
                    );
            }
            else
            {
                methodInfo = classType.GetMethod(
                    name: methodName
                    );
            }
            
            if (methodInfo == null)
                throw new Exception($"methodName : '{methodName}'");

            return (TAttribute)methodInfo.GetCustomAttributes(attrType, false).First();
        }
        #endregion 共通
    }
}
