using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class RecipeCreator
    {
        public int Id { get; set; }

        public int Cooks
        {
            get
            {
                return Client.Cooks;
            }

            set
            {
                Client.Cooks = value;
            }
        }

        public Client Client { get; set; }

        public List<Recipe> Recipes { get; set; }//Liste des recettes créées par le cdr

        public int RecipesSold { get; set; } //Nombre de recettes vendues

        public int CountRecipes //Nombre de recettes créées
        {
            get
            {
                if (Recipes != null)
                {
                    return Recipes.Count();
                }
                else
                {
                    return 0;
                }
                
            }
        }

        public int CountOrdered
        {
            get
            {
                int ordered = 0;
                if (Recipes != null)
                {
                    foreach(Recipe recipe in Recipes)
                    {
                        ordered += recipe.TimesOrdered;
                    }
                }
                return ordered;
            }
        }
    }
}
