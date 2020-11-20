using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinalProject.Models;
using MySql.Data.MySqlClient;

namespace FinalProject.Database
{
    class DaoProduct
    {
        private Database Database;

        public DaoProduct()
        {
            Database = new Database();
        }

        /// <summary>
        /// Crée un produit dans la BDD.
        /// </summary>
        /// <param name="product">informations du produit</param>
        /// <returns>retourne le produit crée (avec sa référence en plus)</returns>
        public Product Create(Product product)
        {            
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "INSERT INTO produit (nom, categorie, unite, stock_actuel, stock_min, stock_max, ref_fournisseur) " +
                    "VALUES (@name, @category, @Unite, @currentStock, @minStock, @maxStock, @providerRef); \n" +
                    "SELECT produit.ref_produit FROM produit ORDER BY produit.ref_produit DESC LIMIT 1;";

                commande.Parameters.AddWithValue("@name", product.Name);
                commande.Parameters.AddWithValue("@category", product.Category);
                commande.Parameters.AddWithValue("@Unite", product.Unity);
                commande.Parameters.AddWithValue("@currentStock", product.CurrentStock);
                commande.Parameters.AddWithValue("@minStock", product.MinStock);
                commande.Parameters.AddWithValue("@maxStock", product.MaxStock);
                commande.Parameters.AddWithValue("@providerRef", product.Provider.Ref);

                product.Ref = (int) commande.ExecuteScalar();
            }
            catch (Exception Exception)
            {
                Console.WriteLine(Exception.Message);
            }
            Database.Connection.Close();
            return product;
        }

        /// <summary>
        /// Récupère le produit d'une certaine référence.
        /// </summary>
        /// <param name="reference">référence du produit voulu</param>
        /// <returns></returns>
        public Product Read(int reference)
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "SELECT * FROM produit WHERE ref_produit = @Ref;";
                commande.Parameters.AddWithValue("@Ref", reference);

                MySqlDataReader reader = commande.ExecuteReader();
                reader.Read();

                Product product = new Product();
                product.Ref = reference;
                product.Name = reader.GetString(reader.GetOrdinal("nom"));
                product.Category = reader.GetString(reader.GetOrdinal("categorie"));
                product.Unity = reader.GetString(reader.GetOrdinal("unite"));
                product.CurrentStock = reader.GetInt32(reader.GetOrdinal("stock_actuel"));
                product.MinStock = reader.GetInt32(reader.GetOrdinal("stock_min"));
                product.MaxStock = reader.GetInt32(reader.GetOrdinal("stock_max"));

