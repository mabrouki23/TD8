using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionScolarite.ModelLayer
{
    internal class Inscription
    {
        private Etudiant etudiant;
        private Cours cours;
        private string session;
        private int? note; // int? pour correspondre à la base de données

        public Etudiant Etudiant
        {
            get => etudiant;
            set => etudiant = value ?? throw new ArgumentNullException(nameof(value));
        }

        public Cours Cours
        {
            get => cours;
            set => cours = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Session
        {
            get => session;
            set => session = value ?? throw new ArgumentNullException(nameof(value));
        }

        public int? Note
        {
            get => note;
            set => note = value;
        }

        // Constructeur
        public Inscription(Etudiant etudiant, Cours cours, string session, int? note = null)
        {
            this.etudiant = etudiant ?? throw new ArgumentNullException(nameof(etudiant));
            this.cours = cours ?? throw new ArgumentNullException(nameof(cours));
            this.session = session ?? throw new ArgumentNullException(nameof(session));
            this.note = note;
        }
    }
}