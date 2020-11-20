using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinalProject.Models;
using MySql.Data.MySqlClient;

namespace FinalProject.Database
{
    class DaoRecipe
    {
        private Database Database;

        public DaoRecipe()
        {
            Database = new Database();
        }

        /// <summary>
        /// Enregistre une recette dans la DB.
        /// </summary>
        /// <param name="recipe">recette à enregistrer</param>
        /// <returns>retourne la recette avec sa référence associée</returns>
        public Recipe Create(Recipe recipe)
        {
            try
            {
                Database.Connection.Open();

                //On insère les attributs de la recette dans la database
                MySqlCommand command = Database.Connection.CreateCommand();

                command.CommandText = "INSERT INTO recette (nom, type, descriptif , prix, identifiant_cdr) " +
                "VALUES (@name, @type, @description, @price, @recipeCreatorId); " +
                "SELECT ref_recette FROM recette ORDER BY ref_recette DESC LIMIT 1;";

                command.Parameters.AddWithValue("@name", recipe.Name);
                command.Parameters.AddWithValue("@type", recipe.Type);
                command.Parameters.AddWithValue("@description", recipe.Description);
                command.Parameters.AddWithValue("@price", recipe.Price);
                command.Parameters.AddWithValue("@recipeCreatorId", recipe.RecipeCreator.Id);

                recipe.Ref = (int) command.ExecuteScalar();

                foreach (Product product in recipe.Products)
                {                    
                    if (product.Ref != 0)
                    {
                        command.CommandText = "INSERT INTO necessiter (ref_recette, ref_produit, quantite) VALUES " +
                            $"({ recipe.Ref }, { product.Ref }, { product.QuantityInRecipe });";
                        command.ExecuteNonQuery();
                    }
                }                                
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
            }

            Database.Connection.Close();
            return recipe;
        }

        /// <summary>
        /// Récupère de la DB les informations de la recette avec la référence entrée en paramètre.
        /// </summary>
        /// <param name="reférence">référence de la recette à récupérer de la DB</param>
        /// <returns>recette récupérée</returns>
        public Recipe Read(int reférence)
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "SELECT * FROM recette WHERE ref_recette = @Ref;";
                commande.Parameters.AddWithValue("@Ref", reférence);

                MySqlDataReader reader = commande.ExecuteReader();
                reader.Read();

                Recipe recipe = new Recipe();
                recipe.Ref = reférence;
                recipe.Name = reader.GetString(reader.GetOrdinal("nom"));
                recipe.Type = reader.GetString(reader.GetOrdinal("type"));
                recipe.Description = reader.GetString(reader.GetOrdinal("descriptif"));
                recipe.Price = reader.GetInt32(reader.GetOrdinal("prix"));
                recipe.TimesOrdered = reader.GetInt32(reader.GetOrdinal("nbr_commande"));

                recipe.RecipeCreator = new RecipeCreator();
                recipe.RecipeCreator.Id = reader.GetInt32(reader.GetOrdinal("identfiant_cdr"));

