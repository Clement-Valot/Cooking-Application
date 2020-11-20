using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FinalProject.Database
{
    /// <summary>
    /// Cette classe va servir a établir la connection avec la base de données mySql avec mon identifiant et mot de passe
    /// </summary>
    class Database
    {
        public MySqlConnection Connection;

        private const string SERVER = "localhost";
        private const string PORT = "3306";
        private const string DATABASE = "cooking";
        private const string UID = "root";
        private const string PASSWORD = "Cv270499@";

        /// <summary>
        /// Constructeur : a chaque fois qu'on instancie un objet de la classe Database, on effectue la connection à la BDD
        /// </summary>
        public Database()
        {
            this.InitConnection();
        }

        private void InitConnection()
        {
            try
            {
                string connectionString =
                    $"SERVER={Database.SERVER};" +
                    $"PORT={Database.PORT};" +
                    $"DATABASE={Database.DATABASE};" +
                    $"UID={Database.UID};" +
                    $"PASSWORD={Database.PASSWORD}";

                this.Connection = new MySqlConnection(connectionString);
            }
            catch (MySqlException e)
            {
                Console.WriteLine(" ErreurConnexion : " + e.ToString());
            }
        }
    }
}
