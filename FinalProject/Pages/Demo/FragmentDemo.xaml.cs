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

namespace FinalProject.Pages.Demo
{
    /// <summary>
    /// Logique d'interaction pour FragmentDemo.xaml
    /// </summary>
    public partial class FragmentDemo : Page
    {
        ServiceRecipeCreator ServiceRecipeCreator;
        ServiceClient ServiceClient;
        ServiceRecipe ServiceRecipe;
        ServiceProduct ServiceProduct;

        Product SelectedProduct;
        ObservableCollection<Recipe> recipesOfSelectedProduct;

        public FragmentDemo()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceRecipeCreator = new ServiceRecipeCreator();
            List<RecipeCreator> recipeCreators = ServiceRecipeCreator.AllWithRecipes();
            ListViewRecipeCreators.ItemsSource = recipeCreators;
            TextBlockCountRecipeCreators.Text = (recipeCreators.Count).ToString();

            ServiceProduct = new ServiceProduct();
            ListViewProductsToRestock.ItemsSource = ServiceProduct.AllForDemo();

            ServiceClient = new ServiceClient();
            TextBlockCountClients.Text = ((ServiceClient.All()).Count).ToString();

            ServiceRecipe = new ServiceRecipe();
            TextBlockCountRecipes.Text = ((ServiceRecipe.All()).Count).ToString();
        }

        private void ListViewProductsToRestock_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedProduct = (Product)((ListBox)sender).SelectedValue;

            TextBlockRecipesProductTitle.Text = SelectedProduct.Name; 

            recipesOfSelectedProduct = new ObservableCollection<Recipe>(ServiceRecipe.AllOf(SelectedProduct));

            ListViewRecipesOfSelectedProduct.ItemsSource = recipesOfSelectedProduct;
        }

        private void Grid_Selected(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(((Product)sender).Name);
        }
    }
}
