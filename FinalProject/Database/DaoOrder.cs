using FinalProject.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Database
{
    class DaoOrder
    {
        private Database Database;

        public DaoOrder()
        {
            Database = new Database();
        }

        /// <summary>
        /// Enregistre la commande dans la base de données.
        /// </summary>
        /// <param name="order">informations de la commande</param>
        /// <returns></returns>
        public Order Create(Order order)
        {
            try
            {
                Database.Connection.Open();

                //On crée la commande sql et on insère ses attributs dans la database
                MySqlCommand command = Database.Connection.CreateCommand();
                //Dans un premier temps, on insère les attributs de la table commande
                command.CommandText = "INSERT INTO Cooking.commande (date, total, identifiant_client) " +
                    "VALUES (@date, @Total, @ID); " +
                    //Ensuite, comme la référence de la commande ne se crée qu'au moment où l'on insère la 
                    //commande dans la BDD, il faut récupérer cette ref. C'est pour cela qu'on récupère 
                    //toutes les références de commande, qu'on les classe dans l'ordre décroissant et qu'on
                    //ne garde que la première (LIMIT 1)
                    "SELECT ref_commande FROM commande ORDER BY ref_commande DESC LIMIT 1;";
                
                // Display the date in the format yyyy-MM-dd (10 char)
                command.Parameters.AddWithValue("@date", order.Date);
                command.Parameters.AddWithValue("@ID", order.Client.Id);
                command.Parameters.AddWithValue("@Total", order.TotalCost);

                //ExecuteScalar exécute la commande et ne récupère que la première colonne de la 
                //première ligne retournée par la commande.
                order.Ref = (int) command.ExecuteScalar();

                //Dans un second temps, il faut linker les recettes contenues dans la commande ainsi que leur quantité associée
                command.CommandText = "INSERT INTO contenir (ref_commande, ref_recette, nombre) VALUES ";

                foreach (Recipe recipe in order.Recipes)
                {
                    command.CommandText += $"({ order.Ref }, { recipe.Ref }, { recipe.QuantityInOrder })";
                    command.CommandText += recipe.Equals(order.Recipes.Last()) ? ";" : ", ";
                }

                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
            }

            Database.Connection.Close();
            return order;
        }

        /// <summary>
        /// Renvoie toutes les commandes d'un client dans une liste de commandes rangées par date
        /// </summary>
        /// <param name="id"></param>
        /// <param name="limit">nombre de commande que l'on veut afficher. Si ce paramètre n'est
        /// pas rempli au moment de l'appel de cette fonction, alors on ne met pas de limite (cf ternaire)</param>
        /// <returns>renvoie la liste des commandes du client id</returns>
        public List<Order> ReadOf(int id, int limit = 0)
        {
            List<Order> orders = new List<Order>();
            try
            {
                Database.Connection.Open();

                MySqlCommand command = Database.Connection.CreateCommand();
                command.CommandText = "SELECT * FROM commande, contenir, recette " +
                    "WHERE commande.ref_commande = contenir.ref_commande " +
                    "AND contenir.ref_recette = recette.ref_recette " +
                    "AND identifiant_client=" + id +
                    " ORDER BY date Desc";
                command.CommandText += limit > 0 ? $" LIMIT {limit};" : ";";

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Recipe recipe = new Recipe();
                    recipe.Ref = reader.GetInt32(reader.GetOrdinal("ref_recette"));
                    recipe.Name = reader.GetString(reader.GetOrdinal("nom"));
                    recipe.Price = reader.GetInt32(reader.GetOrdinal("prix"));
                    recipe.QuantityInOrder = reader.GetInt32(reader.GetOrdinal("nombre"));

                    int orderRef = reader.GetInt32(reader.GetOrdinal("ref_commande"));

                    Order order = null;

                    if (orders.Any(ord => ord.Ref == orderRef))
                    {
                        order = orders.First(ord => ord.Ref == orderRef);
                    }

                    if (order == null)
                    {
                        order = new Order();
                        order.Ref = reader.GetInt32(reader.GetOrdinal("ref_commande"));
                        order.Date = (reader.GetDateTime(reader.GetOrdinal("date"))).ToString("dd/MM/yyyy");
                        order.Recipes = new List<Recipe>();
                        orders.Add(order);
                    }

                    // Total Cost in order is calculate from recipes price * quantity in order, so we need to get list of recipes.
                    order.Recipes.Add(recipe);                    
                }

                reader.Close();
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine(mySqlException.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            Database.Connection.Close();
            return orders;
        }
    }
}
