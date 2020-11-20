using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinalProject.Models;
using MySql.Data.MySqlClient;

namespace FinalProject.Database
{
    class DaoRecipeCreator
    {
        private Database Database;

        public DaoRecipeCreator()
        {
            Database = new Database();
        }

        /// <summary>
        /// Enregistre le Cdr dans la BDD.
        /// </summary>
        /// <param name="recipeCreator">cdr à créer</param>
        /// <returns>retourne le Cdr avec sa référence en plus</returns>
        public RecipeCreator Create(RecipeCreator recipeCreator)
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "INSERT INTO cdr ( identifiant_client ) " +
                    "VALUES (@clientId); " +
                    "SELECT cdr.identifiant_cdr FROM cdr ORDER BY cdr.identifiant_cdr DESC LIMIT 1";

                commande.Parameters.AddWithValue("@clientId", recipeCreator.Client.Id);

                recipeCreator.Id = (int) commande.ExecuteScalar();

                Database.Connection.Close();

                return recipeCreator;
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine(mySqlException.Message);
                return null;
            }
        }

        /// <summary>
        /// Récupère les informations du cdr de la DB à partir de son identifiant.
        /// On veut aussi et surtout récupérer les infos du client relié à ce cdr.
        /// </summary>
        /// <param name="id">id du cdr dont on veut les données</param>
        /// <returns>retourne les infos du cdr</returns>
        public RecipeCreator Read(int id)
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "SELECT * FROM cdr, client WHERE cdr.identifiant_client = client.identifiant_client AND identifiant_cdr = @Id;";
                commande.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = commande.ExecuteReader();
                reader.Read();

                RecipeCreator recipeCreator = new RecipeCreator();
                recipeCreator.Id = id;                

                recipeCreator.Client = new Client();
                recipeCreator.Client.Id = reader.GetInt32(reader.GetOrdinal("identifiant_client"));
                recipeCreator.Client.Mail = reader.GetString(reader.GetOrdinal("mail"));
                recipeCreator.Client.FirstName = reader.GetString(reader.GetOrdinal("prenom"));
                recipeCreator.Client.LastName = reader.GetString(reader.GetOrdinal("nom"));
                recipeCreator.Client.Phone = reader.GetString(reader.GetOrdinal("telephone"));

                recipeCreator.Cooks = reader.GetInt32(reader.GetOrdinal("solde"));

                Database.Connection.Close();
                return recipeCreator;
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine(mySqlException.Message);
                return null;
            }
        }

        /// <summary>
        /// Récupère le Cdr le plus commandé sur les 7 derniers jours.
        /// </summary>
        /// <returns>renvoie le Cdr de la semaine</returns>
        public RecipeCreator ReadOfTheWeek()
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand command = Database.Connection.CreateCommand();

                command.CommandText = "select sum(contenir.nombre) as best, cl.*, cd.* " +
                    "from cdr as cd, client as cl, commande as c, recette, contenir " +
                    "where cl.identifiant_client = cd.identifiant_client " +
                    "and cd.identifiant_cdr = recette.identifiant_cdr " +
                    "and recette.ref_recette = contenir.ref_recette " +
                    "and contenir.ref_commande = c.ref_commande " +
                    "and date_format(c.date,'%Y-%m-%d') >= @Date " +
                    "group by cd.identifiant_cdr " +
                    "order by best desc " +
                    "limit 1";
                TimeSpan decalage = new TimeSpan(7, 0, 0, 0);
                DateTime DayLastWeek = DateTime.Today - decalage;
                command.Parameters.AddWithValue("@Date", DayLastWeek.ToString("yyyy-MM-dd"));

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    RecipeCreator recipeCreator = new RecipeCreator();
                    recipeCreator.Id = reader.GetInt32(reader.GetOrdinal("identifiant_cdr"));
                    recipeCreator.RecipesSold = reader.GetInt32(reader.GetOrdinal("best"));

                    recipeCreator.Client = new Client();
                    recipeCreator.Client.Id = reader.GetInt32(reader.GetOrdinal("identifiant_client"));
                    recipeCreator.Client.Mail = reader.GetString(reader.GetOrdinal("mail"));
                    recipeCreator.Client.FirstName = reader.GetString(reader.GetOrdinal("prenom"));
                    recipeCreator.Client.LastName = reader.GetString(reader.GetOrdinal("nom"));
                    recipeCreator.Client.Phone = reader.GetString(reader.GetOrdinal("telephone"));

                    recipeCreator.Cooks = reader.GetInt32(reader.GetOrdinal("solde"));

                    reader.Close();

                    Database.Connection.Close();
                    return recipeCreator;
                }
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine(mySqlException.Message);
            }

            Database.Connection.Close();
            return null;
        }

        /// <summary>
        /// Récupère le Cdr qui cumule le plus de commandes de ses recettes depuis la création
        /// du site. c'est presque la même méthode que pour récupérer le cdr de la semaine à 
        /// l'exception près que dans cette requête, on ne fait pas attention à la date des commandes.
        /// </summary>
        /// <returns>retourne le cdr d'or</returns>
        public RecipeCreator ReadOfAllTime()
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand command = Database.Connection.CreateCommand();

                command.CommandText = "select sum(contenir.nombre) as best, cl.*, cd.* " +
                    "from cdr as cd, client as cl, commande as c, recette, contenir " +
                    "where cl.identifiant_client = cd.identifiant_client " +
                    "and cd.identifiant_cdr = recette.identifiant_cdr " +
                    "and recette.ref_recette = contenir.ref_recette " +
                    "and contenir.ref_commande = c.ref_commande " +
                    "group by cd.identifiant_cdr " +
                    "order by best desc " +
                    "limit 1";

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    RecipeCreator recipeCreator = new RecipeCreator();
                    recipeCreator.Id = reader.GetInt32(reader.GetOrdinal("identifiant_cdr"));
                    recipeCreator.RecipesSold = reader.GetInt32(reader.GetOrdinal("best"));

                    recipeCreator.Client = new Client();
                    recipeCreator.Client.Id = reader.GetInt32(reader.GetOrdinal("identifiant_client"));
                    recipeCreator.Client.Mail = reader.GetString(reader.GetOrdinal("mail"));
                    recipeCreator.Client.FirstName = reader.GetString(reader.GetOrdinal("prenom"));
                    recipeCreator.Client.LastName = reader.GetString(reader.GetOrdinal("nom"));
                    recipeCreator.Client.Phone = reader.GetString(reader.GetOrdinal("telephone"));

                    recipeCreator.Cooks = reader.GetInt32(reader.GetOrdinal("solde"));

                    reader.Close();

                    Database.Connection.Close();
                    return recipeCreator;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            Database.Connection.Close();
            return null;
        }

        /// <summary>
        /// Récupère tous les cdr de la DB avec toutes les recettes qu'ils ont crée
        /// </summary>
        /// <returns>retourne la liste des cdr</returns>
        public List<RecipeCreator> ReadAll()
        {
            List<RecipeCreator> recipeCreators = new List<RecipeCreator>();
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "SELECT *, client.nom as nom_client FROM cdr, client, recette " +
                    "WHERE cdr.identifiant_client = client.identifiant_client " +
                    "AND cdr.identifiant_cdr = recette.identifiant_cdr;";

                MySqlDataReader reader = commande.ExecuteReader();

                while (reader.Read())
                {
                    Recipe recipe = new Recipe();
                    recipe.Ref = reader.GetInt32(reader.GetOrdinal("ref_recette"));
                    recipe.TimesOrdered = reader.GetInt32(reader.GetOrdinal("nbr_commande"));

                    int recipeCreatorId = reader.GetInt32(reader.GetOrdinal("identifiant_cdr"));

                    RecipeCreator recipeCreator = null;

                    if (recipeCreators.Any(recipeCr => recipeCr.Id == recipeCreatorId))
                    {
                        recipeCreator = recipeCreators.First(recipeCr => recipeCr.Id == recipeCreatorId);
                    }

                    if (recipeCreator == null)
                    {
                        recipeCreator = new RecipeCreator();
                        recipeCreator.Id = reader.GetInt32(reader.GetOrdinal("identifiant_cdr"));

                        recipeCreator.Client = new Client();
                        recipeCreator.Client.Id = reader.GetInt32(reader.GetOrdinal("identifiant_client"));
                        recipeCreator.Client.Mail = reader.GetString(reader.GetOrdinal("mail"));
                        recipeCreator.Client.FirstName = reader.GetString(reader.GetOrdinal("prenom"));
                        recipeCreator.Client.LastName = reader.GetString(reader.GetOrdinal("nom_client"));
                        recipeCreator.Client.Phone = reader.GetString(reader.GetOrdinal("telephone"));
                        recipeCreator.Recipes = new List<Recipe>();
                        recipeCreators.Add(recipeCreator);
                    }
                    recipeCreator.Recipes.Add(recipe);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            Database.Connection.Close();
            return recipeCreators;
        }

        /// <summary>
        /// Met à jour les infos du cdr notamment pour son solde cook.
        /// </summary>
        /// <param name="recipeCreator">cdr à update</param>
        /// <returns>retourne le cdr inchangé</returns>
        public RecipeCreator Update(RecipeCreator recipeCreator)
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "UPDATE cdr, client " +
                    "SET client.solde = @cooks " +
                    "WHERE cdr.identifiant_client = client.identifiant_client " +
                    "AND identifiant_cdr = @id";

                commande.Parameters.AddWithValue("@id", recipeCreator.Id);
                commande.Parameters.AddWithValue("@cooks", recipeCreator.Cooks);

                commande.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            Database.Connection.Close();
            return recipeCreator;
        }

        /// <summary>
        /// Supprime un cdr de la BDD.
        /// </summary>
        /// <param name="id">id du cdr à supprimer</param>
        /// <returns>retourne true si la délétion a fonctionné et falsee sinon</returns>
        public bool Delete(int id)
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "DELETE FROM cdr WHERE identifiant_cdr = @id;";
                commande.Parameters.AddWithValue("@id", id);

                int rowsAffected = commande.ExecuteNonQuery();

                
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Database.Connection.Close();
                return false;
            }

            Database.Connection.Close();
            return true;
        }
    }
}
