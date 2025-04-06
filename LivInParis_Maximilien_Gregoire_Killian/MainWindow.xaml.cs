using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Configuration;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Google.Protobuf.WellKnownTypes;

namespace LivInParis_Maximilien_Gregoire_Killian
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ModeToVisibilityConverter _modeToVisibilityConverter = new ModeToVisibilityConverter();
        public MainWindow()
        {
            InitializeComponent();
            Masque();
            Menu();

            ApplyModeToVisibilityConverter();

        }


        private void ApplyModeToVisibilityConverter()
        {
            // Créer les bindings avec le converter
            Binding nomBinding = new Binding("ChoixModeConnexion")
            {
                Converter = _modeToVisibilityConverter,
                ConverterParameter = "Particulier"
            };
            NomInput.SetBinding(TextBox.VisibilityProperty, nomBinding);

            Binding prenomBinding = new Binding("ChoixModeConnexion")
            {
                Converter = _modeToVisibilityConverter,
                ConverterParameter = "Particulier"
            };
            PrenomInput.SetBinding(TextBox.VisibilityProperty, prenomBinding);

            Binding nomEntrepriseBinding = new Binding("ChoixModeConnexion")
            {
                //Converter = _modeToVisibilityConverter,
                ConverterParameter = "Entreprise"
            };
            //NomEntrepriseInput.SetBinding(TextBox.VisibilityProperty, nomEntrepriseBinding);

            Binding nomReferentBinding = new Binding("ChoixModeConnexion")
            {
                //Converter = _modeToVisibilityConverter,
                ConverterParameter = "Entreprise"
            };
            //NomReferentInput.SetBinding(TextBox.VisibilityProperty, nomReferentBinding);

            Binding telephoneBinding = new Binding("ChoixModeConnexion")
            {
                Converter = _modeToVisibilityConverter,
                ConverterParameter = "Particulier"
            };
            TelephoneInput.SetBinding(TextBox.VisibilityProperty, telephoneBinding);
        }

        public void GoToMenu_Click(Object sender, RoutedEventArgs e)
        {
            Masque();
            Menu();
        }
        public void Menu()
        {
            AffichageMenu.Visibility = Visibility.Visible;

            #region Definis les données
            // Graphe
            graphe.CreationGraph();

            ListeChoix = new List<string> { };
            foreach (Noeud<string> noeud in graphe.Noeuds)
            {
                if (!ListeChoix.Contains(noeud.Nom.Split(" - ")[0]))
                {
                    ListeChoix.Add(noeud.Nom.Split(" - ")[0]);
                }
            }

            ListeAlgo = new List<string>() { "Haversine", "Dijkstra", "Bellman-Ford", "Floyd-Warshall" };


            ChoixDepart = ListeChoix[0];
            ChoixArrivee = ListeChoix[0];
            ChoixAlgo = ListeAlgo[0];

            // BDD
            ListeModule = new List<string>() { "Cuisinier", "Client" };
            ChoixModule = ListeModule[0];
            ListeCommandeCuisinier = new List<string>() { "Afficher mes clients", "Afficher mes commandes" };
            ChoixCommandeCuisinier = ListeCommandeCuisinier[0];
            ListeCommandeClient = new List<string>() { "Afficher les cuisniers disponibles", "Afficher mes commandes" };
            ChoixCommandeClient = ListeCommandeClient[0];
            ListeTrie = new List<string>() { "Alphabétique", "Par stations de metro", "Par la valeurs des achats passé." };

            // Connexion
            ListeConnexionOuCreation = new List<string>() { "Se connecter", "Nouveau compte" };
            ListeModeConnexion = new List<string>() { "Particulier", "Entreprise" };

            ChoixConnexionOuCreation = ListeConnexionOuCreation[0];
            ChoixModeConnexion = ListeModeConnexion[0];

            DataContext = this;


            #endregion
        }

        #region Graphe
        #region Definitions Graphe
        private Graphe graphe = new Graphe();

        private string _choixDepart;
        private string _choixArrivee;
        private string _choixAlgo;
        public List<string> ListeChoix { get; set; }
        public List<string> ListeAlgo { get; set; }

        public string ChoixDepart
        {
            get => _choixDepart;
            set
            {
                _choixDepart = value;
                OnPropertyChanged();
            }
        }
        public string ChoixArrivee
        {
            get => _choixArrivee;
            set
            {
                _choixArrivee = value;
                OnPropertyChanged();
            }
        }
        public string ChoixAlgo
        {
            get => _choixAlgo;
            set
            {
                _choixAlgo = value;
                OnPropertyChanged();
            }
        }


        #endregion

        private void GoToGraph_Click(Object sender, RoutedEventArgs e)
        {
            Masque();
            AffichageGraphe.Visibility = Visibility.Visible;
            Graph();

        }


        public void Graph()
        {

            graphe.DessinerGraphe();

        }



        /// <summary>
        /// Le code destiner à la page graphe qui duit être actualiser à chaque changements.
        /// </summary>
        public void CourantGraphe()
        {
            List<string> chemin = null;
            switch (ChoixAlgo)
            {

                case "Haversine":
                    AffichageGraphAlgo.Visibility = Visibility.Hidden;
                    AffichageGraphChemin.Visibility = Visibility.Hidden;
                    foreach (Noeud<string> noeud in graphe.Noeuds)
                    {
                        if (noeud.Nom.Split(" - ")[0] == ChoixDepart)
                        {
                            foreach (Noeud<string> noeud2 in graphe.Noeuds)
                            {
                                if (noeud2.Nom.Split(" - ")[0] == ChoixArrivee)
                                {
                                    AffichageGraphPoid.Text = $"La distance est {Convert.ToString(graphe.Haversine(noeud.Nom, noeud2.Nom))}km.";
                                }
                            }
                        }
                    }
                    break;
                case "Dijkstra":
                    AffichageGraphAlgo.Visibility = Visibility.Visible;
                    AffichageGraphChemin.Visibility = Visibility.Visible;

                    chemin = null;
                    foreach (Noeud<string> noeud in graphe.Noeuds)
                    {
                        if (noeud.Nom.Split(" - ")[0] == ChoixDepart)
                        {
                            foreach (Noeud<string> noeud2 in graphe.Noeuds)
                            {
                                if (noeud2.Nom.Split(" - ")[0] == ChoixArrivee)
                                {
                                    if (chemin == null || graphe.Poids(graphe.Dijkstra(noeud.Nom, noeud2.Nom)) < graphe.Poids(chemin))
                                    {
                                        chemin = graphe.Dijkstra(noeud.Nom, noeud2.Nom);
                                    }
                                }
                            }
                        }
                    }
                    if (chemin != null)
                    {
                        AffichageGraphAlgo.Text = string.Join("\n", chemin);
                        AffichageGraphPoid.Text = $"Le poid est {graphe.Poids(chemin)}";
                        graphe.DessinerCheminGraphe(chemin);
                    }
                    else { AffichageGraphAlgo.Text = "Il n'y a pas de chemin"; AffichageGraphPoid.Text = ""; }


                    break;
                case "Bellman-Ford":
                    AffichageGraphAlgo.Visibility = Visibility.Visible;
                    AffichageGraphChemin.Visibility = Visibility.Visible;

                    chemin = null;
                    foreach (Noeud<string> noeud in graphe.Noeuds)
                    {
                        if (noeud.Nom.Split(" - ")[0] == ChoixDepart)
                        {
                            foreach (Noeud<string> noeud2 in graphe.Noeuds)
                            {
                                if (noeud2.Nom.Split(" - ")[0] == ChoixArrivee)
                                {
                                    if (chemin == null || graphe.Poids(graphe.FloydWarshall(noeud.Nom, noeud2.Nom)) < graphe.Poids(chemin))
                                    {
                                        chemin = graphe.FloydWarshall(noeud.Nom, noeud2.Nom);
                                    }
                                }
                            }
                        }
                    }
                    if (chemin != null)
                    {
                        AffichageGraphAlgo.Text = string.Join("\n", chemin);
                        AffichageGraphPoid.Text = $"Le poid est {graphe.Poids(chemin)}";
                        graphe.DessinerCheminGraphe(chemin);
                    }
                    else { AffichageGraphAlgo.Text = "Il n'y a pas de chemin"; AffichageGraphPoid.Text = ""; }
                    break;

                case "Floyd-Warshall":
                    chemin = null;
                    foreach (Noeud<string> noeud in graphe.Noeuds)
                    {
                        if (noeud.Nom.Split(" - ")[0] == ChoixDepart)
                        {
                            foreach (Noeud<string> noeud2 in graphe.Noeuds)
                            {
                                if (noeud2.Nom.Split(" - ")[0] == ChoixArrivee)
                                {
                                    if (chemin == null || graphe.Poids(graphe.BellmanFord(noeud.Nom, noeud2.Nom)) < graphe.Poids(chemin))
                                    {
                                        chemin = graphe.BellmanFord(noeud.Nom, noeud2.Nom);
                                    }
                                }
                            }
                        }
                    }
                    // chemin = graphe.Dijkstra(ChoixDepart, ChoixArrivee);
                    if (chemin != null)
                    {
                        AffichageGraphAlgo.Text = string.Join("\n", chemin);
                        AffichageGraphPoid.Text = $"Le poid est {graphe.Poids(chemin)}";
                        graphe.DessinerCheminGraphe(chemin);
                    }
                    else { AffichageGraphAlgo.Text = "Il n'y a pas de chemin"; AffichageGraphPoid.Text = ""; }

                    break;
                default:
                    break;
            }
        }


        #endregion


        #region BDD
        #region Définition BDD
        private Apps App = new Apps();




        private string _choixModule;
        private string _choixCommandeCuisinier;
        private string _choixCommandeClient;
        private string _choixTrie;
        public List<string> ListeModule { get; set; }
        public List<string> ListeCommandeCuisinier { get; set; }
        public List<string> ListeCommandeClient { get; set; }
        public List<string> ListeTrie { get; set; }

        public string ChoixModule
        {
            get => _choixModule;
            set
            {
                _choixModule = value;
                OnPropertyChanged();
            }
        }
        public string ChoixCommandeCuisinier
        {
            get => _choixCommandeCuisinier;
            set
            {
                _choixCommandeCuisinier = value;
                OnPropertyChanged();
            }
        }
        public string ChoixCommandeClient
        {
            get => _choixCommandeClient;
            set
            {
                _choixCommandeClient = value;
                OnPropertyChanged();
            }
        }
        public string ChoixTrie
        {
            get => _choixTrie;
            set
            {
                _choixTrie = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private void GoToBDD_Click(Object sender, RoutedEventArgs e)
        {
            Masque();
            AffichageBDD.Visibility = Visibility.Visible;
            BDD();

        }
        public void BDD()
        {



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

            */
            #endregion
            /*
            #region Création de table
            string createTable = " CREATE TABLE Professeur (Nom VARCHAR(25));";
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
            string requete1 = "SELECT p.Nom, p.preNom FROM personne p, role r, cote c, participation pp, film f " +
                "WHERE AND r.libelle = " + "Acteur" + " AND p.idPersonne = pp.idPersonne AND pp.idFilm = f.idFilm AND pp.idRole = r.idRole;";

            MySqlCommand command1 = Connexion.CreateCommand();
            command1.CommandText = requete;

            MySqlDataReader reader = command1.ExecuteReader();

            string[] valueString = new string[reader.FieldCount];
            while (reader.Read())
            {
                string last_name = (string)reader["Nom"];
                string first_name = (string)reader["preNom"];
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
            MySqlParameter Nom = new MySqlParameter("@Nom", MySqlDbType.VarChar);
            Nom.Value = "Blier";

            string requete4 = "Select * from personne where Nom = @Nom;";
            MySqlCommand command4 = Connexion.CreateCommand();
            command4.CommandText = requete4;
            command4.Parameters.Add(Nom);
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



        }

        public void CourantBDD()
        {
            if (!App.Connecte)
            {
                AffichageBDDResultat.Text = "Veuillez d'abord vous connecter.";
                return;
            }
            switch (ChoixModule)
            {
                case "Cuisinier":
                    PickerComboBoxCommandeCuisinier.Visibility = Visibility.Visible;
                    PickerComboBoxCommandeClient.Visibility = Visibility.Hidden;
                    AffichageBDDResultat.Visibility = Visibility.Visible;
                    NomEntrepriseInput.Visibility = Visibility.Visible;
                    NomReferentInput.Visibility = Visibility.Visible;
                    if (ChoixCommandeCuisinier == "Afficher mes clients")
                    {
                        AffichageBDDResultat.Text = "Le résultat est :" + App.AfficherClients();
                    }
                    else if (ChoixCommandeCuisinier == "Afficher mes commandes")
                    {
                        AffichageBDDResultat.Text = "Le résultat est :" + App.AfficherCommandesCuisinier();
                    }
                    break;

                case "Client":
                    PickerComboBoxCommandeClient.Visibility = Visibility.Visible;
                    PickerComboBoxCommandeCuisinier.Visibility = Visibility.Hidden;
                    AffichageBDDResultat.Visibility = Visibility.Visible;
                    NomEntrepriseInput.Visibility = Visibility.Visible;
                    NomReferentInput.Visibility = Visibility.Visible;
                    if (ChoixCommandeClient == "Afficher les cuisiniers disponibles")
                    {
                        AffichageBDDResultat.Text = "Le résultat est :" + App.AfficherCuisiniersDisponibles();
                    }
                    else if (ChoixCommandeClient == "Afficher mes commandes")
                    {
                        AffichageBDDResultat.Text = "Le résultat est :" + App.AfficherCommandesClients();
                    }
                    break;

                default:
                    break;
            }
        }



        #region Connexion
        #region Dédinition connexion

        public List<string> ListeConnexionOuCreation { get; set; }
        public List<string> ListeModeConnexion { get; set; }
        private string _choixConnexionOuCreation;
        private string _choixModeConnexion;
        public string ChoixConnexionOuCreation
        {
            get => _choixConnexionOuCreation;
            set
            {
                _choixConnexionOuCreation = value;
                OnPropertyChanged();
            }
        }
        public string ChoixModeConnexion
        {
            get => _choixModeConnexion;
            set
            {
                _choixModeConnexion = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private void GoToConnexion_Click(Object sender, RoutedEventArgs e)
        {
            Masque();
            AffichageConnexion.Visibility = Visibility.Visible;
            Connexion();

        }

        public void Connexion()
        {

        }

        public void CourantConnexion()
        {
            switch (ChoixConnexionOuCreation)
            {
                case "Se connecter":
                    CreationComptePanel.Visibility = Visibility.Collapsed;
                    IdentifiantInput.Visibility = Visibility.Visible;
                    MDPInput.Visibility = Visibility.Visible;
 
                    break;
                case "Nouveau compte":
                    CreationComptePanel.Visibility = Visibility.Visible;
                    IdentifiantInput.Visibility = Visibility.Visible;
                    MDPInput.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }
        private void Connexion_Click(object sender, RoutedEventArgs e)
        {
            string identifiant = IdentifiantInput.Text;
            string motDePasse = MDPInput.Text;

            if (string.IsNullOrEmpty(identifiant) || string.IsNullOrEmpty(motDePasse))
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.");
                return;
            }

            if (ChoixConnexionOuCreation == "Se connecter")
            {
                bool connecte = App.SeConnecter(identifiant, motDePasse, ChoixModeConnexion == "Entreprise");
                if (connecte)
                {
                    Masque();
                    AffichageBDD.Visibility = Visibility.Visible;
                    BDD();
                }
                else
                {
                    MessageBox.Show("Échec de la connexion. Vérifiez vos identifiants.");
                }
            }
            else if (ChoixConnexionOuCreation == "Nouveau compte")
            {
                string resultat;
                if (ChoixModeConnexion == "Particulier")
                {
                    if (string.IsNullOrEmpty(NomInput.Text) || string.IsNullOrEmpty(PrenomInput.Text) || string.IsNullOrEmpty(TelephoneInput.Text))
                    {
                        MessageBox.Show("Veuillez remplir tous les champs requis pour un particulier.");
                        return;
                    }
                    resultat = App.NouveauCompteParticulier(
                        NomInput.Text, PrenomInput.Text, NomRueInput.Text, NoRueInput.Text, CodePostalInput.Text,
                        VilleInput.Text, TelephoneInput.Text, identifiant, MetroProcheInput.Text, motDePasse, EstCuisinierCheckBox.IsChecked == true);
                }
                else
                {
                    if (string.IsNullOrEmpty(NomEntrepriseInput.Text) || string.IsNullOrEmpty(NomReferentInput.Text))
                    {
                        MessageBox.Show("Veuillez remplir tous les champs requis pour une entreprise.");
                        return;
                    }
                    resultat = App.NouveauCompteEntreprise(
                        identifiant, NomReferentInput.Text, NoRueInput.Text, NomRueInput.Text, CodePostalInput.Text,
                        VilleInput.Text, MetroProcheInput.Text, motDePasse, EstCuisinierCheckBox.IsChecked == true);
                }
                MessageBox.Show(resultat);
                if (resultat.Contains("succès"))
                {
                    // Optionnel : Connecter automatiquement après création
                    bool connecte = App.SeConnecter(identifiant, motDePasse, ChoixModeConnexion == "Entreprise");
                    if (connecte)
                    {
                        Masque();
                        AffichageBDD.Visibility = Visibility.Visible;
                        BDD();
                    }
                }
            }
        }

        #endregion


        /// <summary>
        /// Crée et peuple la database lorsque le bouton "Definie les données" est cliqué.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Creation_Click(Object sender, RoutedEventArgs e)
        {
            App.Initialisation();
        }

        /// <summary>
        /// Masque l'entiereté des éléments d'affichages
        /// </summary>
        public void Masque()
        {
            AffichageBDD.Visibility = Visibility.Hidden;
            AffichageGraphe.Visibility = Visibility.Hidden;
            AffichageMenu.Visibility = Visibility.Hidden;
            AffichageConnexion.Visibility = Visibility.Hidden;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Ce qui est ici est le code qui actalise les textes à chaques fois qu'un changement est fait
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (AffichageGraphe.Visibility == Visibility.Visible) { CourantGraphe(); }
            if (AffichageBDD.Visibility == Visibility.Visible) { CourantBDD(); }
            if (AffichageConnexion.Visibility == Visibility.Visible) { CourantConnexion(); }

        }
    }
}

