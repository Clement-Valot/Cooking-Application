﻿#pragma checksum "..\..\..\..\Pages\Auth\PageForgotPassword.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "C2DD94363656047BD52548038C10282BE8F3FBF90529206BD9B20B4B6473767D"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using FinalProject.Pages;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace FinalProject.Pages {
    
    
    /// <summary>
    /// PageForgotPassword
    /// </summary>
    public partial class PageForgotPassword : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\..\..\Pages\Auth\PageForgotPassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBoxMail;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\Pages\Auth\PageForgotPassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel StackPanelPassword;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\Pages\Auth\PageForgotPassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TextBlockPassword;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\Pages\Auth\PageForgotPassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonForgotPassword;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\Pages\Auth\PageForgotPassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonLoginRedirect;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/FinalProject;component/pages/auth/pageforgotpassword.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\Auth\PageForgotPassword.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 9 "..\..\..\..\Pages\Auth\PageForgotPassword.xaml"
            ((FinalProject.Pages.PageForgotPassword)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TextBoxMail = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.StackPanelPassword = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.TextBlockPassword = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.ButtonForgotPassword = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\..\Pages\Auth\PageForgotPassword.xaml"
            this.ButtonForgotPassword.Click += new System.Windows.RoutedEventHandler(this.ButtonForgotPassword_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.ButtonLoginRedirect = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\..\..\Pages\Auth\PageForgotPassword.xaml"
            this.ButtonLoginRedirect.Click += new System.Windows.RoutedEventHandler(this.ButtonLoginRedirect_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

