using GestionScolarite.DataAccessLayer.DAO.Interfaces;
using GestionScolarite.ModelLayer;
using GestionScolarite.ViewLayer;
using System;
using System.Collections.Generic;

namespace GestionScolarite.ControlLayer
{
    internal class EtudiantController
    {
        private readonly IEtudiantDAO etudiantDAO;
        private readonly EtudiantView view;

        public EtudiantController(IEtudiantDAO etudiantDAO, EtudiantView view)
        {
            this.etudiantDAO = etudiantDAO;
            this.view = view;
        }

        public void GererMenuEtudiant()
        {
            bool continuer = true;

            while (continuer)
            {
                int choix = view.AfficherMenuEtudiant();

                switch (choix)
                {
                    case 1:
                        ListerEtudiants();
                        break;
                    case 2:
                        AjouterEtudiant();
                        break;
                    case 3:
                        ModifierEtudiant();
                        break;
                    case 4:
                        SupprimerEtudiant();
                        break;
                    case 0:
                        continuer = false;
                        view.AfficherMessage("Retour au menu principal...");
                        break;
                    default:
                        view.AfficherMessage("Choix invalide.");
                        break;
                }
            }
        }

        private void ListerEtudiants()
        {
            try
            {
                List<Etudiant> etudiants = etudiantDAO.GetAll();

                if (etudiants.Count == 0)
                {
                    view.AfficherMessage("Aucun étudiant dans la base de données.");
                    return;
                }

                List<(int id, string prenom, string nom)> liste = new List<(int, string, string)>();

                foreach (var etu in etudiants)
                {
                    liste.Add((etu.Id, etu.Prenom, etu.Nom));
                }

                view.AfficherListe(liste);
            }
            catch (Exception ex)
            {
                view.AfficherMessage($"Erreur: {ex.Message}");
            }
        }

        private void AjouterEtudiant()
        {
            try
            {
                (string prenom, string nom) = view.SaisirInfosEtudiant();

                if (string.IsNullOrWhiteSpace(prenom) || string.IsNullOrWhiteSpace(nom))
                {
                    view.AfficherMessage("Prénom et nom requis.");
                    return;
                }

                Etudiant nouvelEtudiant = new Etudiant(nom, prenom);
                etudiantDAO.Ajouter(nouvelEtudiant);

                view.AfficherMessage("Étudiant ajouté.");
            }
            catch (Exception ex)
            {
                view.AfficherMessage($"Erreur lors de l'ajout: {ex.Message}");
            }
        }

        private void ModifierEtudiant()
        {
            try
            {
                int id = view.DemanderIdEtudiant();
                Etudiant etudiant = etudiantDAO.GetById(id);

                if (etudiant == null)
                {
                    view.AfficherMessage("Étudiant introuvable.");
                    return;
                }

                (string prenom, string nom) = view.SaisirInfosEtudiant();

                if (string.IsNullOrWhiteSpace(prenom) || string.IsNullOrWhiteSpace(nom))
                {
                    view.AfficherMessage("Champs invalides.");
                    return;
                }

                etudiant.Prenom = prenom;
                etudiant.Nom = nom;
                etudiantDAO.Modifier(etudiant);
                view.AfficherMessage("Étudiant modifié.");
            }
            catch (Exception ex)
            {
                view.AfficherMessage($"Erreur lors de la modification: {ex.Message}");
            }
        }

        private void SupprimerEtudiant()
        {
            try
            {
                int id = view.DemanderIdEtudiant();
                Etudiant etudiant = etudiantDAO.GetById(id);

                if (etudiant == null)
                {
                    view.AfficherMessage("Étudiant introuvable.");
                    return;
                }

                // Demander confirmation
                view.AfficherMessage($"Êtes-vous sûr de vouloir supprimer l'étudiant '{etudiant.Prenom} {etudiant.Nom}' ? (O/N)");
                var confirmation = Console.ReadLine();

                if (confirmation?.ToUpper() != "O")
                {
                    view.AfficherMessage("Suppression annulée.");
                    return;
                }

                etudiantDAO.Supprimer(id);
                view.AfficherMessage("Étudiant supprimé.");
            }
            catch (Exception ex)
            {
                view.AfficherMessage($"Erreur lors de la suppression: {ex.Message}");
            }
        }
    }
}