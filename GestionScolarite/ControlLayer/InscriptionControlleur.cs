using GestionScolarite.DataAccessLayer.DAO.Interfaces;
using GestionScolarite.ModelLayer;
using GestionScolarite.ViewLayer;
using System;
using System.Collections.Generic;

namespace GestionScolarite.ControlLayer
{
    internal class InscriptionControlleur
    {
        private readonly IInscriptionDAO _inscriptionDAO;
        private readonly InscriptionView _inscriptionView;
        private readonly IEtudiantDAO _etudiantDAO;
        private readonly ICoursDAO _coursDAO;

        public InscriptionControlleur(
            IInscriptionDAO inscriptionDAO,
            InscriptionView inscriptionView,
            IEtudiantDAO etudiantDAO,
            ICoursDAO coursDAO)
        {
            _inscriptionDAO = inscriptionDAO ?? throw new ArgumentNullException(nameof(inscriptionDAO));
            _inscriptionView = inscriptionView ?? throw new ArgumentNullException(nameof(inscriptionView));
            _etudiantDAO = etudiantDAO ?? throw new ArgumentNullException(nameof(etudiantDAO));
            _coursDAO = coursDAO ?? throw new ArgumentNullException(nameof(coursDAO));
        }

        public void GererMenuInscription()
        {
            bool continuer = true;
            while (continuer)
            {
                int choix = _inscriptionView.AfficherMenuInscription();

                switch (choix)
                {
                    case 1:
                        ListerInscriptionsParEtudiant();
                        break;
                    case 2:
                        ListerInscriptionsParCours();
                        break;
                    case 3:
                        AjouterInscription();
                        break;
                    case 4:
                        SupprimerInscription();
                        break;
                    case 0:
                        continuer = false;
                        _inscriptionView.AfficherMessage("Retour au menu principal...");
                        break;
                    default:
                        _inscriptionView.AfficherMessage("Choix invalide.");
                        break;
                }
            }
        }

        private void ListerInscriptionsParEtudiant()
        {
            try
            {
                int etudiantId = _inscriptionView.DemanderIdEtudiant();
                var inscriptions = _inscriptionDAO.GetInscriptionsParEtudiant(etudiantId);

                if (inscriptions.Count == 0)
                {
                    _inscriptionView.AfficherMessage("Aucune inscription trouvée pour cet étudiant.");
                    return;
                }

                var inscriptionsPourAffichage = new List<(string code, string titre, string session, int? note)>();
                foreach (var inscription in inscriptions)
                {
                    inscriptionsPourAffichage.Add((
                        inscription.Cours?.Code ?? "N/A",
                        inscription.Cours?.Titre ?? "N/A",
                        inscription.Session,
                        inscription.Note
                    ));
                }

                _inscriptionView.AfficherListeParEtudiant(etudiantId, inscriptionsPourAffichage);
            }
            catch (Exception ex)
            {
                _inscriptionView.AfficherMessage($"Erreur: {ex.Message}");
            }
        }

        private void ListerInscriptionsParCours()
        {
            try
            {
                int coursId = _inscriptionView.DemanderIdCours();
                var inscriptions = _inscriptionDAO.GetInscriptionsParCours(coursId);

                if (inscriptions.Count == 0)
                {
                    _inscriptionView.AfficherMessage("Aucune inscription trouvée pour ce cours.");
                    return;
                }

                var inscriptionsPourAffichage = new List<(string prenom, string nom, string session, int? note)>();
                foreach (var inscription in inscriptions)
                {
                    inscriptionsPourAffichage.Add((
                        inscription.Etudiant?.Prenom ?? "N/A",
                        inscription.Etudiant?.Nom ?? "N/A",
                        inscription.Session,
                        inscription.Note
                    ));
                }

                _inscriptionView.AfficherListeParCours(coursId, inscriptionsPourAffichage);
            }
            catch (Exception ex)
            {
                _inscriptionView.AfficherMessage($"Erreur: {ex.Message}");
            }
        }

        private void AjouterInscription()
        {
            try
            {
                var (etudiantId, coursId, session, note) = _inscriptionView.SaisirInfosInscription();

                if (string.IsNullOrWhiteSpace(session))
                {
                    _inscriptionView.AfficherMessage("La session est obligatoire.");
                    return;
                }

                var etudiant = _etudiantDAO.GetById(etudiantId);
                if (etudiant == null)
                {
                    _inscriptionView.AfficherMessage("Étudiant non trouvé.");
                    return;
                }

                var cours = _coursDAO.GetById(coursId);
                if (cours == null)
                {
                    _inscriptionView.AfficherMessage("Cours non trouvé.");
                    return;
                }

                var inscription = new Inscription(etudiant, cours, session, note);
                _inscriptionDAO.Ajouter(inscription);

                _inscriptionView.AfficherMessage("Inscription ajoutée avec succès.");
            }
            catch (Exception ex)
            {
                _inscriptionView.AfficherMessage($"Erreur lors de l'ajout: {ex.Message}");
            }
        }

        private void SupprimerInscription()
        {
            try
            {
                var (etudiantId, coursId, session) = _inscriptionView.DemanderCléInscription();
                _inscriptionDAO.Supprimer(etudiantId, coursId, session);

                _inscriptionView.AfficherMessage("Inscription supprimée avec succès.");
            }
            catch (Exception ex)
            {
                _inscriptionView.AfficherMessage($"Erreur lors de la suppression: {ex.Message}");
            }
        }
    }
}