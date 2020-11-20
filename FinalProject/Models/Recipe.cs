using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinalProject.Models
{
    public class Recipe : INotifyPropertyChanged
    {
        public int Ref { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Remuneration { get; set; }
        public int TimesOrdered { get; set; } //Nombre de fois que la recette a été commandée
        public RecipeCreator RecipeCreator { get; set; } // créateur de la recette
        public List<Product> Products { get; set; } // Liste des produits composant la recette
        
        /// <summary>
        /// Attribut utile pour la démo.
        /// Lorsque l'on clique sur un produit de la DB, on veut afficher les recettes dans lesquelles il est utilisé
        /// et la quantité de ce produit utilisé.
        /// </summary>
        public int QuantityOfProductRequired { get; set; }

        /// <summary>
        /// Cette méthode sert à l'affichage des recettes au moment de réaliser une commande.
        /// Si le stock de produit n'est pas suffisant pour produire la recette, alors la recette 
        /// passera en invisible.
        /// </summary>
        public Recipe() 
        {
            Visibility = Visibility.Visible;
        }

        public int _quantityInOrder;

        /// <summary>
        /// Cette méthode permet de notifier d'un evenement de changement de valeur de la propriété QuantityInOrder
        /// pour que sur l'affichage WPF la valeur se mette à jour. C'est le même principe que l'Observable Collection.
        /// Sans cette méthode, le changement de valeur se fait dans le code mais pas sur l'affichage.
        /// </summary>
        public int QuantityInOrder //Nombre de fois que la recette est présente dans une commande 
        {
            get
            {
                return _quantityInOrder;
            }

            set
            {
                _quantityInOrder = value;
                NotifyPropertyChanged("QuantityInOrder");
            }
        }

        /// <summary>
        /// Calcule la nombre max de fois qu'une recette peut être produite en fonction des stocks
        /// des produits et de la quantité de ces derniers qu'elle utilise.
        /// </summary>
        public int QuantiteFaisable 
        {
            get
            {
                if (Products == null || Products.Count == 0)
                {
                    return 0;
                }
                int max = 100;

                foreach (Product product in Products)
                {
                    int quantite = product.CurrentStock / product.QuantityInRecipe;
                    if (quantite < max)
                    {
                        max = quantite;
                    }
                }
                if (max < 0) max = 0;
                return max;
            }           
        }

        public Visibility _visibility;

        public Visibility Visibility
        {
            get
            {
                return _visibility;
            }
            set
            {
                _visibility = value;

                NotifyPropertyChanged("Visibility");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
