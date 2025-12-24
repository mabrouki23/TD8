-- Script de création de la base de données et des tables
-- Base de données: gestion_scolaire

-- Vérifier si la base de données existe déjà
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'gestion_scolaire')
BEGIN
    -- Créer la base de données
    CREATE DATABASE gestion_scolaire;
    PRINT 'Base de données gestion_scolaire créée avec succès.';
END
ELSE
BEGIN
    PRINT 'Base de données gestion_scolaire existe déjà.';
END
GO

-- Utiliser la base de données
USE gestion_scolaire;
GO

-- Créer la table Etudiants
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Etudiants' AND type = 'U')
BEGIN
    CREATE TABLE Etudiants (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Nom NVARCHAR(50) NOT NULL,
        Prenom NVARCHAR(50) NOT NULL,
        DateCreation DATETIME DEFAULT GETDATE()
    );
    PRINT 'Table Etudiants créée avec succès.';
    
    -- Créer un index sur le nom pour améliorer les recherches
    CREATE INDEX IX_Etudiants_Nom ON Etudiants(Nom);
    CREATE INDEX IX_Etudiants_Prenom ON Etudiants(Prenom);
END
ELSE
BEGIN
    PRINT 'Table Etudiants existe déjà.';
END
GO

-- Créer la table Cours
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Cours' AND type = 'U')
BEGIN
    CREATE TABLE Cours (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Titre NVARCHAR(100) NOT NULL,
        Code NVARCHAR(10) NOT NULL UNIQUE,
        DateCreation DATETIME DEFAULT GETDATE()
    );
    PRINT 'Table Cours créée avec succès.';
    
    -- Créer un index sur le code pour améliorer les recherches
    CREATE INDEX IX_Cours_Code ON Cours(Code);
END
ELSE
BEGIN
    PRINT 'Table Cours existe déjà.';
END
GO

-- Créer la table Inscriptions
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Inscriptions' AND type = 'U')
BEGIN
    CREATE TABLE Inscriptions (
        EtudiantId INT NOT NULL,
        CoursId INT NOT NULL,
        Session NVARCHAR(10) NOT NULL,
        Note DECIMAL(4,2) NULL, -- Note sur 100, peut être NULL
        DateInscription DATETIME DEFAULT GETDATE(),
        
        -- Clé primaire composite
        PRIMARY KEY (EtudiantId, CoursId, Session),
        
        -- Clés étrangères
        FOREIGN KEY (EtudiantId) REFERENCES Etudiants(Id) 
            ON DELETE CASCADE,
        FOREIGN KEY (CoursId) REFERENCES Cours(Id) 
            ON DELETE CASCADE,
        
        -- Contrainte pour la note (entre 0 et 100)
        CONSTRAINT CHK_Inscriptions_Note CHECK (Note IS NULL OR (Note >= 0 AND Note <= 100))
    );
    PRINT 'Table Inscriptions créée avec succès.';
    
    -- Créer des index pour améliorer les performances des jointures
    CREATE INDEX IX_Inscriptions_EtudiantId ON Inscriptions(EtudiantId);
    CREATE INDEX IX_Inscriptions_CoursId ON Inscriptions(CoursId);
    CREATE INDEX IX_Inscriptions_Session ON Inscriptions(Session);
END
ELSE
BEGIN
    PRINT 'Table Inscriptions existe déjà.';
END
GO

