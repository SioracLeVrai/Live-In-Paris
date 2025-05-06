using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;

namespace PSI_Maximilien_Gregoire_Killian
{
    public class BaseDeDonnees
    {
        private MySqlConnection connexion = null;
        public bool Connecte = false;
        public int NoCompteCuisinier;
        public int NoCompteClient;
        public bool TypeDeCompte;

        public MySqlConnection Connexion
        {
            get { return connexion; }
        }

        public BaseDeDonnees()
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
                MessageBox.Show(" ErreurConnexion : " + e.ToString());
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
            catch (FileNotFoundException e) { MessageBox.Show("echec lors de la lecture du fichier" + e.Message); return; }


            MySqlCommand Commande = connexion.CreateCommand();

            Commande = connexion.CreateCommand();
            Commande.CommandText = texte;


            try
            {
                Commande.ExecuteNonQuery();
            }
            catch (MySqlException e) { MessageBox.Show("Erreur Insertion : " + e.ToString()); return; }
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
            //Lecture fichier et traitements données
            string[] texte = null;
            string Chemin = "Peuplement/PeuplementRepas.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { MessageBox.Show("\nEchec lors de la lecture du fichier" + e.Message); return; }

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
                catch (MySqlException e) { MessageBox.Show("\nErreur Insertion : " + e.ToString()); return; }
            }

        }

        /// <summary>
        /// Peuple la table Données_Particulier à partir des données du textes
        /// </summary>
        public void PeuplementDonneesParticulier()
        {
            //Lecture fichier et traitements données
            string[] texte = null;
            string Chemin = "Peuplement/PeuplementDonneesParticulier.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { MessageBox.Show("\nEchec lors de la lecture du fichier" + e.Message); return; }

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
                catch (MySqlException e) { MessageBox.Show("\nErreur Insertion : " + e.ToString()); return; }
            }


        }

        /// <summary>
        /// Peuple la table Données_entreprise à partir des données du textes
        /// </summary>
        public void PeuplementDonneesEntreprise()
        {

            //Lecture fichier et traitements données
            string[] texte = null;
            string Chemin = "Peuplement/PeuplementDonneesEntreprise.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { MessageBox.Show("\nEchec lors de la lecture du fichier" + e.Message); return; }

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
                catch (MySqlException e) { MessageBox.Show("\nErreur Insertion : " + e.ToString()); return; }
            }


        }


        public void PeuplementCompteClient()
        {
            // peuplement des comptes de types particuliers

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
            }
            catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }



            // Peuplement des comptes de type entreprise

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
            }
            catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }


        }

        public void PeuplementCompteCuisinier()
        {
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
                    string insertTable = $"INSERT INTO Compte_Cuisinier (Type_de_compte,No_Données,Disponibilité) VALUES (0,{noDonnees},{r.Next(0, 2)})";
                    MySqlCommand insertCommande = Connexion.CreateCommand();
                    insertCommande.CommandText = insertTable;
                    try
                    {
                        insertCommande.ExecuteNonQuery();
                    }
                    catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }
                }
            }
            catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }

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

                Random r = new Random();
                foreach (int noDonnees in valueDonneP)
                {
                    string insertTable = $"INSERT INTO Compte_Cuisinier (Type_de_compteNo_Données,Disponibilité) VALUES (1,{noDonnees},{r.Next(0, 2)})";
                    MySqlCommand insertCommande = Connexion.CreateCommand();
                    insertCommande.CommandText = insertTable;
                    try
                    {
                        insertCommande.ExecuteNonQuery();
                    }
                    catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }
                }
            }
            catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }
        }

        public void PeuplementElementDeCommande()
        {
            //Lecture fichier et traitements données
            string[] texte = null;
            string Chemin = "Peuplement/PeuplementElementDeCommande.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { MessageBox.Show("\nEchec lors de la lecture du fichier" + e.Message); return; }

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
                    catch (MySqlException e) { MessageBox.Show("\nErreur Insertion : " + e.ToString()); return; }
                }
            }
            catch (MySqlException e) { MessageBox.Show("\nErreur Insertion : " + e.ToString()); return; }


        }


        public void PeuplementIngrédient()
        {
            //Lecture fichier et traitements données
            string[] texte = null;
            string Chemin = "Peuplement/PeuplementIngredient.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { MessageBox.Show("\nEchec lors de la lecture du fichier" + e.Message); return; }

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
            }
            catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }

        }

        public void PeuplementCommande()
        {
            //Lecture fichier et traitements données
            string[] texte = null;
            string Chemin = "Peuplement/PeuplementCommande.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { MessageBox.Show("\nEchec lors de la lecture du fichier" + e.Message); return; }

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
                    string insertTable = $"INSERT INTO Commande (Date_Achat,Terminé,No_Compte_Client,No_Compte_Cuisinier) VALUES ('{donneesElements[i][0]}',{donneesElements[i][1]},{donneesElements[i][2]},{donneesElements[i][3]})";
                    MySqlCommand insertCommande = Connexion.CreateCommand();
                    insertCommande.CommandText = insertTable;
                    try
                    {
                        insertCommande.ExecuteNonQuery();
                    }
                    catch (MySqlException e) { MessageBox.Show("\nErreur Insertion : " + e.ToString()); return; }
                }
            }
            catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }
        }

        public void PeuplementComporte()
        {
            //Lecture fichier et traitements données
            string[] texte = null;
            string Chemin = "Peuplement/PeuplementComporte.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { MessageBox.Show("\nEchec lors de la lecture du fichier" + e.Message); return; }

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
            }
            catch (MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString()); return; }
        }

        public void PeuplementPropose()
        {
            //Lecture fichier et traitements données
            string[] texte = null;
            string Chemin = "Peuplement/PeuplementPropose.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { MessageBox.Show("\nEchec lors de la lecture du fichier" + e.Message); return; }

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
                    catch (MySqlException e) { MessageBox.Show("\nErreur Insertion : " + e.ToString()); return; }
                }
            }
            catch (MySqlException e) { MessageBox.Show("\nErreur Insertion : " + e.ToString()); return; }
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
            if (!typeDeCompte) // Particulier
            {
                commande = "SELECT No_Données FROM Données_Particulier WHERE Mot_de_Passe = @motDePasse AND Adresse_mail = @identifiant";
            }
            else // Entreprise
            {
                commande = "SELECT No_Données FROM Données_Entreprise WHERE Mot_de_Passe = @motDePasse AND Nom_Entreprise = @identifiant";
            }

            int noDonnees = 0;
            try
            {
                MySqlCommand codeCommande = connexion.CreateCommand();
                codeCommande.CommandText = commande;
                codeCommande.Parameters.AddWithValue("@motDePasse", motDePasse);
                codeCommande.Parameters.AddWithValue("@identifiant", identifiant);
                MySqlDataReader reader = codeCommande.ExecuteReader();
                if (reader.Read())
                {
                    noDonnees = reader.GetInt32("No_Données");
                }
                reader.Close();
                codeCommande.Dispose();

                if (noDonnees == 0)
                {
                    return false;
                }
            }
            catch (MySqlException e)
            {
                MessageBox.Show("\nErreur lors de la vérification des identifiants : " + e.ToString());
                return false;
            }

            // Étape 2 : Récupérer No_Compte_Client
            try
            {
                MySqlCommand codeCommande = connexion.CreateCommand();
                commande = "SELECT No_Compte_Client FROM Compte_Client WHERE No_Données = @noDonnees AND Type_de_compte = @typeDeCompte";
                codeCommande.CommandText = commande;
                codeCommande.Parameters.AddWithValue("@noDonnees", noDonnees);
                codeCommande.Parameters.AddWithValue("@typeDeCompte", typeDeCompte ? 1 : 0);
                MySqlDataReader reader = codeCommande.ExecuteReader();
                if (reader.Read())
                {
                    NoCompteClient = reader.GetInt32("No_Compte_Client");
                }
                reader.Close();
                codeCommande.Dispose();

                if (NoCompteClient == 0)
                {
                    return false;
                }
            }
            catch (MySqlException e)
            {
                MessageBox.Show("\nErreur lors de la récupération de No_Compte_Client : " + e.ToString());
                return false;
            }

            // Étape 3 : Récupérer No_Compte_Cuisinier (peut être absent)
            try
            {
                MySqlCommand codeCommande = connexion.CreateCommand();
                commande = "SELECT No_Compte_Cuisinier FROM Compte_Cuisinier WHERE No_Données = @noDonnees AND Type_de_compte = @typeDeCompte";
                codeCommande.CommandText = commande;
                codeCommande.Parameters.AddWithValue("@noDonnees", noDonnees);
                codeCommande.Parameters.AddWithValue("@typeDeCompte", typeDeCompte ? 1 : 0);
                MySqlDataReader reader = codeCommande.ExecuteReader();
                if (reader.Read())
                {
                    NoCompteCuisinier = reader.GetInt32("No_Compte_Cuisinier");
                }
                else
                {
                    NoCompteCuisinier = 0; // Pas un cuisinier, on reste à 0
                }
                reader.Close();
                codeCommande.Dispose();
            }
            catch (MySqlException e)
            {
                MessageBox.Show("\nErreur lors de la récupération de No_Compte_Cuisinier : " + e.ToString());
                NoCompteCuisinier = 0; // En cas d'erreur, on reste à 0
            }

            // Étape 4 : Mettre à jour l'état de connexion
            TypeDeCompte = typeDeCompte;
            Connecte = true;
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
        /// <returns></returns>
        public bool NouveauCompteParticulier(string nom, string prenom, string nomRue, string noRue, string codePostal, string ville, string telephone, string email, string metroProche, string motDePasse)
        {
            try
            {
                string texteCommande = "INSERT INTO Données_Particulier (Nom, Prénom, Nom_Rue, No_Rue, Code_Postal, Ville, " +
                                           "Numéro_de_téléphone, Adresse_mail, Métro_le___proche, Mot_de_Passe) " +
                                           "VALUES (@nom, @prenom, @nomRue, @noRue, @codePostal, @ville, @telephone, @email, @metroProche, @motDePasse);";

                MySqlCommand commande = new MySqlCommand(texteCommande, connexion);
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

                string insertCuisinier = "INSERT INTO Compte_Cuisinier (No_Données, Type_de_compte) VALUES (@noDonnees, 0);";
                commande.CommandText = insertCuisinier;
                commande.ExecuteNonQuery();

                string getNoCuisinier = "SELECT LAST_INSERT_ID();";
                commande.CommandText = getNoCuisinier;
                noCompteCuisinier = Convert.ToInt32(commande.ExecuteScalar());


                commande.Dispose();

                Connecte = true;
                NoCompteClient = noCompteClient;
                NoCompteCuisinier = noCompteCuisinier;
                return true;
            }
            catch (MySqlException e)
            {
                MessageBox.Show("Erreur SQL : " + e.Message);
                return false;
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
        /// <returns></returns>
        public bool NouveauCompteEntreprise(string nomEntreprise, string nomReferent, string noRue, string nomRue, string codePostal, string ville, string metroProche, string motDePasse)
        {
            try
            {
                // Insertion dans Données_Entreprise
                string texteCommande = "INSERT INTO Données_Entreprise (Nom_Entreprise, Nom_Referent, No_Rue, Nom_Rue, Code_Postal, Ville, " +
                                          "Métro_le___proche, Mot_de_Passe) " +
                                          "VALUES (@nomEntreprise, @nomReferent, @noRue, @nomRue, @codePostal, @ville, @metroProche, @motDePasse);";

                MySqlCommand commande = new MySqlCommand(texteCommande, connexion);
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

                string insertCuisinier = "INSERT INTO Compte_Cuisinier (No_Données, Type_de_compte) VALUES (@noDonnees, 1);";
                commande.CommandText = insertCuisinier;
                commande.ExecuteNonQuery();

                string getNoCuisinier = "SELECT LAST_INSERT_ID();";
                commande.CommandText = getNoCuisinier;
                noCompteCuisinier = Convert.ToInt32(commande.ExecuteScalar());

                Connecte = true;
                TypeDeCompte = true;
                NoCompteClient = noCompteClient;
                NoCompteCuisinier = noCompteCuisinier;
                return true;
            }
            catch (MySqlException e)
            {
                MessageBox.Show("Erreur SQL : " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Renvoie un string avec le nom du référent / du particulier propriétaire du compte
        /// </summary>
        /// <returns></returns>
        public string DonnerNom()
        {
            if (!Connecte)
            {
                return null;
            }

            string commande;
            if (TypeDeCompte)
            {
                commande = $"SELECT Nom_Referent FROM Données_Entreprise p LEFT JOIN Compte_Client c ON c.No_Données = p.No_Données WHERE c.No_Compte_Client = {NoCompteClient};";
            }
            else
            {
                commande = $"SELECT Nom FROM Données_Particulier p LEFT JOIN Compte_Client c ON c.No_Données = p.No_Données WHERE c.No_Compte_Client = {NoCompteClient};";
            }

            try
            {
                MySqlCommand codeCommande = connexion.CreateCommand();
                codeCommande.CommandText = commande;
                MySqlDataReader reader = codeCommande.ExecuteReader();
                string nom = null;
                if (reader.Read())
                {
                    nom = reader.GetString(0);
                }
                reader.Close();
                codeCommande.Dispose();
                return nom;
            }
            catch (MySqlException e)
            {
                MessageBox.Show("\nErreur lors de la récupération du nom : " + e.ToString());
                return null;
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
                MessageBox.Show("Erreur SQL : " + e.Message);
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
                MessageBox.Show("\nErreur lors de l'affichage des commandes clients : " + e.ToString());
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
                MessageBox.Show("\nErreur lors de l'affichage des commandes cuisinier : " + e.ToString());
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
                MessageBox.Show("\nErreur lors de l'affichage des clients : " + e.ToString());
                return "Erreur lors de la récupération des clients.";
            }
        }

        /// <summary>
        /// Enregistre la base de donnée dans un fichier "BDD_XML.xml"
        /// </summary>
        public void SauvegarderXML()
        {
            try
            {
                var xmlDoc = new System.Xml.XmlDocument();
                var root = xmlDoc.CreateElement("LiveInParis");
                xmlDoc.AppendChild(root);

                string[] tables = { "Repas", "Données_Particulier", "Données_entreprise", "Compte_Client",
                           "Compte_Cuisinier", "Element_de_commande", "Ingrédient", "Commande",
                           "Comporte", "Propose" };

                foreach (string table in tables)
                {
                    var tableElement = xmlDoc.CreateElement(table);
                    root.AppendChild(tableElement);

                    string CommandeText = $"SELECT * FROM {table}";
                    using (MySqlCommand cmd = new MySqlCommand(CommandeText, connexion))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var rowElement = xmlDoc.CreateElement("Row");
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var fieldElement = xmlDoc.CreateElement(reader.GetName(i));
                                fieldElement.InnerText = reader[i]?.ToString() ?? "";
                                rowElement.AppendChild(fieldElement);
                            }
                            tableElement.AppendChild(rowElement);
                        }
                    }
                }

                xmlDoc.Save("BDD_XML.xml");
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la sauvegarde XML : " + e.Message);
            }
        }

        /// <summary>
        /// Charge la base de donnée à partir du fichier "BDD_XML.xml"
        /// </summary>
        public void ChargerXML()
        {
            try
            {
                var xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.Load("BDD_XML.xml");

                // Désactiver temporairement les contraintes de clés étrangères
                using (MySqlCommand cmd = new MySqlCommand("SET FOREIGN_KEY_CHECKS=0;", connexion))
                {
                    cmd.ExecuteNonQuery();
                }

                // Liste des tables dans l'ordre d'insertion (pour respecter les dépendances)
                string[] tables = { "Repas", "Données_Particulier", "Données_entreprise", "Compte_Client",
                   "Compte_Cuisinier", "Element_de_commande", "Ingrédient", "Commande",
                   "Comporte", "Propose" };

                foreach (string table in tables)
                {
                    // Vider la table
                    using (MySqlCommand cmd = new MySqlCommand($"TRUNCATE TABLE {table};", connexion))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    var tableNodes = xmlDoc.SelectNodes($"//{table}/Row");
                    foreach (System.Xml.XmlNode row in tableNodes)
                    {
                        var columns = new List<string>();
                        var values = new List<string>();
                        foreach (System.Xml.XmlNode field in row.ChildNodes)
                        {
                            columns.Add(field.Name);
                            string value = field.InnerText.Replace("'", "''"); // Échapper les apostrophes

                            // Gestion des champs booléens
                            if (field.Name == "Type_de_Compte" || field.Name == "Type_de_compte" ||
                                field.Name == "Terminé" || field.Name == "Disponibilité")
                            {
                                // Convertir explicitement les valeurs booléennes en 0 ou 1
                                if (value.ToLower() == "true" || value == "1")
                                    values.Add("1");
                                else
                                    values.Add("0");
                            }
                            // Gestion des dates
                            else if (field.Name == "Date_Fabrication" || field.Name == "Date_péremption" ||
                                    field.Name == "Date_Achat")
                            {
                                if (string.IsNullOrEmpty(value))
                                {
                                    values.Add("NULL");
                                }
                                else if (DateTime.TryParse(value, out DateTime dateValue))
                                {
                                    // Formater la date au format MySQL: YYYY-MM-DD
                                    values.Add($"'{dateValue.ToString("yyyy-MM-dd")}'");
                                }
                                else
                                {
                                    // Si impossible de parser, utiliser NULL
                                    values.Add("NULL");
                                }
                            }
                            else if (string.IsNullOrEmpty(value) && field.Name.Contains("No_"))
                                values.Add("NULL");
                            else if (field.Name == "Prix_par_personnes" && float.TryParse(value, out float floatValue))
                                values.Add(floatValue.ToString(System.Globalization.CultureInfo.InvariantCulture));
                            else if ((field.Name == "Pour_n_personnes" || field.Name == "Quantité" ||
                                     field.Name == "Code_Postal" || field.Name == "No_Rue" ||
                                     field.Name == "Numéro_de_téléphone") &&
                                     int.TryParse(value, out int intValue))
                                values.Add(intValue.ToString());
                            else if (string.IsNullOrEmpty(value))
                                values.Add("NULL");
                            else
                                values.Add($"'{value}'");
                        }

                        string CommandeText = $"INSERT INTO {table} ({string.Join(", ", columns)}) " +
                                      $"VALUES ({string.Join(", ", values)});";
                        using (MySqlCommand cmd = new MySqlCommand(CommandeText, connexion))
                        {
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (MySqlException sqlEx)
                            {
                                MessageBox.Show($"Erreur lors de l'insertion dans {table}: {sqlEx.Message}\nRequête: {CommandeText}");
                            }
                        }
                    }
                }

                // Réactiver les contraintes de clés étrangères
                using (MySqlCommand cmd = new MySqlCommand("SET FOREIGN_KEY_CHECKS=1;", connexion))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors du chargement XML : " + e.Message);
            }
        }

        /// <summary>
        /// Enregistre la base de données dans le fichier "BDD_JSON.json"
        /// </summary>
        public void SauvegarderJSON()
        {
            try
            {
                var databaseData = new Dictionary<string, List<Dictionary<string, object>>>();

                string[] tables = { "Repas", "Données_Particulier", "Données_entreprise", "Compte_Client",
                   "Compte_Cuisinier", "Element_de_commande", "Ingrédient", "Commande",
                   "Comporte", "Propose" };

                foreach (string table in tables)
                {
                    var tableData = new List<Dictionary<string, object>>();
                    databaseData[table] = tableData;

                    string CommandeText = $"SELECT * FROM {table}";
                    using (MySqlCommand cmd = new MySqlCommand(CommandeText, connexion))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var rowData = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object value = reader.IsDBNull(i) ? null : reader.GetValue(i);

                                if (value is DateTime dateTime)
                                {
                                    value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else if (value is bool boolean)
                                {
                                    value = boolean;
                                }

                                rowData[columnName] = value;
                            }
                            tableData.Add(rowData);
                        }
                    }
                }

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(databaseData, options);
                File.WriteAllText("BDD_JSON.json", json);
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la sauvegarde JSON : " + e.Message);
            }
        }

        /// <summary>
        /// Charge la base de données à partir du fichier "BDD_JSON.json"
        /// </summary>
        public void ChargerJSON()
        {
            try
            {
                string json = File.ReadAllText("BDD_JSON.json");

                var databaseData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

                using (MySqlCommand cmd = new MySqlCommand("SET FOREIGN_KEY_CHECKS=0;", connexion))
                {
                    cmd.ExecuteNonQuery();
                }

                string[] tables = { "Repas", "Données_Particulier", "Données_entreprise", "Compte_Client",
                   "Compte_Cuisinier", "Element_de_commande", "Ingrédient", "Commande",
                   "Comporte", "Propose" };

                foreach (string table in tables)
                {
                    using (MySqlCommand cmd = new MySqlCommand($"TRUNCATE TABLE {table};", connexion))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    if (databaseData.ContainsKey(table) && databaseData[table].ValueKind == JsonValueKind.Array)
                    {
                        var rows = databaseData[table].EnumerateArray();
                        foreach (var row in rows)
                        {
                            var columns = new List<string>();
                            var values = new List<string>();

                            foreach (var property in row.EnumerateObject())
                            {
                                string columnName = property.Name;
                                columns.Add(columnName);
                                JsonElement valueElement = property.Value;

                                if (valueElement.ValueKind == JsonValueKind.Null)
                                {
                                    values.Add("NULL");
                                }
                                else if (columnName == "Type_de_Compte" || columnName == "Type_de_compte" ||
                                    columnName == "Terminé" || columnName == "Disponibilité")
                                {
                                    bool boolValue = false;
                                    if (valueElement.ValueKind == JsonValueKind.True)
                                        boolValue = true;
                                    else if (valueElement.ValueKind == JsonValueKind.String)
                                        boolValue = valueElement.GetString().ToLower() == "true" || valueElement.GetString() == "1";
                                    else if (valueElement.ValueKind == JsonValueKind.Number)
                                        boolValue = valueElement.GetInt32() != 0;

                                    values.Add(boolValue ? "1" : "0");
                                }
                                else if (columnName == "Date_Fabrication" || columnName == "Date_péremption" ||
                                        columnName == "Date_Achat")
                                {
                                    if (valueElement.ValueKind == JsonValueKind.String)
                                    {
                                        string dateStr = valueElement.GetString();
                                        if (DateTime.TryParse(dateStr, out DateTime dateValue))
                                        {
                                            values.Add($"'{dateValue.ToString("yyyy-MM-dd")}'");
                                        }
                                        else
                                        {
                                            values.Add("NULL");
                                        }
                                    }
                                    else
                                    {
                                        values.Add("NULL");
                                    }
                                }
                                else if (valueElement.ValueKind == JsonValueKind.Number)
                                {
                                    if (columnName == "Prix_par_personnes")
                                    {
                                        double floatValue = valueElement.GetDouble();
                                        values.Add(floatValue.ToString(System.Globalization.CultureInfo.InvariantCulture));
                                    }
                                    else
                                    {
                                        values.Add(valueElement.GetInt32().ToString());
                                    }
                                }
                                else if (valueElement.ValueKind == JsonValueKind.String)
                                {
                                    string strValue = valueElement.GetString().Replace("'", "''");
                                    values.Add($"'{strValue}'");
                                }
                                else
                                {
                                    values.Add($"'{valueElement.ToString().Replace("'", "''")}'");
                                }
                            }

                            string CommandeText = $"INSERT INTO {table} ({string.Join(", ", columns)}) " +
                                          $"VALUES ({string.Join(", ", values)});";
                            using (MySqlCommand cmd = new MySqlCommand(CommandeText, connexion))
                            {
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                }
                                catch (MySqlException sqlEx)
                                {
                                    MessageBox.Show($"Erreur lors de l'insertion dans {table}: {sqlEx.Message}\nRequête: {CommandeText}");
                                }
                            }
                        }
                    }
                }

                using (MySqlCommand cmd = new MySqlCommand("SET FOREIGN_KEY_CHECKS=1;", connexion))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors du chargement JSON : " + e.Message);
            }
        }
    }
}
