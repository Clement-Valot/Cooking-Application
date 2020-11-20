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
using FinalProject.Pages.Admin;
using FinalProject.Models;
using FinalProject.Session;

namespace FinalProject.Pages
{
    /// <summary>
    /// Logique d'interaction pour PageLogin.xaml
    /// </summary>
    public partial class PageLogin : Page
    {
        public PageLogin()
        {
            InitializeComponent();
        }

        private void ButtonForgotRedirect_Click(object sender, RoutedEventArgs e)
        {
            PageForgotPassword PageForgotPassword = new PageForgotPassword();

            ((MainWindow)Parent).Content = PageForgotPassword;
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            string mail = TextBoxMail.Text;
            string password = TextBoxPassword.Password;

            ServiceAuthentication serviceAuthentication = new ServiceAuthentication();

            bool isAuth = serviceAuthentication.Authenticate(mail, password);

            if (isAuth)
            {
                PageHome pageHome = new PageHome();

                ((MainWindow)Parent).Content = pageHome;
            }
            else
            {
                MessageBox.Show("Vos identifiants sont erronés, veuillez réessayer");
            }
        }

        private void ButtonSignupRedirect_Click(object sender, RoutedEventArgs e)
        {
            PageCreateAccount pageCreateAccount = new PageCreateAccount();

            ((MainWindow)Parent).Content = pageCreateAccount;
        }
    }
}
