using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Product : INotifyPropertyChanged
    {
        public int Ref { get; set; }
        public string Name { get; set; }
        public string Unity { get; set; }
        public string Category { get; set; }
        public int CurrentStock { get; set; }
        public int MinStock { get; set; }
        public int MaxStock { get; set; }
        public Provider Provider { get; set; }

        private int _quantityInRecipe;

        public int QuantityInOrder; 

        /// <summary>
        /// Cette méthode permet de notifier d'un evenement de changement de valeur de la propriété QuantityInRecipe
        /// pour que sur l'affichage WPF la valeur se mette à jour. C'est le même principe que l'Observable Collection.
        /// Sans cette méthode, le changement de valeur se fait dans le code mais pas sur l'affichage.
        /// </summary>
        public int QuantityInRecipe //la quantité du produit dans la recette
        {
            get
            {
                return _quantityInRecipe;
            }

            set
            {
                _quantityInRecipe = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("QuantityInRecipe"));
                }
                //NotifyPropertyChanged("QuantityInRecipe");
            }
        }

        public int QuantityToOrder //la quantité à commander en cas de restock
        {
            get
            {
                return MaxStock - CurrentStock;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Méthode implémentée par INotifyPropertyChanged
        /// </summary>
        /// <param name="property"></param>
        //private void NotifyPropertyChanged(string property)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(property));
        //    }
        //}
    }
}
