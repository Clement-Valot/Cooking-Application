using FinalProject.Models;
using FinalProject.Services;
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

namespace FinalProject.Pages.Admin
{
    /// <summary>
    /// Logique d'interaction pour FragmentRecipe.xaml
    /// </summary>
    public partial class FragmentRecipe : Page
    {
        ObservableCollection<Recipe> Recipes;

        ServiceRecipe ServiceRecipe;

        public FragmentRecipe()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceRecipe = new ServiceRecipe();
            Recipes = new ObservableCollection<Recipe>(ServiceRecipe.All(true));

            ListViewRecipes.ItemsSource = Recipes;
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            Recipe recipeToRemove = (Recipe)((Button)sender).Tag;

            ServiceRecipe.Remove(recipeToRemove);
            Recipes.Remove(recipeToRemove);
        }
    }
}
