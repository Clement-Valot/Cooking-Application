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
    /// Logique d'interaction pour FragmentRestock.xaml
    /// </summary>
    public partial class FragmentRestock : Page
    {
        ObservableCollection<Product> productsToRestock;

        ServiceProduct serviceProduct;

        public FragmentRestock()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            serviceProduct = new ServiceProduct();
            productsToRestock = new ObservableCollection<Product>(serviceProduct.AllToRestock());

            ListViewProductsToRestock.ItemsSource = productsToRestock;
        }

        private void ButtonRestock_Click(object sender, RoutedEventArgs e)
        {
            ServiceSerializeXML serviceSerializeXML = new ServiceSerializeXML();
            serviceSerializeXML.SerializerXML();

            foreach(Product product in productsToRestock)
            {
                serviceProduct.Save(product, true);
            }

            productsToRestock.Clear();
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            ServiceProduct serviceProduct = new ServiceProduct();

            serviceProduct.UpdateUnused();
        }
    }
}
