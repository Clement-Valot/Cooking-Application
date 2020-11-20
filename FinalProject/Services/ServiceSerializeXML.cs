using FinalProject.Database;
using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FinalProject.Services
{
    public class ServiceSerializeXML
    {
        /// <summary>
        /// Sérialize, c'est à dire met les informations souhaitées au format XML.
        /// On utilise les références using System.IO pour l'écriture de fichier (de C# vers Editix)
        /// et System.Xml.Serialization pour les méthodes relatives à la mise au format XML
        /// </summary>
        public void SerializerXML()
        {
            DaoProvider daoProvider = new DaoProvider();
            //On récupère tous les fournisseurs de la base de données dont au moins un des produits qu'il fournissent ont
            //leur stock actuel inférieur à leur stock minimal
            List<Provider> providers = daoProvider.ReadAllWithProductToRestock();
            //On crée la liste des Fournisseur que l'on mettra dans le fichier XML.
            //Elle a comme attributs le nom du fournisseur et les produits à réapprovisionner
            //qui ont aussi une classe dédiée (Nom du produit et la quantité à commander [stock max - stock actuel])
            List<ProviderXML> providersXML = new List<ProviderXML>();

            //On parcourt chaque fournisseur de la liste
            foreach (Provider provider in providers)
            {
                //On récupère la liste des produits fournis par ce fournisseur dont leur stock actuel est inférieur à leur
                //stock minimal

                //On crée la liste des ProduitXML (classe définie en dessous)
                List<ProductXML> productsXML = new List<ProductXML>();

                //On parcourt chaque produit de la liste de produits à réapprovisionner de chaque fournisseur
                foreach (Product product in provider.Products)
                {
                    ProductXML productXML = new ProductXML
                    {
                        Nom = product.Name,
                        Quantite_A_Commander = product.QuantityToOrder
                    };
                    productsXML.Add(productXML);
                }
                ProviderXML providerXML = new ProviderXML(provider);
                providerXML.Produits = productsXML;
                providersXML.Add(providerXML);
            }
            // Instanciation des outils ( StreamWriter et Serializer )
            XmlSerializer xs = new XmlSerializer(typeof(List<ProviderXML>)); // l'outil de sérialisation
            StreamWriter wr = new StreamWriter("Fournisseurs.xml"); // accès en écriture d'un fichier ( texte )

            //On veut récupérer seulement certains attributs de nos classes produits et fournisseurs
            xs.Serialize(wr, providersXML); // action de sérialiser en XML l'objet Fxml
            wr.Close();
        }

        /// <summary>
        /// Classe qui sert à la sérialisation des données en XML.
        /// Etant donné qu'on ne veut QUE les infos nom et quantité à commander des produits au stocks insuffisant,
        /// on est dans l'obligation de créer une nouvelle classe.
        /// </summary>
        [XmlType("Produit")]
        public class ProductXML
        {
            public string Nom { get; set; }
            public int Quantite_A_Commander { get; set; }
        }

        /// <summary>
        /// Classe qui sert à la sérialisation des données en XML.
        /// Etant donné qu'on ne veut QUE les infos nom et quantité à commander des produits au stocks insuffisant,
        /// on est dans l'obligation de créer une nouvelle classe.
        /// </summary>
        [XmlType("Fournisseur")]
        public class ProviderXML
        {
            public string Nom { get; set; }
            public List<ProductXML> Produits { get; set; }

            /// <summary>
            /// On initialise le constructeurs de la classe FournisseurXML avec comme paramètre le fournisseur
            /// </summary>
            /// <param name="provider"></param>
            public ProviderXML(Provider provider)
            {
                this.Produits = new List<ProductXML>();
                this.Nom = provider.Name;
            }

            /// <summary>
            /// Pour déclarer une liste avec un type d'objet bien précis, il faut que cet objet soit instanciable, c'est le principe du constructeur.
            /// On a déjà établie un constructeur de cette classe au-dessus mais elle prend un paramètre. Or lorsque l'on instancie une liste d'objet (l.39)
            /// de la classe ProviderXML, on a pas encore le paramètre provider. Il faut donc pouvoir l'instancier au préalable sans paramètre 
            /// d'où la surcharge réalisée juste ici.
            /// </summary>
            public ProviderXML() { }
        }
    }
}
