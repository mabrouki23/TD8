using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionScolarite.ModelLayer
{
    internal class Etudiant
    {
        private int id;
        private string nom;
        private string prenom;
        private List<Cours> coursSuivis;

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string Nom
        {
            get => nom;
            set => nom = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Prenom
        {
            get => prenom;
            set => prenom = value ?? throw new ArgumentNullException(nameof(value));
        }

        public List<Cours> CoursSuivis
        {
            get => coursSuivis;
            set => coursSuivis = value ?? new List<Cours>();
        }

        // Constructeurs
        public Etudiant(string nom, string prenom)
        {
            this.nom = nom ?? throw new ArgumentNullException(nameof(nom));
            this.prenom = prenom ?? throw new ArgumentNullException(nameof(prenom));
            coursSuivis = new List<Cours>();
        }

        public Etudiant(int id, string nom, string prenom)
        {
            this.id = id;
            this.nom = nom ?? throw new ArgumentNullException(nameof(nom));
            this.prenom = prenom ?? throw new ArgumentNullException(nameof(prenom));
            coursSuivis = new List<Cours>();
        }
    }
}