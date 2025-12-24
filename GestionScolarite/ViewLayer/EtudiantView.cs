using System;
using System.Collections.Generic;

namespace GestionScolarite.ViewLayer
{
    internal class EtudiantView
    {
        public int AfficherMenuEtudiant()
        {
            Console.WriteLine("\n--- GESTION DES ÉTUDIANTS ---");
            Console.WriteLine("1. Lister les étudiants");
            Console.WriteLine("2. Ajouter un étudiant");
            Console.WriteLine("3. Modifier un étudiant");
            Console.WriteLine("4. Supprimer un étudiant");
            Console.WriteLine("0. Retour au menu principal");
            Console.Write("Choix : ");
            string input = Console.ReadLine() ?? "0";
            int.TryParse(input, out int choix);
            return choix;
        }

        public (string prenom, string nom) SaisirInfosEtudiant()
        {
            Console.Write("Prénom : ");
            string prenom = Console.ReadLine() ?? string.Empty;
            Console.Write("Nom : ");
            string nom = Console.ReadLine() ?? string.Empty;
            return (prenom, nom);
        }

        public int DemanderIdEtudiant()
        {
            Console.Write("ID de l'étudiant : ");
            string input = Console.ReadLine() ?? "0";
            int.TryParse(input, out int id);
            return id;
        }

        public void AfficherListe(List<(int id, string prenom, string nom)> etudiants)
        {
            Console.WriteLine("\nListe des étudiants :");
            Console.WriteLine("=====================");
            foreach (var e in etudiants)
            {
                Console.WriteLine($"[{e.id}] {e.prenom} {e.nom}");
            }
        }

        public void AfficherMessage(string message)
        {
            Console.WriteLine(message ?? string.Empty);
        }
    }
}