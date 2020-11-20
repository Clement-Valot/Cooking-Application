using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using FinalProject.Services;
using FinalProject.Session;

namespace FinalProject.Pages.Fragment
{
    /// <summary>
    /// Logique d'interaction pour FragmentRecipes.xaml
    /// </summary>
    public partial class FragmentMarket : Page
    {
        ObservableCollection<Recipe> recipesInBucket;
        ObservableCollection<Recipe> recipesAvailable;
        Order order;

        ServiceOrder serviceOrder;

        public FragmentMarket()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceRecipe serviceRecipe = new ServiceRecipe();
            serviceOrder = new ServiceOrder();

            recipesAvailable = new ObservableCollection<Recipe>(serviceRecipe.All());
            ListViewRecipes.ItemsSource = recipesAvailable;

            order = new Order();
            order.Client = AuthUser.Client;

            recipesInBucket = new ObservableCollection<Recipe>();
            ListViewRecipesInBucket.ItemsSource = recipesInBucket;            
            order.Recipes = recipesInBucket.ToList();
        }

        private void ButtonAddToBucket_Click(object sender, RoutedEventArgs e)
        {
            Recipe recipeToAdd = (Recipe)((Button)sender).Tag;           

            if (recipeToAdd.QuantiteFaisable == 0)
            {
                MessageBox.Show("Stock insuffisant pour produire plus de cette recette");
            }
            else if (recipeToAdd.QuantityInOrder != 0)
            {
                recipeToAdd.QuantityInOrder++;
            }
            else
            {
                recipeToAdd.QuantityInOrder = 1;
                recipesInBucket.Add(recipeToAdd);
            }
            serviceOrder.SynchronizeQuantiteFaisable(recipesAvailable, recipeToAdd);
            order.Recipes = recipesInBucket.ToList();
            TextBlockTotalOrderCost.Text = order.TotalCost.ToString();
        }

        private void ButtonRemoveFromBucket_Click(object sender, RoutedEventArgs e)
        {
            Recipe recipeToRemove = (Recipe)((Button)sender).Tag;

            serviceOrder.SynchronizeQuantiteFaisable(recipesAvailable, recipeToRemove, true);

            if (recipeToRemove.QuantityInOrder > 1)
            {
                recipeToRemove.QuantityInOrder--;
            }
            else
            {
                recipeToRemove.QuantityInOrder = 0;                
                recipesInBucket.Remove(recipeToRemove);
            }

            
            
            order.Recipes = recipesInBucket.ToList();
            TextBlockTotalOrderCost.Text = order.TotalCost.ToString();
        }

        private void ButtonOrder_Click(object sender, RoutedEventArgs e)
        {
            if (recipesInBucket.Count == 0)
            {
                MessageBox.Show("Votre commande ne contient aucune recette");
                return;
            }
            if(AuthUser.Client.Cooks<order.TotalCost)
            {
                MessageBox.Show("Solde Cook insuffisant");
                ButtonGiveCooks.Visibility = Visibility.Visible;
                return;
            }

            ServiceOrder serviceOrder = new ServiceOrder();
            serviceOrder.Save(order);

            MessageBox.Show("Votre commande a bien été enregistrée vous serez livrés dans 35 min");

            //Reload la page après une commande
            FragmentMarket fragmentMarket = new FragmentMarket();
            NavigationService.Navigate(fragmentMarket);
        }

        private void ButtonGiveCooks_Click(object sender, RoutedEventArgs e)
        {
            AuthUser.Client.Cooks += 100;
            ServiceClient serviceClient = new ServiceClient();
            serviceClient.Save(AuthUser.Client);
            MessageBox.Show("100 cooks supplémentaires ont été rajoutés à votre compte.");
            ButtonGiveCooks.Visibility= Visibility.Collapsed;
        }
    }
}
