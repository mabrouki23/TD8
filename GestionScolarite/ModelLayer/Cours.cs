using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionScolarite.ModelLayer
{
    internal class Cours
    {
        private int id;
        private string titre;
        private string code;
        private List<Etudiant> etudiantsInscrits;

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string Titre
        {
            get => titre;
            set => titre = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Code
        {
            get => code;
            set => code = value ?? throw new ArgumentNullException(nameof(value));
        }

        public List<Etudiant> EtudiantsInscrits
        {
            get => etudiantsInscrits;
            set => etudiantsInscrits = value ?? new List<Etudiant>();
        }

        // Constructeurs
        public Cours(int id, string titre, string code)
        {
            this.id = id;
            this.titre = titre ?? throw new ArgumentNullException(nameof(titre));
            this.code = code ?? throw new ArgumentNullException(nameof(code));
            this.etudiantsInscrits = new List<Etudiant>();
        }

        public Cours(string titre, string code)
        {
            this.titre = titre ?? throw new ArgumentNullException(nameof(titre));
            this.code = code ?? throw new ArgumentNullException(nameof(code));
            etudiantsInscrits = new List<Etudiant>();
        }
    }
}