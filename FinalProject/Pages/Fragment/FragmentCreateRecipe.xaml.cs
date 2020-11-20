using FinalProject.Models;
using FinalProject.Services;
using FinalProject.Session;
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

namespace FinalProject.Pages.Fragment
{
    /// <summary>
    /// Logique d'interaction pour FragmentCreateRecipe.xaml
    /// </summary>
    public partial class FragmentCreateRecipe : Page
    {
        List<Product> products;
        ObservableCollection<Product> productsFiltered;
        ObservableCollection<Product> ProductsRequiredInRecipe;

        ServiceRecipe ServiceRecipe;
        ServiceRecipeCreator ServiceRecipeCreator;

        Product ProductSelected;

        public FragmentCreateRecipe()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceProduct serviceProduct = new ServiceProduct();

            products = serviceProduct.All();
            productsFiltered = new ObservableCollection<Product>(products);
            ListViewAvailableProducts.ItemsSource = productsFiltered;

            ProductsRequiredInRecipe = new ObservableCollection<Product>();
            ListViewProductsRequired.ItemsSource = ProductsRequiredInRecipe;
        }

        private void ListViewAvailableProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductSelected = (Product)((ListBox)sender).SelectedItem;
            if (ProductSelected != null)
            {
                TextBlockUnity.Text = ProductSelected.Unity;
                TextBoxNewProduct.Clear();
            }
            
        }

        private void TextBoxSearchProduct_KeyUp(object sender, KeyEventArgs e)
        {
            productsFiltered = new ObservableCollection<Product>(products.Where(product => product.Name.Contains(((TextBox)sender).Text)));
            ListViewAvailableProducts.ItemsSource = productsFiltered;
            ListViewAvailableProducts.SelectedItem = null;
        }

        private void TextBoxNewProduct_KeyDown(object sender, KeyEventArgs e)
        {
            ListViewAvailableProducts.SelectedItem = null;
            TextBlockUnity.Text = ComboBoxUnity.Text;
        }

        private void ButtonAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxNewProduct.Text != "")
            {
                if (products.Any(product => product.Name.Equals(TextBoxNewProduct.Text)))
                {
                    MessageBox.Show("This product already exists");
                    return;
                }
                ProductSelected = new Product();
                ProductSelected.Name = TextBoxNewProduct.Text;

                ProductSelected.Category = ComboBoxCategorie.Text;
                ProductSelected.Unity = TextBlockUnity.Text;

                products.Add(ProductSelected);
                productsFiltered.Add(ProductSelected);
            }

            int quantity = Convert.ToInt32(TextBoxQuantityOfProductToAdd.Text);

            if (ProductSelected == null)
            {
                MessageBox.Show("Please select a product");
            }
            else if (quantity < 1)
            {
                MessageBox.Show("Please select a quantity");
            }
            else
            {
                ProductSelected.QuantityInRecipe = Convert.ToInt32(TextBoxQuantityOfProductToAdd.Text);

                if (!ProductsRequiredInRecipe.Contains(ProductSelected))
                {
                    ProductsRequiredInRecipe.Add(ProductSelected);
                }
            }
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            Product productToRemove = (Product)((Button)sender).Tag;

            ProductsRequiredInRecipe.Remove(productToRemove);            
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            string name = TextBoxName.Text;
            string type = ComboBoxType.Text;
            string priceString = TextBoxPrice.Text;
            int price;
            string description = TextBoxDescription.Text;

            string[] fields = new string[] { name, type, priceString, description };

            string message = "";
            if (fields.Contains(""))
            {
                message +=  "- All fields are required." + Environment.NewLine;
            }

            if (!Int32.TryParse(priceString, out price))
            {
                message += "- Price must be a number." + Environment.NewLine;
            }

            if (price < 10 || price > 40)
            {
                message += "- Price must be between 10 and 40 cooks." + Environment.NewLine;
            }

            if (ProductsRequiredInRecipe.Count() == 0)
            {
                message += "- You must select at least one product." + Environment.NewLine;
            }

            if (message != "")
            {
                MessageBox.Show(message);
                return;
            }

            Recipe recipe = new Recipe();

            recipe.Name = name;
            recipe.Type = type;
            recipe.Price = price;
            recipe.Description = description;
            recipe.Remuneration = 2;

            recipe.RecipeCreator = new RecipeCreator();
            if (AuthUser.RecipeCreator != null)
            {
                recipe.RecipeCreator = AuthUser.RecipeCreator;
            }
            else
            {
                ServiceRecipeCreator = new ServiceRecipeCreator();
                recipe.RecipeCreator = ServiceRecipeCreator.Save(AuthUser.Client);                
            }

            recipe.Products = ProductsRequiredInRecipe.ToList();

            ServiceRecipe = new ServiceRecipe();
            if (ServiceRecipe.Save(recipe))
            {
                if (AuthUser.Role != "Admin")
                {
                    AuthUser.Role = "RecipeCreator";
                }                
                AuthUser.RecipeCreator = recipe.RecipeCreator;
                MessageBox.Show("Your recipe has been created successfully!");

                FragmentCreateRecipe fragmentCreateRecipe = new FragmentCreateRecipe();

                PageHome.SetButtonActive(PageHome.GlobalButtonCreate, true);

                NavigationService.Navigate(fragmentCreateRecipe);
            }
            else
            {
                ServiceRecipeCreator.Remove(recipe.RecipeCreator);
                MessageBox.Show("Something went wrong, your recipe has not been created!");
            }            
        }

        private void ComboBoxUnity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(TextBlockUnity!=null)
            {
                TextBlockUnity.Text = (e.AddedItems[0] as ComboBoxItem).Content as string; ;
            }
            
        }
    }
}
