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
    /// Logique d'interaction pour FragmentRecipeCreator.xaml
    /// </summary>
    public partial class FragmentRecipeCreator : Page
    {
        ObservableCollection<RecipeCreator> RecipeCreators;

        ServiceRecipeCreator ServiceRecipeCreator;

        public FragmentRecipeCreator()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceRecipeCreator = new ServiceRecipeCreator();
            RecipeCreators = new ObservableCollection<RecipeCreator>(ServiceRecipeCreator.All());

            ListViewRecipereators.ItemsSource = RecipeCreators;
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            RecipeCreator recipeCreatorToRemove = (RecipeCreator)((Button)sender).Tag;

            ServiceRecipeCreator.Remove(recipeCreatorToRemove);
            RecipeCreators.Remove(recipeCreatorToRemove);
        }
    }
}
