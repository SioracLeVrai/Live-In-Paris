
DROP DATABASE IF EXISTS App;
CREATE DATABASE IF NOT EXISTS App;
--- comments ---
USE App;

CREATE TABLE Repas(
   No_Repas INT,
   Nom VARCHAR(50),
   Type_de_plat__entrée__plat__dessert_ VARCHAR(50) NOT NULL,
   Pour_n_personnes INT NOT NULL,
   Prix_par_personnes FLOAT NOT NULL,
   Régime_alimentaire VARCHAR(50),
   Nature VARCHAR(50) NOT NULL,
   No_Photo INT,
   PRIMARY KEY(No_Repas)
);

CREATE TABLE Données_Particulier(
   No_Données INT,
   Nom VARCHAR(20),
   Prénom VARCHAR(20),
   Nom_Rue VARCHAR(50),
   No_Rue INT,
   Code_Postal INT,
   Ville VARCHAR(50),
   Numéro_de_téléphone INT,
   Adresse_mail VARCHAR(50),
   Métro_le___proche VARCHAR(50),
   Mot_de_Passe VARCHAR(20),
   PRIMARY KEY(No_Données)
);

CREATE TABLE Données_entreprise(
   No_Données INT,
   Nom_Entreprise VARCHAR(50),
   Nom_Referent VARCHAR(20),
   No_Rue INT,
   Nom_Rue VARCHAR(50),
   Code_Postal INT,
   Ville VARCHAR(50),
   Métro_le___proche VARCHAR(50),
   Mot_de_Passe VARCHAR(20),
   PRIMARY KEY(No_Données)
);

CREATE TABLE Compte_Client(
   No_Compte_Client INT,
   No_Données INT,
   Type_de_Compte__parti_ou_entre_ BOOL,
   No_Données_1 INT,
   No_Données_2 INT,
   PRIMARY KEY(No_Compte_Client),
   UNIQUE(No_Données_1),
   UNIQUE(No_Données_2),
   FOREIGN KEY(No_Données_1) REFERENCES Données_entreprise(No_Données),
   FOREIGN KEY(No_Données_2) REFERENCES Données_Particulier(No_Données)
);

CREATE TABLE Compte_Cuisinier(
   No_Compte_Cuisinier INT,
   No_Données INT,
   Type_de_compte BOOL,
   Disponibilité BOOL,
   No_Données_1 INT,
   No_Données_2 INT,
   PRIMARY KEY(No_Compte_Cuisinier),
   UNIQUE(No_Données_1),
   UNIQUE(No_Données_2),
   FOREIGN KEY(No_Données_1) REFERENCES Données_entreprise(No_Données),
   FOREIGN KEY(No_Données_2) REFERENCES Données_Particulier(No_Données)
);

CREATE TABLE Element_de_commande(
   No_Element INT,
   Date_Fabrication DATE NOT NULL,
   Date_péremption DATE NOT NULL,
   Quantité INT,
   No_Repas INT NOT NULL,
   PRIMARY KEY(No_Element),
   FOREIGN KEY(No_Repas) REFERENCES Repas(No_Repas)
);

CREATE TABLE Ingrédient(
   Nom VARCHAR(50),
   Quantité VARCHAR(10),
   No_Repas INT NOT NULL,
   PRIMARY KEY(Nom, Quantité, No_Repas),
   FOREIGN KEY(No_Repas) REFERENCES Repas(No_Repas)
);

CREATE TABLE Commande(
   No_Achat INT,
   Date_Achat DATE,
   No_Compte_Client INT NOT NULL,
   No_Compte_Cuisinier INT NOT NULL,
   PRIMARY KEY(No_Achat),
   FOREIGN KEY(No_Compte_Client) REFERENCES Compte_Client(No_Compte_Client),
   FOREIGN KEY(No_Compte_Cuisinier) REFERENCES Compte_Cuisinier(No_Compte_Cuisinier)
);

CREATE TABLE Comporte(
   No_Achat INT,
   No_Element INT,
   PRIMARY KEY(No_Achat, No_Element),
   FOREIGN KEY(No_Achat) REFERENCES Commande(No_Achat),
   FOREIGN KEY(No_Element) REFERENCES Element_de_commande(No_Element)
);

