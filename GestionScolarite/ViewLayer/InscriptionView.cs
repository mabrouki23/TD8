using System;
using System.Collections.Generic;

namespace GestionScolarite.ViewLayer
{
    internal class InscriptionView
    {
        public int AfficherMenuInscription()
        {
            Console.WriteLine("\n--- GESTION DES INSCRIPTIONS ---");
            Console.WriteLine("1. Lister les inscriptions par étudiant");
            Console.WriteLine("2. Lister les inscriptions par cours");
            Console.WriteLine("3. Ajouter une inscription");
            Console.WriteLine("4. Supprimer une inscription");
            Console.WriteLine("0. Retour au menu principal");
            Console.Write("Choix : ");

            string input = Console.ReadLine() ?? "0";
            int.TryParse(input, out int choix);
            return choix;
        }

        public (int etudiantId, int coursId, string session, int? note) SaisirInfosInscription()
        {
            Console.Write("ID de l'étudiant : ");
            string etudiantInput = Console.ReadLine() ?? "0";
            int.TryParse(etudiantInput, out int etudiantId);

            Console.Write("ID du cours : ");
            string coursInput = Console.ReadLine() ?? "0";
            int.TryParse(coursInput, out int coursId);

            Console.Write("Session (ex: H25, A24) : ");
            string session = Console.ReadLine() ?? string.Empty;

            Console.Write("Note (optionnel, appuyez sur Entrée pour ignorer) : ");
            string noteInput = Console.ReadLine();
            int? note = null;

            if (!string.IsNullOrWhiteSpace(noteInput) && int.TryParse(noteInput, out int noteValue))
            {
                note = noteValue;
            }

            return (etudiantId, coursId, session, note);
        }

        public void AfficherListeParEtudiant(int etudiantId, List<(string code, string titre, string session, int? note)> inscriptions)
        {
            Console.WriteLine($"\nInscriptions pour l'étudiant ID {etudiantId}:");
            Console.WriteLine("==========================================");

            foreach (var inscription in inscriptions)
            {
                string noteAffichage = inscription.note.HasValue ?
                    $"Note: {inscription.note.Value}" :
                    "Note: Non attribuée";

                Console.WriteLine($"[{inscription.code}] {inscription.titre} - Session: {inscription.session} - {noteAffichage}");
            }
        }

        public void AfficherListeParCours(int coursId, List<(string prenom, string nom, string session, int? note)> inscriptions)
        {
            Console.WriteLine($"\nInscriptions pour le cours ID {coursId}:");
            Console.WriteLine("=======================================");

            foreach (var inscription in inscriptions)
            {
                string noteAffichage = inscription.note.HasValue ?
                    $"Note: {inscription.note.Value}" :
                    "Note: Non attribuée";

                Console.WriteLine($"{inscription.prenom} {inscription.nom} - Session: {inscription.session} - {noteAffichage}");
            }
        }

        public (int etudiantId, int coursId, string session) DemanderCléInscription()
        {
            Console.Write("ID de l'étudiant : ");
            string etudiantInput = Console.ReadLine() ?? "0";
            int.TryParse(etudiantInput, out int etudiantId);

            Console.Write("ID du cours : ");
            string coursInput = Console.ReadLine() ?? "0";
            int.TryParse(coursInput, out int coursId);

            Console.Write("Session : ");
            string session = Console.ReadLine() ?? string.Empty;

            return (etudiantId, coursId, session);
        }

        public int DemanderIdEtudiant()
        {
            Console.Write("ID de l'étudiant : ");
            string input = Console.ReadLine() ?? "0";
            int.TryParse(input, out int id);
            return id;
        }

        public int DemanderIdCours()
        {
            Console.Write("ID du cours : ");
            string input = Console.ReadLine() ?? "0";
            int.TryParse(input, out int id);
            return id;
        }

        public void AfficherMessage(string message)
        {
            Console.WriteLine(message ?? string.Empty);
        }
    }
}