using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinalProject.Database;
using FinalProject.Models;

namespace FinalProject.Services
{
    /// <summary>
    /// Cette classe sert de lien entre les fichiers du répertoire Pages (IHM) et le répertoire Database (DAO).
    /// C'est les fichiers de ce répertoire Service qui vont réaliser du traitement sur les données récupérées.
    /// </summary>
    class ServiceClient
    {
        private DaoClient DaoClient;

        public ServiceClient()
        {
            DaoClient = new DaoClient();
        }

        /// <summary>
        /// Récupère le client avec l'id entré en paramètre
        /// </summary>
        /// <param name="id">id du client à récupérer</param>
        /// <returns>Le client avec ses infos</returns>
        public Client One(int id)
        {
            return DaoClient.Read(id);
        }

        /// <summary>
        /// Récupère la liste de tout les clients.
        /// </summary>
        /// <returns>liste Client de tous les clients</returns>
        public List<Client> All()
        {
            return DaoClient.ReadAll();
        }

        /// <summary>
        /// Crée un client et l'enregistre dans la DB si ce dernier n'existe pas encore.
        /// S'il existe, fait un simple update de ses infos dans la DB
        /// </summary>
        /// <param name="client"></param>
        /// <returns>True si l'action s'est réalisée sans encombre et false sinon</returns>
        public bool Save(Client client)
        {
            if (client.Id == 0)
            {
                return DaoClient.Create(client);
            }
            else
            {
                return DaoClient.Update(client);
            }
        }

        /// <summary>
        /// Supprime un client de la DB.
        /// On n'utilise pas cette fonction sauf si exception oblige.
        /// </summary>
        /// <param name="client">client à supprimer</param>
        /// <returns>True si l'action s'est réalisée sans encombre et false sinon</returns>
        public bool Remove(Client client)
        {
            return DaoClient.Delete(client.Id);
        }
    }
}
