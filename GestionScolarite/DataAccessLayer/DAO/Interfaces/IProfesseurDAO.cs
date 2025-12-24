using System.Collections.Generic;

namespace GestionScolarite.ControlLayer
{
    public interface IProfesseurDAO
    {
        List<Professeur> GetAll();
        Professeur? GetById(int id);
        void Ajouter(Professeur professeur);
        void Modifier(Professeur professeur);
        void Supprimer(int id);
    }
}