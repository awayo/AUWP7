﻿#pragma checksum "C:\Users\Fran\Documents\Visual Studio 2010\Projects\AUWP7\AUWP7\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B0ADB8E91C73ACAD24FB49DE9BD71475"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace AUWP7 {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ListBox novedades;
        
        internal System.Windows.Controls.ListBox series;
        
        internal System.Windows.Controls.ListBox entes;
        
        internal Microsoft.Phone.Controls.WebBrowser webBrowser1;
        
        internal System.Windows.Controls.ProgressBar progressBar1;
        
        internal System.Windows.Controls.TextBlock textCargando;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/AUWP7;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.novedades = ((System.Windows.Controls.ListBox)(this.FindName("novedades")));
            this.series = ((System.Windows.Controls.ListBox)(this.FindName("series")));
            this.entes = ((System.Windows.Controls.ListBox)(this.FindName("entes")));
            this.webBrowser1 = ((Microsoft.Phone.Controls.WebBrowser)(this.FindName("webBrowser1")));
            this.progressBar1 = ((System.Windows.Controls.ProgressBar)(this.FindName("progressBar1")));
            this.textCargando = ((System.Windows.Controls.TextBlock)(this.FindName("textCargando")));
        }
    }
}

