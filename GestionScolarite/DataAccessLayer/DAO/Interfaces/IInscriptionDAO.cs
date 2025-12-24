using GestionScolarite.ModelLayer;
using System;
using System.Collections.Generic;

namespace GestionScolarite.DataAccessLayer.DAO.Interfaces
{
    internal interface IInscriptionDAO
    {
        // Méthodes CRUD pour entités avec clés composées
        void Ajouter(Inscription inscription);
        void Supprimer(Inscription inscription);
        List<Inscription> GetInscriptionsParEtudiant(int etudiantId);
        List<Inscription> GetInscriptionsParCours(int coursId);
        void Supprimer(int etudiantId, int coursId, string session); // clé composée

        // Méthodes supplémentaires utiles
        Inscription? GetInscription(int etudiantId, int coursId, string session);
        void ModifierNote(int etudiantId, int coursId, string session, decimal? nouvelleNote);
        bool ExisteInscription(int etudiantId, int coursId, string session);
    }
}
    