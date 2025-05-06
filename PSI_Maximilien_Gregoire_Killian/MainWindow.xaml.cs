using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Cache;
using System.Runtime.CompilerServices;
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

namespace PSI_Maximilien_Gregoire_Killian
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BaseDeDonnees BDD;
        private Graphe graphe;

        private List<(int No_Repas, string Nom, double Prix, int Quantite)> currentOrder;

        public MainWindow()
        {
            BDD = new BaseDeDonnees();
            // BDD.Initialisation();
            currentOrder = new List<(int, string, double, int)>();
            graphe = new Graphe();
            graphe.CreationGraph();
            graphe.WelshPowell();
            InitializeComponent();
            InitialisationNouveauCompte();
            GoTo_Connexion();
        }

        #region Connexion / Création Compte
        private bool Connecte = false;

        #region Connexion
        /// <summary>
        /// Affiche la page de connexion
        /// </summary>
        private void GoTo_Connexion()
        {
            Connexion.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Modifie les champs nécessaires selon qu'il s'agisse d'une entreprise ou d'un particulier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EstEntreprise_Checked(object sender, RoutedEventArgs e)
        {
            if (EstEntreprise.IsChecked == true)
            {
                Identifiant.Text = "Nom de l'entreprise :";
            }
            else
            {
                Identifiant.Text = "Email :";
            }
        }

        /// <summary>
        /// Tente de connecter l'utilisateur à la base de donnée.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SeConnecter_Click(Object sender, RoutedEventArgs e)
        {
            string identifiant = Identifiant.Text;
            string motDePasse = MotDePasse.Text;
            if(identifiant == "admin" && motDePasse == "admin")
            {
                Connexion.Visibility = Visibility.Collapsed;
                Connecte = true;
                GoTo_Admin();
                return;
            }


            Connecte = BDD.SeConnecter(identifiant, motDePasse, Convert.ToBoolean(EstEntreprise.IsChecked));
            if (Connecte)
            {
                Connexion.Visibility = Visibility.Collapsed;
                GoTo_Menu();

            }
            else
            {
                MessageBox.Show("Identifiant ou mot de passe incorrect.");
            }

        }

        /// <summary>
        /// Quitte la page de connexion pour amener à la page de création de compte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GoTo_NouveauCompte_Click(Object sender, RoutedEventArgs e)
        {
           Connexion.Visibility = Visibility.Collapsed;
           GoTo_NouveauCompte();

        }
        #endregion

        #region Nouveau compte
        /// <summary>
        /// Affiche la page de création de compte
        /// </summary>
        private void GoTo_NouveauCompte()
        {
            NouveauCompte.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Modifie les champs nécessaires selon qu'il s'agisse d'une entreprise ou d'un particulier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EstEntreprise2_Checked(object sender, RoutedEventArgs e)
        {
            if (EstEntreprise2.IsChecked == true)
            {
                NouveauCompte_Particulier.Visibility = Visibility.Collapsed;
                NouveauCompte_Entreprise.Visibility = Visibility.Visible;
            }
            else
            {
                NouveauCompte_Particulier.Visibility = Visibility.Visible;
                NouveauCompte_Entreprise.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Initialise le choix de metro le plus proche pour la création de compte
        /// </summary>
        private void InitialisationNouveauCompte()
        {
            List<string> ListeChoix = new List<string> { };
            foreach (Noeud<string> noeud in graphe.Noeuds)
            {
                if (!ListeChoix.Contains(noeud.Nom.Split(" - ")[0]))
                {
                    ListeChoix.Add(noeud.Nom.Split(" - ")[0]);
                }
            }
            ListeChoix.Sort();
            ListeChoix.Insert(0,  "Métro le plus proche");
            Metro.ItemsSource = ListeChoix;
            Metro.SelectedIndex = 0;
        }

        /// <summary>
        /// Tente de créer un nouveau compte pour l'utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NouveauCompte_Click(Object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NomRue.Text) || NomRue.Text == "Nom de rue")
            {
                MessageBox.Show("Erreur : Le nom de la rue ne peut pas être vide.");
                return;
            }
            if (NomRue.Text.Length > 50)
            {
                MessageBox.Show("Erreur : Le nom de la rue ne peut pas dépasser 50 caractères.");
                return;
            }

            if (string.IsNullOrWhiteSpace(NoRue.Text) || NoRue.Text == "Numéro de rue")
            {
                MessageBox.Show("Erreur : Le numéro de rue ne peut pas être vide.");
                return;
            }
            if (!int.TryParse(NoRue.Text, out int tempo) || tempo<= 0)
            {
                MessageBox.Show("Erreur : Le numéro de rue doit être un entier positif.");
                return;
            }

            if (string.IsNullOrWhiteSpace(CodePostal.Text) || CodePostal.Text == "Code postal")
            {
                MessageBox.Show("Erreur : Le code postal ne peut pas être vide.");
                return;
            }
            if (!int.TryParse(CodePostal.Text, out int tempo2) || tempo2<= 0 || CodePostal.Text.Length != 5)
            {
                MessageBox.Show("Erreur : Le code postal doit être un entier positif de 5 chiffres.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Ville.Text) || Ville.Text == "Ville")
            {
                MessageBox.Show("Erreur : La ville ne peut pas être vide.");
                return;
            }
            if (Ville.Text.Length > 50)
            {
                MessageBox.Show("Erreur : La ville ne peut pas dépasser 50 caractères.");
                return;
            }

            if (Metro.SelectedIndex == 0 || Metro.Text == "Métro le plus proche")
            {
                MessageBox.Show("Erreur : Vous devez sélectionner une station de métro.");
                return;
            }

            if (string.IsNullOrWhiteSpace(MdP.Text) || MdP.Text == "Mot de passe")
            {
                MessageBox.Show("Erreur : Le mot de passe ne peut pas être vide.");
                return;
            }
            if (MdP.Text.Length > 20)
            {
                MessageBox.Show("Erreur : Le mot de passe ne peut pas dépasser 20 caractères.");
                return;
            }


            if (EstEntreprise2.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(NomEntreprise.Text) || NomEntreprise.Text == "Nom de l'entreprise")
                {
                    MessageBox.Show("Erreur : Le nom de l'entreprise ne peut pas être vide.");
                    return;
                }
                if (NomEntreprise.Text.Length > 50)
                {
                    MessageBox.Show("Erreur : Le nom de l'entreprise ne peut pas dépasser 50 caractères.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(NomReferent.Text) || NomReferent.Text == "Nom du référent")
                {
                    MessageBox.Show("Erreur : Le nom du référent ne peut pas être vide.");
                    return;
                }
                if (NomReferent.Text.Length > 20)
                {
                    MessageBox.Show("Erreur : Le nom du référent ne peut pas dépasser 20 caractères.");
                    return;
                }

                string texteCommande = $"SELECT COUNT(*) FROM Données_Entreprise WHERE Nom_Entreprise = @nomEntreprise";
                MySqlCommand commande = new MySqlCommand(texteCommande, BDD.Connexion);
                commande.Parameters.AddWithValue("@nomEntreprise", NomEntreprise.Text.Trim());
                int count = Convert.ToInt32(commande.ExecuteScalar());
                if (count > 0)
                {
                    MessageBox.Show("Erreur : Ce nom d'entreprise est déjà utilisé.");
                    return;
                }

                string nomEntreprise = NomEntreprise.Text.Trim();
                string nomReferent = NomReferent.Text.Trim();
                string noRue = NoRue.Text.Trim();
                string nomRue = NomRue.Text.Trim();
                string codePostal = CodePostal.Text.Trim();
                string ville = Ville.Text.Trim();
                string metroProche = Metro.Text.Trim();
                string motDePasse = MdP.Text.Trim();
                Connecte = BDD.NouveauCompteEntreprise(nomEntreprise, nomReferent, noRue, nomRue, codePostal, ville, metroProche, motDePasse);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Nom.Text) || Nom.Text == "Nom")
                {
                    MessageBox.Show("Erreur : Le nom ne peut pas être vide.");
                    return;
                }
                if (Nom.Text.Length > 20)
                {
                    MessageBox.Show("Erreur : Le nom ne peut pas dépasser 20 caractères.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Prenom.Text) || Prenom.Text == "Prénom")
                {
                    MessageBox.Show("Erreur : Le prénom ne peut pas être vide.");
                    return;
                }
                if (Prenom.Text.Length > 20)
                {
                    MessageBox.Show("Erreur : Le prénom ne peut pas dépasser 20 caractères.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Num.Text) || Num.Text == "Numéro de téléphone")
                {
                    MessageBox.Show("Erreur : Le numéro de téléphone ne peut pas être vide.");
                    return;
                }
                if (!long.TryParse(Num.Text, out long tempo3) || tempo3 <= 0 || Num.Text.Length < 10)
                {
                    MessageBox.Show("Erreur : Le numéro de téléphone doit être un numéro valide (10 chiffres minimum).");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Mail.Text) || Mail.Text == "Adresse Mail")
                {
                    MessageBox.Show("Erreur : L'adresse email ne peut pas être vide.");
                    return;
                }
                if (Mail.Text.Length > 50)
                {
                    MessageBox.Show("Erreur : L'adresse email ne peut pas dépasser 50 caractères.");
                    return;
                }

                string texteCommande = $"SELECT COUNT(*) FROM Données_Particulier WHERE Adresse_mail = @email";
                MySqlCommand commande = new MySqlCommand(texteCommande, BDD.Connexion);
                commande.Parameters.AddWithValue("@email", Mail.Text.Trim());
                int count = Convert.ToInt32(commande.ExecuteScalar());
                if (count > 0)
                {
                    MessageBox.Show("Erreur : Cette adresse email est déjà utilisée.");
                    return;
                }

                string nom = Nom.Text.Trim();
                string prenom = Prenom.Text.Trim();
                string nomRue = NomRue.Text.Trim();
                string noRue = NoRue.Text.Trim();
                string codePostal = CodePostal.Text.Trim();
                string ville = Ville.Text.Trim();
                string telephone = Num.Text.Trim();
                string email = Mail.Text.Trim();
                string metroProche = Metro.Text.Trim();
                string motDePasse = MdP.Text.Trim();
                Connecte = BDD.NouveauCompteParticulier(nom, prenom, nomRue, noRue, codePostal, ville, telephone, email, metroProche, motDePasse);
            }

            if (Connecte)
            {
                NouveauCompte.Visibility = Visibility.Collapsed;
                GoTo_Menu();

            }
            else
            {
                MessageBox.Show("Echec lors de la création du compte.");
            }
        }

        /// <summary>
        /// Quitte la page de Création de compte pour amener à la page de connexion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GoTo_SeConnecter_Click(Object sender, RoutedEventArgs e)
        {
            NouveauCompte.Visibility= Visibility.Collapsed;
            GoTo_Connexion();
        }


        #endregion

        #endregion

        #region Menu
        /// <summary>
        /// Affiche et initialise le menu
        /// </summary>
        private void GoTo_Menu()
        {
            Menu_Barre.Visibility= Visibility.Visible;
            Bonjour.Text = $"Bonjour {BDD.DonnerNom()}";

            InitialisationMenuGraphe();

            GoTo_Menu_Client_Click(null, null);
        }

        #region Menu Client
        /// <summary>
        /// Affiche la page client du menu lorsque le bouton est cliqué
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoTo_Menu_Client_Click(object sender, RoutedEventArgs e)
        {
            GoTo_MenuClient();
        }

        /// <summary>
        /// Affiche la page client du menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoTo_MenuClient()
        {
            masqueMenu();
            Menu_Client.Visibility = Visibility.Visible;

            if (!BDD.Connecte || BDD.NoCompteClient <= 0)
            {
                MessageBox.Show("Erreur : Vous devez être connecté en tant que client.");
                GoTo_Connexion();
                return;
            }

            ClientViewSelector.SelectionChanged -= Changement_ChoixClient;
            ClientViewSelector.SelectedIndex = 0;
            ClientViewSelector.SelectionChanged += Changement_ChoixClient;

            AffichagePlatsDispo();
        }

        /// <summary>
        /// Affiche la page du menu selon le combobox ClientViewSelector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Changement_ChoixClient(object sender, SelectionChangedEventArgs e)
        {
            if (ClientViewSelector.SelectedIndex == 0)
            {
                OrderView.Visibility = Visibility.Visible;
                PastOrdersView.Visibility = Visibility.Collapsed;
                AffichagePlatsDispo();
            }
            else
            {
                OrderView.Visibility = Visibility.Collapsed;
                PastOrdersView.Visibility = Visibility.Visible;
                InitializePastOrdersView();
            }
        }

        /// <summary>
        /// Affiche les plats disponibles à la vente dans un tableau
        /// </summary>
        private void AffichagePlatsDispo()
        {
            DétailsPlat.Visibility = Visibility.Collapsed;
            DetailsPlatNom.Text = "Détails du plat";

            List<object> plats = new List<object>();
            try
            {
                MySqlCommand commande = BDD.Connexion.CreateCommand();
                commande.CommandText = $"SELECT r.No_Repas, r.Nom, r.Prix_par_personnes, r.Régime_alimentaire, r.Nature " +
                                      $"FROM Repas r " +
                                      $"INNER JOIN Propose p ON r.No_Repas = p.No_Repas " +
                                      $"INNER JOIN Compte_Cuisinier cc ON p.No_Compte_Cuisinier = cc.No_Compte_Cuisinier " +
                                      $"WHERE cc.Disponibilité = 1";
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    plats.Add(new
                    {
                        No_Repas = reader.GetInt32("No_Repas"),
                        Nom = reader.GetString("Nom"),
                        Prix = reader.GetDouble("Prix_par_personnes").ToString("F2"),
                        Regime = reader.IsDBNull(reader.GetOrdinal("Régime_alimentaire")) ? "" : reader.GetString("Régime_alimentaire"),
                        Nature = reader.IsDBNull(reader.GetOrdinal("Nature")) ? "" : reader.GetString("Nature")
                    });
                }
                reader.Close();
                commande.Dispose();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur lors du chargement des plats : " + ex.Message);
                return;
            }

            TableauPlats.ItemsSource = plats;
            TableauPlats.Tag = plats;

            AffichageCommande();
        }

        /// <summary>
        /// Affiche les détails du plats de la ligne lorsque le bouton afficher est cliqué
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AfficherPlat_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DataGridRow row = FindVisualParent<DataGridRow>(button);
            if (row != null)
            {
                var dish = row.Item as dynamic;
                int noRepas = dish.No_Repas;

                try
                {
                    MySqlCommand commande = BDD.Connexion.CreateCommand();
                    commande.CommandText = $"SELECT Nom, Type_de_plat__entrée__plat__dessert_, Pour_n_personnes, " +
                                          $"Prix_par_personnes, Régime_alimentaire, Nature " +
                                          $"FROM Repas WHERE No_Repas = @noRepas";
                    commande.Parameters.AddWithValue("@noRepas", noRepas);
                    MySqlDataReader reader = commande.ExecuteReader();
                    if (reader.Read())
                    {
                        DetailsPlatNom.Text = "Détails de " + reader.GetString("Nom");
                        DishDetailType.Text = reader.GetString("Type_de_plat__entrée__plat__dessert_");
                        DishDetailPersonnes.Text = reader.GetInt32("Pour_n_personnes").ToString();
                        DishDetailPrix.Text = reader.GetDouble("Prix_par_personnes").ToString("F2");
                        DishDetailRegime.Text = reader.IsDBNull(reader.GetOrdinal("Régime_alimentaire")) ? "" : reader.GetString("Régime_alimentaire");
                        DishDetailNature.Text = reader.IsDBNull(reader.GetOrdinal("Nature")) ? "" : reader.GetString("Nature");
                    }
                    reader.Close();

                    StringBuilder ingredients = new StringBuilder();
                    commande.CommandText = $"SELECT Nom, Quantité FROM Ingrédient WHERE No_Repas = @noRepas";
                    reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        string nom = reader.GetString("Nom");
                        string quantite = reader.IsDBNull(reader.GetOrdinal("Quantité")) ? "0" : reader["Quantité"].ToString();
                        ingredients.AppendLine($"{nom}: {quantite}");
                    }
                    reader.Close();
                    DishDetailIngredients.Text = ingredients.Length > 0 ? ingredients.ToString() : "Aucun ingrédient";
                    commande.Dispose();

                    DétailsPlat.Visibility = Visibility.Visible;
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erreur lors du chargement des détails du plat : " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Ajoute le plat de la ligne à la commande en cours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AjouterDansCommande_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DataGridRow row = FindVisualParent<DataGridRow>(button);
            if (row != null)
            {
                var dish = row.Item as dynamic;
                int noRepas = dish.No_Repas;
                string nom = dish.Nom;
                double prix = Convert.ToDouble(dish.Prix);

                var existingItem = currentOrder.FirstOrDefault(item => item.No_Repas == noRepas);
                if (existingItem.No_Repas != 0)
                {
                    currentOrder.Remove(existingItem);
                    currentOrder.Add((noRepas, nom, prix, existingItem.Quantite + 1));
                }
                else
                {
                    currentOrder.Add((noRepas, nom, prix, 1));
                }

                AffichageCommande();
            }
        }

        /// <summary>
        /// Supprime une ligne de commande de la commande lorsque le bouton supprimé est cliqué
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RetirerDeCommande_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DataGridRow row = FindVisualParent<DataGridRow>(button);
            if (row != null)
            {
                var item = row.Item as dynamic;
                int noRepas = item.No_Repas;
                currentOrder.RemoveAll(x => x.No_Repas == noRepas);
                AffichageCommande();
            }
        }

        /// <summary>
        /// Affiche la commande en cours
        /// </summary>
        private void AffichageCommande()
        {
            var orderItems = currentOrder.Select(item => new
            {
                No_Repas = item.No_Repas,
                Nom = item.Nom,
                Quantite = item.Quantite,
                PrixTotal = (item.Prix * item.Quantite).ToString("F2")
            }).ToList();

            OrderDataGrid.ItemsSource = orderItems;
            double total = currentOrder.Sum(item => item.Prix * item.Quantite);
            OrderTotal.Text = $"Total: {total:F2} €";
        }

        /// <summary>
        /// Tente d'enregistrer la commande en cours dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValidateOrder_Click(object sender, RoutedEventArgs e)
        {
            if (currentOrder.Count == 0)
            {
                MessageBox.Show("Erreur : Votre commande est vide.");
                return;
            }

            try
            {
                MySqlCommand commande = BDD.Connexion.CreateCommand();

                Dictionary<int, int> dishToCook = new Dictionary<int, int>();
                string clientMetro = "";
                string query = BDD.TypeDeCompte
                    ? $"SELECT Métro_le___proche FROM Données_Entreprise WHERE No_Données = (SELECT No_Données FROM Compte_Client WHERE No_Compte_Client = @noClient)"
                    : $"SELECT Métro_le___proche FROM Données_Particulier WHERE No_Données = (SELECT No_Données FROM Compte_Client WHERE No_Compte_Client = @noClient)";
                commande.CommandText = query;
                commande.Parameters.AddWithValue("@noClient", BDD.NoCompteClient);
                clientMetro = commande.ExecuteScalar()?.ToString() ?? "";
                if (string.IsNullOrEmpty(clientMetro))
                {
                    MessageBox.Show("Erreur : Impossible de déterminer votre station de métro.");
                    commande.Dispose();
                    return;
                }

                foreach (var dish in currentOrder)
                {
                    int noRepas = dish.No_Repas;
                    commande.CommandText = $"SELECT p.No_Compte_Cuisinier, cc.No_Données " +
                                          $"FROM Propose p " +
                                          $"INNER JOIN Compte_Cuisinier cc ON p.No_Compte_Cuisinier = cc.No_Compte_Cuisinier " +
                                          $"WHERE p.No_Repas = @noRepas AND cc.Disponibilité = 1";
                    commande.Parameters.Clear();
                    commande.Parameters.AddWithValue("@noRepas", noRepas);
                    MySqlDataReader reader = commande.ExecuteReader();
                    List<(int NoCuisinier, int NoDonnees)> cooks = new List<(int, int)>();
                    while (reader.Read())
                    {
                        cooks.Add((reader.GetInt32("No_Compte_Cuisinier"), reader.GetInt32("No_Données")));
                    }
                    reader.Close();

                    if (cooks.Count == 0)
                    {
                        MessageBox.Show($"Erreur : Aucun cuisinier disponible pour le plat n°{noRepas}.");
                        commande.Dispose();
                        return;
                    }

                    int selectedCook = -1;
                    double minDistance = double.MaxValue;
                    foreach (var cook in cooks)
                    {
                        string cookMetro = "";
                        query = $"SELECT Métro_le___proche FROM {(BDD.TypeDeCompte ? "Données_Entreprise" : "Données_Particulier")} WHERE No_Données = @noDonnees";
                        commande.CommandText = query;
                        commande.Parameters.Clear();
                        commande.Parameters.AddWithValue("@noDonnees", cook.NoDonnees);
                        cookMetro = commande.ExecuteScalar()?.ToString() ?? "";
                        if (string.IsNullOrEmpty(cookMetro))
                        {
                            continue;
                        }

                        double distance = double.MaxValue;
                        foreach (var node1 in graphe.Noeuds)
                        {
                            if (node1.Nom.Split(" - ")[0] != clientMetro) continue;
                            foreach (var node2 in graphe.Noeuds)
                            {
                                if (node2.Nom.Split(" - ")[0] != cookMetro) continue;
                                var path = graphe.Dijkstra(node1.Nom, node2.Nom);
                                double pathWeight = Convert.ToDouble(graphe.Poids(path));
                                if (pathWeight < distance) distance = pathWeight;
                            }
                        }
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            selectedCook = cook.NoCuisinier;
                        }
                    }

                    if (selectedCook == -1)
                    {
                        MessageBox.Show($"Erreur : Aucun cuisinier disponible avec une station de métro valide pour le plat n°{noRepas}.");
                        commande.Dispose();
                        return;
                    }

                    dishToCook[noRepas] = selectedCook;
                }

                foreach (var cook in dishToCook.Values.Distinct())
                {
                    var platsForCook = currentOrder.Where(d => dishToCook[d.No_Repas] == cook).ToList();
                    if (platsForCook.Count == 0) continue;

                    commande.CommandText = $"INSERT INTO Commande (No_Compte_Client, No_Compte_Cuisinier, Date_Achat) VALUES (@noClient, @noCuisinier, @dateAchat)";
                    commande.Parameters.Clear();
                    commande.Parameters.AddWithValue("@noClient", BDD.NoCompteClient);
                    commande.Parameters.AddWithValue("@noCuisinier", cook);
                    commande.Parameters.AddWithValue("@dateAchat", DateTime.Today);
                    commande.ExecuteNonQuery();

                    commande.CommandText = $"SELECT LAST_INSERT_ID()";
                    int noAchat = Convert.ToInt32(commande.ExecuteScalar());

                    foreach (var dish in platsForCook)
                    {
                        DateTime today = DateTime.Today;
                        DateTime expirationDate = today.AddDays(3);

                        commande.CommandText = $"INSERT INTO Element_de_commande (No_Repas, Quantité, Date_Fabrication, Date_péremption) VALUES (@noRepas, @quantite, @dateFabrication, @datePeremption)";
                        commande.Parameters.Clear();
                        commande.Parameters.AddWithValue("@noRepas", dish.No_Repas);
                        commande.Parameters.AddWithValue("@quantite", dish.Quantite);
                        commande.Parameters.AddWithValue("@dateFabrication", today);
                        commande.Parameters.AddWithValue("@datePeremption", expirationDate);
                        commande.ExecuteNonQuery();

                        commande.CommandText = $"SELECT LAST_INSERT_ID()";
                        int noElement = Convert.ToInt32(commande.ExecuteScalar());

                        commande.CommandText = $"INSERT INTO Comporte (No_Achat, No_Element) VALUES (@noAchat, @noElement)";
                        commande.Parameters.Clear();
                        commande.Parameters.AddWithValue("@noAchat", noAchat);
                        commande.Parameters.AddWithValue("@noElement", noElement);
                        commande.ExecuteNonQuery();
                    }
                }

                commande.Dispose();
                MessageBox.Show("Commande validée avec succès.");
                currentOrder.Clear();
                AffichageCommande();
                AffichagePlatsDispo();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur lors de la validation de la commande : " + ex.Message);
            }
        }

        /// <summary>
        /// Remplie le tableau des anciennes commandes
        /// </summary>
        private void InitializePastOrdersView()
        {
            List<object> orders = new List<object>();
            try
            {
                MySqlCommand commande = BDD.Connexion.CreateCommand();
                commande.CommandText = $"SELECT c.No_Achat, c.Terminé, c.Date_Achat FROM Commande c " +
                                      $"WHERE c.No_Compte_Client = {BDD.NoCompteClient}";
                MySqlDataReader reader = commande.ExecuteReader();
                List<(int NoAchat, bool? Termine, DateTime? DateAchat)> noAchats = new List<(int, bool?, DateTime?)>();
                while (reader.Read())
                {
                    bool? termine = reader.IsDBNull(reader.GetOrdinal("Terminé")) ? (bool?)null : reader.GetBoolean("Terminé");
                    DateTime? dateAchat = reader.IsDBNull(reader.GetOrdinal("Date_Achat")) ? (DateTime?)null : reader.GetDateTime("Date_Achat");
                    noAchats.Add((reader.GetInt32("No_Achat"), termine, dateAchat));
                }
                reader.Close();

                foreach (var achat in noAchats)
                {
                    StringBuilder plats = new StringBuilder();
                    double prixTotal = 0;

                    commande.CommandText = $"SELECT r.Nom, e.Quantité, r.Prix_par_personnes " +
                                          $"FROM Comporte co INNER JOIN Element_de_commande e ON co.No_Element = e.No_Element " +
                                          $"INNER JOIN Repas r ON e.No_Repas = r.No_Repas " +
                                          $"WHERE co.No_Achat = @noAchat";
                    commande.Parameters.Clear();
                    commande.Parameters.AddWithValue("@noAchat", achat.NoAchat);
                    reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        string nom = reader.GetString("Nom");
                        int quantite = reader.GetInt32("Quantité");
                        double prix = reader.GetDouble("Prix_par_personnes");
                        plats.AppendLine($"{nom} ({quantite})");
                        prixTotal += quantite * prix;
                    }
                    reader.Close();

                    string statut;
                    if (achat.Termine == null)
                    {
                        statut = "En cours";
                    }
                    else if (achat.Termine == true)
                    {
                        statut = "Complétée";
                    }
                    else
                    {
                        statut = "Refusée";
                    }

                    orders.Add(new
                    {
                        No_Achat = achat.NoAchat,
                        Plats = plats.ToString().Trim(),
                        PrixTotal = prixTotal.ToString("F2"),
                        Statut = statut,
                        Date_Achat = achat.DateAchat?.ToString("yyyy/MM/dd") ?? ""
                    });
                }
                commande.Dispose();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur lors du chargement des commandes : " + ex.Message);
            }

            PastOrdersDataGrid.ItemsSource = orders;
            PastOrdersDataGrid.Tag = orders;
        }

        /// <summary>
        /// Ajoute la gare de la ligne du tableau comme gare de départ du graphe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AjouterDepart_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DataGridRow row = FindVisualParent<DataGridRow>(button);
            if (row != null)
            {
                var order = row.Item as dynamic;
                int noAchat = order.No_Achat;

                try
                {
                    MySqlCommand commande = BDD.Connexion.CreateCommand();
                    commande.CommandText = @"SELECT cc.No_Données, cc.Type_de_compte
                                    FROM Commande c
                                    INNER JOIN Compte_Cuisinier cc ON c.No_Compte_Cuisinier = cc.No_Compte_Cuisinier
                                    WHERE c.No_Achat = @noAchat";
                    commande.Parameters.AddWithValue("@noAchat", noAchat);
                    MySqlDataReader reader = commande.ExecuteReader();
                    int noDonnees = 0;
                    bool typeDeCompte = false;
                    if (reader.Read())
                    {
                        noDonnees = reader.GetInt32("No_Données");
                        typeDeCompte = reader.GetBoolean("Type_de_compte");
                    }
                    reader.Close();

                    string metroProche = "";
                    if (noDonnees > 0)
                    {
                        string query = typeDeCompte
                            ? $"SELECT Métro_le___proche FROM Données_Entreprise WHERE No_Données = @noDonnees"
                            : $"SELECT Métro_le___proche FROM Données_Particulier WHERE No_Données = @noDonnees";
                        commande.CommandText = query;
                        commande.Parameters.Clear();
                        commande.Parameters.AddWithValue("@noDonnees", noDonnees);
                        metroProche = commande.ExecuteScalar()?.ToString() ?? "";
                    }

                    commande.Dispose();

                    if (string.IsNullOrEmpty(metroProche))
                    {
                        MessageBox.Show("Erreur : Aucune gare de départ trouvée pour cette commande.");
                        return;
                    }

                    MessageBox.Show($"La gare de départ est : {metroProche}");
                    MetroDepart.Text = metroProche;
                    afficherChemin();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erreur lors de la récupération de la gare de départ : " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Ajoute la gare de la ligne du tableau comme gare d'arrivée du graphe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AjouterArrivee_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DataGridRow row = FindVisualParent<DataGridRow>(button);
            if (row != null)
            {
                var order = row.Item as dynamic;
                int noAchat = order.No_Achat;

                try
                {
                    MySqlCommand commande = BDD.Connexion.CreateCommand();
                    commande.CommandText = @"SELECT cc.No_Données, cc.Type_de_compte
                                    FROM Commande c
                                    INNER JOIN Compte_Cuisinier cc ON c.No_Compte_Cuisinier = cc.No_Compte_Cuisinier
                                    WHERE c.No_Achat = @noAchat";
                    commande.Parameters.AddWithValue("@noAchat", noAchat);
                    MySqlDataReader reader = commande.ExecuteReader();
                    int noDonnees = 0;
                    bool typeDeCompte = false;
                    if (reader.Read())
                    {
                        noDonnees = reader.GetInt32("No_Données");
                        typeDeCompte = reader.GetBoolean("Type_de_compte");
                    }
                    reader.Close();

                    string metroProche = "";
                    if (noDonnees > 0)
                    {
                        string query = typeDeCompte
                            ? $"SELECT Métro_le___proche FROM Données_Entreprise WHERE No_Données = @noDonnees"
                            : $"SELECT Métro_le___proche FROM Données_Particulier WHERE No_Données = @noDonnees";
                        commande.CommandText = query;
                        commande.Parameters.Clear();
                        commande.Parameters.AddWithValue("@noDonnees", noDonnees);
                        metroProche = commande.ExecuteScalar()?.ToString() ?? "";
                    }

                    commande.Dispose();

                    if (string.IsNullOrEmpty(metroProche))
                    {
                        MessageBox.Show("Erreur : Aucune gare d'arrivée trouvée pour cette commande.");
                        return;
                    }

                    MessageBox.Show($"La gare d'arrivée est : {metroProche}");
                    MetroArrivee.Text = metroProche;
                    afficherChemin();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erreur lors de la récupération de la gare d'arrivée : " + ex.Message);
                }
            }
        }

        #endregion

        #region Menu Cuisinier

        /// <summary>
        /// Affiche la page Cuisinier du menu lorsque le bouton est cliqué
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoTo_Menu_Cuisinier_Click(object sender, RoutedEventArgs e)
        {
            GoTo_MenuCuisinier();
        }

        /// <summary>
        /// Affiche la page Cuisinier du menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoTo_MenuCuisinier()
        {
            masqueMenu();
            Menu_Cuisinier.Visibility = Visibility.Visible;

            if (!BDD.Connecte || BDD.NoCompteCuisinier <= 0)
            {
                MessageBox.Show("Erreur : Vous devez être connecté en tant que cuisinier.");
                GoTo_Connexion();
                return;
            }

            try
            {
                MySqlCommand commande = BDD.Connexion.CreateCommand();
                commande.CommandText = $"SELECT Disponibilité FROM Compte_Cuisinier WHERE No_Compte_Cuisinier = @noCuisinier";
                commande.Parameters.AddWithValue("@noCuisinier", BDD.NoCompteCuisinier);
                object result = commande.ExecuteScalar();
                bool disponible = false;
                if (result != null && result != DBNull.Value)
                {
                    disponible = Convert.ToBoolean(result);
                }
                Disponibilite.IsChecked = disponible;
                commande.Dispose();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur lors du chargement de la disponibilité : " + ex.Message);
                Disponibilite.IsChecked = false;
            }

            CuisinierViewSelector.SelectionChanged -= CuisinierViewSelector_SelectionChanged;
            CuisinierViewSelector.SelectedIndex = 0;
            CuisinierViewSelector.SelectionChanged += CuisinierViewSelector_SelectionChanged;

            InitializePlatsView();
        }

        /// <summary>
        /// Affiche la page du menu selon le combobox CuisinierViewSelector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CuisinierViewSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CuisinierViewSelector.SelectedIndex == 0)
            {
                PlatsView.Visibility = Visibility.Visible;
                CommandesView.Visibility = Visibility.Collapsed;
                InitializePlatsView();
            }
            else
            {
                PlatsView.Visibility = Visibility.Collapsed;
                CommandesView.Visibility = Visibility.Visible;
                InitializeCommandesView();
            }
        }

        /// <summary>
        /// Définis si le cuisinier est disponible pour commander ou non
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Disponibilite_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlCommand commande = BDD.Connexion.CreateCommand();
                commande.CommandText = $"UPDATE Compte_Cuisinier SET Disponibilité = @disponible WHERE No_Compte_Cuisinier = {BDD.NoCompteCuisinier}";
                commande.Parameters.AddWithValue("@disponible", Disponibilite.IsChecked == true ? 1 : 0);
                commande.ExecuteNonQuery();
                commande.Dispose();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur lors de la mise à jour de la disponibilité : " + ex.Message);
            }
        }

        /// <summary>
        /// Affiche tous les plats proposés par le cuisinier
        /// </summary>
        private void InitializePlatsView()
        {
            PlatDetails.Visibility = Visibility.Collapsed;
            DetailNom.Text = "Détails du plat";

            List<object> plats = new List<object>();
            try
            {
                MySqlCommand commande = BDD.Connexion.CreateCommand();
                commande.CommandText = $"SELECT r.No_Repas, r.Nom, r.Prix_par_personnes " +
                                      $"FROM Repas r INNER JOIN Propose p ON r.No_Repas = p.No_Repas " +
                                      $"WHERE p.No_Compte_Cuisinier = @noCuisinier";
                commande.Parameters.AddWithValue("@noCuisinier", BDD.NoCompteCuisinier);
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    plats.Add(new
                    {
                        No_Repas = reader.GetInt32("No_Repas"),
                        Nom = reader.GetString("Nom"),
                        Prix = reader.GetDouble("Prix_par_personnes").ToString("F2")
                    });
                }
                reader.Close();
                commande.Dispose();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur lors du chargement des plats : " + ex.Message);
                return;
            }

            PlatsDataGrid.ItemsSource = plats;
            PlatsDataGrid.Tag = plats;

            PlatExistant.Items.Clear();
            PlatExistant.Items.Add(new ComboBoxItem { Content = "Sélectionner un plat existant" });
            try
            {
                MySqlCommand commande = BDD.Connexion.CreateCommand();
                commande.CommandText = $"SELECT No_Repas, Nom FROM Repas " +
                                      $"WHERE No_Repas NOT IN (SELECT No_Repas FROM Propose WHERE No_Compte_Cuisinier = {BDD.NoCompteCuisinier})";
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    ComboBoxItem item = new ComboBoxItem { Content = reader.GetString("Nom"), Tag = reader.GetInt32("No_Repas") };
                    PlatExistant.Items.Add(item);
                }
                reader.Close();
                commande.Dispose();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur lors du chargement des plats existants : " + ex.Message);
            }
            PlatExistant.SelectedIndex = 0;

            NouveauType.SelectedIndex = 0;
        }

        private void AfficherPlatCuisinier_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DataGridRow row = FindVisualParent<DataGridRow>(button);
            if (row != null)
            {
                var plat = row.Item as dynamic;
                int noRepas = plat.No_Repas;

                try
                {
                    MySqlCommand commande = BDD.Connexion.CreateCommand();
                    commande.CommandText = $"SELECT Nom, Type_de_plat__entrée__plat__dessert_, Pour_n_personnes, " +
                                          $"Prix_par_personnes, Régime_alimentaire, Nature " +
                                          $"FROM Repas WHERE No_Repas = @noRepas";
                    commande.Parameters.AddWithValue("@noRepas", noRepas);
                    MySqlDataReader reader = commande.ExecuteReader();
                    if (reader.Read())
                    {
                        DetailNom.Text = "Détails de " + reader.GetString("Nom");
                        DetailType.Text = reader.GetString("Type_de_plat__entrée__plat__dessert_");
                        DetailPersonnes.Text = reader.GetInt32("Pour_n_personnes").ToString();
                        DetailPrix.Text = reader.GetDouble("Prix_par_personnes").ToString("F2");
                        DetailRegime.Text = reader.IsDBNull(reader.GetOrdinal("Régime_alimentaire")) ? "" : reader.GetString("Régime_alimentaire");
                        DetailNature.Text = reader.IsDBNull(reader.GetOrdinal("Nature")) ? "" : reader.GetString("Nature");
                    }
                    reader.Close();

                    StringBuilder ingredients = new StringBuilder();
                    commande.CommandText = $"SELECT Nom, Quantité FROM Ingrédient WHERE No_Repas = @noRepas";
                    reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        string nom = reader.GetString("Nom");
                        string quantite = reader.IsDBNull(reader.GetOrdinal("Quantité")) ? "0" : reader["Quantité"].ToString();
                        ingredients.AppendLine($"{nom}: {quantite}");
                    }
                    reader.Close();
                    DetailIngredients.Text = ingredients.Length > 0 ? ingredients.ToString() : "Aucun ingrédient";
                    commande.Dispose();

                    PlatDetails.Visibility = Visibility.Visible;
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erreur lors du chargement des détails du plat : " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Supprime le plat des plats proposé lorsque le bouton supprimer en cliqué
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SupprimerPlat_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DataGridRow row = FindVisualParent<DataGridRow>(button);
            if (row != null)
            {
                var plat = row.Item as dynamic;
                int noRepas = plat.No_Repas;
                string nomPlat = plat.Nom;

                MessageBoxResult result = MessageBox.Show($"Voulez-vous vraiment supprimer le plat '{nomPlat}' ?",
                                                        "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        MySqlCommand commande = BDD.Connexion.CreateCommand();
                        commande.CommandText = $"DELETE FROM Propose WHERE No_Compte_Cuisinier = @noCuisinier AND No_Repas = @noRepas";
                        commande.Parameters.AddWithValue("@noCuisinier", BDD.NoCompteCuisinier);
                        commande.Parameters.AddWithValue("@noRepas", noRepas);
                        commande.ExecuteNonQuery();
                        commande.Dispose();
                        MessageBox.Show("Plat supprimé avec succès.");
                        InitializePlatsView();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Erreur lors de la suppression du plat : " + ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Ajoute le plat du combobox aux plats proposés
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AjouterPlatExistant_Click(object sender, RoutedEventArgs e)
        {
            if (PlatExistant.SelectedIndex <= 0)
            {
                MessageBox.Show("Veuillez sélectionner un plat existant.");
                return;
            }

            ComboBoxItem selectedItem = PlatExistant.SelectedItem as ComboBoxItem;
            int noRepas = (int)selectedItem.Tag;

            try
            {
                MySqlCommand commande = BDD.Connexion.CreateCommand();
                MessageBox.Show("no compte" + BDD.NoCompteCuisinier + "no repas" + noRepas);
                commande.CommandText = $"INSERT INTO Propose (No_Compte_Cuisinier, No_Repas) VALUES (@noCuisinier, @noRepas);";
                commande.Parameters.AddWithValue("@noCuisinier", BDD.NoCompteCuisinier);
                commande.Parameters.AddWithValue("@noRepas", noRepas);
                commande.ExecuteNonQuery();
                commande.Dispose();
                MessageBox.Show("Plat ajouté avec succès.");
                InitializePlatsView();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur lors de l'ajout du plat : " + ex.Message);

            }
        }

        /// <summary>
        /// Crée un nouveau plat et l'ajoute aux plats proposés
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreerNouveauPlat_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NouveauNom.Text) || NouveauNom.Text == "Nom du plat")
            {
                MessageBox.Show("Erreur : Le nom du plat ne peut pas être vide.");
                return;
            }
            if (NouveauNom.Text.Length > 50)
            {
                MessageBox.Show("Erreur : Le nom du plat ne peut pas dépasser 50 caractères.");
                return;
            }

            if (NouveauType.SelectedIndex < 0)
            {
                MessageBox.Show("Erreur : Vous devez sélectionner un type de plat.");
                return;
            }
            string typePlat = (NouveauType.SelectedItem as ComboBoxItem).Content.ToString();

            if (string.IsNullOrWhiteSpace(NouveauPersonnes.Text) || NouveauPersonnes.Text == "Pour n personnes")
            {
                MessageBox.Show("Erreur : Le nombre de personnes ne peut pas être vide.");
                return;
            }
            if (!int.TryParse(NouveauPersonnes.Text, out int pourNPersonnes) || pourNPersonnes <= 0)
            {
                MessageBox.Show("Erreur : Le nombre de personnes doit être un entier positif.");
                return;
            }

            if (string.IsNullOrWhiteSpace(NouveauPrix.Text) || NouveauPrix.Text == "Prix par personne (€)")
            {
                MessageBox.Show("Erreur : Le prix ne peut pas être vide.");
                return;
            }
            if (!double.TryParse(NouveauPrix.Text, out double prix) || prix <= 0)
            {
                MessageBox.Show("Erreur : Le prix doit être un nombre positif.");
                return;
            }

            string regime = string.IsNullOrWhiteSpace(NouveauRegime.Text) || NouveauRegime.Text == "Régime alimentaire" ? null : NouveauRegime.Text.Trim();
            if (regime != null && regime.Length > 50)
            {
                MessageBox.Show("Erreur : Le régime alimentaire ne peut pas dépasser 50 caractères.");
                return;
            }

            string nature = string.IsNullOrWhiteSpace(NouveauNature.Text) || NouveauNature.Text == "Nature" ? null : NouveauNature.Text.Trim();
            if (nature != null && nature.Length > 50)
            {
                MessageBox.Show("Erreur : La nature ne peut pas dépasser 50 caractères.");
                return;
            }

            List<(string Nom, string Quantite)> ingredients = new List<(string, string)>();
            if (!string.IsNullOrWhiteSpace(NouveauIngredients.Text) && NouveauIngredients.Text != "Ingrédients (nom:quantité, séparés par ;)")
            {
                string[] ingredientEntries = NouveauIngredients.Text.Split(';', StringSplitOptions.RemoveEmptyEntries);
                foreach (string ingredient in ingredientEntries)
                {
                    string[] parts = ingredient.Split(':');
                    if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]))
                    {
                        MessageBox.Show("Erreur : Format d'ingrédient invalide. Utilisez 'nom:quantité;nom:quantité'.");
                        return;
                    }
                    if (parts[0].Trim().Length > 50)
                    {
                        MessageBox.Show("Erreur : Le nom de l'ingrédient ne peut pas dépasser 50 caractères.");
                        return;
                    }
                    ingredients.Add((parts[0].Trim(), parts[1].Trim()));
                }
            }

            try
            {
                MySqlCommand commande = BDD.Connexion.CreateCommand();

                commande.CommandText = $"SELECT COUNT(*) FROM Repas WHERE Nom = @nom";
                commande.Parameters.AddWithValue("@nom", NouveauNom.Text.Trim());
                if (Convert.ToInt32(commande.ExecuteScalar()) > 0)
                {
                    MessageBox.Show("Erreur : Un plat avec ce nom existe déjà.");
                    commande.Dispose();
                    return;
                }

                commande.CommandText = $"INSERT INTO Repas (Nom, Type_de_plat__entrée__plat__dessert_, Pour_n_personnes, " +
                                      $"Prix_par_personnes, Régime_alimentaire, Nature, No_Photo) " +
                                      $"VALUES (@nom, @type, @personnes, @prix, @regime, @nature, NULL)";
                commande.Parameters.Clear();
                commande.Parameters.AddWithValue("@nom", NouveauNom.Text.Trim());
                commande.Parameters.AddWithValue("@type", typePlat);
                commande.Parameters.AddWithValue("@personnes", pourNPersonnes);
                commande.Parameters.AddWithValue("@prix", prix);
                commande.Parameters.AddWithValue("@regime", (object)regime ?? DBNull.Value);
                commande.Parameters.AddWithValue("@nature", (object)nature ?? DBNull.Value);
                commande.ExecuteNonQuery();

                commande.CommandText = $"SELECT LAST_INSERT_ID()";
                int noRepas = Convert.ToInt32(commande.ExecuteScalar());

                foreach (var ingredient in ingredients)
                {
                    commande.CommandText = $"INSERT INTO Ingrédient (No_Repas, Nom, Quantité) VALUES (@noRepas, @nom, @quantite)";
                    commande.Parameters.Clear();
                    commande.Parameters.AddWithValue("@noRepas", noRepas);
                    commande.Parameters.AddWithValue("@nom", ingredient.Nom);
                    commande.Parameters.AddWithValue("@quantite", ingredient.Quantite);
                    commande.ExecuteNonQuery();
                }

                commande.CommandText = $"INSERT INTO Propose (No_Compte_Cuisinier, No_Repas) VALUES (@noCuisinier, @noRepas)";
                commande.Parameters.Clear();
                commande.Parameters.AddWithValue("@noCuisinier", BDD.NoCompteCuisinier);
                commande.Parameters.AddWithValue("@noRepas", noRepas);
                commande.ExecuteNonQuery();

                commande.Dispose();
                MessageBox.Show("Nouveau plat créé et ajouté avec succès.");
                InitializePlatsView();

                NouveauNom.Text = "Nom du plat";
                NouveauType.SelectedIndex = 0;
                NouveauPersonnes.Text = "Pour n personnes";
                NouveauPrix.Text = "Prix par personne (€)";
                NouveauRegime.Text = "Régime alimentaire";
                NouveauNature.Text = "Nature";
                NouveauIngredients.Text = "Ingrédients (nom:quantité, séparés par ;)";
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur lors de la création du plat : " + ex.Message);
            }
        }

        /// <summary>
        /// Classe commande pour afficher la commande en cours
        /// </summary>
        private class Commande
        {
            public int No_Achat { get; set; }
            public string Plats { get; set; }
            public string PrixTotal { get; set; }
            public string Statut { get; set; }
            public bool CanModify { get; set; }
        }

        /// <summary>
        /// Affiche la commande du tableau des commandes lorsque le bouton afficher est cliqué
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AfficherCommande_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DataGridRow row = FindVisualParent<DataGridRow>(button);
            if (row != null)
            {
                var commandeItem = row.Item as Commande;
                int noAchat = commandeItem.No_Achat;

                try
                {
                    MySqlCommand commande = BDD.Connexion.CreateCommand();
                    commande.CommandText = $"SELECT c.No_Compte_Client, " +
                                          $"CASE " +
                                          $"    WHEN EXISTS (SELECT 1 FROM Compte_Client cc JOIN Données_Entreprise de ON cc.No_Données = de.No_Données WHERE cc.No_Compte_Client = c.No_Compte_Client) " +
                                          $"    THEN (SELECT Nom_Entreprise FROM Données_Entreprise de JOIN Compte_Client cc ON cc.No_Données = de.No_Données WHERE cc.No_Compte_Client = c.No_Compte_Client) " +
                                          $"    ELSE (SELECT CONCAT(Nom, ' ', Prénom) FROM Données_Particulier dp JOIN Compte_Client cc ON cc.No_Données = dp.No_Données WHERE cc.No_Compte_Client = c.No_Compte_Client) " +
                                          $"END AS ClientName " +
                                          $"FROM Commande c " +
                                          $"WHERE c.No_Achat = @noAchat";
                    commande.Parameters.AddWithValue("@noAchat", noAchat);
                    MySqlDataReader reader = commande.ExecuteReader();
                    string clientName = "";
                    if (reader.Read())
                    {
                        clientName = reader.GetString("ClientName");
                    }
                    reader.Close();

                    StringBuilder plats = new StringBuilder();
                    double prixTotal = 0;
                    DateTime? dateFabrication = null;
                    DateTime? datePeremption = null;

                    commande.CommandText = $"SELECT r.Nom, e.Quantité, r.Prix_par_personnes, e.Date_Fabrication, e.Date_péremption " +
                                          $"FROM Comporte co " +
                                          $"INNER JOIN Element_de_commande e ON co.No_Element = e.No_Element " +
                                          $"INNER JOIN Repas r ON e.No_Repas = r.No_Repas " +
                                          $"WHERE co.No_Achat = @noAchat";
                    commande.Parameters.Clear();
                    commande.Parameters.AddWithValue("@noAchat", noAchat);
                    reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        string nom = reader.GetString("Nom");
                        int quantite = reader.GetInt32("Quantité");
                        double prix = reader.GetDouble("Prix_par_personnes");
                        plats.AppendLine($"{nom} ({quantite})");
                        prixTotal += quantite * prix;
                        if (!reader.IsDBNull(reader.GetOrdinal("Date_Fabrication")))
                        {
                            dateFabrication = reader.GetDateTime("Date_Fabrication");
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("Date_péremption")))
                        {
                            datePeremption = reader.GetDateTime("Date_péremption");
                        }
                    }
                    reader.Close();

                    commande.CommandText = $"SELECT Terminé FROM Commande WHERE No_Achat = @noAchat";
                    commande.Parameters.Clear();
                    commande.Parameters.AddWithValue("@noAchat", noAchat);
                    object termineObj = commande.ExecuteScalar();
                    string statut;
                    if (termineObj == DBNull.Value)
                    {
                        statut = "En cours";
                    }
                    else if ((bool)termineObj)
                    {
                        statut = "Complétée";
                    }
                    else
                    {
                        statut = "Refusée";
                    }

                    CommandeDetailTitre.Text = $"Détails de la commande n°{noAchat}";
                    CommandeDetailClient.Text = $"Client : {clientName}";
                    CommandeDetailPlats.Text = $"Plats :\n{plats.ToString().Trim()}";
                    CommandeDetailPrix.Text = $"Prix Total : {prixTotal:F2} €";
                    CommandeDetailFabrication.Text = $"Date de Fabrication : {(dateFabrication.HasValue ? dateFabrication.Value.ToString("dd/MM/yyyy") : "N/A")}";
                    CommandeDetailPeremption.Text = $"Date de Péremption : {(datePeremption.HasValue ? datePeremption.Value.ToString("dd/MM/yyyy") : "N/A")}";
                    CommandeDetailStatut.Text = $"Statut : {statut}";

                    CommandeDetails.Visibility = Visibility.Visible;

                    commande.Dispose();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erreur lors du chargement des détails de la commande : " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Affiche toutes les commandes du cuinier dans un tableau
        /// </summary>
        private void InitializeCommandesView()
        {
            CommandeDetails.Visibility = Visibility.Collapsed;
            CommandeDetailTitre.Text = "Détails de la commande";

            List<Commande> commandes = new List<Commande>();
            try
            {
                MySqlCommand commande = BDD.Connexion.CreateCommand();
                commande.CommandText = $"SELECT c.No_Achat, c.No_Compte_Client, c.Terminé " +
                                      $"FROM Commande c " +
                                      $"WHERE c.No_Compte_Cuisinier = @noCuisinier";
                commande.Parameters.AddWithValue("@noCuisinier", BDD.NoCompteCuisinier);
                MySqlDataReader reader = commande.ExecuteReader();
                List<(int NoAchat, int NoClient, bool? Termine)> noAchats = new List<(int, int, bool?)>();
                while (reader.Read())
                {
                    bool? termine = reader.IsDBNull(reader.GetOrdinal("Terminé")) ? (bool?)null : reader.GetBoolean("Terminé");
                    noAchats.Add((reader.GetInt32("No_Achat"), reader.GetInt32("No_Compte_Client"), termine));
                }
                reader.Close();

                foreach (var achat in noAchats)
                {
                    StringBuilder plats = new StringBuilder();
                    double prixTotal = 0;

                    commande.CommandText = @"SELECT r.Nom, e.Quantité, r.Prix_par_personnes
                                          FROM Comporte co
                                          INNER JOIN Element_de_commande e ON co.No_Element = e.No_Element
                                          INNER JOIN Repas r ON e.No_Repas = r.No_Repas
                                          WHERE co.No_Achat = @noAchat";
                    commande.Parameters.Clear();
                    commande.Parameters.AddWithValue("@noAchat", achat.NoAchat);
                    reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        string nom = reader.GetString("Nom");
                        int quantite = reader.GetInt32("Quantité");
                        double prix = reader.GetDouble("Prix_par_personnes");
                        plats.AppendLine($"{nom} ({quantite})");
                        prixTotal += quantite * prix;
                    }
                    reader.Close();

                    string statut;
                    bool canModify;
                    if (achat.Termine == null)
                    {
                        statut = "En cours";
                        canModify = true;
                    }
                    else if (achat.Termine == true)
                    {
                        statut = "Complétée";
                        canModify = false;
                    }
                    else
                    {
                        statut = "Refusée";
                        canModify = false;
                    }

                    commandes.Add(new Commande
                    {
                        No_Achat = achat.NoAchat,
                        Plats = plats.ToString().Trim(),
                        PrixTotal = prixTotal.ToString("F2"),
                        Statut = statut,
                        CanModify = canModify
                    });
                }
                commande.Dispose();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur lors du chargement des commandes : " + ex.Message);
            }

            CommandesDataGrid.ItemsSource = commandes;
            CommandesDataGrid.Tag = commandes;
        }

        /// <summary>
        /// Valide une commande passée par un client sélectionnée (la marque teminée) lorque le bouton Valider est cliqué
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValiderCommande_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DataGridRow row = FindVisualParent<DataGridRow>(button);
            if (row != null)
            {
                var commande = row.Item as dynamic;
                int noAchat = commande.No_Achat;

                MessageBoxResult result = MessageBox.Show($"Voulez-vous valider la commande n°{noAchat} ?",
                                                        "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        MySqlCommand cmd = BDD.Connexion.CreateCommand();
                        cmd.CommandText = $"UPDATE Commande SET Terminé = 1 WHERE No_Achat = @noAchat";
                        cmd.Parameters.AddWithValue("@noAchat", noAchat);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        MessageBox.Show("Commande validée avec succès.");
                        InitializeCommandesView();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Erreur lors de la validation de la commande : " + ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Refuse une commande passée par un client lorque le bouton Refuser est cliqué
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefuserCommande_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DataGridRow row = FindVisualParent<DataGridRow>(button);
            if (row != null)
            {
                var commandeItem = row.Item as Commande;
                int noAchat = commandeItem.No_Achat;

                try
                {
                    MySqlCommand commande = BDD.Connexion.CreateCommand();
                    commande.CommandText = $"UPDATE Commande SET Terminé = 0 WHERE No_Achat = @noAchat";
                    commande.Parameters.AddWithValue("@noAchat", noAchat);
                    commande.ExecuteNonQuery();
                    commande.Dispose();

                    MessageBox.Show("Commande refusée avec succès.");
                    InitializeCommandesView();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erreur lors du refus de la commande : " + ex.Message);
                }
            }
        }


        #endregion

        #region Menu Graphe

        /// <summary>
        /// Initialise la carte et les combobox du menu graphe
        /// </summary>
        private void InitialisationMenuGraphe()
        {

            graphe.DessinerGraphe();
            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(@"graphe.png", UriKind.RelativeOrAbsolute);
            _image.EndInit();
            Img.Source = _image;


            List<string> ListeChoix = new List<string> { };
            foreach (Noeud<string> noeud in graphe.Noeuds)
            {
                if (!ListeChoix.Contains(noeud.Nom.Split(" - ")[0]))
                {
                    ListeChoix.Add(noeud.Nom.Split(" - ")[0]);
                }
            }
            ListeChoix.Sort();


            string gare = "";
            try
            {
                MySqlCommand codeCommande = BDD.Connexion.CreateCommand();
                string commande;
                if (BDD.TypeDeCompte)
                {
                    commande = $"SELECT Métro_le___proche FROM Données_Entreprise " +
                               $"WHERE No_Données = (SELECT No_Données FROM Compte_Client WHERE No_Compte_Client = @noClient)";
                }
                else
                {
                    commande = $"SELECT Métro_le___proche FROM Données_Particulier " +
                               $"WHERE No_Données = (SELECT No_Données FROM Compte_Client WHERE No_Compte_Client = @noClient)";
                }
                codeCommande.CommandText = commande;
                codeCommande.Parameters.AddWithValue("@noClient", BDD.NoCompteClient);
                MySqlDataReader reader = codeCommande.ExecuteReader();

                if (reader.Read())
                {
                    gare = reader.IsDBNull(reader.GetOrdinal("Métro_le___proche"))
                        ? ""
                        : reader.GetString("Métro_le___proche");
                }
                reader.Close();
                codeCommande.Dispose();

                if (string.IsNullOrEmpty(gare))
                {
                    MessageBox.Show("Erreur : Aucune gare d'arrivée trouvée pour ce compte.");
                    return;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur lors de la récupération de la gare du compte : " + ex.Message);
                return;
            }

            ListeChoix.Insert(0, $"Domicile : {gare}");



            MetroDepart.ItemsSource = ListeChoix;
            MetroDepart.SelectedIndex = 0;
            MetroArrivee.ItemsSource = ListeChoix;
            MetroArrivee.SelectedIndex = 0;
        }

        /// <summary>
        /// Affiche le menu graphe lorsque le bouton est cliqué
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoTo_Menu_Graphe_Click(object sender, RoutedEventArgs e)
        {
            masqueMenu();
            Menu_Graphe.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Affiche le chemin sur la carte, les étapes du chemin et le temps total selon les stations de départ et d'arrivée.
        /// </summary>
        private void afficherChemin()
        {

            Trajet.Visibility = Visibility.Visible;
            string ChoixDepart = MetroDepart.Text;
            try
            {
                ChoixDepart = ChoixDepart.Split("omicile : ")[1];
            } catch {}

            string ChoixArrivee = MetroArrivee.Text;
            try
            {
                ChoixArrivee = ChoixArrivee.Split("omicile : ")[1];
            }
            catch {}


            List<string> chemin = null;
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
            if (chemin == null)
            {
                Chemin.Text = "Il n'y a pas de chemin";
                graphe.DessinerGraphe();

                BitmapImage _image = new BitmapImage();
                _image.BeginInit();
                _image.CacheOption = BitmapCacheOption.None;
                _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                _image.CacheOption = BitmapCacheOption.OnLoad;
                _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                _image.UriSource = new Uri(@"graphe.png", UriKind.RelativeOrAbsolute);
                _image.EndInit();
                Img.Source = _image;


                //Img.Source = bitmap;

            }
            try
            {
                Chemin.Text = $"Chemin le plus court :\n{string.Join("\n", chemin)}\n\nTemps total : {graphe.Poids(chemin)}";
            }
            catch
            {
                return;
            }



            try
            {


                graphe.DessinerCheminGraphe(chemin);

                BitmapImage _image = new BitmapImage();
                _image.BeginInit();
                _image.CacheOption = BitmapCacheOption.None;
                _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                _image.CacheOption = BitmapCacheOption.OnLoad;
                _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                _image.UriSource = new Uri(@"graphe.png", UriKind.RelativeOrAbsolute);
                _image.EndInit();
                Img.Source = _image;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du dessin ou du chargement de l'image : " + ex.Message);
            }


        }

        /// <summary>
        /// Affiche le chemin sur la carte, les étapes du chemin et le temps total selon les stations de départ et d'arrivée. lorsque le bouton est cliqué
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTrajet_Click(object sender, EventArgs e)
        {
            afficherChemin();
        }


        #endregion

        #region Menu Compte

        /// <summary>
        /// Affiche le menu compte lorsque le bouton est cliqué
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoTo_Menu_Compte_Click(object sender, RoutedEventArgs e)
        {
            GoTo_MenuCompte();
        }

        /// <summary>
        /// Affiche le menu compte
        /// </summary>
        private void GoTo_MenuCompte()
        {
            masqueMenu();
            Menu_Compte.Visibility = Visibility.Visible;


            List<string> ListeChoix = new List<string>();
            foreach (Noeud<string> noeud in graphe.Noeuds)
            {
                string station = noeud.Nom.Split(" - ")[0];
                if (!ListeChoix.Contains(station))
                {
                    ListeChoix.Add(station);
                }
            }
            ListeChoix.Sort();
            ListeChoix.Insert(0, "Métro le plus proche");
            Metro2.ItemsSource = ListeChoix;
            Metro2.SelectedIndex = 0;

            if (BDD.Connecte && BDD.NoCompteClient > 0)
            {
                try
                {
                    string commande;
                    MySqlCommand codeCommande = BDD.Connexion.CreateCommand();

                    if (BDD.TypeDeCompte)
                    {
                        Compte_Particulier.Visibility = Visibility.Collapsed;
                        Compte_Entreprise2.Visibility = Visibility.Visible;

                        commande = $"SELECT Nom_Entreprise, Nom_Referent, Nom_Rue, No_Rue, Code_Postal, Ville, Métro_le___proche, Mot_de_Passe " +
                                   $"FROM Données_Entreprise WHERE No_Données = (SELECT No_Données FROM Compte_Client WHERE No_Compte_Client = {BDD.NoCompteClient})";
                        codeCommande.CommandText = commande;
                        MySqlDataReader reader = codeCommande.ExecuteReader();
                        if (reader.Read())
                        {
                            NomEntreprise2.Text = reader["Nom_Entreprise"].ToString();
                            NomReferent2.Text = reader["Nom_Referent"].ToString();
                            NomRue2.Text = reader["Nom_Rue"].ToString();
                            NoRue2.Text = reader["No_Rue"].ToString();
                            CodePostal2.Text = reader["Code_Postal"].ToString();
                            Ville2.Text = reader["Ville"].ToString();
                            Metro2.Text = reader["Métro_le___proche"].ToString();
                            MotDePasse2.Text = reader["Mot_de_Passe"].ToString();
                        }
                        reader.Close();
                    }
                    else
                    {
                        Compte_Entreprise2.Visibility = Visibility.Collapsed;
                        Compte_Particulier.Visibility = Visibility.Visible;

                        commande = $"SELECT Nom, Prénom, Numéro_de_téléphone, Adresse_mail, Nom_Rue, No_Rue, Code_Postal, Ville, Métro_le___proche, Mot_de_Passe " +
                                   $"FROM Données_Particulier WHERE No_Données = (SELECT No_Données FROM Compte_Client WHERE No_Compte_Client = {BDD.NoCompteClient})";
                        codeCommande.CommandText = commande;
                        MySqlDataReader reader = codeCommande.ExecuteReader();
                        if (reader.Read())
                        {
                            Nom2.Text = reader["Nom"].ToString();
                            Prenom2.Text = reader["Prénom"].ToString();
                            Telephone2.Text = reader["Numéro_de_téléphone"].ToString();
                            if(Telephone2.Text.Length == 9) { Telephone2.Text = "0" + Telephone2.Text; }
                            Email.Text = reader["Adresse_mail"].ToString();
                            NomRue2.Text = reader["Nom_Rue"].ToString();
                            NoRue2.Text = reader["No_Rue"].ToString();
                            CodePostal2.Text = reader["Code_Postal"].ToString();
                            Ville2.Text = reader["Ville"].ToString();
                            Metro2.Text = reader["Métro_le___proche"].ToString();
                            MotDePasse2.Text = reader["Mot_de_Passe"].ToString();
                        }
                        reader.Close();
                    }
                    codeCommande.Dispose();
                }
                catch (MySqlException e)
                {
                    MessageBox.Show("Erreur lors du chargement des données du compte : " + e.Message);
                }
            }
            else
            {
                MessageBox.Show("Erreur : Vous devez être connecté pour modifier votre compte.");
                GoTo_Connexion();
            }
        }

        /// <summary>
        /// Modifie les informations personnelles lorsque le bouton est cliqué
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MettreAJour_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(NomRue2.Text) || NomRue2.Text == "Nom de rue")
            {
                MessageBox.Show("Erreur : Le nom de la rue ne peut pas être vide.");
                return;
            }
            if (NomRue2.Text.Length > 50)
            {
                MessageBox.Show("Erreur : Le nom de la rue ne peut pas dépasser 50 caractères.");
                return;
            }

            if (string.IsNullOrWhiteSpace(NoRue2.Text) || NoRue2.Text == "Numéro de rue")
            {
                MessageBox.Show("Erreur : Le numéro de rue ne peut pas être vide.");
                return;
            }
            if (!int.TryParse(NoRue2.Text, out int noRue) || noRue <= 0)
            {
                MessageBox.Show("Erreur : Le numéro de rue doit être un entier positif.");
                return;
            }

            if (string.IsNullOrWhiteSpace(CodePostal2.Text) || CodePostal2.Text == "Code postal")
            {
                MessageBox.Show("Erreur : Le code postal ne peut pas être vide.");
                return;
            }
            if (!int.TryParse(CodePostal2.Text, out int codePostal) || codePostal <= 0 || CodePostal2.Text.Length != 5)
            {
                MessageBox.Show("Erreur : Le code postal doit être un entier positif de 5 chiffres.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Ville2.Text) || Ville2.Text == "Ville")
            {
                MessageBox.Show("Erreur : La ville ne peut pas être vide.");
                return;
            }
            if (Ville2.Text.Length > 50)
            {
                MessageBox.Show("Erreur : La ville ne peut pas dépasser 50 caractères.");
                return;
            }

            if (Metro2.SelectedIndex == 0 || Metro2.Text == "Métro le plus proche")
            {
                MessageBox.Show("Erreur : Vous devez sélectionner une station de métro.");
                return;
            }

            if (string.IsNullOrWhiteSpace(MotDePasse2.Text) || MotDePasse2.Text == "Mot de passe")
            {
                MessageBox.Show("Erreur : Le mot de passe ne peut pas être vide.");
                return;
            }
            if (MotDePasse2.Text.Length > 20)
            {
                MessageBox.Show("Erreur : Le mot de passe ne peut pas dépasser 20 caractères.");
                return;
            }

            try
            {
                MySqlCommand commande = BDD.Connexion.CreateCommand();
                string texteCommande = $"SELECT No_Données FROM Compte_Client WHERE No_Compte_Client = {BDD.NoCompteClient}";
                commande.CommandText = texteCommande;
                int noDonnees = Convert.ToInt32(commande.ExecuteScalar());

                if (BDD.TypeDeCompte)
                {
                    if (string.IsNullOrWhiteSpace(NomEntreprise2.Text) || NomEntreprise2.Text == "Nom de l'entreprise")
                    {
                        MessageBox.Show("Erreur : Le nom de l'entreprise ne peut pas être vide.");
                        return;
                    }
                    if (NomEntreprise2.Text.Length > 50)
                    {
                        MessageBox.Show("Erreur : Le nom de l'entreprise ne peut pas dépasser 50 caractères.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(NomReferent2.Text) || NomReferent2.Text == "Nom du référent")
                    {
                        MessageBox.Show("Erreur : Le nom du référent ne peut pas être vide.");
                        return;
                    }
                    if (NomReferent2.Text.Length > 20)
                    {
                        MessageBox.Show("Erreur : Le nom du référent ne peut pas dépasser 20 caractères.");
                        return;
                    }

                    texteCommande = $"SELECT COUNT(*) FROM Données_Entreprise WHERE Nom_Entreprise = @nomEntreprise AND No_Données != @noDonnees";
                    commande.CommandText = texteCommande;
                    commande.Parameters.AddWithValue("@nomEntreprise", NomEntreprise2.Text.Trim());
                    commande.Parameters.AddWithValue("@noDonnees", noDonnees);
                    int count = Convert.ToInt32(commande.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Erreur : Ce nom d'entreprise est déjà utilisé.");
                        return;
                    }

                    texteCommande = $"UPDATE Données_Entreprise SET Nom_Entreprise = @nomEntreprise, Nom_Referent = @nomReferent, " +
                                           $"Nom_Rue = @nomRue, No_Rue = @noRue, Code_Postal = @codePostal, Ville = @ville, " +
                                           $"Métro_le___proche = @metroProche, Mot_de_Passe = @motDePasse " +
                                           $"WHERE No_Données = @noDonnees";
                    commande.CommandText = texteCommande;
                    commande.Parameters.Clear();
                    commande.Parameters.AddWithValue("@nomEntreprise", NomEntreprise2.Text.Trim());
                    commande.Parameters.AddWithValue("@nomReferent", NomReferent2.Text.Trim());
                    commande.Parameters.AddWithValue("@nomRue", NomRue2.Text.Trim());
                    commande.Parameters.AddWithValue("@noRue", noRue);
                    commande.Parameters.AddWithValue("@codePostal", codePostal);
                    commande.Parameters.AddWithValue("@ville", Ville2.Text.Trim());
                    commande.Parameters.AddWithValue("@metroProche", Metro2.Text.Trim());
                    commande.Parameters.AddWithValue("@motDePasse", MotDePasse2.Text.Trim());
                    commande.Parameters.AddWithValue("@noDonnees", noDonnees);
                    commande.ExecuteNonQuery();
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(Nom2.Text) || Nom2.Text == "Nom")
                    {
                        MessageBox.Show("Erreur : Le nom ne peut pas être vide.");
                        return;
                    }
                    if (Nom2.Text.Length > 20)
                    {
                        MessageBox.Show("Erreur : Le nom ne peut pas dépasser 20 caractères.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(Prenom2.Text) || Prenom2.Text == "Prénom")
                    {
                        MessageBox.Show("Erreur : Le prénom ne peut pas être vide.");
                        return;
                    }
                    if (Prenom2.Text.Length > 20)
                    {
                        MessageBox.Show("Erreur : Le prénom ne peut pas dépasser 20 caractères.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(Telephone2.Text) || Telephone2.Text == "Numéro de téléphone")
                    {
                        MessageBox.Show("Erreur : Le numéro de téléphone ne peut pas être vide.");
                        return;
                    }
                    if (!long.TryParse(Telephone2.Text, out long telephone) || telephone <= 0 || Telephone2.Text.Length < 10)
                    {
                        MessageBox.Show("Erreur : Le numéro de téléphone doit être un numéro valide (10 chiffres minimum).");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(Email.Text) || Email.Text == "Adresse Mail")
                    {
                        MessageBox.Show("Erreur : L'adresse email ne peut pas être vide.");
                        return;
                    }
                    if (Email.Text.Length > 50)
                    {
                        MessageBox.Show("Erreur : L'adresse email ne peut pas dépasser 50 caractères.");
                        return;
                    }

                    texteCommande = $"SELECT COUNT(*) FROM Données_Particulier WHERE Adresse_mail = @email AND No_Données != @noDonnees";
                    commande.CommandText = texteCommande;
                    commande.Parameters.AddWithValue("@email", Email.Text.Trim());
                    commande.Parameters.AddWithValue("@noDonnees", noDonnees);
                    int count = Convert.ToInt32(commande.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Erreur : Cette adresse email est déjà utilisée.");
                        return;
                    }

                    texteCommande = $"UPDATE Données_Particulier SET Nom = @nom, Prénom = @prenom, Numéro_de_téléphone = @telephone, " +
                                           $"Adresse_mail = @email, Nom_Rue = @nomRue, No_Rue = @noRue, Code_Postal = @codePostal, " +
                                           $"Ville = @ville, Métro_le___proche = @metroProche, Mot_de_Passe = @motDePasse " +
                                           $"WHERE No_Données = @noDonnees";
                    commande.CommandText = texteCommande;
                    commande.Parameters.Clear();
                    commande.Parameters.AddWithValue("@nom", Nom2.Text.Trim());
                    commande.Parameters.AddWithValue("@prenom", Prenom2.Text.Trim());
                    commande.Parameters.AddWithValue("@telephone", Telephone2.Text.Trim());
                    commande.Parameters.AddWithValue("@email", Email.Text.Trim());
                    commande.Parameters.AddWithValue("@nomRue", NomRue2.Text.Trim());
                    commande.Parameters.AddWithValue("@noRue", noRue);
                    commande.Parameters.AddWithValue("@codePostal", codePostal);
                    commande.Parameters.AddWithValue("@ville", Ville2.Text.Trim());
                    commande.Parameters.AddWithValue("@metroProche", Metro2.Text.Trim());
                    commande.Parameters.AddWithValue("@motDePasse", MotDePasse2.Text.Trim());
                    commande.Parameters.AddWithValue("@noDonnees", noDonnees);
                    commande.ExecuteNonQuery();
                }

                commande.Dispose();
                MessageBox.Show("Compte mis à jour avec succès.");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur lors de la mise à jour du compte : " + ex.Message);
            }
        }

        /// <summary>
        /// Supprime le compte de l'utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SupprimerCompte_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer votre compte ? Cette action est irréversible.", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                MySqlCommand commande = BDD.Connexion.CreateCommand();
                string texteCommande = $"SELECT No_Données FROM Compte_Client WHERE No_Compte_Client = {BDD.NoCompteClient}";
                commande.CommandText = texteCommande;
                int noDonnees = Convert.ToInt32(commande.ExecuteScalar());

                texteCommande = $"DELETE FROM Compte_Cuisinier WHERE No_Données = @noDonnees AND Type_de_compte = @typeDeCompte";
                commande.CommandText = texteCommande;
                commande.Parameters.AddWithValue("@noDonnees", noDonnees);
                commande.Parameters.AddWithValue("@typeDeCompte", BDD.TypeDeCompte ? 1 : 0);
                commande.ExecuteNonQuery();
                commande.Parameters.Clear();

                texteCommande = $"DELETE FROM Compte_Client WHERE No_Compte_Client = @noCompteClient";
                commande.CommandText = texteCommande;
                commande.Parameters.AddWithValue("@noCompteClient", BDD.NoCompteClient);
                commande.ExecuteNonQuery();
                commande.Parameters.Clear();

                if (BDD.TypeDeCompte)
                {
                    texteCommande = $"DELETE FROM Données_Entreprise WHERE No_Données = @noDonnees";
                    commande.CommandText = texteCommande;
                    commande.Parameters.AddWithValue("@noDonnees", noDonnees);
                    commande.ExecuteNonQuery();
                }
                else
                {
                    texteCommande = $"DELETE FROM Données_Particulier WHERE No_Données = @noDonnees";
                    commande.CommandText = texteCommande;
                    commande.Parameters.AddWithValue("@noDonnees", noDonnees);
                    commande.ExecuteNonQuery();
                }

                commande.Dispose();
                MessageBox.Show("Compte supprimé.");
                Connecte = false;
                BDD.Connecte = false;
                BDD.NoCompteClient = 0;
                BDD.NoCompteCuisinier = 0;
                masqueMenu();
                Menu_Barre.Visibility = Visibility.Hidden;
                GoTo_Connexion();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur lors de la suppression du compte : " + ex.Message);
            }
        }
        #endregion


        /// <summary>
        /// masque les sous pages du menu
        /// </summary>
        private void masqueMenu()
        {
            Menu_Client.Visibility = Visibility.Hidden;
            Menu_Cuisinier.Visibility = Visibility.Hidden;
            Menu_Graphe.Visibility = Visibility.Hidden;
            Menu_Compte.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Renvoie la sstructure XAML parent d'une structure XAML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private T FindVisualParent<T>(DependencyObject obj) where T : DependencyObject
        {
            while (obj != null && !(obj is T))
            {
                obj = VisualTreeHelper.GetParent(obj);
            }
            return obj as T;
        }

        #endregion

        #region Admin

        /// <summary>
        /// Va à la page admin et intialise le nécessaire pour admin
        /// </summary>
        private void GoTo_Admin()
        {
            Admin.Visibility = Visibility.Visible;

            ChoixAdmin.SelectionChanged -= Changement_ChoixAdmin;
            ChoixAdmin.SelectedIndex = 0; 
            ChoixAdmin.SelectionChanged += Changement_ChoixAdmin;

            AdminDataGrid.ItemsSource = null;
            InitialisationGrapheCouleur();
        }

        /// <summary>
        /// Détecte les changements d'états et affiche la bonne page de l'admin selon la sélection du combobox ChoixAdmin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Changement_ChoixAdmin(object sender, SelectionChangedEventArgs e)
        {
            if (AdminDataGrid == null && Connecte)
            {
                MessageBox.Show("Erreur : La grille de données n'est pas encore initialisée. Veuillez réessayer.");
                return;
            } else if (AdminDataGrid == null) { return; }

            AdminDataGrid.ItemsSource = null; 

            if (ChoixAdmin.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedOption = selectedItem.Content.ToString();
                switch (selectedOption)
                {
                    case "Repas":
                        masqueAdmin();
                        AdminDataGrid.Visibility = Visibility.Visible;
                        DisplayTable("Repas");
                        break;
                    case "Données_Particulier":
                        masqueAdmin();
                        AdminDataGrid.Visibility = Visibility.Visible;
                        DisplayTable("Données_Particulier");
                        break;
                    case "Données_Entreprise":
                        masqueAdmin();
                        AdminDataGrid.Visibility = Visibility.Visible;
                        DisplayTable("Données_Entreprise");
                        break;
                    case "Compte_Client":
                        masqueAdmin();
                        AdminDataGrid.Visibility = Visibility.Visible;
                        DisplayTable("Compte_Client");
                        break;
                    case "Compte_Cuisinier":
                        masqueAdmin();
                        AdminDataGrid.Visibility = Visibility.Visible;
                        DisplayTable("Compte_Cuisinier");
                        break;
                    case "Element_de_commande":
                        masqueAdmin();
                        AdminDataGrid.Visibility = Visibility.Visible;
                        DisplayTable("Element_de_commande");
                        break;
                    case "Ingrédient":
                        masqueAdmin();
                        AdminDataGrid.Visibility = Visibility.Visible;
                        DisplayTable("Ingrédient");
                        break;
                    case "Commande":
                        masqueAdmin();
                        AdminDataGrid.Visibility = Visibility.Visible;
                        DisplayTable("Commande");
                        break;
                    case "Comporte":
                        masqueAdmin();
                        AdminDataGrid.Visibility = Visibility.Visible;
                        DisplayTable("Comporte");
                        break;
                    case "Propose":
                        masqueAdmin();
                        AdminDataGrid.Visibility = Visibility.Visible;
                        DisplayTable("Propose");
                        break;
                    case "Statistiques":
                        masqueAdmin();
                        Statistiques.Visibility = Visibility.Visible;
                        break;
                    case "Graphe":
                        masqueAdmin();
                        GrapheAdmin.Visibility = Visibility.Visible;
                        break;
                    case "Sauvegarde/Charge":
                        masqueAdmin();
                        SauvegarderCharger.Visibility=Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Affiche dans un tableau le contenu de la table sélectionnée
        /// </summary>
        /// <param name="tableName"></param>
        private void DisplayTable(string tableName)
        {
            try
            {
                MySqlCommand commande = BDD.Connexion.CreateCommand();
                commande.CommandText = $"SELECT * FROM {tableName}";
                MySqlDataReader reader = commande.ExecuteReader();

                System.Data.DataTable dataTable = new System.Data.DataTable();
                
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    dataTable.Columns.Add(columnName, typeof(object));
                }

                while (reader.Read())
                {
                    var row = dataTable.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[i] = reader.IsDBNull(i) ? DBNull.Value : reader.GetValue(i);
                    }
                    dataTable.Rows.Add(row);
                }
                reader.Close();
                commande.Dispose();

                AdminDataGrid.ItemsSource = dataTable.DefaultView;

                foreach (var column in AdminDataGrid.Columns)
                {
                    column.Header = column.Header.ToString().Replace("_", " ");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données de la table {tableName} : {ex.Message}");
            }
        }

        #region Statistiques
        /// <summary>
        /// affiche la page des statistiques
        /// </summary>
        private void GoTo_Statistiques()
        {
            Statistiques.Visibility = Visibility.Visible;
            StatsDataGrid.ItemsSource = null;
            StatsStartDatePicker.SelectedDate = DateTime.Now;
            StatsEndDatePicker.SelectedDate = DateTime.Now;
            ApplyStats_Click(null, null);
        }

        /// <summary>
        /// Génère et affiche les statistiques selon la plage temporelle sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyStats_Click(object sender, RoutedEventArgs e)
        {

            DateTime? startDate = StatsStartDatePicker.SelectedDate;
            DateTime? endDate = StatsEndDatePicker.SelectedDate;

            if (!startDate.HasValue || !endDate.HasValue)
            {
                MessageBox.Show("Veuillez sélectionner une période valide.");
                return;
            }

            if (startDate > endDate)
            {
                MessageBox.Show("La date de début doit être antérieure à la date de fin.");
                return;
            }

            System.Data.DataTable statsTable = new System.Data.DataTable();
            statsTable.Columns.Add("Statistique", typeof(string));
            statsTable.Columns.Add("Valeur", typeof(string));

            try
            {
                MySqlCommand commande = BDD.Connexion.CreateCommand();

                commande.CommandText = @"
                    SELECT cc.No_Compte_Cuisinier, cc.No_Données, COUNT(*) as Livraisons
                    FROM Commande c
                    INNER JOIN Compte_Cuisinier cc ON c.No_Compte_Cuisinier = cc.No_Compte_Cuisinier
                    WHERE c.Terminé = 1
                    GROUP BY cc.No_Compte_Cuisinier, cc.No_Données";
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    int cookId = reader.GetInt32("No_Compte_Cuisinier");
                    int dataId = reader.GetInt32("No_Données");
                    int deliveries = reader.GetInt32("Livraisons");
                    statsTable.Rows.Add($"Cuisinier {cookId} (Données {dataId})", deliveries.ToString());
                }
                reader.Close();

                commande.CommandText = @"
                    SELECT AVG(PrixTotal) as MoyennePrix
                    FROM (
                        SELECT c.No_Achat, SUM(r.Prix_par_personnes * e.Quantité) as PrixTotal
                        FROM Commande c
                        INNER JOIN Comporte co ON c.No_Achat = co.No_Achat
                        INNER JOIN Element_de_commande e ON co.No_Element = e.No_Element
                        INNER JOIN Repas r ON e.No_Repas = r.No_Repas
                        GROUP BY c.No_Achat
                    ) as OrderTotals";
                reader = commande.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        double avgPrice = reader.IsDBNull(reader.GetOrdinal("MoyennePrix")) ? 0 : reader.GetDouble("MoyennePrix");
                        statsTable.Rows.Add("Moyenne des prix des commandes", avgPrice.ToString("F2") + " €");
                    }
                }
                else
                {
                    statsTable.Rows.Add("Moyenne des prix des commandes", "0.00 €");
                }
                reader.Close();

                commande.CommandText = @"
                    SELECT AVG(OrderCount) as MoyenneCommandes
                    FROM (
                        SELECT cc.No_Compte_Client, COUNT(*) as OrderCount
                        FROM Commande c
                        INNER JOIN Compte_Client cc ON c.No_Compte_Client = cc.No_Compte_Client
                        GROUP BY cc.No_Compte_Client
                    ) as ClientOrders";
                reader = commande.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        double avgOrders = reader.IsDBNull(reader.GetOrdinal("MoyenneCommandes")) ? 0 : reader.GetDouble("MoyenneCommandes");
                        statsTable.Rows.Add("Moyenne des commandes par client", avgOrders.ToString("F2"));
                    }
                }
                else
                {
                    statsTable.Rows.Add("Moyenne des commandes par client", "0.00");
                }
                reader.Close();

                commande.CommandText = @"
                    SELECT c.No_Achat, c.Date_Achat
                    FROM Commande c
                    WHERE c.Date_Achat BETWEEN @startDate AND @endDate";
                commande.Parameters.AddWithValue("@startDate", startDate.Value);
                commande.Parameters.AddWithValue("@endDate", endDate.Value);
                reader = commande.ExecuteReader();
                StringBuilder ordersList = new StringBuilder();
                while (reader.Read())
                {
                    int orderId = reader.GetInt32("No_Achat");
                    DateTime orderDate = reader.GetDateTime("Date_Achat");
                    ordersList.AppendLine($"Commande {orderId} - {orderDate:yyyy/MM/dd}");
                }
                reader.Close();
                statsTable.Rows.Add("Commandes par période", ordersList.ToString().Trim() ?? "Aucune commande");

                commande.Dispose();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur lors de l'application des filtres : " + ex.Message);
            }

            StatsDataGrid.ItemsSource = statsTable.DefaultView;
        }

        #endregion

        #region Graphe couleur

        /// <summary>
        /// Génère et affiche le graphe des cuisinier/clients et si le graphe est bipartie / planaire
        /// </summary>
        private void InitialisationGrapheCouleur()
        {
            Graphe graphe = new Graphe();
            Random random = new Random();
            try
            {
                MySqlCommand commande = new MySqlCommand("SELECT No_Données FROM Données_Particulier", BDD.Connexion);
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    int noDonnee = reader.GetInt32("No_Données");
                    string nomNoeud = $"dp " + Convert.ToString(noDonnee);
                    double xCoord = random.NextDouble() * 500;
                    double yCoord = random.NextDouble() * 500;
                    graphe.AjouterNoeud(nomNoeud, xCoord, yCoord);
                }
                reader.Close();

                commande.CommandText = "SELECT No_Données FROM Données_entreprise";
                reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    int noDonnee = reader.GetInt32("No_Données");
                    string nomNoeud = $"de {noDonnee}";
                    double xCoord = random.NextDouble() * 500;
                    double yCoord = random.NextDouble() * 500;
                    graphe.AjouterNoeud(nomNoeud, xCoord, yCoord);
                }
                reader.Close();

                commande.CommandText = @"
                    SELECT c.No_Compte_Cuisinier, c.No_Compte_Client, 
                           dpc.No_Données AS CuisinierParticulierDonnee, 
                           dce.No_Données AS CuisinierEntrepriseDonnee, 
                           cp.No_Données AS ClientParticulierDonnee, 
                           ce.No_Données AS ClientEntrepriseDonnee
                    FROM Commande c
                    LEFT JOIN Compte_Cuisinier cc ON c.No_Compte_Cuisinier = cc.No_Compte_Cuisinier
                    LEFT JOIN Données_Particulier dpc ON cc.No_Données = dpc.No_Données
                    LEFT JOIN Données_entreprise dce ON cc.No_Données = dce.No_Données
                    LEFT JOIN Compte_Client cc2 ON c.No_Compte_Client = cc2.No_Compte_Client
                    LEFT JOIN Données_Particulier cp ON cc2.No_Données = cp.No_Données
                    LEFT JOIN Données_entreprise ce ON cc2.No_Données = ce.No_Données
                    WHERE c.Terminé = 1";
                reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    int? cuisinierParticulierDonnee = reader.IsDBNull(reader.GetOrdinal("CuisinierParticulierDonnee")) ? (int?)null : reader.GetInt32("CuisinierParticulierDonnee");
                    int? cuisinierEntrepriseDonnee = reader.IsDBNull(reader.GetOrdinal("CuisinierEntrepriseDonnee")) ? (int?)null : reader.GetInt32("CuisinierEntrepriseDonnee");
                    string nomCuisinier = cuisinierParticulierDonnee.HasValue ? $"dp {cuisinierParticulierDonnee}" : $"de {cuisinierEntrepriseDonnee}";
                    int? clientParticulierDonnee = reader.IsDBNull(reader.GetOrdinal("ClientParticulierDonnee")) ? (int?)null : reader.GetInt32("ClientParticulierDonnee");
                    int? clientEntrepriseDonnee = reader.IsDBNull(reader.GetOrdinal("ClientEntrepriseDonnee")) ? (int?)null : reader.GetInt32("ClientEntrepriseDonnee");
                    string nomClient = clientParticulierDonnee.HasValue ? $"dp {clientParticulierDonnee}" : $"de {clientEntrepriseDonnee}";

                    if (!string.IsNullOrEmpty(nomCuisinier) && !string.IsNullOrEmpty(nomClient))
                    {
                        graphe.AjouterLien(nomCuisinier, nomClient, 1.0);
                    }
                }
                reader.Close();
                commande.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur lors de la génération du graphe : " + ex.Message);
            }



            int n = graphe.WelshPowell();
            NbCouleur.Text += Convert.ToString(n);
            if (n == 2)
            {
                Bipartie.Text = "Le graphe est bipartie car sa coloration a deux couleurs.";
            }
            else
            {
                Bipartie.Text = "Le graphe n'est pas bipartie car sa coloration n'a pas deux couleurs.";
            }

            if(n <= 4)
            {
                Planaire.Text = "Le graphe est planiare car sa coloration a 4 couleurs ou moins.";
            }
            else
            {
                Planaire.Text = "Le graphe n'est pas planiare car sa coloration n'a pas 4 couleurs ou moins.";
            }

            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.CacheOption = BitmapCacheOption.None;
            _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _image.UriSource = new Uri(@"Couleur.png", UriKind.RelativeOrAbsolute);
            _image.EndInit();
            ImgCouleur.Source = _image;

        }


        #endregion

        #region Sauvegarder / Charger
        /// <summary>
        /// Sauvegarde/enregistre la base de donnée en XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SauvegarderXML_Click(object sender, EventArgs e)
        {
            BDD.SauvegarderXML();
        }

        /// <summary>
        /// Charge la base de donnée à partir d'un XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChargerXML_Click(object sender, EventArgs e)
        {
            BDD.CreationTables();
            BDD.ChargerXML();
        }

        /// <summary>
        /// Sauvegarde/enregistre la base de donnée en JSON
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SauvegarderJSON_Click(object sender, EventArgs e)
        {
            BDD.SauvegarderJSON();
        }

        /// <summary>
        /// Charge la base de donnée à partir d'un JSON
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChargerJSON_Click(object sender, EventArgs e)
        {
            BDD.CreationTables();
            BDD.ChargerJSON();
        }
        #endregion

        /// <summary>
        /// masque les sous pages du menu admin
        /// </summary>
        private void masqueAdmin()
        {
            SauvegarderCharger.Visibility = Visibility.Collapsed;
            GrapheAdmin.Visibility = Visibility.Collapsed;
            Statistiques.Visibility = Visibility.Collapsed;
            AdminDataGrid.Visibility = Visibility.Collapsed;
        }

        #endregion

    }
}