-- Créer une vue pour les statistiques
IF NOT EXISTS (SELECT * FROM sys.views WHERE name = 'Vue_Statistiques')
BEGIN
    EXEC('
    CREATE VIEW Vue_Statistiques AS
    SELECT 
        COUNT(DISTINCT e.Id) AS NombreEtudiants,
        COUNT(DISTINCT c.Id) AS NombreCours,
        COUNT(DISTINCT i.EtudiantId) AS EtudiantsInscrits,
        COUNT(DISTINCT i.CoursId) AS CoursAvecInscriptions,
        COUNT(i.EtudiantId) AS TotalInscriptions,
        AVG(CASE WHEN i.Note IS NOT NULL THEN i.Note END) AS MoyenneGenerale
    FROM Etudiants e
    CROSS JOIN Cours c
    LEFT JOIN Inscriptions i ON e.Id = i.EtudiantId AND c.Id = i.CoursId
    ');
    PRINT 'Vue Vue_Statistiques créée avec succès.';
END
ELSE
BEGIN
    PRINT 'Vue Vue_Statistiques existe déjà.';
END
GO

-- Créer une procédure stockée pour ajouter un étudiant
IF NOT EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_AjouterEtudiant')
BEGIN
    EXEC('
    CREATE PROCEDURE sp_AjouterEtudiant
        @Nom NVARCHAR(50),
        @Prenom NVARCHAR(50)
    AS
    BEGIN
        INSERT INTO Etudiants (Nom, Prenom)
        VALUES (@Nom, @Prenom);
        
        SELECT SCOPE_IDENTITY() AS NouvelId;
    END
    ');
    PRINT 'Procédure sp_AjouterEtudiant créée avec succès.';
END
ELSE
BEGIN
    PRINT 'Procédure sp_AjouterEtudiant existe déjà.';
END
GO

-- Créer une procédure stockée pour ajouter un cours
IF NOT EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_AjouterCours')
BEGIN
    EXEC('
    CREATE PROCEDURE sp_AjouterCours
        @Titre NVARCHAR(100),
        @Code NVARCHAR(10)
    AS
    BEGIN
        INSERT INTO Cours (Titre, Code)
        VALUES (@Titre, @Code);
        
        SELECT SCOPE_IDENTITY() AS NouvelId;
    END
    ');
    PRINT 'Procédure sp_AjouterCours créée avec succès.';
END
ELSE
BEGIN
    PRINT 'Procédure sp_AjouterCours existe déjà.';
END
GO

-- Créer une procédure stockée pour inscrire un étudiant à un cours
IF NOT EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_InscrireEtudiant')
BEGIN
    EXEC('
    CREATE PROCEDURE sp_InscrireEtudiant
        @EtudiantId INT,
        @CoursId INT,
        @Session NVARCHAR(10),
        @Note DECIMAL(4,2) = NULL
    AS
    BEGIN
        -- Vérifier si l''inscription existe déjà
        IF EXISTS (SELECT 1 FROM Inscriptions 
                  WHERE EtudiantId = @EtudiantId 
                  AND CoursId = @CoursId 
                  AND Session = @Session)
        BEGIN
            RAISERROR(''L''étudiant est déjà inscrit à ce cours pour cette session.'', 16, 1);
            RETURN;
        END
        
        -- Vérifier si l''étudiant existe
        IF NOT EXISTS (SELECT 1 FROM Etudiants WHERE Id = @EtudiantId)
        BEGIN
            RAISERROR(''L''étudiant n''existe pas.'', 16, 1);
            RETURN;
        END
        
        -- Vérifier si le cours existe
        IF NOT EXISTS (SELECT 1 FROM Cours WHERE Id = @CoursId)
        BEGIN
            RAISERROR(''Le cours n''existe pas.'', 16, 1);
            RETURN;
        END
        
        INSERT INTO Inscriptions (EtudiantId, CoursId, Session, Note)
        VALUES (@EtudiantId, @CoursId, @Session, @Note);
        
        PRINT ''Inscription ajoutée avec succès.'';
    END
    ');
    PRINT 'Procédure sp_InscrireEtudiant créée avec succès.';
END
ELSE
BEGIN
    PRINT 'Procédure sp_InscrireEtudiant existe déjà.';
END
GO

-- Insérer des données de test (optionnel)
IF NOT EXISTS (SELECT 1 FROM Etudiants)
BEGIN
    PRINT 'Insertion de données de test...';
    
    -- Insérer des étudiants
    INSERT INTO Etudiants (Nom, Prenom) VALUES
    ('Dupont', 'Jean'),
    ('Martin', 'Marie'),
    ('Bernard', 'Pierre'),
    ('Dubois', 'Sophie'),
    ('Leroy', 'Thomas');
    
    PRINT '5 étudiants insérés.';
END

IF NOT EXISTS (SELECT 1 FROM Cours)
BEGIN
    -- Insérer des cours
    INSERT INTO Cours (Titre, Code) VALUES
    ('Programmation 1', 'INF1001'),
    ('Bases de données', 'INF1002'),
    ('Algorithmique', 'INF1003'),
    ('Réseaux informatiques', 'INF1004'),
    ('Mathématiques discrètes', 'MAT1001');
    
    PRINT '5 cours insérés.';
END

IF NOT EXISTS (SELECT 1 FROM Inscriptions)
BEGIN
    -- Insérer des inscriptions
    INSERT INTO Inscriptions (EtudiantId, CoursId, Session, Note) VALUES
    (1, 1, 'H25', 85.5),
    (1, 2, 'H25', 90.0),
    (2, 1, 'H25', 78.0),
    (2, 3, 'H25', 92.5),
    (3, 2, 'H25', NULL),
    (3, 4, 'H25', 88.0),
    (4, 1, 'H25', 95.0),
    (4, 5, 'H25', 87.5),
    (5, 3, 'H25', 76.0),
    (5, 4, 'H25', 91.0);
    
    PRINT '10 inscriptions insérées.';
END
GO

-- Afficher un résumé de la base de données
PRINT '=== RÉSUMÉ DE LA BASE DE DONNÉES ===';
PRINT 'Base de données: gestion_scolaire';
PRINT 'Tables créées: Etudiants, Cours, Inscriptions';
PRINT 'Vue créée: Vue_Statistiques';
PRINT 'Procédures stockées créées: sp_AjouterEtudiant, sp_AjouterCours, sp_InscrireEtudiant';
PRINT '=====================================';
GO

-- Exemple de requête pour vérifier les données
SELECT 
    'Nombre d''étudiants: ' + CAST(COUNT(*) AS VARCHAR(10)) AS Résultat
FROM Etudiants
UNION ALL
SELECT 
    'Nombre de cours: ' + CAST(COUNT(*) AS VARCHAR(10))
FROM Cours
UNION ALL
SELECT 
    'Nombre d''inscriptions: ' + CAST(COUNT(*) AS VARCHAR(10))
FROM Inscriptions;
GO