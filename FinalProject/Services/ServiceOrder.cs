using FinalProject.Database;
using FinalProject.Models;
using FinalProject.Session;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinalProject.Services
{
    class ServiceOrder
    {
        DaoOrder DaoOrder;

        public ServiceOrder()
        {
            DaoOrder = new DaoOrder();
        }

        /// <summary>
        /// Renvoie toutes les commandes d'un client dans une liste rangée par date
        /// </summary>
        /// <param name="client">client</param>
        /// <param name="limit">nombre limite de commande</param>
        /// <returns></returns>
        public List<Order> AllOf(Client client, int limit = 0)
        {
            return DaoOrder.ReadOf(client.Id, limit);
        }

        /// <summary>
        /// Enregistre la commande dans la DB.
        /// </summary>
        /// <param name="order">commande à sauvegarder</param>
        /// <returns></returns>
        public Order Save(Order order)
        {
            DateTime thisDay = DateTime.Today;            
            order.Date = thisDay.ToString("yyyy-MM-dd");

            DaoOrder.Create(order);
            //si la ref de la commande est différente de 0, cad que la commande a été ajoutée avec succès à la DB,
            //alors on va pouvoir linker les recettes et leur quantité respectives à la commande (dans table contenir)
            if (order.Ref != 0)
            {
                ServiceRecipe serviceRecipe = new ServiceRecipe();
                ServiceProduct serviceProduct = new ServiceProduct();

                AuthUser.Client.Cooks -= order.TotalCost;

                foreach (Recipe recipe in order.Recipes)
                {
                    //Pour chaque recette dans la commande, 
                    serviceRecipe.Save(recipe);

                    //Ici on va maj les stocks actuels des produits de la recette
                    foreach (Product product in recipe.Products)
                    {
                        serviceProduct.Save(product);
                    }
                }
                
                ServiceClient serviceClient = new ServiceClient();
                //On sauvegarde les infos updates du client (notamment son solde)
                serviceClient.Save(AuthUser.Client);
            }

            return order;
        }

        /// <summary>
        /// Cette méthode est la plus compliquée de tout le code.
        /// Lorsque l'on veut ajouter des recettes dans une commande, les stocks actuels des produits doivent être mis à jour 
        /// sans pour autant être modifiés dans la DB. Il faut donc enregistrer ces nouveaux stocks quelque part et recalculer
        /// la quantité max de recettes que l'on peut produire avec ces stocks. Lorsque la quantité max de la recette que l'on
        /// peut produire est nulle, cela signifie que l'on a plus assez de stocks de produit pour la produire. Il faut donc ne
        /// plus afficher cette recette dans la liste des recettes et c'est là qu'interviennent les ObservableCollection. Ce sont
        /// en fait des listes à la différence que lorsque leur données changent, l'affichage WPF change également.
        /// Une autre particularité est lorsque le client veut retirer une recette de sa commande, auquel cas il faudrait incrémenter
        /// les stocks et reafficher la recette dans les recettes disponibles faisables.
        /// </summary>
        /// <param name="recipes">listes des recettes faisables</param>
        /// <param name="recipeAdded">recette ajoutée</param>
        /// <param name="remove">true si on enlève la recette et false si on la rajoute</param>
        public void SynchronizeQuantiteFaisable(ObservableCollection<Recipe> recipes, Recipe recipeAdded, bool remove = false)
        {
            //On commence par instancier une liste des produits qu'il faudra update à la fin/validation de la commande
            List<Product> productsToUpdate = new List<Product>();
            //On parcourt chaque recette de la liste des recettes faisables
            foreach (Recipe recipe in recipes)
            {
                //Tant que la recette choisie n'est pas égale à une recette de la liste faisable, on ne passe pas à l'étape suivante 
                if (!recipe.Equals(recipeAdded)) continue;

                //On parcourt chaque produit de la recette
                foreach (Product product in recipe.Products)
                {
                    //Si le nombre de fois que la recette est présente dans la commande est supérieure stricte à 0,
                    //alors la variable indicatrice add prend la valeur 1
                    int add = recipe.QuantityInOrder > 0 ? 1 : 0;

                    //Si la liste des produits à update est vide ou si aucun des produits de la recette n'est compris dans cette liste
                    //alors on retire au stock du produit la quantité du produit présente dans la recette ou on l'ajoute dans le cas du remove
                    if (productsToUpdate.Count == 0 || !productsToUpdate.Any(prod => prod.Ref == product.Ref))
                    {                        
                        product.CurrentStock += (remove ? product.QuantityInRecipe : -product.QuantityInRecipe) * add;
                        //Le stock de ce produit a été modifié donc on l'ajoute à la liste
                        productsToUpdate.Add(product);
                    }
                    //Si ce produit a déjà été modifié auparavant, alors on doit modifier la valeur de son produit correspondant dans 
                    //la liste des produits modifiés
                    else
                    {
                        Product productTo = productsToUpdate.First(prod => prod.Ref == product.Ref);
                        productTo.CurrentStock += (remove ? product.QuantityInRecipe : -product.QuantityInRecipe) * add;
                    }
                }
            }

            //Ensuite, on va égaliser les stocks des produits des recettes avec les stocks des produits de la liste des produits modifiés
            foreach (Product productUpdated in productsToUpdate)
            {
                foreach (Recipe recipe in recipes)
                {
                    foreach (Product product in recipe.Products)
                    {
                        if (product.Ref == productUpdated.Ref)
                        {
                            product.CurrentStock = productUpdated.CurrentStock;
                        }
                    }
                }
            }

            //Ici, si la recette n'est plus faisable, alors on la rend invisible dans l'affichage WPF
            foreach (Recipe recipe in recipes)
            {
                recipe.Visibility = recipe.QuantiteFaisable > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
