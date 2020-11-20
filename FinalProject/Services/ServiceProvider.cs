using FinalProject.Database;
using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    class ServiceProvider
    {
        private DaoProvider DaoProvider;

        public ServiceProvider()
        {
            DaoProvider = new DaoProvider();
        }

        public List<Provider> All()
        {
            return DaoProvider.ReadAll();
        }
    }
}
