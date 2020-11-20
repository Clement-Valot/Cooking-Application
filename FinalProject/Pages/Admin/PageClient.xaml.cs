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
    /// Logique d'interaction pour PageClient.xaml
    /// </summary>
    public partial class PageClient : Page
    {
        ObservableCollection<Client> Clients;
        ServiceClient ServiceClient;

        public PageClient()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceClient = new ServiceClient();
            Clients = new ObservableCollection<Client>( ServiceClient.All() );

            ListViewClients.ItemsSource = Clients;
        }
    }
}
