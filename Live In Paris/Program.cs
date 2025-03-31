using SkiaSharp;
using System;


namespace Live_In_Paris
{
    internal class Program
    {
        static void Main(string[] args) 
        {
            Apps App = new Apps();
            App.Initialisation();

            App.Connexion.Close();

            #region Commandes SELECT
            /*
            string commande = "SELECT * FROM Données_Particulier;";
            MySqlCommand codeCommande = App.Connexion.CreateCommand();
            codeCommande.CommandText = commande;
            MySqlDataReader reader = codeCommande.ExecuteReader();
            string[] valueString = new string[reader.FieldCount];
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valueString[i] = reader.GetValue(i).ToString();
                    App.SortieConsole = valueString[i] + " , ";
                }
                App.SortieConsole = "\n";
            }
            reader.Close();
            codeCommande.Dispose();
            #endregion
            */

            /*
            #region Création de table
            string createTable = " CREATE TABLE Professeur (nom VARCHAR(25));";
            MySqlCommand command2 = Connexion.CreateCommand();
            command2.CommandText = createTable;
            try
            {
                command2.ExecuteNonQuery();

            }
            catch (MySqlException e)
            {
                Console.WriteLine(" ErreurConnexion : " + e.ToString());
                Console.ReadLine();
                return;
            }
            command2.Dispose();
            #endregion
            #region Insertion
            string insertTable = " insert into Professeur  Values ('toto');";
            MySqlCommand command3 = Connexion.CreateCommand();
            command2.CommandText = insertTable;
            try
            {
                command2.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(" ErreurConnexion : " + e.ToString());
                Console.ReadLine();
                return;
            }

            command3.Dispose();
            #endregion
            #region Selection
            string requete = " SELECT * FROM personne;";
            string requete1 = "SELECT p.nom, p.prenom FROM personne p, role r, cote c, participation pp, film f " +
                "WHERE AND r.libelle = " + "Acteur" + " AND p.idPersonne = pp.idPersonne AND pp.idFilm = f.idFilm AND pp.idRole = r.idRole;";

            MySqlCommand command1 = Connexion.CreateCommand();
            command1.CommandText = requete;

            MySqlDataReader reader = command1.ExecuteReader();

            string[] valueString = new string[reader.FieldCount];
            while (reader.Read())
            {
                string last_name = (string)reader["nom"];
                string first_name = (string)reader["prenom"];
                //DateTime mylastupdate = (DateTime)reader["mydate"];
                Console.WriteLine(first_name + " " + last_name);

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valueString[i] = reader.GetValue(i).ToString();
                    Console.Write(valueString[i] + " , ");
                }
                Console.WriteLine();
            }
            reader.Close();
            command1.Dispose();
            #endregion
            #region Selection avec variable
            MySqlParameter nom = new MySqlParameter("@nom", MySqlDbType.VarChar);
            nom.Value = "Blier";

            string requete4 = "Select * from personne where nom = @nom;";
            MySqlCommand command4 = Connexion.CreateCommand();
            command4.CommandText = requete4;
            command4.Parameters.Add(nom);
            MySqlDataReader reader1 = command4.ExecuteReader();

            valueString = new string[reader1.FieldCount];
            while (reader1.Read())
            {

                for (int i = 0; i < reader1.FieldCount; i++)
                {
                    valueString[i] = reader1.GetValue(i).ToString();
                    //  valueString[i] = reader1.GetString(i);
                    Console.Write(valueString[i] + " , ");
                }
                Console.WriteLine();
            }
            reader.Close();
            */
            #endregion


            Console.WriteLine(App.SortieConsole);

            /*
            Graphe graphe = new Graphe();
            graphe.CreationGraph();
            Console.WriteLine("Les liens caractérisant le graph sont : ");
            graphe.AffichageLiens();

            Console.WriteLine("\nLa matrice du graph est : ");
            graphe.MatriceAdjacence();

            Console.WriteLine("\nParcour en largeur : " + graphe.ParcoursEnLargeur("0"));
            Console.WriteLine("\nParcour en profondeur : " + graphe.ParcoursEnProfondeur("0"));
            Console.WriteLine("\nLe graphe est connexe : " + graphe.Connexe());
            Console.WriteLine("\nUn cycle est : " + graphe.Cycle(graphe.Noeuds[0]) + ". Il existe donc un cycle.");


            graphe.DessinerGraphe();
            */
        }
    }
}