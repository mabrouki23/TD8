using GestionScolarite.DataAccessLayer.DAO.Interfaces;
using GestionScolarite.ModelLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace GestionScolarite.DataAccessLayer.DAO.Implémentations
{
    internal class InscriptionDAO : IInscriptionDAO
    {
        private readonly SqlConnection connection;
        private readonly IDao<Etudiant> etudiantDAO;
        private readonly IDao<Cours> coursDAO;

        public InscriptionDAO(SqlConnection connection, IDao<Etudiant> etudiantDAO, IDao<Cours> coursDAO)
        {
            this.connection = connection;
            this.etudiantDAO = etudiantDAO;
            this.coursDAO = coursDAO;
        }

        public void Ajouter(Inscription inscription)
        {
            if (ExisteInscription(inscription.Etudiant.Id, inscription.Cours.Id, inscription.Session))
            {
                throw new InvalidOperationException($"L'étudiant est déjà inscrit à ce cours pour la session {inscription.Session}");
            }

            var requete = @"INSERT INTO Inscriptions (EtudiantId, CoursId, Session, Note) 
                           VALUES (@EtudiantId, @CoursId, @Session, @Note)";

            using (var cmd = new SqlCommand(requete, connection))
            {
                cmd.Parameters.AddWithValue("@EtudiantId", inscription.Etudiant.Id);
                cmd.Parameters.AddWithValue("@CoursId", inscription.Cours.Id);
                cmd.Parameters.AddWithValue("@Session", inscription.Session);

                if (inscription.Note.HasValue)
                    cmd.Parameters.AddWithValue("@Note", inscription.Note.Value);
                else
                    cmd.Parameters.AddWithValue("@Note", DBNull.Value);

                cmd.ExecuteNonQuery();
            }
        }

        public void Supprimer(Inscription inscription)
        {
            Supprimer(inscription.Etudiant.Id, inscription.Cours.Id, inscription.Session);
        }

        public List<Inscription> GetInscriptionsParEtudiant(int etudiantId)
        {
            var inscriptions = new List<Inscription>();
            var requete = @"SELECT i.EtudiantId, i.CoursId, i.Session, i.Note, 
                                   c.Id as CoursId, c.Titre, c.Code,
                                   e.Id as EtudiantId, e.Nom, e.Prenom
                            FROM Inscriptions i
                            INNER JOIN Cours c ON i.CoursId = c.Id
                            INNER JOIN Etudiants e ON i.EtudiantId = e.Id
                            WHERE i.EtudiantId = @EtudiantId
                            ORDER BY i.Session DESC, c.Code ASC";

            using (var cmd = new SqlCommand(requete, connection))
            {
                cmd.Parameters.AddWithValue("@EtudiantId", etudiantId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var etudiant = new Etudiant(
                            (int)reader["EtudiantId"],
                            (string)reader["Nom"],
                            (string)reader["Prenom"]
                        );

                        var cours = new Cours(
                            (int)reader["CoursId"],
                            (string)reader["Titre"],
                            (string)reader["Code"]
                        );

                        // Lire la note comme int? (decimal dans BD mais int? dans modèle)
                        int? note = null;
                        if (reader["Note"] != DBNull.Value)
                        {
                            // Convertir de decimal à int
                            decimal noteDecimal = (decimal)reader["Note"];
                            note = (int)Math.Round(noteDecimal, 0); // Arrondir à l'entier le plus proche
                        }

                        var inscription = new Inscription(
                            etudiant,
                            cours,
                            (string)reader["Session"],
                            note
                        );

                        inscriptions.Add(inscription);
                    }
                }
            }

            return inscriptions;
        }

        public List<Inscription> GetInscriptionsParCours(int coursId)
        {
            var inscriptions = new List<Inscription>();
            var requete = @"SELECT i.EtudiantId, i.CoursId, i.Session, i.Note, 
                                   c.Id as CoursId, c.Titre, c.Code,
                                   e.Id as EtudiantId, e.Nom, e.Prenom
                            FROM Inscriptions i
                            INNER JOIN Cours c ON i.CoursId = c.Id
                            INNER JOIN Etudiants e ON i.EtudiantId = e.Id
                            WHERE i.CoursId = @CoursId
                            ORDER BY e.Nom, e.Prenom, i.Session DESC";

            using (var cmd = new SqlCommand(requete, connection))
            {
                cmd.Parameters.AddWithValue("@CoursId", coursId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var etudiant = new Etudiant(
                            (int)reader["EtudiantId"],
                            (string)reader["Nom"],
                            (string)reader["Prenom"]
                        );

                        var cours = new Cours(
                            (int)reader["CoursId"],
                            (string)reader["Titre"],
                            (string)reader["Code"]
                        );

                        // Lire la note comme int? (decimal dans BD mais int? dans modèle)
                        int? note = null;
                        if (reader["Note"] != DBNull.Value)
                        {
                            // Convertir de decimal à int
                            decimal noteDecimal = (decimal)reader["Note"];
                            note = (int)Math.Round(noteDecimal, 0);
                        }

                        var inscription = new Inscription(
                            etudiant,
                            cours,
                            (string)reader["Session"],
                            note
                        );

                        inscriptions.Add(inscription);
                    }
                }
            }

            return inscriptions;
        }

        public void Supprimer(int etudiantId, int coursId, string session)
        {
            var requete = "DELETE FROM Inscriptions WHERE EtudiantId = @EtudiantId AND CoursId = @CoursId AND Session = @Session";

            using (var cmd = new SqlCommand(requete, connection))
            {
                cmd.Parameters.AddWithValue("@EtudiantId", etudiantId);
                cmd.Parameters.AddWithValue("@CoursId", coursId);
                cmd.Parameters.AddWithValue("@Session", session ?? throw new ArgumentNullException(nameof(session)));

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("Inscription non trouvée");
                }
            }
        }

        public Inscription? GetInscription(int etudiantId, int coursId, string session)
        {
            var requete = @"SELECT i.EtudiantId, i.CoursId, i.Session, i.Note, 
                                   c.Id as CoursId, c.Titre, c.Code,
                                   e.Id as EtudiantId, e.Nom, e.Prenom
                            FROM Inscriptions i
                            INNER JOIN Cours c ON i.CoursId = c.Id
                            INNER JOIN Etudiants e ON i.EtudiantId = e.Id
                            WHERE i.EtudiantId = @EtudiantId 
                            AND i.CoursId = @CoursId 
                            AND i.Session = @Session";

            using (var cmd = new SqlCommand(requete, connection))
            {
                cmd.Parameters.AddWithValue("@EtudiantId", etudiantId);
                cmd.Parameters.AddWithValue("@CoursId", coursId);
                cmd.Parameters.AddWithValue("@Session", session ?? throw new ArgumentNullException(nameof(session)));

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var etudiant = new Etudiant(
                            (int)reader["EtudiantId"],
                            (string)reader["Nom"],
                            (string)reader["Prenom"]
                        );

                        var cours = new Cours(
                            (int)reader["CoursId"],
                            (string)reader["Titre"],
                            (string)reader["Code"]
                        );

                        // Lire la note comme int?
                        int? note = null;
                        if (reader["Note"] != DBNull.Value)
                        {
                            decimal noteDecimal = (decimal)reader["Note"];
                            note = (int)Math.Round(noteDecimal, 0);
                        }

                        return new Inscription(
                            etudiant,
                            cours,
                            (string)reader["Session"],
                            note
                        );
                    }
                }
            }

            return null;
        }

        public void ModifierNote(int etudiantId, int coursId, string session, decimal? nouvelleNote)
        {
            var requete = @"UPDATE Inscriptions 
                           SET Note = @Note 
                           WHERE EtudiantId = @EtudiantId 
                           AND CoursId = @CoursId 
                           AND Session = @Session";

            using (var cmd = new SqlCommand(requete, connection))
            {
                cmd.Parameters.AddWithValue("@EtudiantId", etudiantId);
                cmd.Parameters.AddWithValue("@CoursId", coursId);
                cmd.Parameters.AddWithValue("@Session", session ?? throw new ArgumentNullException(nameof(session)));

                if (nouvelleNote.HasValue)
                    cmd.Parameters.AddWithValue("@Note", nouvelleNote.Value);
                else
                    cmd.Parameters.AddWithValue("@Note", DBNull.Value);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("Inscription non trouvée");
                }
            }
        }

        public bool ExisteInscription(int etudiantId, int coursId, string session)
        {
            var requete = @"SELECT COUNT(*) 
                           FROM Inscriptions 
                           WHERE EtudiantId = @EtudiantId 
                           AND CoursId = @CoursId 
                           AND Session = @Session";

            using (var cmd = new SqlCommand(requete, connection))
            {
                cmd.Parameters.AddWithValue("@EtudiantId", etudiantId);
                cmd.Parameters.AddWithValue("@CoursId", coursId);
                cmd.Parameters.AddWithValue("@Session", session ?? throw new ArgumentNullException(nameof(session)));

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
    }
}