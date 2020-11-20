using FinalProject.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Database
{
    class DaoProvider
    {
        private Database Database;

        public DaoProvider()
        {
            Database = new Database();
        }

        /// <summary>
        /// Récupère tous les fournisseurs de la DB.
        /// </summary>
        /// <returns>retourne la liste de ces fournisseurs</returns>
        public List<Provider> ReadAll()
        {
            List<Provider> providers = new List<Provider>();
            try
            {
                Database.Connection.Open();

                MySqlCommand command = Database.Connection.CreateCommand();
                command.CommandText = "SELECT * FROM fournisseur;";

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Provider provider = new Provider();
                    provider.Ref = reader.GetInt32(reader.GetOrdinal("ref_fournisseur"));
                    provider.Name = reader.GetString(reader.GetOrdinal("nom"));
                    provider.Phone = reader.GetString(reader.GetOrdinal("tel"));
                    providers.Add(provider);
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
            }

            Database.Connection.Close();
            return providers;
        }

        /// <summary>
        /// Récupère tous les fournisseurs qui fournissent des produits dont le stock actuel
        /// est inférieur à leur stock minimal
        /// </summary>
        /// <returns>retourne une liste de fournisseurs</returns>
        public List<Provider> ReadAllWithProductToRestock()
        {
            List<Provider> providers = new List<Provider>();
            try
            {
                Database.Connection.Open();

                MySqlCommand command = Database.Connection.CreateCommand();
                command.CommandText = "SELECT *, P.nom as nom_produit FROM fournisseur F, produit P " +
                    "WHERE P.ref_fournisseur = F.ref_fournisseur " +
                    "AND P.stock_actuel < P.stock_min;";

                MySqlDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    Product product = new Product();
                    product.Ref = reader.GetInt32(reader.GetOrdinal("ref_produit"));
                    product.Name = reader.GetString(reader.GetOrdinal("nom_produit"));
                    product.Category = reader.GetString(reader.GetOrdinal("categorie"));
                    product.CurrentStock = reader.GetInt32(reader.GetOrdinal("stock_actuel"));
                    product.MinStock = reader.GetInt32(reader.GetOrdinal("stock_min"));
                    product.MaxStock = reader.GetInt32(reader.GetOrdinal("stock_max"));

                    int providerRef = reader.GetInt32(reader.GetOrdinal("ref_fournisseur"));

                    Provider provider = null;

                    if (providers.Any(pro => pro.Ref == providerRef))
                    {
                        provider = providers.First(pro => pro.Ref == providerRef);
                    }   
                    
                    if (provider == null)
                    {
                        provider = new Provider();
                        provider.Ref = reader.GetInt32(reader.GetOrdinal("ref_fournisseur"));
                        provider.Name = reader.GetString(reader.GetOrdinal("nom"));
                        provider.Phone = reader.GetString(reader.GetOrdinal("tel"));
                        provider.Products = new List<Product>();
                        providers.Add(provider);
                    }

                    provider.Products.Add(product);
                }
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine(mySqlException.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            

            Database.Connection.Close();
            return providers;
        }
    }
}