                Database.Connection.Close();
                return product;
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine(mySqlException.Message);
                return null;
            }
        }

        /// <summary>
        /// Récupère tous les produits de la DB.
        /// </summary>
        /// <returns>retourne une liste de la classe Produit contenant tous les produits</returns>
        public List<Product> ReadAll()
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "SELECT * FROM produit;";

                MySqlDataReader reader = commande.ExecuteReader();

                List<Product> products = new List<Product>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Product product = new Product();
                        product.Ref = reader.GetInt16(reader.GetOrdinal("ref_produit"));
                        product.Name = reader.GetString(reader.GetOrdinal("nom"));
                        product.Category = reader.GetString(reader.GetOrdinal("categorie"));
                        product.Unity = reader.GetString(reader.GetOrdinal("unite"));
                        product.CurrentStock = reader.GetInt32(reader.GetOrdinal("stock_actuel"));
                        product.MinStock = reader.GetInt32(reader.GetOrdinal("stock_min"));
                        product.MaxStock = reader.GetInt32(reader.GetOrdinal("stock_max"));
                        product.Provider = new Provider();
                        product.Provider.Ref = reader.GetInt32(reader.GetOrdinal("ref_fournisseur"));

                        products.Add(product);
                    }
                }

                Database.Connection.Close();
                return products;
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine(mySqlException.Message);
                return null;
            }
        }

        /// <summary>
        /// Récupère les produits contenus dans une recette ainsi que leur quantité associée.
        /// </summary>
        /// <param name="id">référence de la recette</param>
        /// <returns>retourne une liste de la classe Produit contenant tous les produits de la recette</returns>
        public List<Product> ReadAllOfRecipe(int id)
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand command = Database.Connection.CreateCommand();

                command.CommandText = "SELECT * FROM produit, necessiter " +
                    "WHERE produit.ref_produit = necessiter.ref_produit " +
                    "AND necessiter.ref_recette = @recipeId;";
                command.Parameters.AddWithValue("@recipeId", id);

                MySqlDataReader reader = command.ExecuteReader();

                List<Product> products = new List<Product>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Product product = new Product();
                        product.Ref = reader.GetInt16(reader.GetOrdinal("ref_produit"));
                        product.Name = reader.GetString(reader.GetOrdinal("nom"));
                        product.Category = reader.GetString(reader.GetOrdinal("categorie"));
                        product.Unity = reader.GetString(reader.GetOrdinal("unite"));
                        product.CurrentStock = reader.GetInt32(reader.GetOrdinal("stock_actuel"));
                        product.MinStock = reader.GetInt32(reader.GetOrdinal("stock_min"));
                        product.MaxStock = reader.GetInt32(reader.GetOrdinal("stock_max"));
                        product.QuantityInRecipe = reader.GetInt32(reader.GetOrdinal("quantite"));
                        product.Provider = new Provider();
                        product.Provider.Ref = reader.GetInt32(reader.GetOrdinal("ref_fournisseur"));

                        products.Add(product);
                    }
                }

                Database.Connection.Close();
                return products;
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine(mySqlException.Message);
                return null;
            }
        }

        /// <summary>
        /// Met à jour le produit, notamment ses stocks (min, actuel et max)
        /// </summary>
        /// <param name="product">produit à modifier</param>
        /// <returns>retourne le produit même si aucun changement n'a été fait sur ce dernier</returns>
        public Product Update(Product product)
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "UPDATE produit " +
                    "SET nom = @name, categorie = @category, stock_actuel = @currentStock, " +
                    "stock_min = @minStock, stock_max = @maxStock, ref_fournisseur = @providerRef " +
                    "WHERE ref_produit = @ref";

                commande.Parameters.AddWithValue("@ref", product.Ref);
                commande.Parameters.AddWithValue("@name", product.Name);
                commande.Parameters.AddWithValue("@category", product.Category);
                commande.Parameters.AddWithValue("@currentStock", product.CurrentStock);
                commande.Parameters.AddWithValue("@minStock", product.MinStock);
                commande.Parameters.AddWithValue("@maxStock", product.MaxStock);
                commande.Parameters.AddWithValue("@providerRef", product.Provider.Ref);

                int rowsAffected = commande.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            Database.Connection.Close();
            return product;
        }

        /// <summary>
        /// Récupère tous les produits de la DB qui n'ont pas été utilisés depuis les 30 derniers jours
        /// </summary>
        /// <returns>retourne la liste des produits non utilisés</returns>
        public List<Product> ReadAllUnused()
        {
            List<Product> lstProdNonUsed = new List<Product>();
            try
            {
                Database.Connection.Open();

                MySqlCommand command = Database.Connection.CreateCommand();
                command.CommandText = "SELECT * FROM produit " +
                    //Cette première requête récupère les produits qui ne sont pas dans une recette commandée il y a moins de 30 jours
                    "WHERE produit.ref_produit IN (" +
                        "SELECT P.ref_produit " +
                        "FROM produit P, recette R, commande C, contenir Ct, necessiter N " +
                        "WHERE C.ref_commande= Ct.ref_commande " +
                        "AND Ct.Ref_recette= R.Ref_recette " +
                        "AND R.Ref_recette= N.Ref_recette " +
                        "AND N.Ref_produit= P.Ref_produit " +
                        "AND (date_format(C.date,'%Y-%m-%d'))< @Date " +
                        "GROUP BY P.ref_produit ) " +
                    //Cette deuxième requête sélectionne les produits qui ne sont pas dans une recette
                    "OR produit.ref_produit IN (" +
                        "SELECT produit.ref_produit " +
                        "FROM produit, necessiter " +
                        "WHERE produit.ref_produit != necessiter.ref_produit ) " +
                    //cette troisième requête récupère les produits dont les recettes n'ont jamais été commandées
                    "OR produit.ref_produit IN (" +
                        "SELECT produit.ref_produit " +
                        "FROM produit, necessiter, recette, contenir " +
                        "WHERE produit.ref_produit = necessiter.ref_produit " +
                        "AND necessiter.ref_recette = recette.ref_recette " +
                        "AND recette.ref_recette != contenir.ref_recette )";

                TimeSpan decalage = new TimeSpan(30, 0, 0, 0);
                DateTime DayLastWeek = DateTime.Today - decalage;
                command.Parameters.AddWithValue("@Date", DayLastWeek.ToString("yyyy-MM-dd"));
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product();
                    product.Ref = reader.GetInt32(reader.GetOrdinal("ref_produit"));
                    product.Name = reader.GetString(reader.GetOrdinal("nom"));
                    product.Unity = reader.GetString(reader.GetOrdinal("unite"));
                    product.CurrentStock = reader.GetInt32(reader.GetOrdinal("stock_actuel"));
                    product.MaxStock = reader.GetInt32(reader.GetOrdinal("stock_max"));
                    product.MinStock = reader.GetInt32(reader.GetOrdinal("stock_min"));
                    product.Category = reader.GetString(reader.GetOrdinal("categorie"));

                    product.Provider = new Provider();
                    product.Provider.Ref = reader.GetInt32(reader.GetOrdinal("ref_fournisseur"));
                    lstProdNonUsed.Add(product);
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
            }

            Database.Connection.Close();
            return lstProdNonUsed;
        }

        /// <summary>
        /// Supprimer un produit de la DB.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true si la délétion a fonctionné et false sinon</returns>
        public bool Delete(int id)
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "DELETE FROM produit WHERE ref_produit = @ref";
                commande.Parameters.AddWithValue("@ref", id);

                int rowsAffected = commande.ExecuteNonQuery();

                Database.Connection.Close();
                return rowsAffected > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
