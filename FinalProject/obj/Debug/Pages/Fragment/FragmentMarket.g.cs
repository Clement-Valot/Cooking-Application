﻿#pragma checksum "..\..\..\..\Pages\Fragment\FragmentMarket.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "9DE4CCAFF601D1AD0928FADDE0F3707B6018FA3C741DCBD68F4E8B126F5B7A92"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using FinalProject.Pages.Fragment;
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


namespace FinalProject.Pages.Fragment {
    
    
    /// <summary>
    /// FragmentMarket
    /// </summary>
    public partial class FragmentMarket : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 26 "..\..\..\..\Pages\Fragment\FragmentMarket.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ListViewRecipes;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\..\Pages\Fragment\FragmentMarket.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ListViewRecipesInBucket;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\..\..\Pages\Fragment\FragmentMarket.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TextBlockTotalOrderCost;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\..\..\Pages\Fragment\FragmentMarket.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonOrder;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\..\..\Pages\Fragment\FragmentMarket.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonGiveCooks;
        
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
            System.Uri resourceLocater = new System.Uri("/FinalProject;component/pages/fragment/fragmentmarket.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\Fragment\FragmentMarket.xaml"
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
            
            #line 9 "..\..\..\..\Pages\Fragment\FragmentMarket.xaml"
            ((FinalProject.Pages.Fragment.FragmentMarket)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ListViewRecipes = ((System.Windows.Controls.ListView)(target));
            return;
            case 4:
            this.ListViewRecipesInBucket = ((System.Windows.Controls.ListView)(target));
            return;
            case 6:
            this.TextBlockTotalOrderCost = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.ButtonOrder = ((System.Windows.Controls.Button)(target));
            
            #line 113 "..\..\..\..\Pages\Fragment\FragmentMarket.xaml"
            this.ButtonOrder.Click += new System.Windows.RoutedEventHandler(this.ButtonOrder_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.ButtonGiveCooks = ((System.Windows.Controls.Button)(target));
            
            #line 116 "..\..\..\..\Pages\Fragment\FragmentMarket.xaml"
            this.ButtonGiveCooks.Click += new System.Windows.RoutedEventHandler(this.ButtonGiveCooks_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 3:
            
            #line 46 "..\..\..\..\Pages\Fragment\FragmentMarket.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonAddToBucket_Click);
            
            #line default
            #line hidden
            break;
            case 5:
            
            #line 94 "..\..\..\..\Pages\Fragment\FragmentMarket.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonRemoveFromBucket_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

