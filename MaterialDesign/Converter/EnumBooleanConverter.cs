using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace MaterialDesign.Converter
{

    /// <summary>
    /// RadioButtonのIsCheck(bool型)を列挙型に紐づけます。
    /// 好きな場所にコピーしてnamespaceを変更してください。
    /// App.xamlでパスを通した上で、Application.Resourcesに
    /// 記述することにより全てのxamlで利用可能です。
    /// ex.)localにコピーした場合(先頭の'＜'は、大文字になっています。)
    /// ＜Application.Resources>
    ///     ＜ResourceDictionary>
    ///         ＜local:EnumBooleanConverter x:Key="EnumBool"/>
    ///     ＜/ResourceDictionary>
    /// ＜/Application.Resources>
    /// 
    /// コントロール側のバインディングは以下の通りです。
    /// エレメント(elementName)のIsEnabledの変化に応じて切り替わります。
    /// ex.)
    /// IsChecked="{Binding Path=バインド名,Mode=TwoWay,Converter={StaticResource EnumBoolean},ConverterParameter=列挙型文字,UpdateSourceTrigger=PropertyChanged}"
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class EnumBooleanConverter : IValueConverter
    {
        /// <summary>
        /// EnumからBooleanへの変換を行います。
        /// </summary>
        /// <param name="value">Enum値を設定します。</param>
        /// <param name="targetType">ターゲットのタイプを設定します。</param>
        /// <param name="parameter">パラメータを設定します。</param>
        /// <param name="culture">カルチャー情報を設定します。</param>
        /// <returns>変換結果を返します。</returns>
        public object Convert(
            [param: Required]object value,
            Type targetType,
            object parameter,
            CultureInfo culture
            )
        {
            // nullチェック
            if (value == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(value));
            /*
            if (targetType == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(targetType");
            if (parameter == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(parameter");
            if (culture == null)
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().Name + Utility.ConstUtili.ERR_SEPA +nameof(culture");
            */

            if (!(parameter is string parameterString))
                return DependencyProperty.UnsetValue;

            if (!Enum.IsDefined(value.GetType(), value))
                return DependencyProperty.UnsetValue;

            var parameterValue = Enum.Parse(value.GetType(), parameterString);
            return parameterValue.Equals(value);
        }

        /// <summary>
        /// BooleanからEnumへの変換を行います。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
            )
        {
            if (!(parameter is string parameterString))
                return DependencyProperty.UnsetValue;

            if (true.Equals(value))
                return Enum.Parse(targetType, parameterString);
            else
                return DependencyProperty.UnsetValue;
        }
    }
}
