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
using FinalProject.Models;
using FinalProject.Pages;
using FinalProject.Services;
using FinalProject.Session;

namespace FinalProject.Pages.Fragment
{
    /// <summary>
    /// Logique d'interaction pour FragmentAccount.xaml
    /// </summary>
    public partial class FragmentAccount : Page
    {
        ServiceRecipe ServiceRecipe;
        ServiceOrder ServiceOrder;

        public FragmentAccount()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlockFullName.Text = AuthUser.Client.FullName;
            TextBlockMail.Text = AuthUser.Client.Mail;
            TextBlocRole.Text = AuthUser.Role;
            TextBlockCooks.Text = AuthUser.Client.Cooks + " Cks";

            ServiceRecipe = new ServiceRecipe();

            if (AuthUser.RecipeCreator != null )
            {
                List<Recipe> recipes = ServiceRecipe.AllOf(AuthUser.RecipeCreator);
                if (recipes.Count > 0)
                {
                    ListViewRecipes.ItemsSource = recipes;
                    StackPanelCreateNow.Visibility = Visibility.Collapsed;
                    GridRecipes.Visibility = Visibility.Visible;
                }
            }

            ServiceOrder = new ServiceOrder();

            ListViewOrders.ItemsSource = ServiceOrder.AllOf(AuthUser.Client);

            if (ListViewOrders.Items.Count > 0)
            {
                StackPanelOrderNow.Visibility = Visibility.Collapsed;
                GridOrders.Visibility = Visibility.Visible;
            }

        }

        private void ButtonOrderNow_Click(object sender, RoutedEventArgs e)
        {
            FragmentMarket fragmentMarket = new FragmentMarket();

            PageHome.SetButtonActive(PageHome.GlobalButtonMarket);

            NavigationService.Navigate(fragmentMarket);
        }

        private void ButtonCreateNow_Click(object sender, RoutedEventArgs e)
        {
            FragmentCreateRecipe fragmentCreateRecipe = new FragmentCreateRecipe();

            PageHome.SetButtonActive(PageHome.GlobalButtonCreate);

            NavigationService.Navigate(fragmentCreateRecipe);
        }
    }
}
