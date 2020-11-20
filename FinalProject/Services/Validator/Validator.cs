using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Services.Validator
{
    /// <summary>
    /// Cette classe sert à valider les inputs de l'utilisateur au moment de se connecter au site CooKinG
    /// </summary>
    public class Validator
    {
        public List<Input> Inputs; //Liste des infos rentrées par l'utilisateur

        public List<Input> Errors; //Liste des erreurs à afficher en cas de mauvais input rentré par l'utilisateur

        /// <summary>
        /// Constructeur de la classe
        /// </summary>
        /// <param name="inputs">informations rentrées par l'utilisateur (mail, mdp, nom, etc.)</param>
        public Validator(List<Input> inputs)
        {
            Inputs = inputs;
            Validate();
        }

        public Validator() {}

        /// <summary>
        /// Cette méthode vérifie simplement si les informations rentrées par l'utilisateur sont sémantiquement conforme:
        ///   -Pour le mail, on verifie que c'est bien la forme d'un mail (@, .com etc.)
        ///   -Pour le mot de passe, on vérifie qu'il fait plus de 6 caractères et qu'il est bien réécrit après
        /// </summary>
        public void Validate()
        {
            Errors = new List<Input>();
            foreach (Input input in Inputs)
            {
                switch (input.Field)
                {
                    case "email":
                        EmailAddressAttribute EmailValidator = new EmailAddressAttribute();
                        
                        if (!EmailValidator.IsValid(input.Value))
                        {
                            input.Message = "Email format is not valid.";
                            Errors.Add(input);
                        }
                        break;
                    case "password":
                        if (input.Value.Length < 6)
                        {
                            input.Message = "Password must be at least 6 characters long.";
                            Errors.Add(input);
                        }
                        break;
                    case "confirm":
                        if (input.Value != input.Confirm)
                        {
                            input.Message = "Password confirmation does not match.";
                            Errors.Add(input);
                        }
                        break;
                }
            }         
        }

        public bool HasErrors
        {
            get
            {
                return Errors.Count > 0;
            }
        }
    }

    /// <summary>
    /// Classe d'input. Chaque input de l'utilisateur a une instance de cette class: 
    ///     - Field=mail ; 
    ///     - Value=val.croissy@gmail.com ; 
    ///     - Confirm = null ; 
    ///     - Message = email format is not valid , on ne remplit cet attribut que si on rentre dans la condition de Validation()
    /// </summary>
    public class Input
    {
        public string Field; //Champ TextBox pour la création de compte (mail, mdp, confirmer mdp)
        public string Value; //valeur pour le champ
        public string Confirm;
        public string Message;

        /// <summary>
        /// Constructeur de la classe
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="confirm"></param>
        public Input(string field, string value, string confirm = "")
        {
            Field = field;
            Value = value;
            Confirm = confirm;
        }
    }
}
