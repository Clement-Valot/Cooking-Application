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

using FinalProject.Models;
using FinalProject.Services.Auth;
using FinalProject.Pages.Admin;
using FinalProject.Services.Validator;

namespace FinalProject.Pages
{
    /// <summary>
    /// Logique d'interaction pour PageCreateAccount.xaml
    /// </summary>
    public partial class PageCreateAccount : Page
    {
        public PageCreateAccount()
        {
            InitializeComponent();
        }

        private void ButtonSignup_Click(object sender, RoutedEventArgs e)
        {
            string mail = TextBoxMail.Text;
            string password = TextBoxPassword.Password;
            string confirmPassword = TextBoxConfirmPassword.Password;
            string firstName = TextBoxFirstName.Text;
            string lastName = TextBoxLastName.Text;
            string phone = TextBoxPhone.Text;

            Validator validator = new Validator(new List<Input> {
                new Input("email", mail),
                new Input("password", password),
                new Input("confirm", password, confirmPassword)
            });

            //Affiche les erreurs
            if (validator.HasErrors)
            {
                string message = "";
                foreach (Input error in validator.Errors)
                {
                    message += error.Message + Environment.NewLine; // Environment.NewLine=\n
                }
                MessageBox.Show(message); //Ouvre une nouvelle petite fenêtre pour afficher les erreurs
            }
            else
            {
                Client client = new Client();
                client.Mail = mail;
                client.Password = password;
                client.FirstName = firstName;
                client.LastName = lastName;
                client.Phone = phone;

                ServiceAuthentication serviceAuthentication = new ServiceAuthentication();
                if (serviceAuthentication.Signup(client))
                {
                    PageHome pageHome = new PageHome();

                    ((MainWindow)Parent).Content = pageHome;
                }
                else
                {
                    MessageBox.Show("Sorry, something went wrong");
                }
            }                      
        }

        private void ButtonLoginRedirect_Click(object sender, RoutedEventArgs e)
        {
            PageLogin PageLogin = new PageLogin();

            ((MainWindow)Parent).Content = PageLogin;
        }


    }
}
