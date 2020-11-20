using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Order
    {
        public int Ref { get; set; }

        public string Date { get; set; } //date à laquelle la commande a été effectuée (créée au moment de l'ajout de la commande en BDD)

        public Client Client { get; set; } //Le client à qui est affilié cette commande

        public List<Recipe> Recipes { get; set; } //La liste des recettes composants la commande

        public int TotalCost // Le coût total de la commande. Pour la calculer, on parcourt chaque recette de la commande
                            //et on incrémente le total par le prix de la recette par la quantité de cette recette dans la commande
        {
            get
            {
                if (Recipes == null) return 0;

                int total = 0;
                foreach (Recipe recipe in Recipes)
                {
                    total += recipe.Price * recipe.QuantityInOrder;
                }
                return total;
            }
        }
    }
}
