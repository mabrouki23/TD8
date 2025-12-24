namespace GestionScolarite.ControlLayer
{
    public class Professeur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Departement { get; set; }

        public Professeur(int id, string nom, string prenom, string departement)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Departement = departement;
        }

        // Constructeur par défaut pour la sérialisation
        public Professeur() { }
    }
}