using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GestionScolarite.ControlLayer;
using GestionScolarite.ViewLayer;
using System.Collections.Generic;

namespace GestionScolarite.UnitTests.ControlLayer
{
    [TestClass]
    public class ProfesseurControllerTests
    {
        [TestMethod]
        public void ListerProfesseurs_DoitRecupererEtAfficherProfesseurs()
        {
            // Arrange
            var mockProfesseurDAO = new Mock<IProfesseurDAO>();
            var professeurView = new ProfesseurView();
            var controller = new ProfesseurController(mockProfesseurDAO.Object, professeurView);

            // Créer des données fictives
            var professeursFictifs = new List<Professeur>
            {
                new Professeur(1, "Dupont", "Jean", "Informatique"),
                new Professeur(2, "Martin", "Marie", "Mathématiques"),
                new Professeur(3, "Bernard", "Pierre", "Physique")
            };

            // Configurer le mock pour retourner les données fictives
            mockProfesseurDAO.Setup(dao => dao.GetAll()).Returns(professeursFictifs);

            // Act
            controller.ListerProfesseurs();

            // Assert
            // Vérifier que la vue a reçu les données
            Assert.AreEqual(3, professeurView.ListeAffichée.Count);

            // Vérifier les données du premier professeur
            var premierProfesseur = professeurView.ListeAffichée[0];
            Assert.AreEqual(1, premierProfesseur.id);
            Assert.AreEqual("Dupont", premierProfesseur.nom);
            Assert.AreEqual("Jean", premierProfesseur.prenom);
            Assert.AreEqual("Informatique", premierProfesseur.departement);

            // Vérifier que la méthode GetAll a été appelée exactement une fois
            mockProfesseurDAO.Verify(dao => dao.GetAll(), Times.Once());
        }

        [TestMethod]
        public void ObtenirProfesseurParId_DoitRecupererProfesseurExistant()
        {
            // Arrange
            var mockProfesseurDAO = new Mock<IProfesseurDAO>();
            var professeurView = new ProfesseurView();
            var controller = new ProfesseurController(mockProfesseurDAO.Object, professeurView);

            var professeurFictif = new Professeur(1, "Dupont", "Jean", "Informatique");

            // Configurer le mock pour retourner un professeur spécifique
            mockProfesseurDAO.Setup(dao => dao.GetById(1)).Returns(professeurFictif);
            mockProfesseurDAO.Setup(dao => dao.GetById(999)).Returns((Professeur?)null);

            // Act & Assert
            // Test avec ID existant
            var resultat = controller.ObtenirProfesseurParId(1);
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Dupont", resultat.Nom);
            mockProfesseurDAO.Verify(dao => dao.GetById(1), Times.Once());

            // Test avec ID non existant
            var resultatInexistant = controller.ObtenirProfesseurParId(999);
            Assert.IsNull(resultatInexistant);
            mockProfesseurDAO.Verify(dao => dao.GetById(999), Times.Once());
        }

        [TestMethod]
        public void ListerProfesseurs_DoitGererListeVide()
        {
            // Arrange
            var mockProfesseurDAO = new Mock<IProfesseurDAO>();
            var professeurView = new ProfesseurView();
            var controller = new ProfesseurController(mockProfesseurDAO.Object, professeurView);

            // Configurer le mock pour retourner une liste vide
            mockProfesseurDAO.Setup(dao => dao.GetAll()).Returns(new List<Professeur>());

            // Act
            controller.ListerProfesseurs();

            // Assert
            Assert.AreEqual(0, professeurView.ListeAffichée.Count);
            mockProfesseurDAO.Verify(dao => dao.GetAll(), Times.Once());
        }

        [TestMethod]
        public void TestIntegrationControllerDAO_VueRecoitDonneesCorrectes()
        {
            // Arrange
            var mockProfesseurDAO = new Mock<IProfesseurDAO>();
            var professeurView = new ProfesseurView();
            var controller = new ProfesseurController(mockProfesseurDAO.Object, professeurView);

            var professeursTest = new List<Professeur>
            {
                new Professeur(10, "Gagnon", "Luc", "Chimie"),
                new Professeur(20, "Tremblay", "Sophie", "Biologie")
            };

            mockProfesseurDAO.Setup(dao => dao.GetAll()).Returns(professeursTest);

            // Act
            controller.ListerProfesseurs();

            // Assert - Vérifier l'intégration complète
            Assert.AreEqual(2, professeurView.ListeAffichée.Count);

            // Vérifier l'ordre et toutes les propriétés
            Assert.AreEqual(10, professeurView.ListeAffichée[0].id);
            Assert.AreEqual("Gagnon", professeurView.ListeAffichée[0].nom);
            Assert.AreEqual("Luc", professeurView.ListeAffichée[0].prenom);
            Assert.AreEqual("Chimie", professeurView.ListeAffichée[0].departement);

            Assert.AreEqual(20, professeurView.ListeAffichée[1].id);
            Assert.AreEqual("Tremblay", professeurView.ListeAffichée[1].nom);
            Assert.AreEqual("Sophie", professeurView.ListeAffichée[1].prenom);
            Assert.AreEqual("Biologie", professeurView.ListeAffichée[1].departement);

            // Vérifier que le contrôleur a bien appelé le DAO
            mockProfesseurDAO.Verify(dao => dao.GetAll(), Times.Once());
        }
    }
}