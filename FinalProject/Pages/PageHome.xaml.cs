using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using FinalProject.Pages;
using FinalProject.Pages.Admin;
using FinalProject.Pages.Demo;
using FinalProject.Pages.Fragment;
using FinalProject.Session;

namespace FinalProject.Pages
{
    /// <summary>
    /// Logique d'interaction pour PageHome.xaml
    /// </summary>
    public partial class PageHome : Page
    {
        public static Button ActiveButton;
        public static Button GlobalButtonMarket;
        public static Button GlobalButtonCreate;

        public static Style Unactive;
        public static Style Active;

        public PageHome()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ActiveButton = ButtonMarket;
            ActiveButton.Style = (Style) FindResource("styleButtonActive");

            GlobalButtonMarket = ButtonMarket;
            GlobalButtonCreate = ButtonCreate;
            Unactive = (Style)FindResource("styleButtonHome");
            Active = (Style)FindResource("styleButtonActive");

            FragmentMarket fragmentMarket = new FragmentMarket();
            FrameContent.Content = fragmentMarket;

            ButtonMarket.Visibility = Visibility.Visible;
            ButtonAccount.Visibility = Visibility.Visible;
            ButtonLogout.Visibility = Visibility.Visible;

            if (AuthUser.Role == "Admin")
            {
                ButtonDemo.Visibility = Visibility.Visible;
                ButtonDashboard.Visibility = Visibility.Visible;
                ButtonRestock.Visibility = Visibility.Visible;
                ButtonRecipes.Visibility = Visibility.Visible;
                ButtonRecipeCreators.Visibility = Visibility.Visible;
            }
            if (AuthUser.RecipeCreator != null)
            {
                ButtonCreate.Visibility = Visibility.Visible;
            }
        }

        private void ButtonMarket_Click(object sender, RoutedEventArgs e)
        {
            SetButtonActive(sender);

            FragmentMarket fragmentMarket = new FragmentMarket();
            FrameContent.Content = fragmentMarket;
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            SetButtonActive(sender);

            FragmentCreateRecipe fragmentCreateRecipe = new FragmentCreateRecipe();
            FrameContent.Content = fragmentCreateRecipe;
        }

        private void ButtonAccount_Click(object sender, RoutedEventArgs e)
        {
            SetButtonActive(sender);

            FragmentAccount fragmentAccount = new FragmentAccount();
            FrameContent.Content = fragmentAccount;
        }

        private void ButtonDemo_Click(object sender, RoutedEventArgs e)
        {
            SetButtonActive(sender);

            FragmentDemo fragmentDemo = new FragmentDemo();
            FrameContent.Content = fragmentDemo;
        }

        private void ButtonDashboard_Click(object sender, RoutedEventArgs e)
        {
            SetButtonActive(sender);

            FragmentDashboard fragmentDashboard = new FragmentDashboard();
            FrameContent.Content = fragmentDashboard;
        }

        private void ButtonRestock_Click(object sender, RoutedEventArgs e)
        {
            SetButtonActive(sender);

            FragmentRestock fragmentRestock = new FragmentRestock();
            FrameContent.Content = fragmentRestock;
        }

        private void ButtonRecipes_Click(object sender, RoutedEventArgs e)
        {
            SetButtonActive(sender);

            FragmentRecipe fragmentRecipe = new FragmentRecipe();
            FrameContent.Content = fragmentRecipe;
        }

        private void ButtonRecipeCreators_Click(object sender, RoutedEventArgs e)
        {
            SetButtonActive(sender);

            FragmentRecipeCreator fragmentRecipeCreator = new FragmentRecipeCreator();
            FrameContent.Content = fragmentRecipeCreator;
        }

        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            AuthUser.Role = "";
            AuthUser.Client = null;
            AuthUser.RecipeCreator = null;
            PageLogin pageLogin = new PageLogin();
            ((MainWindow)Parent).Content = pageLogin;
        }

        public static void SetButtonActive(object sender, bool show = false)
        {
            ActiveButton.Style = Unactive;
            ActiveButton = (Button)sender;
            ActiveButton.Style = Active;
            if (show)
            {
                ActiveButton.Visibility = Visibility.Visible;
            }
            
        }
    }
}
