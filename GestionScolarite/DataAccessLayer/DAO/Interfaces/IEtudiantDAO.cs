using GestionScolarite.ModelLayer;
using System.Collections.Generic;

namespace GestionScolarite.DataAccessLayer.DAO.Interfaces
{
    internal interface IEtudiantDAO : IDao<Etudiant>
    {
        // Méthodes spécifiques à l'étudiant
        List<Etudiant> RechercherParNom(string nom);
        List<Etudiant> RechercherParPrenom(string prenom);
        List<Etudiant> RechercherParNomComplet(string nom, string prenom);
        int GetNombreTotalEtudiants();
    }
}