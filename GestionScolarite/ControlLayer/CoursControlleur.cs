using GestionScolarite.DataAccessLayer.DAO.Interfaces;
using GestionScolarite.ModelLayer;
using GestionScolarite.ViewLayer;
using System;
using System.Collections.Generic;

namespace GestionScolarite.ControlLayer
{
    internal class CoursControlleur
    {
        private readonly ICoursDAO _coursDAO;
        private readonly CoursView _coursView;

        // Constructeur avec 2 arguments
        public CoursControlleur(ICoursDAO coursDAO, CoursView coursView)
        {
            _coursDAO = coursDAO ?? throw new ArgumentNullException(nameof(coursDAO));
            _coursView = coursView ?? throw new ArgumentNullException(nameof(coursView));
        }

        public void GererMenuCours()
        {
            bool continuer = true;
            while (continuer)
            {
                int choix = _coursView.AfficherMenuCours();

                switch (choix)
                {
                    case 1:
                        ListerCours();
                        break;
                    case 2:
                        AjouterCours();
                        break;
                    case 3:
                        ModifierCours();
                        break;
                    case 4:
                        SupprimerCours();
                        break;
                    case 0:
                        continuer = false;
                        _coursView.AfficherMessage("Retour au menu principal...");
                        break;
                    default:
                        _coursView.AfficherMessage("Choix invalide.");
                        break;
                }
            }
        }

        private void ListerCours()
        {
            try
            {
                var coursList = _coursDAO.GetAll();

                if (coursList.Count == 0)
                {
                    _coursView.AfficherMessage("Aucun cours disponible.");
                    return;
                }

                // Convertir pour l'affichage
                var coursPourAffichage = new List<(int id, string code, string titre)>();
                foreach (var cours in coursList)
                {
                    coursPourAffichage.Add((cours.Id, cours.Code, cours.Titre));
                }

                _coursView.AfficherListe(coursPourAffichage);
            }
            catch (Exception ex)
            {
                _coursView.AfficherMessage($"Erreur: {ex.Message}");
            }
        }

        private void AjouterCours()
        {
            try
            {
                var (code, titre) = _coursView.SaisirInfosCours();

                // Validation
                if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(titre))
                {
                    _coursView.AfficherMessage("Le code et le titre sont obligatoires.");
                    return;
                }

                // Vérifier si le code existe déjà
                var coursExist = _coursDAO.GetByCode(code);
                if (coursExist != null)
                {
                    _coursView.AfficherMessage($"Un cours avec le code {code} existe déjà.");
                    return;
                }

                // Créer un nouveau cours
                var cours = new Cours(titre, code);
                _coursDAO.Ajouter(cours);

                _coursView.AfficherMessage("Cours ajouté avec succès.");
            }
            catch (Exception ex)
            {
                _coursView.AfficherMessage($"Erreur lors de l'ajout: {ex.Message}");
            }
        }

        private void ModifierCours()
        {
            try
            {
                int id = _coursView.DemanderIdCours();
                var cours = _coursDAO.GetById(id);

                if (cours == null)
                {
                    _coursView.AfficherMessage("Cours non trouvé.");
                    return;
                }

                var (code, titre) = _coursView.SaisirInfosCours();

                // Validation
                if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(titre))
                {
                    _coursView.AfficherMessage("Le code et le titre sont obligatoires.");
                    return;
                }

                // Vérifier si le code existe déjà pour un autre cours
                var coursAvecMemeCode = _coursDAO.GetByCode(code);
                if (coursAvecMemeCode != null && coursAvecMemeCode.Id != id)
                {
                    _coursView.AfficherMessage($"Un autre cours avec le code {code} existe déjà.");
                    return;
                }

                // Mettre à jour le cours
                cours.Code = code;
                cours.Titre = titre;

                _coursDAO.Modifier(cours);

                _coursView.AfficherMessage("Cours modifié avec succès.");
            }
            catch (Exception ex)
            {
                _coursView.AfficherMessage($"Erreur lors de la modification: {ex.Message}");
            }
        }

        private void SupprimerCours()
        {
            try
            {
                int id = _coursView.DemanderIdCours();

                // Vérifier si le cours existe
                var cours = _coursDAO.GetById(id);
                if (cours == null)
                {
                    _coursView.AfficherMessage("Cours non trouvé.");
                    return;
                }

                // Demander confirmation
                _coursView.AfficherMessage($"Êtes-vous sûr de vouloir supprimer le cours '{cours.Code} - {cours.Titre}' ? (O/N)");
                var confirmation = Console.ReadLine();

                if (confirmation?.ToUpper() != "O")
                {
                    _coursView.AfficherMessage("Suppression annulée.");
                    return;
                }

                _coursDAO.Supprimer(id);

                _coursView.AfficherMessage("Cours supprimé avec succès.");
            }
            catch (Exception ex)
            {
                _coursView.AfficherMessage($"Erreur lors de la suppression: {ex.Message}");
            }
        }
    }
}