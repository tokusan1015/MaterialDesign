using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Utility
{
    /// <summary>
    /// DatePickerEx
    /// </summary>
    [Utility.Developer(name: "tokusan1015")]
    public partial class DatePickerEx : DatePicker
    {
        /// <summary>
        /// 標準日時
        /// </summary>
        public DateTime DefalutDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DatePickerEx()
        {
            this.DateValidationError += OnDateValidationError;
        }

        #region エラーハンドリング
        /// <summary>
        /// 日時エラーイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnDateValidationError(
            object sender,
            DatePickerDateValidationErrorEventArgs e
            )
        {
            if (sender is DatePicker dp)
            {
                // 標準設定日時
                var dt = DefalutDateTime;

                try
                {
                    // 入力された"yyyyMMdd"書式での日付でDateTimeに変換
                    dt = System.DateTime.ParseExact(dp.Text, "yyyyMMdd",
                                System.Globalization.DateTimeFormatInfo.InvariantInfo,
                                System.Globalization.DateTimeStyles.None);
                }
                catch (Exception)
                {
                    throw;
                }

                // DatePicker用のDateTimeをセット
                dp.SelectedDate = dt;
            }
        }
        #endregion エラーハンドリング

        #region IsReadOnly
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(
                name: "IsReadOnly",
                propertyType: typeof(bool),
                ownerType: typeof(DatePickerEx),
                typeMetadata: new FrameworkPropertyMetadata(
                    false, (d, e) => 
                    {
                        (d as DatePickerEx).OnIsReadOnlyPropertyChanged(e);
                    }));

        /// <summary>
        /// IsReadOnlyを表します。
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            OnIsReadOnlyPropertyChangedSub(IsReadOnly);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void OnIsReadOnlyPropertyChanged(
            DependencyPropertyChangedEventArgs e
            )
        {
            OnIsReadOnlyPropertyChangedSub((bool)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isReadOnly"></param>
        private void OnIsReadOnlyPropertyChangedSub(
            bool isReadOnly
            )
        {
            if (null != this.Template)
            {
                if (this.Template.FindName("PART_TextBox", this) is DatePickerTextBox textBox)
                {
                    textBox.IsReadOnly = isReadOnly;
                }

                if (this.Template.FindName("PART_Button", this) is Button button)
                {
                    button.Visibility = isReadOnly ? Visibility.Collapsed : Visibility.Visible;
                }
            }
        }
        #endregion IsReadOnly
    }
}
