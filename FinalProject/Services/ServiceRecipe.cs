using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinalProject.Database;
using FinalProject.Models;
using FinalProject.Session;

namespace FinalProject.Services
{
    class ServiceRecipe
    {
        private DaoRecipe DaoRecipe;

        public ServiceRecipe()
        {
            DaoRecipe = new DaoRecipe();
        }

        /// <summary>
        /// Récupère la recette de la DB à partir de sa référence.
        /// </summary>
        /// <param name="reference">référence de la recette</param>
        /// <returns>la recette</returns>
        public Recipe One(int reference)
        {
            return DaoRecipe.Read(reference);
        }

        /// <summary>
        /// Récupère la liste de toutes les recettes avec les quantités de produit associées à chaque recette.
        /// Si le paramètre all est initié à false, alors naturellement on ne va pas récupérer toutes les recettes
        /// mais garder uniquement celles dont la quantité faisable (à partir des stocks de leur produit) est 
        /// strictement supérieure à 0.
        /// </summary>
        /// <param name="all">True si on veut récupérer toutes les recettes et false si on veut simplement les recettes faisables</param>
        /// <returns>La liste des recettes</returns>
        public List<Recipe> All(bool all = false)
        {
            ServiceProduct serviceProduct = new ServiceProduct();
            //On récupère d'abord toutes les recettes de la DB
            List<Recipe> recipes = DaoRecipe.ReadAll();
            //Puis pour chaque recette, on récupère sa liste des produits et la quantité associée.
            foreach (Recipe recipe in recipes)
            {
                recipe.Products = serviceProduct.Of(recipe);
            }
            if (!all)
            {
                recipes = recipes.Where(recipi => recipi.QuantiteFaisable > 0).ToList();
            }            
            return recipes;
        }

        /// <summary>
        /// Récupère toutes les recettes qui ont un produit spécifique (référencé en paramètre) dans leurs ingrédients.
        /// </summary>
        /// <param name="product">produit dont on cherche les recettes qui l'utilisent</param>
        /// <returns></returns>
        public List<Recipe> AllOf(Product product)
        {
            return DaoRecipe.ReadByRecipeProductRef(product.Ref);
        }

        /// <summary>
        /// Surcharge de la méthode précédente.
        /// Récupère les recettes créées par un certain créateur de recette spécifié en paramètre
        /// </summary>
        /// <param name="recipeCreator">créateur de recette dont on veut récupérer les recettes créées</param>
        /// <param name="limit">limite du nombre de recettes que l'on veut récupérer (toutes par défaut)</param>
        /// <returns></returns>
        public List<Recipe> AllOf(RecipeCreator recipeCreator, int limit = 0)
        {
            return DaoRecipe.ReadOf(recipeCreator.Id, limit);
        }

        /// <summary>
        /// Récupère les meilleurs recettes de la DB.
        /// </summary>
        /// <param name="limit">limite du nombre de recettes que l'on veut retourner (toutes par défaut)</param>
        /// <returns></returns>
        public List<Recipe> Top(int limit = 0)
        {
            return DaoRecipe.ReadTop(limit);
        }

