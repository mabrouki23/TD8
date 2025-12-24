using System.Collections.Generic;

namespace GestionScolarite.ViewLayer
{
    public class ProfesseurView
    {
        public List<(int id, string nom, string prenom, string departement)> ListeAffichée { get; private set; }
            = new List<(int, string, string, string)>();

        public void AfficherListe(List<(int id, string nom, string prenom, string departement)> professeurs)
        {
            ListeAffichée = professeurs;
        }

        public int AfficherMenuProfesseur() => 0; // Méthode fictive
        public (string nom, string prenom, string departement) SaisirInfosProfesseur() => ("", "", "");
        public int DemanderIdProfesseur() => 0;
        public void AfficherMessage(string message) { }
    }
}