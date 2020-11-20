using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinalProject.Database;
using FinalProject.Models;

namespace FinalProject.Services
{
    class ServiceProduct
    {
        private DaoProduct DaoProduct;

        public ServiceProduct()
        {
            DaoProduct = new DaoProduct();
        }

        /// <summary>
        /// Récupère toutes les infos d'un produit à partir de sa référence.
        /// </summary>
        /// <param name="reference">référence du produit dont on veut récupérer les info</param>
        /// <returns>le produit avec ses infos</returns>
        public Product One(int reference)
        {
            return DaoProduct.Read(reference);
        }

        /// <summary>
        /// Récupère tous les produits de la DB dans une liste.
        /// </summary>
        /// <returns>la liste de tous les produits de la DB (stocks, ref, nom, fournisseur, catégorie, mais pas les quantités dans rct)</returns>
        public List<Product> All()
        {
            return DaoProduct.ReadAll();
        }

        /// <summary>
        /// Récupère la liste des produits contenus dans une recette entrée en paramètre.
        /// </summary>
        /// <param name="recipe">recette dont on veut récupérer les ingrédients</param>
        /// <returns>la liste des produits composant la recette avec les quantités associées pour chaque produit</returns>
        public List<Product> Of(Recipe recipe)
        {
            return DaoProduct.ReadAllOfRecipe(recipe.Ref);
        }

        /// <summary>
        /// Retourne la liste des produits ayant un stock actuel inférieur à 2 fois leur stock min.
        /// Fonction pour l'affichage démo.
        /// </summary>
        /// <returns> liste des produits ayant un stock actuel inférieur à 2 fois leur stock min</returns>
        public List<Product> AllForDemo()
        {
            List<Product> productsForDemo = new List<Product>();

            foreach (Product product in DaoProduct.ReadAll())
            {
                if (product.CurrentStock < (2 * product.MinStock))
                {
                    productsForDemo.Add(product);
                }
            }
            return productsForDemo;
        }

        /// <summary>
        /// Retourne la liste des produits ayant un stock actuel inférieur à leur stock min.
        /// Fonction pour l'affichage Xml et le réapprovisionnement des produits.
        /// </summary>
        /// <returns> liste des produits ayant un stock actuel inférieur à leur stock min</returns>
        public List<Product> AllToRestock()
        {
            List<Product> productsToRestock = new List<Product>();

            foreach (Product product in DaoProduct.ReadAll())
            {
                if (product.CurrentStock < product.MinStock)
                {
                    productsToRestock.Add(product);
                }
            }

            return productsToRestock;
        }

        /// <summary>
        /// Si le produit n'existe pas, enregistre ses infos dans la DB.
        /// Sinon, on décremente le stock actuel du produit par le nombre total de fois que ce produit est utilisé dans la commande.
        /// Si on est dans le cas d'un restock, alors QuantityInORder=0 donc le stock actuel n'est pas decrémenté mais plutôt 
        /// incrémenté par la quantité à commander (=stock max - stock actuel)
        /// </summary>
        /// <param name="product">produit à mettre à jour ou insérer dans la DB</param>
        /// <param name="restock">true si on est dans le cas d'un restsock et false si on est dans le cas d'une création de recette ou de commande</param>
        /// <returns></returns>
        public Product Save(Product product, bool restock = false)
        {
            if (product.Ref == 0)
            {
                return DaoProduct.Create(product);
            }
            else
            {
                product.CurrentStock -= product.QuantityInOrder;
                if (restock)
                {
                    product.CurrentStock += product.QuantityToOrder;
                }
                return DaoProduct.Update(product);
            }
        }

        /// <summary>
        /// Met à jour les produits (stocks min et max) non utilisés depuis 30 jours
        /// </summary>
        public void UpdateUnused()
        {
            List<Product> productsUnused = DaoProduct.ReadAllUnused();

            foreach (Product productUnused in productsUnused)
            {
                productUnused.MaxStock /= 2;
                //On doit forcément diminuer le stock_actuel du produit car il ne peut pas avoir un stock supérieur à stock max.
                productUnused.CurrentStock /= 2;
                productUnused.MinStock /= 2;
                Save(productUnused);
            }            
        }

        /// <summary>
        /// Supprime un produit de la DB.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>true si la suppresion a été effectuée et false sinon</returns>
        public bool Remove(Product product)
        {
            return DaoProduct.Delete(product.Ref);
        }
    }
}