        /// <summary>
        /// Met à jour les recettes (rémunération, nbr commande, prix) ou les crée totalement, 
        /// les produits (stocks min et max) ou les crée totalement,
        /// et les cdr (solde)
        /// </summary>
        /// <param name="recipe">recette qu'on veut mettre à jour ou créer (contenant une liste de produits que l'on veut update ou ajouter à la DB)</param>
        /// <returns>true si on est dans le cas d'une création de recette et false si on est dans le cas d'une création de commande</returns>
        public bool Save(Recipe recipe)
        {
            ServiceProduct serviceProduct = new ServiceProduct();            
            //Si la recette n'existe pas dans la DB, on la crée.
            //Ce cas n'arrive pas lors d'une commande mais uniquement lors de la création d'une recette
            if (recipe.Ref == 0)
            {
                ServiceProvider serviceProvider = new ServiceProvider();
                //On enregistre la recette dans la DB (sans ses produits et quantités associées)
                recipe = DaoRecipe.Create(recipe);

                foreach(Product product in recipe.Products)
                {
                    //On parcourt chaque produit de la recette
                    //Si la référence du produit est nulle, cela signifie que le produit n'est pas encore enregistré dans la DB
                    //car c'est le cdr qui a crée ce produit lors de sa création de recette. Normalement, le cas de la référence 
                    //produit nulle n'arrive pas dans le cas de l'enregistrement d'une commande
                    if (product.Ref == 0)
                    {
                        //Pour le nouveau produit, on fixe sa quantité actuelle à 4 fois la quantité requise pour produire
                        //la recette pour laquelle il a été crée
                        product.CurrentStock = 4 * product.QuantityInRecipe;
                        //Inutile d'initialiser les stocks min et max de ce nouveau produit car ils sont définis après.

                        //On récupère tous les fournisseurs de la base de données et on en assigne un aléatoirement à ce nouveau produit
                        List<Provider> providers = serviceProvider.All();
                        Random rnd = new Random();
                        product.Provider = providers[rnd.Next(0, providers.Count - 1)];
                    }

                    product.MinStock = product.MinStock + 2 * product.QuantityInRecipe;
                    product.MaxStock = product.MaxStock + 5 * product.QuantityInRecipe;
                    //On enregistre le produit dans la DB
                    serviceProduct.Save(product);
                }
                //Une fois qu'on a fait ca pour chaque produit de la nouvelle recette, on update cette même recette
                //pour linker sa liste des produits et leur quantité dans la table necessiter
                return DaoRecipe.Update(recipe) != null;
            }
            //Si la recette existe déjà (cas de la création de commande)
            else
            {
                //Dans un premier temps, on incrémente le nombre de fois que la recette a été commandée
                //et on modifie les valeurs du prix de la recette et de la rémunération du cdr en fonction
                int oldTimesOrdered = recipe.TimesOrdered;
                recipe.TimesOrdered += recipe.QuantityInOrder;

                if (oldTimesOrdered < 10 && recipe.TimesOrdered >= 10)
                {
                    recipe.Price += 2;
                }
                else if (oldTimesOrdered < 50 && recipe.TimesOrdered >= 50)
                {
                    recipe.Price += 5;
                    recipe.Remuneration += 4;
                }
                //On update la recette pour changer sa rémunération, son nombre de commande et son prix
                DaoRecipe.Update(recipe);

                ServiceRecipeCreator serviceRecipeCreator = new ServiceRecipeCreator();

                //On récupère le cdr de la recette (ses informations, notamment son solde)
                recipe.RecipeCreator = serviceRecipeCreator.One(recipe.RecipeCreator.Id);

                //Si le créateur de la recette est le même que celui qui commande, alors on incrémente le solde de l'utilisateur et non seulement
                //le solde du cdr. La différence réside uniquement dans l'affichage du solde cook de l'utilisateur après sa commande.
                if (AuthUser.RecipeCreator != null && AuthUser.RecipeCreator.Id == recipe.RecipeCreator.Id)
                {
                    
                    //On incrémente son solde cook par la rémunération de la recette multiplié par la quantité de cette recette dans la commande
                    AuthUser.Client.Cooks += recipe.Remuneration * recipe.QuantityInOrder;
                    
                }
                else
                {
                    recipe.RecipeCreator.Cooks += recipe.Remuneration * recipe.QuantityInOrder;
                    serviceRecipeCreator.Save(recipe.RecipeCreator);
                }
                return false;
            }
        }

        /// <summary>
        /// Supprime une recette de la DB.
        /// </summary>
        /// <param name="recipe">recette à supprimer</param>
        /// <returns></returns>
        public bool Remove(Recipe recipe)
        {
            return DaoRecipe.Delete(recipe.Ref);
        }

        /// <summary>
        /// Supprime les recettes créées par un certain cdr même si la BDD MySql fait cela 
        /// automatiquement dès la suppression du cdr (ON DELETE CASCADE)
        /// </summary>
        /// <param name="recipeCreator"> cdr dont on veut supprimer les recettes </param>
        /// <returns></returns>
        public bool RemoveOf(RecipeCreator recipeCreator)
        {
            return DaoRecipe.DeleteByRecipeCreatorId(recipeCreator.Id);
        }
    }
}
