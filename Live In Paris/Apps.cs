using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_In_Paris
{
    internal class Apps
    {
        private string sortieConsole = "Sortie console :\n";
        private MySqlConnection connexion = null;


        public string SortieConsole
        {
            get { return sortieConsole; }
            set { sortieConsole += value; }
        }
        public MySqlConnection Connexion { get { return connexion; } }


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
           // CreationTables();
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
            PeuplementElementDeCommande();
            PeuplementIngrédient();
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
                    MySqlCommand insertCommande=Connexion.CreateCommand();
                    insertCommande.CommandText = insertTable;
                    try
                    {
                        insertCommande.ExecuteNonQuery();
                    }
                    catch(MySqlException e) { Console.WriteLine("\nErreur Insertion : "+e.ToString()); return; }
                }
                this.SortieConsole = "\n Insertion réussie.";
            }
            catch(MySqlException e) { Console.WriteLine("\nErreur Insertion : " + e.ToString());return; }



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

                for(int i =0;i<donneesElements.Count;i++) 
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



        #endregion
    }
}
