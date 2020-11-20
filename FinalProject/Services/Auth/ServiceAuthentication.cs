using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinalProject.Database;
using FinalProject.Models;
using FinalProject.Session;
using MySql.Data.MySqlClient;

namespace FinalProject.Services.Auth
{
    class ServiceAuthentication
    {
        Database.Database Database;
        ServiceClient ServiceClient;

        public ServiceAuthentication ()
        {
            Database = new Database.Database();
        }

        /// <summary>
        /// Cette méthode vérifie dans un premier temps si le mail et le mdp de l'utilisateur appartiennent à la DB.
        /// Si la combinaison du mail et du password ne correspond à aucun client, alors le reader n'a pas de ligne et on renvoie false.
        /// Sinon, on instancie l'attribut de classe client de la classe AuthUser et on lui rentre les infos du client récupéré.
        /// Ensuite, on vérifie si ce même client n'est pas cdr. Si tel est le cas l'attribut de classe cdr de la classe AuthUser est instancié avec
        /// les infos récupérées du cdr. On passe également l'attribut role de AuthUser à 'RecipeCreator'; sinon 'Client'
        /// Enfin, on regarde si l'utilisateur n'est pas l'admin (identifiant client 1 ou 2) auquel cas le rôle de AuthUser devient 'Admin'.
        /// </summary>
        /// <param name="mail">mail rentré par l'utilisateur</param>
        /// <param name="password">mdp rentré par l'utilisateur</param>
        /// <returns></returns>
        public bool Authenticate(string mail, string password)
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = String.Format(
                    "SELECT * FROM client " +
                    "WHERE mail = @mail AND mdp = @password;"
                );
                commande.Parameters.AddWithValue("@mail", mail);
                commande.Parameters.AddWithValue("@password", password);

                MySqlDataReader reader = commande.ExecuteReader();

                if (!reader.HasRows)
                {
                    Database.Connection.Close();
                    return false;
                }

                reader.Read();

                AuthUser.Client = new Client();
                AuthUser.Client.Id = reader.GetInt32(reader.GetOrdinal("identifiant_client"));
                AuthUser.Client.Mail = reader.GetString(reader.GetOrdinal("mail"));
                AuthUser.Client.FirstName = reader.GetString(reader.GetOrdinal("prenom"));
                AuthUser.Client.LastName = reader.GetString(reader.GetOrdinal("nom"));
                AuthUser.Client.Cooks = reader.GetInt32(reader.GetOrdinal("solde"));
                AuthUser.Client.Phone = reader.GetString(reader.GetOrdinal("telephone"));

                reader.Close();

                commande.CommandText = "SELECT * FROM cdr WHERE cdr.identifiant_client = @clientId";
                commande.Parameters.AddWithValue("@clientId", AuthUser.Client.Id);

                reader = commande.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    AuthUser.RecipeCreator = new RecipeCreator();
                    AuthUser.RecipeCreator.Id = reader.GetInt32( reader.GetOrdinal("identifiant_cdr") );
                    AuthUser.Role = "RecipeCreator";
                }
                else
                {
                    AuthUser.Role = "Client";
                }

                if (AuthUser.Client.Id < 3)
                {
                    AuthUser.Role = "Admin";
                }

                reader.Close();
                Database.Connection.Close();

                return true;
            }
            //Si on catch une exception, on oublie pas de remettre les valeurs de authuser à null
            catch (MySqlException mySqlException)
            {
                AuthUser.Client = null;
                AuthUser.RecipeCreator = null;
                AuthUser.Role = "";
                Console.WriteLine(mySqlException.Message);
                return false;
            }
            
        }

        /// <summary>
        /// Cette méthode intervient dans le cas d'une création de compte
        /// </summary>
        /// <param name="client">client qui contient déjà les info mails, mdp, nom, prénom, tel rentrées par l'utilisateur</param>
        /// <returns>retourne true si le compte du client a été créer avec succès et si son rôle dans le site a bien été défini</returns>
        public bool Signup(Client client)
        {
            try
            {
                //// Réaliser le test de format de mail et de telephone ici
                ////On vérifie si le format de l'adresse mail est valide
                //EmailAddressAttribute EmailValidator = new EmailAddressAttribute();
                //if (!EmailValidator.IsValid(client.Mail))
                //{
                //    return false;
                //}
                ////On vérifie si le format du mdp est correct
                //if (client.Password.Length < 6)
                //{
                //    return false;
                //}     

                ServiceClient = new ServiceClient();
                //Si l'enregistrment du client dans la DB est réusso et que l'autentification se passe sans encombre (affilie un rôle à l'utilisateur)
                //alors on retourne true
                return ServiceClient.Save(client) && Authenticate(client.Mail, client.Password);
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Renvoie le mdp correspondant au mail rentré en paramètre. Si même le mail est erroné et n'existe pas dans la DB, alors
        /// la méthode renvoie null.
        /// </summary>
        /// <param name="mail">mail du client qui souhaite retrouver son mdp</param>
        /// <returns>renvoie le mdp si le mail existe dans la DB, null sinon</returns>
        public string ForgotPassword(string mail)
        {
            //On initialise le mdp à null
            string mdp = null;
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "SELECT mdp FROM client WHERE mail = @mail;";
                commande.Parameters.AddWithValue("@mail", mail);

                MySqlDataReader reader = commande.ExecuteReader();

                //Si le reader a des lignes cela signifie que le mail existe dans la DB donc mdp prend une valeur non nulle
                if (reader.HasRows)
                {
                    reader.Read();
                    mdp= reader.GetString( reader.GetOrdinal("mdp") );
                }
            }
            catch(MySqlException e)
            {
                Console.WriteLine(e.Message);
            }
            Database.Connection.Close();
            return mdp;
        }
    }
}
