using GestionScolarite.ViewLayer;

namespace GestionScolarite.ControlLayer
{
    internal class ApplicationController
    {
        private readonly EtudiantController etudiantController;
        private readonly CoursControlleur coursController;
        private readonly InscriptionControlleur inscriptionController;
        private readonly PrincipalView principalView;

        //injection de dépendances
        public ApplicationController(
            EtudiantController etudiantCtrl,
            CoursControlleur coursCtrl,
            InscriptionControlleur inscriptionCtrl,
            PrincipalView menuView)
        {
            etudiantController = etudiantCtrl;
            coursController = coursCtrl;
            inscriptionController = inscriptionCtrl;
            principalView = menuView;
        }

        public void Demarrer()
        {
            bool continuer = true;
            while (continuer)
            {
                int choix = principalView.AfficherMenuPrincipal();
                switch (choix)
                {
                    case 1:
                        //interpeler le controleur Etudiant
                        etudiantController.GererMenuEtudiant();
                        break;
                    case 2:
                        coursController.GererMenuCours();
                        break;
                    case 3:
                        inscriptionController.GererMenuInscription();
                        break;
                    case 0:
                        continuer = false;
                        principalView.AfficherMessage("Fin du programme.");
                        break;
                    default:
                        principalView.AfficherMessage("Choix invalide.");
                        break;
                }
            }
        }
    }
}