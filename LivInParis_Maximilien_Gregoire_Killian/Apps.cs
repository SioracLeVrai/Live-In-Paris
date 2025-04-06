using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivInParis_Maximilien_Gregoire_Killian
{
    internal class Apps
    {
        private string sortieConsole = "Sortie console :\n";
        private MySqlConnection connexion = null;
        public bool Connecte = false;
        public int NoCompteCuisinier;
        public int NoCompteClient;
        public bool TypeDeCompte;


        public string SortieConsole
        {
            get { return sortieConsole; }
            set { sortieConsole += value; }
        }
        public MySqlConnection Connexion
        {
            get { return connexion; }
        }


        public Apps()
        {

            try
            {
                string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=LiveInParis;" +
                                         "UID=root;PASSWORD=root";

                this.connexion = new MySqlConnection(connexionString);
                this.connexion.Open();
            }
            catch (MySqlException e)
            {
                this.sortieConsole += " ErreurConnexion : " + e.ToString();
                return;
            }

        }

        #region Initialisation
        /// <summary>
        /// Crée la DATABASE puis 
        /// </summary>
        public void Initialisation()
        {
            CreationTables();
            PeuplementTables();
        }

        public void CreationTables()
        {
            string texte = null;
            string Chemin = "SQL Creation Database.txt";
            try
            {
                texte = File.ReadAllText(Chemin);
            }
            catch (FileNotFoundException e) { this.SortieConsole = "echec lors de la lecture du fichier" + e.Message; return; }


            MySqlCommand Commande = connexion.CreateCommand();

            Commande = connexion.CreateCommand();
            Commande.CommandText = texte;


            try
            {
                Commande.ExecuteNonQuery();
            }
            catch (MySqlException e) { this.SortieConsole = "Erreur Insertion : " + e.ToString(); return; }
        }

        /// <summary>
        /// Peuplement de l'ensemble des tables
        /// </summary>
        public void PeuplementTables()
        {
            PeuplementRepas();
            PeuplementDonneesParticulier();
            PeuplementDonneesEntreprise();
            PeuplementCompteClient();
            PeuplementCompteCuisinier();
            PeuplementElementDeCommande();
            PeuplementIngrédient();
            PeuplementCommande();
            PeuplementComporte();
            PeuplementPropose();
        }

        /// <summary>
        /// Peuple la table Repas à partir des données du textes
        /// </summary>
        public void PeuplementRepas()
        {
            this.SortieConsole = "\nDébut insertion table Repas.";
            //Lecture fichier et traitements données
            string[] texte = null;
            string Chemin = "Peuplement/PeuplementRepas.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { this.SortieConsole = "\nEchec lors de la lecture du fichier" + e.Message; return; }

            string[][] tab = new string[texte.Length][];
            for (int i = 0; i < texte.Length; i++)
            {
                tab[i] = texte[i].Split(",");
                for (int j = 0; j < tab[i].Length; j++)
                {
                    tab[i][j] = tab[i][j].Trim();
                }
            }

            //Insertion SQL
            string insertTable = "";
            MySqlCommand Commande = connexion.CreateCommand();
            Commande.CommandText = insertTable;

            foreach (string[] ligne in tab)
            {
                insertTable = $"INSERT INTO Repas (Nom, Type_de_plat__entrée__plat__dessert_, Pour_n_personnes, Prix_par_personnes, Régime_alimentaire, Nature, No_Photo) VALUES ({ligne[0]}, {ligne[1]}, {ligne[2]}, {ligne[3]}, {ligne[4]}, {ligne[5]}, {ligne[6]});";
                Commande = connexion.CreateCommand();
                Commande.CommandText = insertTable;

                try
                {
                    Commande.ExecuteNonQuery();
                }
                catch (MySqlException e) { this.SortieConsole = "\nErreur Insertion : " + e.ToString(); return; }
            }

            this.SortieConsole = "\n Insertion réussie.";
        }

        /// <summary>
        /// Peuple la table Données_Particulier à partir des données du textes
        /// </summary>
        public void PeuplementDonneesParticulier()
        {

            this.SortieConsole = "\nDébut insertion table Données_Particulier.";
            //Lecture fichier et traitements données
            string[] texte = null;
            string Chemin = "Peuplement/PeuplementDonneesParticulier.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { this.SortieConsole = "\nEchec lors de la lecture du fichier" + e.Message; return; }

            string[][] tab = new string[texte.Length][];
            for (int i = 0; i < texte.Length; i++)
            {
                tab[i] = texte[i].Split(",");
                for (int j = 0; j < tab[i].Length; j++)
                {
                    tab[i][j] = tab[i][j].Trim();
                }


            }
            //Insertion SQL
            string insertTable = "";
            MySqlCommand Commande = connexion.CreateCommand();
            Commande.CommandText = insertTable;

            foreach (string[] ligne in tab)
            {
                insertTable = $"INSERT INTO Données_Particulier (Nom, Prénom, Nom_Rue, No_Rue, Code_Postal, Ville, Numéro_de_téléphone, Adresse_mail, Métro_le___proche, Mot_de_Passe) " +
                              $"VALUES ({ligne[0]}, {ligne[1]}, {ligne[2]}, {ligne[3]}, {ligne[4]}, {ligne[5]}, {ligne[6]}, {ligne[7]}, {ligne[8]}, {ligne[9]});";
                Commande = connexion.CreateCommand();
                Commande.CommandText = insertTable;

                try
                {
                    Commande.ExecuteNonQuery();
                }
                catch (MySqlException e) { this.SortieConsole = "\nErreur Insertion : " + e.ToString(); return; }
            }


            this.SortieConsole = "\n Insertion Réussie.";
        }

        /// <summary>
        /// Peuple la table Données_entreprise à partir des données du textes
        /// </summary>
        public void PeuplementDonneesEntreprise()
        {

            this.SortieConsole = "\nDébut insertion table Données_entreprise.";
            //Lecture fichier et traitements données
            string[] texte = null;
            string Chemin = "Peuplement/PeuplementDonneesEntreprise.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { this.SortieConsole = "\nEchec lors de la lecture du fichier" + e.Message; return; }

            string[][] tab = new string[texte.Length][];
            for (int i = 0; i < texte.Length; i++)
            {
                tab[i] = texte[i].Split(",");
                for (int j = 0; j < tab[i].Length; j++)
                {
                    tab[i][j] = tab[i][j].Trim();
                }


            }
            //Insertion SQL
            string insertTable = "";
            MySqlCommand Commande = connexion.CreateCommand();
            Commande.CommandText = insertTable;

            foreach (string[] ligne in tab)
            {
                insertTable = $"INSERT INTO Données_entreprise (Nom_Entreprise, Nom_Referent, No_Rue, Nom_Rue, Code_Postal, Ville, Métro_le___proche, Mot_de_Passe) " +
                              $"VALUES ({ligne[0]}, {ligne[1]}, {ligne[2]}, {ligne[3]}, {ligne[4]}, {ligne[5]}, {ligne[6]}, {ligne[7]});";
                Commande = connexion.CreateCommand();
                Commande.CommandText = insertTable;

                try
                {
                    Commande.ExecuteNonQuery();
                }
                catch (MySqlException e) { this.SortieConsole = "\nErreur Insertion : " + e.ToString(); return; }
            }


            this.SortieConsole = "\n Insertion Réussie.";
        }


        public void PeuplementCompteClient()
        {
            // peuplement des comptes de types particuliers
            this.SortieConsole = "\nDébut insertion table CompteClient Particulier";

            try
            {
                string commande = "SELECT No_Données FROM Données_Particulier;";
                MySqlCommand codeCommande = Connexion.CreateCommand();
                codeCommande.CommandText = commande;
                List<int> valueDonneP = new List<int>();
                MySqlDataReader reader = codeCommande.ExecuteReader();
                while (reader.Read())
                {
                    valueDonneP.Add(reader.GetInt32(0));
                }
                reader.Close();

                foreach (int noDonnees in valueDonneP)
                {
                    string insertTable = $"INSERT INTO Compte_Client (Type_de_compte,No_Données) VALUES (0,{noDonnees})";
                    MySqlCommand insertCommande = Connexion.CreateCommand();
                    insertCommande.CommandText = insertTable;
                    try
                    {
                        insertCommande.ExecuteNonQuery();
                    }
                    catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }
                }
                this.SortieConsole = "\n Insertion réussie.";
            }
            catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }



            // Peuplement des comptes de type entreprise
            this.SortieConsole = "\nDébut insertion table CompteClient Entreprise";

            try
            {
                string commande = "SELECT No_Données FROM Données_Entreprise;";
                MySqlCommand codeCommande = Connexion.CreateCommand();
                codeCommande.CommandText = commande;
                List<int> valueDonneP = new List<int>();
                MySqlDataReader reader = codeCommande.ExecuteReader();
                while (reader.Read())
                {
                    valueDonneP.Add(reader.GetInt32(0));
                }
                reader.Close();

                foreach (int noDonnees in valueDonneP)
                {
                    string insertTable = $"INSERT INTO Compte_Client (Type_de_compte,No_Données) VALUES (1,{noDonnees})";
                    MySqlCommand insertCommande = Connexion.CreateCommand();
                    insertCommande.CommandText = insertTable;
                    try
                    {
                        insertCommande.ExecuteNonQuery();
                    }
                    catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }
                }
                this.SortieConsole = "\n Insertion réussie.";
            }
            catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }


        }

        public void PeuplementCompteCuisinier()
        {
            this.SortieConsole = "\nDébut insertion table CompteCuisinier";

            try
            {
                string commande = "SELECT No_Données FROM Données_Particulier;";
                MySqlCommand codeCommande = Connexion.CreateCommand();
                codeCommande.CommandText = commande;
                List<int> valueDonneP = new List<int>();
                MySqlDataReader reader = codeCommande.ExecuteReader();
                while (reader.Read())
                {
                    valueDonneP.Add(reader.GetInt32(0));
                }
                reader.Close();
                Random r = new Random();
                foreach (int noDonnees in valueDonneP)
                {
                    string insertTable = $"INSERT INTO Compte_Cuisinier (No_Données,Disponibilité) VALUES ({noDonnees},{r.Next(0, 2)})";
                    MySqlCommand insertCommande = Connexion.CreateCommand();
                    insertCommande.CommandText = insertTable;
                    try
                    {
                        insertCommande.ExecuteNonQuery();
                    }
                    catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }
                }
                this.SortieConsole = "\n Insertion réussie.";
            }
            catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }
        }

        public void PeuplementElementDeCommande()
        {
            this.SortieConsole = "\nDébut insertion table Element de commande.";
            //Lecture fichier et traitements données
            string[] texte = null;
            string Chemin = "Peuplement/PeuplementElementDeCommande.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { this.SortieConsole = "\nEchec lors de la lecture du fichier" + e.Message; return; }

            var donneesElements = new List<string[]>();
            foreach (var ligne in texte)
            {
                var colonnes = ligne.Split(',').Select(c => c.Trim().Trim('"')).ToArray();
                donneesElements.Add(colonnes);

            }

            //insertion SQL 
            try
            {
                //recupération No_repas
                string commande = "SELECT No_Repas FROM Repas;";
                MySqlCommand codeCommande = Connexion.CreateCommand();
                codeCommande.CommandText = commande;
                List<int> valueNoRepas = new List<int>();
                MySqlDataReader reader = codeCommande.ExecuteReader();
                while (reader.Read())
                {
                    valueNoRepas.Add(reader.GetInt32(0));
                }
                reader.Close();

                //Insertion

                for (int i = 0; i < donneesElements.Count; i++)
                {
                    string insertTable = $"INSERT INTO Element_de_commande (Date_Fabrication,date_péremption,Quantité,No_Repas) VALUES ('{donneesElements[i][0]}','{donneesElements[i][1]}',{donneesElements[i][2]},{valueNoRepas[i]})";
                    MySqlCommand insertCommande = Connexion.CreateCommand();
                    insertCommande.CommandText = insertTable;
                    try
                    {
                        insertCommande.ExecuteNonQuery();
                    }
                    catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }
                }
                this.SortieConsole = "\n Insertion réussie.";
            }
            catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }


        }


        public void PeuplementIngrédient()
        {
            this.SortieConsole = "\nDébut insertion table Ingrédient.";
            //Lecture fichier et traitements données
            string[] texte = null;
            string Chemin = "Peuplement/PeuplementIngredient.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { this.SortieConsole = "\nEchec lors de la lecture du fichier" + e.Message; return; }

            var donneesElements = new List<string[]>();
            foreach (var ligne in texte)
            {
                var colonnes = ligne.Split(',').Select(c => c.Trim().Trim('"')).ToArray();
                donneesElements.Add(colonnes);

            }

            //insertion SQL 
            try
            {


                //Insertion

                for (int i = 0; i < donneesElements.Count; i++)
                {
                    string insertTable = $"INSERT INTO Ingrédient (Nom,Quantité,No_repas) VALUES ('{donneesElements[i][0]}','{donneesElements[i][1]}',{donneesElements[i][2]})";
                    MySqlCommand insertCommande = Connexion.CreateCommand();
                    insertCommande.CommandText = insertTable;
                    try
                    {
                        insertCommande.ExecuteNonQuery();
                    }
                    catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }
                }
                this.SortieConsole = "\n Insertion réussie.";
            }
            catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }

        }

        public void PeuplementCommande()
        {
            this.SortieConsole = "\nDébut insertion table Commande.";
            //Lecture fichier et traitements données
            string[] texte = null;
            string Chemin = "Peuplement/PeuplementCommande.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { this.SortieConsole = "\nEchec lors de la lecture du fichier" + e.Message; return; }

            var donneesElements = new List<string[]>();
            foreach (var ligne in texte)
            {
                var colonnes = ligne.Split(',').Select(c => c.Trim().Trim('"')).ToArray();
                donneesElements.Add(colonnes);

            }

            //insertion SQL 
            try
            {


                //Insertion

                for (int i = 0; i < donneesElements.Count; i++)
                {
                    string insertTable = $"INSERT INTO Commande (Date_Achat,No_Compte_Client,No_Compte_Cuisinier) VALUES ('{donneesElements[i][0]}',{donneesElements[i][1]},{donneesElements[i][2]})";
                    MySqlCommand insertCommande = Connexion.CreateCommand();
                    insertCommande.CommandText = insertTable;
                    try
                    {
                        insertCommande.ExecuteNonQuery();
                    }
                    catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }
                }
                this.SortieConsole = "\n Insertion réussie.";
            }
            catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }
        }

        public void PeuplementComporte()
        {
            this.SortieConsole = "\nDébut insertion table Comporte.";
            //Lecture fichier et traitements données
            string[] texte = null;
            string Chemin = "Peuplement/PeuplementComporte.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { this.SortieConsole = "\nEchec lors de la lecture du fichier" + e.Message; return; }

            var donneesElements = new List<string[]>();
            foreach (var ligne in texte)
            {
                var colonnes = ligne.Split(',').Select(c => c.Trim().Trim('"')).ToArray();
                donneesElements.Add(colonnes);

            }

            //insertion SQL 
            try
            {


                //Insertion

                for (int i = 0; i < donneesElements.Count; i++)
                {
                    string insertTable = $"INSERT INTO Comporte (No_Achat,No_Element) VALUES ({donneesElements[i][0]},{donneesElements[i][1]})";
                    MySqlCommand insertCommande = Connexion.CreateCommand();
                    insertCommande.CommandText = insertTable;
                    try
                    {
                        insertCommande.ExecuteNonQuery();
                    }
                    catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }
                }
                this.SortieConsole = "\n Insertion réussie.";
            }
            catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }
        }

        public void PeuplementPropose()
        {
            this.SortieConsole = "\nDébut insertion table Propose.";
            //Lecture fichier et traitements données
            string[] texte = null;
            string Chemin = "Peuplement/PeuplementPropose.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { this.SortieConsole = "\nEchec lors de la lecture du fichier" + e.Message; return; }

            var donneesElements = new List<string[]>();
            foreach (var ligne in texte)
            {
                var colonnes = ligne.Split(',').Select(c => c.Trim().Trim('"')).ToArray();
                donneesElements.Add(colonnes);

            }

            //insertion SQL 
            try
            {


                //Insertion

                for (int i = 0; i < donneesElements.Count; i++)
                {
                    string insertTable = $"INSERT INTO Propose (No_Repas,No_Compte_Cuisinier) VALUES ({donneesElements[i][0]},{donneesElements[i][1]})";
                    MySqlCommand insertCommande = Connexion.CreateCommand();
                    insertCommande.CommandText = insertTable;
                    try
                    {
                        insertCommande.ExecuteNonQuery();
                    }
                    catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }
                }
                this.SortieConsole = "\n Insertion réussie.";
            }
            catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }
        }
        #endregion

        /// <summary>
        /// Permet de se connecter à partir de l'identifiant, du mot de passe et du tyep de compte.
        /// </summary>
        /// <param name="identifiant"></param>
        /// <param name="motDePasse"></param>
        /// <param name="typeDeCompte"></param>
        /// <returns></returns>
        public bool SeConnecter(string identifiant, string motDePasse, bool typeDeCompte)
        {
            string commande;
            string sortie = "";
            switch (typeDeCompte) // Correction : utiliser typeDeCompte au lieu de TypeDeCompte
            {
                case false:
                    commande = $"SELECT No_Données FROM Données_Particulier WHERE Mot_de_Passe=\"{motDePasse}\" AND Adresse_mail=\"{identifiant}\";";
                    break;
                case true:
                    commande = $"SELECT No_Données FROM Données_Entreprise WHERE Mot_de_Passe=\"{motDePasse}\" AND Nom_Entreprise=\"{identifiant}\";";
                    break;
                default:
                    return false;
            }
            MySqlCommand codeCommande = connexion.CreateCommand();
            codeCommande.CommandText = commande;
            MySqlDataReader reader = codeCommande.ExecuteReader();
            string[] valueString = new string[reader.FieldCount];
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valueString[i] = reader.GetValue(i).ToString();
                    sortie = valueString[i];
                }
            }
            reader.Close();
            if (string.IsNullOrEmpty(sortie))
            {
                return false;
            }

            TypeDeCompte = typeDeCompte;
            Connecte = true;

            commande = $"SELECT No_Compte_Client FROM Compte_Client WHERE No_Données=\"{sortie}\" AND Type_de_compte=\"{typeDeCompte}\";";
            codeCommande = connexion.CreateCommand();
            codeCommande.CommandText = commande;
            reader = codeCommande.ExecuteReader();
            valueString = new string[reader.FieldCount];
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valueString[i] = reader.GetValue(i).ToString();
                    sortie = valueString[i];
                }
            }
            reader.Close();
            NoCompteClient = Convert.ToInt32(sortie);

            commande = $"SELECT No_Compte_Cuisinier FROM Compte_Cuisinier WHERE No_Données=\"{sortie}\" AND Type_de_compte=\"{typeDeCompte}\";";
            codeCommande = connexion.CreateCommand();
            codeCommande.CommandText = commande;
            reader = codeCommande.ExecuteReader();
            valueString = new string[reader.FieldCount];
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valueString[i] = reader.GetValue(i).ToString();
                    sortie = valueString[i];
                }
            }
            reader.Close();
            NoCompteCuisinier = string.IsNullOrEmpty(sortie) ? 0 : Convert.ToInt32(sortie); // Gérer le cas où il n'y a pas de compte cuisinier

            codeCommande.Dispose();
            return true;
        }

        /// <summary>
        /// Crée un nouveau compte particulier et l'ajoute à la base de données.
        /// </summary>
        /// <param name="nom">Nom du particulier</param>
        /// <param name="prenom">Prénom du particulier</param>
        /// <param name="nomRue">Nom de la rue</param>
        /// <param name="noRue">Numéro de la rue</param>
        /// <param name="codePostal">Code postal</param>
        /// <param name="ville">Ville</param>
        /// <param name="telephone">Numéro de téléphone</param>
        /// <param name="email">Adresse email</param>
        /// <param name="metroProche">Station de métro la plus proche</param>
        /// <param name="motDePasse">Mot de passe</param>
        /// <param name="estCuisinier">Indique si le compte est aussi un cuisinier</param>
        /// <returns>Message de confirmation ou d'erreur</returns>
        public string NouveauCompteParticulier(string nom, string prenom, string nomRue, string noRue, string codePostal,
            string ville, string telephone, string email, string metroProche, string motDePasse, bool estCuisinier)
        {
            try
            {
                // Insertion dans Données_Particulier
                string insertParticulier = "INSERT INTO Données_Particulier (Nom, Prénom, Nom_Rue, No_Rue, Code_Postal, Ville, " +
                                           "Numéro_de_téléphone, Adresse_mail, Métro_le___proche, Mot_de_Passe) " +
                                           "VALUES (@nom, @prenom, @nomRue, @noRue, @codePostal, @ville, @telephone, @email, @metroProche, @motDePasse);";

                MySqlCommand commande = new MySqlCommand(insertParticulier, connexion);
                commande.Parameters.AddWithValue("@nom", nom);
                commande.Parameters.AddWithValue("@prenom", prenom);
                commande.Parameters.AddWithValue("@nomRue", nomRue);
                commande.Parameters.AddWithValue("@noRue", noRue);
                commande.Parameters.AddWithValue("@codePostal", codePostal);
                commande.Parameters.AddWithValue("@ville", ville);
                commande.Parameters.AddWithValue("@telephone", telephone);
                commande.Parameters.AddWithValue("@email", email);
                commande.Parameters.AddWithValue("@metroProche", metroProche);
                commande.Parameters.AddWithValue("@motDePasse", motDePasse);

                commande.ExecuteNonQuery();

                // Récupérer le No_Données généré
                string getNoDonnees = "SELECT LAST_INSERT_ID();";
                commande.CommandText = getNoDonnees;
                int noDonnees = Convert.ToInt32(commande.ExecuteScalar());

                // Insertion dans Compte_Client
                string insertClient = "INSERT INTO Compte_Client (No_Données, Type_de_compte) VALUES (@noDonnees, 0);";
                commande.CommandText = insertClient;
                commande.Parameters.Clear();
                commande.Parameters.AddWithValue("@noDonnees", noDonnees);
                commande.ExecuteNonQuery();

                // Récupérer le No_Compte_Client
                string getNoClient = "SELECT LAST_INSERT_ID();";
                commande.CommandText = getNoClient;
                int noCompteClient = Convert.ToInt32(commande.ExecuteScalar());

                // Si c'est aussi un cuisinier, insertion dans Compte_Cuisinier
                int noCompteCuisinier = 0;
                if (estCuisinier)
                {
                    string insertCuisinier = "INSERT INTO Compte_Cuisinier (No_Données, Type_de_compte) VALUES (@noDonnees, 0);";
                    commande.CommandText = insertCuisinier;
                    commande.ExecuteNonQuery();

                    string getNoCuisinier = "SELECT LAST_INSERT_ID();";
                    commande.CommandText = getNoCuisinier;
                    noCompteCuisinier = Convert.ToInt32(commande.ExecuteScalar());
                }

                commande.Dispose();
                return $"Compte particulier créé avec succès ! ID Client : {noCompteClient}" +
                       (estCuisinier ? $", ID Cuisinier : {noCompteCuisinier}" : "");
            }
            catch (MySqlException e)
            {
                this.SortieConsole = "\nErreur lors de la création du compte particulier : " + e.ToString();
                return "Erreur lors de la création du compte particulier : " + e.Message;
            }
        }

        /// <summary>
        /// Crée un nouveau compte entreprise et l'ajoute à la base de données.
        /// </summary>
        /// <param name="nomEntreprise">Nom de l'entreprise</param>
        /// <param name="nomReferent">Nom du référent</param>
        /// <param name="noRue">Numéro de la rue</param>
        /// <param name="nomRue">Nom de la rue</param>
        /// <param name="codePostal">Code postal</param>
        /// <param name="ville">Ville</param>
        /// <param name="metroProche">Station de métro la plus proche</param>
        /// <param name="motDePasse">Mot de passe</param>
        /// <param name="estCuisinier">Indique si le compte est aussi un cuisinier</param>
        /// <returns>Message de confirmation ou d'erreur</returns>
        public string NouveauCompteEntreprise(string nomEntreprise, string nomReferent, string noRue, string nomRue,
            string codePostal, string ville, string metroProche, string motDePasse, bool estCuisinier)
        {
            try
            {
                // Insertion dans Données_Entreprise
                string insertEntreprise = "INSERT INTO Données_Entreprise (Nom_Entreprise, Nom_Referent, No_Rue, Nom_Rue, Code_Postal, Ville, " +
                                          "Métro_le___proche, Mot_de_Passe) " +
                                          "VALUES (@nomEntreprise, @nomReferent, @noRue, @nomRue, @codePostal, @ville, @metroProche, @motDePasse);";

                MySqlCommand commande = new MySqlCommand(insertEntreprise, connexion);
                commande.Parameters.AddWithValue("@nomEntreprise", nomEntreprise);
                commande.Parameters.AddWithValue("@nomReferent", nomReferent);
                commande.Parameters.AddWithValue("@noRue", noRue);
                commande.Parameters.AddWithValue("@nomRue", nomRue);
                commande.Parameters.AddWithValue("@codePostal", codePostal);
                commande.Parameters.AddWithValue("@ville", ville);
                commande.Parameters.AddWithValue("@metroProche", metroProche);
                commande.Parameters.AddWithValue("@motDePasse", motDePasse);

                commande.ExecuteNonQuery();

                // Récupérer le No_Données généré
                string getNoDonnees = "SELECT LAST_INSERT_ID();";
                commande.CommandText = getNoDonnees;
                int noDonnees = Convert.ToInt32(commande.ExecuteScalar());

                // Insertion dans Compte_Client
                string insertClient = "INSERT INTO Compte_Client (No_Données, Type_de_compte) VALUES (@noDonnees, 1);";
                commande.CommandText = insertClient;
                commande.Parameters.Clear();
                commande.Parameters.AddWithValue("@noDonnees", noDonnees);
                commande.ExecuteNonQuery();

                // Récupérer le No_Compte_Client
                string getNoClient = "SELECT LAST_INSERT_ID();";
                commande.CommandText = getNoClient;
                int noCompteClient = Convert.ToInt32(commande.ExecuteScalar());

                // Si c'est aussi un cuisinier, insertion dans Compte_Cuisinier
                int noCompteCuisinier = 0;
                if (estCuisinier)
                {
                    string insertCuisinier = "INSERT INTO Compte_Cuisinier (No_Données, Type_de_compte) VALUES (@noDonnees, 1);";
                    commande.CommandText = insertCuisinier;
                    commande.ExecuteNonQuery();

                    string getNoCuisinier = "SELECT LAST_INSERT_ID();";
                    commande.CommandText = getNoCuisinier;
                    noCompteCuisinier = Convert.ToInt32(commande.ExecuteScalar());
                }

                commande.Dispose();
                return $"Compte entreprise créé avec succès ! ID Client : {noCompteClient}" +
                       (estCuisinier ? $", ID Cuisinier : {noCompteCuisinier}" : "");
            }
            catch (MySqlException e)
            {
                this.SortieConsole = "\nErreur lors de la création du compte entreprise : " + e.ToString();
                return "Erreur lors de la création du compte entreprise : " + e.Message;
            }
        }


        /// <summary>
        /// Affiche la liste des cuisiniers disponibles pour un client connecté.
        /// </summary>
        public string AfficherCuisiniersDisponibles()
        {
            if (!Connecte)
            {
                return "Erreur : Vous devez être connecté pour voir les cuisiniers disponibles.";
            }

            StringBuilder result = new StringBuilder("Cuisiniers disponibles :\n");
            string commande = "SELECT DISTINCT dp.Nom, dp.Prénom, dp.Métro_le___proche " +
                              "FROM Compte_Cuisinier cc " +
                              "LEFT JOIN Données_Particulier dp ON cc.No_Données = dp.No_Données AND cc.Type_de_compte = 0 " +
                              "LEFT JOIN Données_Entreprise de ON cc.No_Données = de.No_Données AND cc.Type_de_compte = 1 " +
                              "WHERE cc.No_Compte_Cuisinier IS NOT NULL;";

            try
            {
                MySqlCommand codeCommande = connexion.CreateCommand();
                codeCommande.CommandText = commande;
                MySqlDataReader reader = codeCommande.ExecuteReader();

                while (reader.Read())
                {
                    string noCompteCuisinier = reader["No_Compte_Cuisinier"].ToString();
                    string nom = reader["Nom"]?.ToString() ?? reader["Nom_Entreprise"]?.ToString() ?? "N/A";
                    string prenom = reader["Prénom"]?.ToString() ?? "";
                    string metro = reader["Métro_le___proche"]?.ToString() ?? "Non spécifié";

                    result.AppendLine($"ID Cuisinier : {noCompteCuisinier}, Nom : {nom} {prenom}, Métro : {metro}");
                }

                reader.Close();
                codeCommande.Dispose();

                return result.Length > "Cuisiniers disponibles :\n".Length ? result.ToString() : "Aucun cuisinier disponible.";
            }
            catch (MySqlException e)
            {
                this.SortieConsole = "\nErreur lors de l'affichage des cuisiniers : " + e.ToString();
                return "Erreur lors de la récupération des cuisiniers disponibles.";
            }
        }

        /// <summary>
        /// Affiche les commandes passées par le client connecté.
        /// </summary>
        public string AfficherCommandesClients()
        {
            if (!Connecte || NoCompteClient == 0)
            {
                return "Erreur : Vous devez être connecté avec un compte client valide pour voir vos commandes.";
            }

            StringBuilder result = new StringBuilder($"Commandes du client (ID : {NoCompteClient}) :\n");
            string commande = "SELECT c.No_Achat, c.Date_Achat, r.Nom, ed.Quantité, r.Prix_par_personnes " +
                             "FROM Commande c " +
                             "JOIN Comporte cp ON c.No_Achat = cp.No_Achat " +
                             "JOIN Element_de_commande ed ON cp.No_Element = ed.No_Element " +
                             "JOIN Repas r ON ed.No_Repas = r.No_Repas " +
                             $"WHERE c.No_Compte_Client = {NoCompteClient};";

            try
            {
                MySqlCommand codeCommande = connexion.CreateCommand();
                codeCommande.CommandText = commande;
                MySqlDataReader reader = codeCommande.ExecuteReader();

                while (reader.Read())
                {
                    result.AppendLine($"Commande #{reader[0]} - Repas : {reader[1]}, Date : {reader[2]}, Quantité : {reader[3]}, Total : {Convert.ToInt32(reader[3]) * Convert.ToDecimal(reader[4])}€");
                }

                reader.Close();
                codeCommande.Dispose();

                return result.Length > $"Commandes du client (ID : {NoCompteClient}) :\n".Length ? result.ToString() : "Aucune commande trouvée.";
            }
            catch (MySqlException e)
            {
                this.SortieConsole = "\nErreur lors de l'affichage des commandes clients : " + e.ToString();
                return "Erreur lors de la récupération des commandes : " + e.Message;
            }
        }

        /// <summary>
        /// Affiche les commandes associées au cuisinier connecté.
        /// </summary>
        public string AfficherCommandesCuisinier()
        {
            if (!Connecte || NoCompteCuisinier == 0)
            {
                return "Erreur : Vous devez être connecté avec un compte cuisinier valide pour voir vos commandes.";
            }

            StringBuilder result = new StringBuilder($"Commandes du cuisinier (ID : {NoCompteCuisinier}) :\n");
            string commande = "SELECT c.No_Commande, r.Nom, c.Date_commande, c.Quantité, r.Prix_par_personnes, cc.No_Compte_Client " +
                              "FROM Commandes c " +
                              "JOIN Repas r ON c.No_Repas = r.No_Repas " +
                              $"WHERE c.No_Compte_Cuisinier = {NoCompteCuisinier};";

            try
            {
                MySqlCommand codeCommande = connexion.CreateCommand();
                codeCommande.CommandText = commande;
                MySqlDataReader reader = codeCommande.ExecuteReader();

                while (reader.Read())
                {
                    string noCommande = reader["No_Commande"].ToString();
                    string nomRepas = reader["Nom"].ToString();
                    string dateCommande = reader["Date_commande"].ToString();
                    int quantite = Convert.ToInt32(reader["Quantité"]);
                    decimal prixParPersonne = Convert.ToDecimal(reader["Prix_par_personnes"]);
                    string noCompteClient = reader["No_Compte_Client"].ToString();
                    decimal total = quantite * prixParPersonne;

                    result.AppendLine($"Commande #{noCommande} - Repas : {nomRepas}, Client ID : {noCompteClient}, Date : {dateCommande}, Quantité : {quantite}, Total : {total}€");
                }

                reader.Close();
                codeCommande.Dispose();

                return result.Length > $"Commandes du cuisinier (ID : {NoCompteCuisinier}) :\n".Length ? result.ToString() : "Aucune commande trouvée.";
            }
            catch (MySqlException e)
            {
                this.SortieConsole = "\nErreur lors de l'affichage des commandes cuisinier : " + e.ToString();
                return "Erreur lors de la récupération des commandes.";
            }
        }

        /// <summary>
        /// Affiche la liste des clients ayant passé affaire avec le cuisinier
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// Affiche la liste des clients ayant passé affaire avec le cuisinier connecté.
        /// </summary>
        /// <returns>Une chaîne contenant la liste des clients ou un message d'erreur/absence.</returns>
        public string AfficherClients()
        {
            if (!Connecte || NoCompteCuisinier == 0)
            {
                return "Erreur : Vous devez être connecté avec un compte cuisinier valide pour voir vos clients.";
            }

            StringBuilder result = new StringBuilder($"Clients ayant commandé auprès du cuisinier (ID : {NoCompteCuisinier}) :\n");
            string commande = "SELECT DISTINCT cc.No_Compte_Client, dp.Nom, dp.Prénom, dp.Adresse_mail, de.Nom_Entreprise " +
                              "FROM Commandes c " +
                              "JOIN Compte_Client cc ON c.No_Compte_Client = cc.No_Compte_Client " +
                              "LEFT JOIN Données_Particulier dp ON cc.No_Données = dp.No_Données AND cc.Type_de_compte = 0 " +
                              "LEFT JOIN Données_Entreprise de ON cc.No_Données = de.No_Données AND cc.Type_de_compte = 1 " +
                              $"WHERE c.No_Compte_Cuisinier = {NoCompteCuisinier};";

            try
            {
                MySqlCommand codeCommande = connexion.CreateCommand();
                codeCommande.CommandText = commande;
                MySqlDataReader reader = codeCommande.ExecuteReader();

                while (reader.Read())
                {
                    string noCompteClient = reader["No_Compte_Client"].ToString();
                    string nom = reader["Nom"]?.ToString() ?? reader["Nom_Entreprise"]?.ToString() ?? "N/A";
                    string prenom = reader["Prénom"]?.ToString() ?? ""; // Prénom est null pour les entreprises
                    string email = reader["Adresse_mail"]?.ToString() ?? "Non spécifié";

                    result.AppendLine($"ID Client : {noCompteClient}, Nom : {nom} {prenom}, Email : {email}");
                }

                reader.Close();
                codeCommande.Dispose();

                return result.Length > $"Clients ayant commandé auprès du cuisinier (ID : {NoCompteCuisinier}) :\n".Length
                    ? result.ToString()
                    : "Aucun client n'a passé de commande avec vous.";
            }
            catch (MySqlException e)
            {
                this.SortieConsole = "\nErreur lors de l'affichage des clients : " + e.ToString();
                return "Erreur lors de la récupération des clients.";
            }
        }
    }
}

