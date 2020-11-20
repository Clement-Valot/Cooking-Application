using FinalProject.Models;
using FinalProject.Services;
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

namespace FinalProject.Pages.Admin
{
    /// <summary>
    /// Logique d'interaction pour FragmentDashboard.xaml
    /// </summary>
    public partial class FragmentDashboard : Page
    {
        ServiceRecipeCreator ServiceRecipeCreator;
        ServiceRecipe ServiceRecipe;

        public FragmentDashboard()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceRecipeCreator = new ServiceRecipeCreator();
            RecipeCreator recipeCreatorOfTheWeek = ServiceRecipeCreator.BestOfTheWeek();

            if (recipeCreatorOfTheWeek != null)
            {
                TextBlockRecipeCreatorOfTheWeek.Text = recipeCreatorOfTheWeek.Client.FullName;
                TextBlockCountRecipesOfCreatorOfTheWeek.Text = $"{recipeCreatorOfTheWeek.RecipesSold} recipes sold last 7 days";
            }
            else
            {
                TextBlockCountRecipesOfCreatorOfTheWeek.Text = "No recipes sold last 7 days";
            }

            RecipeCreator recipeCreatorOfAllTime = ServiceRecipeCreator.BestOfAllTime();

            ServiceRecipe = new ServiceRecipe();

            if (recipeCreatorOfAllTime != null)
            {
                TextBlockRecipeCreatorOfAllTime.Text = recipeCreatorOfAllTime.Client.FullName;
                TextBlockCountRecipesOfCreatorOfAllTime.Text = $"{recipeCreatorOfAllTime.RecipesSold} recipes sold";

                TextBlockTopOfBestCreatorTitle.Text = "Top 5 recipes of: " + recipeCreatorOfAllTime.Client.FullName;
                ListViewTopRecipesOfBestCreator.ItemsSource = ServiceRecipe.AllOf(recipeCreatorOfAllTime, 5);
            }
            else
            {
                TextBlockCountRecipesOfCreatorOfTheWeek.Text = "No recipes sold";
            }

            ListViewTop5Recipes.ItemsSource = ServiceRecipe.Top(5);
        }
    }
}
