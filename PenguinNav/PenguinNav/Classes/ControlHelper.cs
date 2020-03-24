using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PenguinNav
{
    public static class ControlHelper
    {
        /// <summary>  
        /// The disable double click property  
        /// </summary>  
        public static readonly DependencyProperty DisableDoubleClickProperty =
            DependencyProperty.RegisterAttached("DisableDoubleClick", typeof(bool), typeof(ControlHelper), new FrameworkPropertyMetadata(false, OnDisableDoubleClickChanged));

        /// <summary>  
        /// Sets the disable double click.  
        /// </summary>  
        /// <param name="element">The element.</param>  
        /// <param name="value">if set to <c>true</c> [value].</param>  
        public static void SetDisableDoubleClick(UIElement element, bool value)
        {
            element.SetValue(DisableDoubleClickProperty, value);
        }

        /// <summary>  
        /// Gets the disable double click.  
        /// </summary>  
        /// <param name="element">The element.</param>  
        /// <returns></returns>  
        public static bool GetDisableDoubleClick(UIElement element)
        {
            return (bool)element.GetValue(DisableDoubleClickProperty);
        }

        /// <summary>  
        /// Called when [disable double click changed].  
        /// </summary>  
        /// <param name="d">The d.</param>  
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>  
        private static void OnDisableDoubleClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Control)d;
            if ((bool)e.NewValue)
            {
                control.PreviewMouseDown += (sender, args) =>
                {
                    if (args.ClickCount > 1)
                    {
                        args.Handled = true;
                    }
                };
            }
        }
    }
}
