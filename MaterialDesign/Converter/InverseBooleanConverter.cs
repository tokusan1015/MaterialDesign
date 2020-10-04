using System;
using System.Globalization;
using System.Windows.Data;

namespace MaterialDesign.Converter
{
    /// <summary>
    /// bool型を反転して返します。
    /// 好きな場所にコピーしてnamespaceを変更してください。
    /// App.xamlでパスを通した上で、Application.Resourcesに
    /// 記述することにより全てのxamlで利用可能です。
    /// ex.)localにコピーした場合(先頭の'＜'は、大文字になっています。)
    /// ＜Application.Resources>
    ///     ＜ResourceDictionary>
    ///         ＜local:InverseBooleanConverter x:Key="InvertBool"/>
    ///     ＜/ResourceDictionary>
    /// ＜/Application.Resources>
    ///
    /// コントロール側のバインディングは以下の通りです。
    /// エレメント(elementName)のIsEnabledの変化に応じて切り替わります。
    /// ex.)
    /// IsEnabled="{Binding ElementName=elementName, Path=IsEnabled, Converter={StaticResource InvertBool}}"
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public class InverseBooleanConverter : IValueConverter
    {
        /// <summary>
        /// bool値を反転して返します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is bool && (bool)value);
        }

        /// <summary>
        /// bool値を反転して返します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is bool && (bool)value);
        }
    }
}
