using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Session
{
    public static class AuthUser
    {
        public static string Role { get; set; } //Chaque utilisateur a un role (client, cdr, admin)
        public static Client Client { get; set; } //Chaque utilisateur est au moins un client
        public static RecipeCreator RecipeCreator { get; set; } //un utilisateur peut être un cdr
    }
}
