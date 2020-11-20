using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Provider
    {
        public int Ref { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public List<Product> Products { get; set; } //liste des produits fournis par le fournisseur
    }
}
