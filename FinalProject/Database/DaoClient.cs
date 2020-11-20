using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Models;
using MySql.Data.MySqlClient;

namespace FinalProject.Database
{
    class DaoClient
    {
        private Database Database;

        //Constructeur : a chaque fois qu'on instancie un objet de la classe DaoClient, 
        //on instancie un objet de la classe Database qui lui effectue la connection à la BDD
        public DaoClient()
        {
            Database = new Database();
        }

        /// <summary>
        /// Enregistre le client dans la base de données.
        /// </summary>
        /// <param name="client">informations du client</param>
        /// <returns>renvoie true si le client a pe être ajouté et false sinon </returns>
        public bool Create(Client client)
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "INSERT INTO client (mail,mdp,prenom,nom,telephone) " +
                    "VALUES (@mail, @password, @firstname, @lastname, @phone)";

                commande.Parameters.AddWithValue("@mail", client.Mail);
                commande.Parameters.AddWithValue("@password", client.Password);
                commande.Parameters.AddWithValue("@firstname", client.FirstName);
                commande.Parameters.AddWithValue("@lastname", client.LastName);
                commande.Parameters.AddWithValue("@phone", client.Phone);

                int rowsAffected = commande.ExecuteNonQuery();

                Database.Connection.Close();
                return rowsAffected > 0;
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine(mySqlException.Message);
                return false;
            }
        }

        /// <summary>
        /// Récupère les informations du client d'identifiant id dans la BDD.
        /// </summary>
        /// <param name="id">identifiant du client dont on souhaite récupérer les infos</param>
        /// <returns>Renvoie le client récupérer</returns>
        public Client Read(int id)
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "SELECT * FROM client WHERE identifiant_client = @Id;";
                commande.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = commande.ExecuteReader();
                reader.Read();

                Client client = new Client();
                client.Id = id;
                client.Mail = reader.GetString( reader.GetOrdinal("mail") );
                client.FirstName = reader.GetString( reader.GetOrdinal("prenom") );
                client.LastName = reader.GetString( reader.GetOrdinal("nom") );
                client.Phone = reader.GetString( reader.GetOrdinal("telephone") );

                Database.Connection.Close();
                return client;
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine(mySqlException.Message);
                return null;
            }
        }

        /// <summary>
        /// Récupère la liste de tous les clients de la BDD
        /// </summary>
        /// <returns>une liste de tous les clients ou null s'il y a eu une erreur</returns>
        public List<Client> ReadAll()
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "SELECT * FROM client;";

                MySqlDataReader reader = commande.ExecuteReader();

                List<Client> clients = new List<Client>();

                if (reader.HasRows)
                {
                    //S'il y a plusieurs clients dans la BDD, le reader va avoir plusieurs lignes.
                    //Or lorsque l'on fait reader.Read(), reader prend la valeur d'une seule ligne.
                    //On doit donc boucler jusqu'à que reader.read() soit faux
                    while (reader.Read())
                    {
                        //On récupère les informations de chaque client à chaque itération
                        Client client = new Client();
                        client.Id = reader.GetInt16(reader.GetOrdinal("identifiant_client"));
                        client.Cooks = reader.GetInt16(reader.GetOrdinal("solde"));
                        client.Mail = reader.GetString(reader.GetOrdinal("mail"));
                        client.FirstName = reader.GetString(reader.GetOrdinal("prenom"));
                        client.LastName = reader.GetString(reader.GetOrdinal("nom"));
                        client.Phone = reader.GetString(reader.GetOrdinal("telephone"));

                        //On ajoute le client à la liste de Client
                        clients.Add(client);
                    }                    
                }                

                Database.Connection.Close();
                return clients;
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine(mySqlException.Message);
                return null;
            }
        }

        /// <summary>
        /// Met à jour les informations du client dans la BDD
        /// </summary>
        /// <param name="client">client à mettre à jour</param>
        /// <returns>retourne true si la maj a réussi et false sinon</returns>
        public bool Update(Client client)
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "UPDATE client " +
                    "SET mail = @mail, solde=@cooks, prenom = @firstname, nom = @lastname, telephone = @phone " +
                    "WHERE identifiant_client = @id";

                commande.Parameters.AddWithValue("@id", client.Id);
                commande.Parameters.AddWithValue("@mail", client.Mail);
                commande.Parameters.AddWithValue("@cooks", client.Cooks);
                commande.Parameters.AddWithValue("@firstname", client.FirstName);
                commande.Parameters.AddWithValue("@lastname", client.LastName);
                commande.Parameters.AddWithValue("@phone", client.Phone);

                int rowsAffected = commande.ExecuteNonQuery();

                Database.Connection.Close();
                return rowsAffected > 0;
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine(mySqlException.Message);
                return false;
            }
        }

        /// <summary>
        /// Supprime un client de la BDD.
        /// </summary>
        /// <param name="id">identifiant du client à supprimer</param>
        /// <returns>retourne true si la délétion a réussi et false sinon></returns>
        public bool Delete(int id)
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "DELETE FROM client WHERE identifiant_client = @id;";
                commande.Parameters.AddWithValue("@id", id);

                int rowsAffected = commande.ExecuteNonQuery();

                Database.Connection.Close();
                return rowsAffected > 0;
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine(mySqlException.Message);
                return false;
            }
        }
    }
}