CREATE TABLE Propose(
   No_Repas INT,
   No_Compte_Cuisinier INT,
   PRIMARY KEY(No_Repas, No_Compte_Cuisinier),
   FOREIGN KEY(No_Repas) REFERENCES Repas(No_Repas),
   FOREIGN KEY(No_Compte_Cuisinier) REFERENCES Compte_Cuisinier(No_Compte_Cuisinier)
);


--- INSERTION CLIENTS ---
INSERT INTO Compte_Client (No_Compte_Client, No_Données, Type_de_Compte__parti_ou_entre_) VALUES (1, 1, false);
INSERT INTO Données_Particulier VALUES (1, "Durand", "Medhy", "Rue Cardinet", 15, 75017, "Paris", 1234567890, "Mdurand@gmail.com", "Cardinet", null);

--- INSERTION CUISINIERS ---
INSERT INTO Compte_Cuisinier (No_Compte_Cuisinier, No_Données, Type_de_compte, Disponibilité) VALUES (1, 2, false, false);
INSERT INTO Données_Particulier VALUES (2, "Dupond", "Marie", "Rue de la République", 30, 75011, "Paris", 1234567890, "Mdupond@gmail.com", "République", null);

--- INSERTION COMMANDES j ai supposé que les deux éléments apprtenais à une même commande ---
INSERT INTO Commande (No_Achat, Date_Achat, No_Compte_Client, No_Compte_Cuisinier) VALUES (1, "2025-01-10", 1, 1);

INSERT INTO Repas(No_Repas, Nom, Prix_par_personnes, Type_de_plat__entrée__plat__dessert_, Pour_n_personnes, Nature) VALUES (1, "Raclette", 10, "Plat", 1, "Française");
INSERT INTO Ingrédient(Nom, Quantité, No_Repas) VALUES ("raclette fromage", "250g", 1);
INSERT INTO Ingrédient(Nom, Quantité, No_Repas) VALUES ("pommes_de_terre", "200g", 1);
INSERT INTO Ingrédient(Nom, Quantité, No_Repas) VALUES ("jambon", "200g", 1);
INSERT INTO Ingrédient(Nom, Quantité, No_Repas) VALUES ("cornichon", "3p", 1);
INSERT INTO Element_de_commande(No_Element, Date_Fabrication, Date_péremption, Quantité, No_Repas) VALUES (1, "2025-01-10", "2025-01-15", 6, 1);
INSERT INTO Comporte(No_Achat, No_Element) VALUES (1, 1);

INSERT INTO Repas(No_Repas, Nom, Prix_par_personnes, Type_de_plat__entrée__plat__dessert_, Pour_n_personnes, Régime_alimentaire, Nature) VALUES (2, "Salade de fruit", 5, "Dessert", 1, "Végétarien", "Française");
INSERT INTO Ingrédient(Nom, Quantité, No_Repas) VALUES ("fraise", "100g", 2);
INSERT INTO Ingrédient(Nom, Quantité, No_Repas) VALUES ("kiwi", "100g", 2);
INSERT INTO Ingrédient(Nom, Quantité, No_Repas) VALUES ("sucre", "10g", 2);
INSERT INTO Element_de_commande(No_Element, Date_Fabrication, Date_péremption, Quantité, No_Repas) VALUES (2, "2025-01-10", "2025-01-15", 6, 2);
INSERT INTO Comporte(No_Achat, No_Element) VALUES (1, 2);

--- REQUÊTE AFFICHAGE Cuisinier de la commande 1 ---
SELECT Nom, Prénom
FROM Commande
JOIN Compte_Cuisinier ON Commande.No_Compte_Cuisinier = Compte_Cuisinier.No_Compte_Cuisinier
JOIN Données_Particulier ON Compte_Cuisinier.No_Données = Données_Particulier.No_Données
WHERE Commande.No_Achat = 1;


--- REQUÊTE AFFICHAGE contenu de la commande passée par le client Durand ---
SELECT Repas.Nom, Element_de_commande.Quantité
FROM Données_Particulier 
JOIN Compte_Client ON Données_Particulier.No_Données = Compte_Client.No_Données
JOIN Commande ON Compte_Client.No_Compte_Client = Commande.No_Compte_Client
JOIN Comporte ON Commande.No_Achat = Comporte.No_Achat
JOIN Element_de_commande ON Comporte.No_Element = Element_de_commande.No_Element
JOIN Repas ON Element_de_commande.No_Repas = Repas.No_Repas
WHERE Données_Particulier.Nom = "Durand";
