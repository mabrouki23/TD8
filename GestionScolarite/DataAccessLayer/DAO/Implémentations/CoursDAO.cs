using GestionScolarite.DataAccessLayer.DAO.Interfaces;
using GestionScolarite.ModelLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace GestionScolarite.DataAccessLayer.DAO.Implémentations
{
    internal class CoursDAO : ICoursDAO
    {
        private readonly SqlConnection connection;

        //injecter la connection pour le DAO Cours
        public CoursDAO(SqlConnection connection)
        {
            this.connection = connection;
        }

        public void Ajouter(Cours cours)
        {
            // Vérifier si un cours avec le même code existe déjà
            if (GetByCode(cours.Code) != null)
            {
                throw new InvalidOperationException($"Un cours avec le code {cours.Code} existe déjà");
            }

            var requete = "INSERT INTO Cours (Titre, Code) VALUES (@Titre, @Code)";

            using (var cmd = new SqlCommand(requete, connection))
            {
                cmd.Parameters.AddWithValue("@Titre", cours.Titre);
                cmd.Parameters.AddWithValue("@Code", cours.Code);
                cmd.ExecuteNonQuery();
            }
        }

        public void Modifier(Cours cours)
        {
            // Vérifier si le cours existe
            var existingCours = GetById(cours.Id);
            if (existingCours == null)
            {
                throw new InvalidOperationException($"Le cours avec l'ID {cours.Id} n'existe pas");
            }

            // Vérifier si le nouveau code n'est pas déjà utilisé par un autre cours
            var coursWithSameCode = GetByCode(cours.Code);
            if (coursWithSameCode != null && coursWithSameCode.Id != cours.Id)
            {
                throw new InvalidOperationException($"Le code {cours.Code} est déjà utilisé par un autre cours");
            }

            var requete = "UPDATE Cours SET Titre = @Titre, Code = @Code WHERE Id = @Id";

            using (var cmd = new SqlCommand(requete, connection))
            {
                cmd.Parameters.AddWithValue("@Titre", cours.Titre);
                cmd.Parameters.AddWithValue("@Code", cours.Code);
                cmd.Parameters.AddWithValue("@Id", cours.Id);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("Échec de la modification du cours");
                }
            }
        }

        public void Supprimer(int id)
        {
            // Vérifier si le cours existe
            var cours = GetById(id);
            if (cours == null)
            {
                throw new InvalidOperationException($"Le cours avec l'ID {id} n'existe pas");
            }

            // Vérifier si le cours a des inscriptions
            var requeteCheck = "SELECT COUNT(*) FROM Inscriptions WHERE CoursId = @CoursId";
            int countInscriptions = 0;

            using (var cmdCheck = new SqlCommand(requeteCheck, connection))
            {
                cmdCheck.Parameters.AddWithValue("@CoursId", id);
                countInscriptions = Convert.ToInt32(cmdCheck.ExecuteScalar());
            }

            if (countInscriptions > 0)
            {
                throw new InvalidOperationException($"Impossible de supprimer le cours. Il a {countInscriptions} inscription(s) associée(s)");
            }

            var requete = "DELETE FROM Cours WHERE Id = @Id";

            using (var cmd = new SqlCommand(requete, connection))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("Échec de la suppression du cours");
                }
            }
        }

        public Cours? GetById(int id)
        {
            Cours? cours = null;
            var requete = "SELECT * FROM Cours WHERE Id = @Id";

            using (var cmd = new SqlCommand(requete, connection))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cours = new Cours(
                            (int)reader["Id"],
                            (string)reader["Titre"],
                            (string)reader["Code"]
                        );
                    }
                }
            }

            return cours;
        }

        public List<Cours> GetAll()
        {
            var liste = new List<Cours>();
            var requete = "SELECT * FROM Cours ORDER BY Code ASC";

            using (var cmd = new SqlCommand(requete, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    liste.Add(new Cours(
                        (int)reader["Id"],
                        (string)reader["Titre"],
                        (string)reader["Code"]
                    ));
                }
            }

            return liste;
        }

        public Cours? GetByCode(string code)
        {
            Cours? cours = null;
            var requete = "SELECT * FROM Cours WHERE Code = @Code";

            using (var cmd = new SqlCommand(requete, connection))
            {
                cmd.Parameters.AddWithValue("@Code", code);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cours = new Cours(
                            (int)reader["Id"],
                            (string)reader["Titre"],
                            (string)reader["Code"]
                        );
                    }
                }
            }

            return cours;
        }

        // Méthodes supplémentaires de l'interface ICoursDAO
        public List<Cours> RechercherParTitre(string motCle)
        {
            var liste = new List<Cours>();
            var requete = "SELECT * FROM Cours WHERE Titre LIKE @MotCle ORDER BY Code ASC";

            using (var cmd = new SqlCommand(requete, connection))
            {
                cmd.Parameters.AddWithValue("@MotCle", $"%{motCle}%");

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        liste.Add(new Cours(
                            (int)reader["Id"],
                            (string)reader["Titre"],
                            (string)reader["Code"]
                        ));
                    }
                }
            }

            return liste;
        }

        public List<Cours> GetCoursDisponiblesPourSession(string session)
        {
            // Cette méthode pourrait retourner les cours disponibles
            // pour une session donnée (non encore inscrits par exemple)
            // Pour l'instant, retournons tous les cours
            return GetAll();
        }
    }
}