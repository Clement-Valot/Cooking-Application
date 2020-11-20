using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinalProject.Database;
using FinalProject.Models;

namespace FinalProject.Services
{
    class ServiceRecipeCreator
    {
        private DaoRecipeCreator DaoRecipeCreator;

        private ServiceRecipe ServiceRecipe;

        public ServiceRecipeCreator()
        {
            DaoRecipeCreator = new DaoRecipeCreator();

            ServiceRecipe = new ServiceRecipe();
        }

        /// <summary>
        /// Récupère de la DB le cdr d'dientifiant id rentré en paramètres
        /// </summary>
        /// <param name="id">identifiant cdr du cdr que l'on veut récupérer</param>
        /// <returns></returns>
        public RecipeCreator One(int id)
        {
            return DaoRecipeCreator.Read(id);
        }

        /// <summary>
        /// Récupère de la DB le cdr de la semaine aka celui qui a le plus de ses recettes commandées cumulées sur les 7 derniers jours
        /// </summary>
        /// <returns></returns>
        public RecipeCreator BestOfTheWeek()
        {
            return DaoRecipeCreator.ReadOfTheWeek();
        }

        /// <summary>
        /// Récupère de la DB le cdr d'or aka celui qui a le plus de ses recettes commandées cumulées all time
        /// </summary>
        /// <returns></returns>
        public RecipeCreator BestOfAllTime()
        {
            return DaoRecipeCreator.ReadOfAllTime();
        }

        /// <summary>
        /// Récupère la liste de tous les cdr de la DB.
        /// </summary>
        /// <returns>liste d'objet de classe Cdr de tous les cdr de la DB</returns>
        public List<RecipeCreator> All()
        {
            return DaoRecipeCreator.ReadAll();
        }

        /// <summary>
        /// Récupère la liste de tous les cdr puis associe à chaque cdr sa liste de recettes créées.
        /// </summary>
        /// <returns>liste de tous les cdr avec recettes</returns>
        public List<RecipeCreator> AllWithRecipes()
        {
            List<RecipeCreator> recipeCreators = DaoRecipeCreator.ReadAll();

            foreach (RecipeCreator recipeCreator in recipeCreators)
            {
                recipeCreator.Recipes = ServiceRecipe.AllOf(recipeCreator);
            }

            return recipeCreators;
        }

        /// <summary>
        /// Si le cdr n'existe pas déjà dans la DB, le crée.
        /// Sinon, update ses informations.
        /// </summary>
        /// <param name="recipeCreator">cdr à créer ou modifier</param>
        /// <returns>le cdr crée ou modifié</returns>
        public RecipeCreator Save(RecipeCreator recipeCreator)
        {
            if (recipeCreator.Id == 0)
            {
                return DaoRecipeCreator.Create(recipeCreator);
            }
            else
            {
                return DaoRecipeCreator.Update(recipeCreator);
            }
        }

        /// <summary>
        /// Crée le cdr dans la DB en fonction du client rentré en paramètre
        /// </summary>
        /// <param name="client">client qu'on veut faire devenir cdr</param>
        /// <returns>le cdr lié au client</returns>
        public RecipeCreator Save(Client client)
        {
            RecipeCreator recipeCreator = new RecipeCreator();
            recipeCreator.Client = client;
            return DaoRecipeCreator.Create(recipeCreator);
        }

        /// <summary>
        /// Supprimer le cdr de la DB, et toutes ses recettes dans le même temps.
        /// </summary>
        /// <param name="recipeCreator"></param>
        /// <returns></returns>
        public bool Remove(RecipeCreator recipeCreator)
        {
            return DaoRecipeCreator.Delete(recipeCreator.Id);
        }
    }
}
