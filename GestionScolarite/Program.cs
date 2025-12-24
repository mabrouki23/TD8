// See https://aka.ms/new-console-template for more information

using GestionScolarite.ControlLayer;
using GestionScolarite.DataAccessLayer.DAO.Implémentations;
using GestionScolarite.DataAccessLayer.DataAccess;
using GestionScolarite.ViewLayer;
using Microsoft.Data.SqlClient;

//Point d'entrée du programme...

//ouvre la connexion
//Crée et initialise les DAO, les vues et les contrôleurs,
//le controlleur principal demarre l'Application.


try
{
    using (SqlConnection conn = DatabaseConnection.GetConnection())
    {
        conn.Open();

        var etudiantDAO = new EtudiantDAO(conn);
        var coursDAO = new CoursDAO(conn);
        var inscriptionDAO = new InscriptionDAO(conn, etudiantDAO, coursDAO);

        var etudiantView = new EtudiantView();
        var coursView = new CoursView();
        var inscriptionView = new InscriptionView();
        var principalView = new PrincipalView();

        var etudiantCtrl = new EtudiantController(etudiantDAO, etudiantView);
        var coursCtrl = new CoursControlleur(coursDAO, coursView);
        var inscriptionCtrl = new InscriptionControlleur(inscriptionDAO, inscriptionView, etudiantDAO, coursDAO);

        var appCtrl = new ApplicationController(etudiantCtrl, coursCtrl, inscriptionCtrl, principalView);

        appCtrl.Demarrer();
    }
}
catch (SqlException sql)
{
    Console.WriteLine($"Erreur SQL : {sql.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Exception : {ex.Message}");
}
