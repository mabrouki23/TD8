using System;
using System.Collections.Generic;

namespace GestionScolarite.ViewLayer
{
    internal class CoursView
    {
        public int AfficherMenuCours()
        {
            Console.WriteLine("\n--- GESTION DES COURS ---");
            Console.WriteLine("1. Lister les cours");
            Console.WriteLine("2. Ajouter un cours");
            Console.WriteLine("3. Modifier un cours");
            Console.WriteLine("4. Supprimer un cours");
            Console.WriteLine("0. Retour au menu principal");
            Console.Write("Choix : ");
            string input = Console.ReadLine() ?? "0";
            int.TryParse(input, out int choix);
            return choix;
        }

        public (string code, string titre) SaisirInfosCours()
        {
            Console.Write("Code du cours (ex: INF1001) : ");
            string code = Console.ReadLine() ?? string.Empty;

            Console.Write("Titre du cours : ");
            string titre = Console.ReadLine() ?? string.Empty;

            return (code, titre);
        }

        public int DemanderIdCours()
        {
            Console.Write("ID du cours : ");
            string input = Console.ReadLine() ?? "0";
            int.TryParse(input, out int id);
            return id;
        }

        public void AfficherListe(List<(int id, string code, string titre)> coursList)
        {
            Console.WriteLine("\nListe des cours :");
            Console.WriteLine("=================");

            foreach (var cours in coursList)
            {
                Console.WriteLine($"[{cours.id}] {cours.code} - {cours.titre}");
            }
        }

        public void AfficherMessage(string message)
        {
            Console.WriteLine(message ?? string.Empty);
        }
    }
}