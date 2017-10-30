using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TheGuardianProject.UWP
{
    public class WebViewHelper
    {
        // Using a DependencyProperty as the backing store for HTML.  This enables animation, styling, binding, etc... 
        public static readonly DependencyProperty HTMLProperty = DependencyProperty.RegisterAttached(
            "HTML",
            typeof(string),
            typeof(WebViewHelper),
            new PropertyMetadata(0, new PropertyChangedCallback(OnHTMLChanged)));


        private static void OnHTMLChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            WebView wv = d as WebView;
            if (wv != null)
            {
                var webContent = e.NewValue as string;
                if (webContent != null)
                    wv.NavigateToString(webContent);
            }

        }

        public static string GetHTML(DependencyObject obj)
        {
            return (string)obj.GetValue(HTMLProperty);
        }

        public static void SetHTML(DependencyObject obj, string value)
        {
            obj.SetValue(HTMLProperty, value);
        }

    }
}
