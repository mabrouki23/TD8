using GestionScolarite.DataAccessLayer.DAO.Interfaces;
using GestionScolarite.ModelLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace GestionScolarite.DataAccessLayer.DAO.Implémentations
{
    internal class EtudiantDAO : IEtudiantDAO
    {
        private readonly SqlConnection connection;

        //injecter la connection pour le DAO Etudiant
        public EtudiantDAO(SqlConnection connection)
        {
            this.connection = connection;
        }

        public void Ajouter(Etudiant etudiant)
        {
            var requeteAjout = "INSERT INTO Etudiants (Nom, Prenom) VALUES (@Nom, @Prenom)";
            using (var cmd = new SqlCommand(requeteAjout, connection))
            {
                cmd.Parameters.AddWithValue("@Nom", etudiant.Nom);
                cmd.Parameters.AddWithValue("@Prenom", etudiant.Prenom);
                cmd.ExecuteNonQuery();
            }
        }

        public void Modifier(Etudiant etudiant)
        {
            var requeteMaj = "UPDATE Etudiants SET Nom = @Nom, Prenom = @Prenom WHERE Id = @Id";
            using (var cmd = new SqlCommand(requeteMaj, connection))
            {
                cmd.Parameters.AddWithValue("@Nom", etudiant.Nom);
                cmd.Parameters.AddWithValue("@Prenom", etudiant.Prenom);
                cmd.Parameters.AddWithValue("@Id", etudiant.Id);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException($"L'étudiant avec l'ID {etudiant.Id} n'existe pas");
                }
            }
        }

        public void Supprimer(int id)
        {
            // Vérifier si l'étudiant existe
            var etudiant = GetById(id);
            if (etudiant == null)
            {
                throw new InvalidOperationException($"L'étudiant avec l'ID {id} n'existe pas");
            }

            // Vérifier si l'étudiant a des inscriptions
            var requeteCheck = "SELECT COUNT(*) FROM Inscriptions WHERE EtudiantId = @EtudiantId";
            int countInscriptions = 0;

            using (var cmdCheck = new SqlCommand(requeteCheck, connection))
            {
                cmdCheck.Parameters.AddWithValue("@EtudiantId", id);
                countInscriptions = Convert.ToInt32(cmdCheck.ExecuteScalar());
            }

            if (countInscriptions > 0)
            {
                throw new InvalidOperationException($"Impossible de supprimer l'étudiant. Il a {countInscriptions} inscription(s) associée(s)");
            }

            var requeteSupp = "DELETE FROM Etudiants WHERE Id = @Id";
            using (var cmd = new SqlCommand(requeteSupp, connection))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("Échec de la suppression de l'étudiant");
                }
            }
        }

        public Etudiant? GetById(int id)
        {
            Etudiant? etudiant = null;
            var requetteLectureParId = "SELECT * FROM Etudiants WHERE Id = @Id";
            using (var cmd = new SqlCommand(requetteLectureParId, connection))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        etudiant = new Etudiant
                        (
                            (int)reader["Id"],
                            (string)reader["Nom"],
                            (string)reader["Prenom"]
                        );
                    }
                }
            }

            return etudiant;
        }

        public List<Etudiant> GetAll()
        {
            var liste = new List<Etudiant>();

            var requetteGetAll = "SELECT * FROM Etudiants ORDER BY Nom, Prenom";
            using (var cmd = new SqlCommand(requetteGetAll, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    liste.Add(new Etudiant
                    (
                        (int)reader["Id"],
                        (string)reader["Nom"],
                        (string)reader["Prenom"]
                    ));
                }
            }

            return liste;
        }

        // Méthodes supplémentaires de l'interface IEtudiantDAO
        public List<Etudiant> RechercherParNom(string nom)
        {
            var liste = new List<Etudiant>();
            var requete = "SELECT * FROM Etudiants WHERE Nom LIKE @Nom ORDER BY Nom, Prenom";

            using (var cmd = new SqlCommand(requete, connection))
            {
                cmd.Parameters.AddWithValue("@Nom", $"%{nom}%");

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        liste.Add(new Etudiant
                        (
                            (int)reader["Id"],
                            (string)reader["Nom"],
                            (string)reader["Prenom"]
                        ));
                    }
                }
            }

            return liste;
        }

        public List<Etudiant> RechercherParPrenom(string prenom)
        {
            var liste = new List<Etudiant>();
            var requete = "SELECT * FROM Etudiants WHERE Prenom LIKE @Prenom ORDER BY Nom, Prenom";

            using (var cmd = new SqlCommand(requete, connection))
            {
                cmd.Parameters.AddWithValue("@Prenom", $"%{prenom}%");

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        liste.Add(new Etudiant
                        (
                            (int)reader["Id"],
                            (string)reader["Nom"],
                            (string)reader["Prenom"]
                        ));
                    }
                }
            }

            return liste;
        }

        public List<Etudiant> RechercherParNomComplet(string nom, string prenom)
        {
            var liste = new List<Etudiant>();
            var requete = "SELECT * FROM Etudiants WHERE Nom LIKE @Nom AND Prenom LIKE @Prenom ORDER BY Nom, Prenom";

            using (var cmd = new SqlCommand(requete, connection))
            {
                cmd.Parameters.AddWithValue("@Nom", $"%{nom}%");
                cmd.Parameters.AddWithValue("@Prenom", $"%{prenom}%");

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        liste.Add(new Etudiant
                        (
                            (int)reader["Id"],
                            (string)reader["Nom"],
                            (string)reader["Prenom"]
                        ));
                    }
                }
            }

            return liste;
        }

        public int GetNombreTotalEtudiants()
        {
            var requete = "SELECT COUNT(*) FROM Etudiants";

            using (var cmd = new SqlCommand(requete, connection))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}