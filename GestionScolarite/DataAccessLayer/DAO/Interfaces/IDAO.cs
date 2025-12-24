using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionScolarite.DataAccessLayer.DAO.Interfaces
{
    public interface IDao<T>
    {

        //Méthodes CRUD génériques pour n'importe quelle entité du domaine
        T? GetById(int id);
        List<T> GetAll();
        void Ajouter(T entity);
        void Modifier(T entity);
        void Supprimer(int id);
    }

}
