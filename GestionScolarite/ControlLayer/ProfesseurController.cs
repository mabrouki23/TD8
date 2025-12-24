using GestionScolarite.ViewLayer;
using System.Collections.Generic;

namespace GestionScolarite.ControlLayer
{
    public class ProfesseurController
    {
        private readonly IProfesseurDAO _professeurDAO;
        private readonly ProfesseurView _professeurView;

        public ProfesseurController(IProfesseurDAO professeurDAO, ProfesseurView professeurView)
        {
            _professeurDAO = professeurDAO;
            _professeurView = professeurView;
        }

        public void ListerProfesseurs()
        {
            // Récupérer les professeurs via le DAO
            var professeurs = _professeurDAO.GetAll();

            // Préparer les données pour la vue
            var listePourAffichage = new List<(int id, string nom, string prenom, string departement)>();
            foreach (var professeur in professeurs)
            {
                listePourAffichage.Add((professeur.Id, professeur.Nom, professeur.Prenom, professeur.Departement));
            }

            // Passer les données à la vue
            _professeurView.AfficherListe(listePourAffichage);
        }

        public Professeur? ObtenirProfesseurParId(int id)
        {
            return _professeurDAO.GetById(id);
        }
    }
}