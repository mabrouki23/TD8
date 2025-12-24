using GestionScolarite.ModelLayer;
using System.Collections.Generic;

namespace GestionScolarite.DataAccessLayer.DAO.Interfaces
{
    internal interface ICoursDAO : IDao<Cours>
    {
        // Méthodes spécifiques au cours
        Cours? GetByCode(string code);
        List<Cours> RechercherParTitre(string motCle);
        List<Cours> GetCoursDisponiblesPourSession(string session);
    }
}