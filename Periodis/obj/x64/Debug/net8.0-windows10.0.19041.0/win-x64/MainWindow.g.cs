﻿#pragma checksum "D:\GitHub\Repositories\periodis\Periodis\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "3B07FB3BB4959C5CFE8A98CDE9DD3EE7F1269E7343B3667B9B82FC6C13C55A3F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Periodis
{
    partial class MainWindow : 
        global::Microsoft.UI.Xaml.Window, 
        global::Microsoft.UI.Xaml.Markup.IComponentConnector
    {

        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2503")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // MainWindow.xaml line 12
                {
                    this.RootCanvas = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Canvas>(target);
                }
                break;
            case 3: // MainWindow.xaml line 13
                {
                    this.PeriodicTable = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Grid>(target);
                }
                break;
            case 4: // MainWindow.xaml line 36
                {
                    this.InfoPanel = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Border>(target);
                }
                break;
            case 5: // MainWindow.xaml line 44
                {
                    this.ElementName = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 6: // MainWindow.xaml line 45
                {
                    this.ElementDetails = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 7: // MainWindow.xaml line 18
                {
                    this.SearchBox = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBox>(target);
                    ((global::Microsoft.UI.Xaml.Controls.TextBox)this.SearchBox).KeyDown += this.SearchBox_KeyDown;
                }
                break;
            case 8: // MainWindow.xaml line 22
                {
                    this.SearchSelector = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.ComboBox>(target);
                }
                break;
            case 9: // MainWindow.xaml line 29
                {
                    this.ResultText = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 10: // MainWindow.xaml line 32
                {
                    this.ResultsPanel = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.StackPanel>(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }


        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2503")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Microsoft.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

