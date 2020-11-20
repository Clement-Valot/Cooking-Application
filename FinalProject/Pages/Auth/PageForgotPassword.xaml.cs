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

using FinalProject.Services.Auth;

namespace FinalProject.Pages
{
    /// <summary>
    /// Logique d'interaction pour PageForgotPassword.xaml
    /// </summary>
    public partial class PageForgotPassword : Page
    {
        public PageForgotPassword()
        {
            InitializeComponent();
        }

        private void ButtonLoginRedirect_Click(object sender, RoutedEventArgs e)
        {
            PageLogin PageLogin = new PageLogin();

            ((MainWindow)Parent).Content = PageLogin;
        }

        private void ButtonForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            string mail = TextBoxMail.Text;

            ServiceAuthentication serviceAuthentication = new ServiceAuthentication();

            string password = serviceAuthentication.ForgotPassword(mail);

            if (password != null)
            {
                StackPanelPassword.Visibility = Visibility.Visible;
                TextBlockPassword.Text = password;
            }
            else
            {
                MessageBox.Show("Your mail does not exist in our database");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StackPanelPassword.Visibility = Visibility.Collapsed;
        }
    }
}
