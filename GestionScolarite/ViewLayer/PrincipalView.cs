using System;

namespace GestionScolarite.ViewLayer
{
    internal class PrincipalView
    {
        public int AfficherMenuPrincipal()
        {
            int choix = 0;
            Console.WriteLine("\n=== MENU PRINCIPAL ===");
            Console.WriteLine("1. Gérer les étudiants");
            Console.WriteLine("2. Gérer les cours");
            Console.WriteLine("3. Gérer les inscriptions");
            Console.WriteLine("0. Quitter");
            Console.Write("Choix : ");

            string saisie = Console.ReadLine();

            try
            {
                choix = Convert.ToInt32(saisie);
            }
            catch (FormatException)
            {
                Console.WriteLine("Erreur : veuillez entrer un nombre entier valide.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Erreur : le nombre est trop grand ou trop petit.");
            }

            return choix;
        }

        public void AfficherMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}