                Database.Connection.Close();
                return recipe;
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine(mySqlException.Message);
                return null;
            }
        }

        /// <summary>
        /// Récupère les recettes créées par un certain créateur de recette et les range dans une liste par ordre décroissant de
        /// nombre de commandes.
        /// </summary>
        /// <param name="id">identifiant du cdr dont on cherche les recettes</param>
        /// <param name="limit">limite du nombre de recettes que l'on veut récupérer (toutes par défaut)</param>
        /// <returns></returns>
        public List<Recipe> ReadOf(int id, int limit = 0)
        {
            List<Recipe> recipes = new List<Recipe>();
            try
            {
                Database.Connection.Open();

                MySqlCommand command = Database.Connection.CreateCommand();
                command.CommandText = "SELECT * FROM recette WHERE identifiant_cdr=" + id +
                    " ORDER BY nbr_commande";
                command.CommandText += limit > 0 ? $" LIMIT {limit};" : ";";

                MySqlDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    Recipe recipe = new Recipe();
                    recipe.Ref = reader.GetInt32(reader.GetOrdinal("ref_recette")); ;
                    recipe.Name = reader.GetString(reader.GetOrdinal("nom"));
                    recipe.Type = reader.GetString(reader.GetOrdinal("type"));
                    recipe.Price = reader.GetInt32(reader.GetOrdinal("prix"));
                    recipe.Description = reader.GetString(reader.GetOrdinal("descriptif"));
                    recipe.TimesOrdered = reader.GetInt32(reader.GetOrdinal("nbr_commande"));
                    recipes.Add(recipe);
                }

                reader.Close();                
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine(mySqlException.Message);
            }

            Database.Connection.Close();
            return recipes;
        }

        /// <summary>
        /// Récupère les meilleurs recettes de la DB.
        /// </summary>
        /// <param name="limit">limite du nombre de recettes que l'on veut retourner (toutes par défaut)</param>
        /// <returns></returns>
        public List<Recipe> ReadTop(int limit = 0)
        {
            List<Recipe> recipes = new List<Recipe>();
            try
            {
                Database.Connection.Open();

                MySqlCommand command = Database.Connection.CreateCommand();
                //On récupère toutes les recettes, classées par le nombre de fois qu'elles ont été commandées, 
                //de la plus commandée à la moins commandée
                command.CommandText = "SELECT * FROM recette R, client Cl, cdr C WHERE " +
                    "Cl.identifiant_client = C.identifiant_client " +
                    "AND C.identifiant_cdr = R.identifiant_cdr " +
                    "ORDER BY nbr_commande DESC";
                command.CommandText += limit > 0 ? $" LIMIT {limit};" : ";";

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Recipe recipe = new Recipe();
                    recipe.Ref = reader.GetInt32(reader.GetOrdinal("ref_recette"));
                    recipe.Name = reader.GetString(1);
                    recipe.Type = reader.GetString(reader.GetOrdinal("type"));
                    recipe.Price = reader.GetInt32(reader.GetOrdinal("prix"));
                    recipe.Description = reader.GetString(reader.GetOrdinal("descriptif"));
                    recipe.TimesOrdered = reader.GetInt32(reader.GetOrdinal("nbr_commande"));

                    recipe.RecipeCreator = new RecipeCreator();
                    recipe.RecipeCreator.Id = reader.GetInt32(reader.GetOrdinal("identifiant_cdr"));

                    recipe.RecipeCreator.Client = new Client();
                    recipe.RecipeCreator.Client.Id = reader.GetInt32(reader.GetOrdinal("identifiant_client"));
                    recipe.RecipeCreator.Client.LastName = reader.GetString(11);
                    recipe.RecipeCreator.Client.FirstName = reader.GetString(reader.GetOrdinal("prenom"));
                    recipe.RecipeCreator.Client.Mail = reader.GetString(reader.GetOrdinal("mail"));
                    recipe.RecipeCreator.Client.Phone = reader.GetString(reader.GetOrdinal("telephone"));

                    recipes.Add(recipe);
                }
                reader.Close();
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine(mySqlException.Message);
            }

            Database.Connection.Close();
            return recipes;
        }

        /// <summary>
        /// Récupère toutes les recettes de la DB avec les infos sur la recette et le cdr.
        /// </summary>
        /// <returns>retourne une liste des recettes</returns>
        public List<Recipe> ReadAll()
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "SELECT *, client.nom as nom_client FROM recette, cdr, client " +
                    "WHERE recette.identifiant_cdr = cdr.identifiant_cdr " +
                    "AND cdr.identifiant_client = client.identifiant_client;";

                MySqlDataReader reader = commande.ExecuteReader();

                List<Recipe> recipes = new List<Recipe>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Recipe recipe = new Recipe();
                        recipe.Ref = reader.GetInt32(reader.GetOrdinal("ref_recette"));
                        recipe.Name = reader.GetString(reader.GetOrdinal("nom"));
                        recipe.Type = reader.GetString(reader.GetOrdinal("type"));
                        recipe.Description = reader.GetString(reader.GetOrdinal("descriptif"));
                        recipe.Price = reader.GetInt32(reader.GetOrdinal("prix"));
                        recipe.Remuneration = reader.GetInt32(reader.GetOrdinal("remuneration"));
                        recipe.TimesOrdered = reader.GetInt32(reader.GetOrdinal("nbr_commande"));

                        recipe.RecipeCreator = new RecipeCreator();
                        recipe.RecipeCreator.Id = reader.GetInt32(reader.GetOrdinal("identifiant_cdr"));

                        recipe.RecipeCreator.Client = new Client();
                        recipe.RecipeCreator.Client.Id = reader.GetInt32(reader.GetOrdinal("identifiant_client"));
                        recipe.RecipeCreator.Client.LastName = reader.GetString(reader.GetOrdinal("nom_client"));
                        recipe.RecipeCreator.Client.FirstName = reader.GetString(reader.GetOrdinal("prenom"));

                        recipes.Add(recipe);
                    }
                }
                Database.Connection.Close();
                return recipes;
            }
            catch (MySqlException mySqlException)
            {
                Database.Connection.Close();
                Console.WriteLine(mySqlException.Message);
                return null;
            }
        }

        /// <summary>
        /// Récupère toutes les recettes qui ont un produit spécifique (référencé en paramètre) dans leur composition
        /// </summary>
        /// <param name="référence_produit">référence du produit dont on cherche les recettes qui l'utilisent</param>
        /// <returns>retourne une liste des recettes qui utilisent ce produit</returns>
        public List<Recipe> ReadByRecipeProductRef(int référence_produit)
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "SELECT * FROM recette, necessiter WHERE recette.ref_recette = necessiter.ref_recette AND necessiter.ref_produit = @productRef;";
                commande.Parameters.AddWithValue("@productRef", référence_produit);

                MySqlDataReader reader = commande.ExecuteReader();

                List<Recipe> recipes = new List<Recipe>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Recipe recipe = new Recipe();
                        recipe.Ref = reader.GetInt16(reader.GetOrdinal("ref_recette"));
                        recipe.Name = reader.GetString(reader.GetOrdinal("nom"));
                        recipe.Type = reader.GetString(reader.GetOrdinal("type"));
                        recipe.Description = reader.GetString(reader.GetOrdinal("descriptif"));
                        recipe.Price = reader.GetInt32(reader.GetOrdinal("prix"));
                        recipe.Price = reader.GetInt32(reader.GetOrdinal("remuneration"));
                        recipe.TimesOrdered = reader.GetInt32(reader.GetOrdinal("nbr_commande"));
                        recipe.QuantityOfProductRequired = reader.GetInt32(reader.GetOrdinal("quantite"));

                        recipes.Add(recipe);
                    }
                }

                Database.Connection.Close();
                return recipes;
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine(mySqlException.Message);
                return null;
            }
        }

        /// <summary>
        /// Met à jour les infos d'une recette notamment pour le prix, la rémunération au cdr et
        /// le nombre de fois que cette recette a été commandée.
        /// Cette méthode sert également à ajouter les produits à une recette et leur quantité associée.
        /// </summary>
        /// <param name="recipe">recette à update</param>
        /// <returns>retourne la recette updaté</returns>
        public Recipe Update(Recipe recipe)
        {
            try
            {
                Database.Connection.Open();

                //On insère les attributs de la recette dans la database
                MySqlCommand command = Database.Connection.CreateCommand();

                command.CommandText = "UPDATE recette " +
                "SET nom = @name, type = @type, descriptif = @description, prix = @price, remuneration = @remuneration, nbr_commande = @timesOrdered, identifiant_cdr = @recipeCreatorId " +
                "WHERE ref_recette = @recipeRef;";

                command.Parameters.AddWithValue("@recipeRef", recipe.Ref);
                command.Parameters.AddWithValue("@name", recipe.Name);
                command.Parameters.AddWithValue("@type", recipe.Type);
                command.Parameters.AddWithValue("@description", recipe.Description);
                command.Parameters.AddWithValue("@price", recipe.Price);
                command.Parameters.AddWithValue("@remuneration", recipe.Remuneration);
                command.Parameters.AddWithValue("@timesOrdered", recipe.TimesOrdered);
                command.Parameters.AddWithValue("@recipeCreatorId", recipe.RecipeCreator.Id);

                int affectedRows = command.ExecuteNonQuery();                

                foreach (Product product in recipe.Products)
                {
                    command.CommandText = $"UPDATE necessiter " +
                        $"SET quantite = {product.QuantityInRecipe} " +
                        $"WHERE ref_recette = {recipe.Ref} AND ref_produit = {product.Ref};";
                    int AffectedRows = command.ExecuteNonQuery();
                    //Si la requête précédente renvoie 0 ligne affectée, cela signifie que la recette n'existe pas encore:
                    //On est dans le cas d'une création de recette et on doit donc insérer les références produits et leur quantité
                    //dans la table nécessiter.
                    if (AffectedRows == 0)
                    {
                        command.CommandText = "INSERT INTO necessiter (ref_recette, ref_produit, quantite) VALUES " +
                        $"({recipe.Ref}, {product.Ref}, {product.QuantityInRecipe});";
                        command.ExecuteNonQuery();
                    }
                }                
            }
            catch (Exception Exception)
            {
                Console.Write(Exception.Message);
            }

            Database.Connection.Close();
            return recipe;
        }

        /// <summary>
        /// Supprime une recette de la DB.
        /// La suppression d'une recette provoque aussi la maj de toutes les commandes la composant.
        /// </summary>
        /// <param name="Référence_recette"></param>
        /// <returns></returns>
        public bool Delete(int Référence_recette)
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "DELETE FROM recette WHERE ref_recette = @Ref;";
                commande.Parameters.AddWithValue("@Ref", Référence_recette);

                commande.ExecuteNonQuery();                
            }
            catch (MySqlException mySqlException)
            {
                Database.Connection.Close();
                Console.WriteLine(mySqlException.Message);
                return false;
            }

            Database.Connection.Close();
            return true;
        }

        /// <summary>
        /// Supprime les recettes créées par un certain cdr même si la BDD MySql fait cela 
        /// automatiquement dès la suppression du cdr (ON DELETE CASCADE)
        /// </summary>
        /// <param name="id">identifiant du cdr</param>
        /// <returns>retourne true si la suppression a marché et false sinon</returns>
        public bool DeleteByRecipeCreatorId(int id)
        {
            try
            {
                Database.Connection.Open();

                MySqlCommand commande = Database.Connection.CreateCommand();

                commande.CommandText = "DELETE FROM recette WHERE identifiant_cdr = @Id";
                commande.Parameters.AddWithValue("@Id", id);

                int rowsAffected = commande.ExecuteNonQuery();

                Database.Connection.Close();
                return rowsAffected > 0;
            }
            catch (MySqlException mySqlException)
            {
                Database.Connection.Close();
                Console.WriteLine(mySqlException.Message);
                return false;
            }
        }
    }
}
