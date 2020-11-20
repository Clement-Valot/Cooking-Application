using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Client
    {
        public int Id { get; set; } //l'id sert à identifier le client dans le site
        public string Mail { get; set; } //le mail sert à l'identification du client dans le site
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int Cooks { get; set; } //le solde cook du client pour ne pas avoir à gérer de flux financier autre notamment pour payer les commandes


        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        } //string composé du prénom + du nom, plus simple d'utiliser cet attribut pour certains affichage
    }
}
