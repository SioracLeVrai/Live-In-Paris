using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using SkiaSharp;
using System.ComponentModel;
using MySql.Data.MySqlClient;
using System.Windows.Shapes;


namespace LivInParis_Maximilien_Gregoire_Killian
{
    internal class Graphe
    {
        public List<Noeud<string>> Noeuds;
        public List<Lien<double>> Liens;

        public Graphe()
        {
            Noeuds = new List<Noeud<string>>();
            Liens = new List<Lien<double>>();
        }

        public void AjouterNoeud(string Nom, double x_coord, double y_coord)
        {
            foreach (Noeud<string> noeud in Noeuds)
            {
                if (noeud.Nom == Nom)
                    return; // Le noeud existe deja
            }

            Noeuds.Add(new Noeud<string>(Nom, x_coord, y_coord));
        }

        /// <summary>
        /// Ajoute un lien entre deux noeuds avec un poid pris en paramètre
        /// </summary>
        /// <param name="Nom1"></param>
        /// <param name="Nom2"></param>
        /// <param name="poids"></param>
        public void AjouterLien(string NomDepart, string NomArrivee, double poids)
        {
            Noeud<string> Depart = null;
            Noeud<string> Arrivee = null;

            foreach (Noeud<string> noeud in Noeuds)
            {
                if (noeud.Nom == NomDepart)
                    Depart = noeud;
                if (noeud.Nom == NomArrivee)
                    Arrivee = noeud;
            }

            if (Depart == null || Arrivee == null)
            {
                Console.WriteLine("Erreur : Un ou les deux nœuds n'existent pas.");
                return;
            }

            if (poids == null) { poids = 0; }

            Lien<double> lien = new Lien<double>(Depart, Arrivee, poids);
            Liens.Add(lien);

        }

        /// <summary>
        /// Affiche tout les liens existant dans le graph
        /// </summary>
        public string AffichageLiens()
        {
            string txt = "";
            foreach (Lien<double> liens in Liens)
            {
                txt += "\nLien entre " + liens.Depart.Nom + " et " + liens.Arrivee.Nom + " ayant le poid " + liens.Poids + ".";
            }
            return txt;
        }

        /// <summary>
        /// Affiche la matrice d'adjacence du graph
        /// </summary>
        public string MatriceAdjacence()
        {
            int taille = Noeuds.Count;
            double[,] mat = new double[taille, taille];

            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    mat[i, j] = 0; // Initialisation à 0
                }
            }

            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    foreach (Lien<double> lien in Liens)
                    {
                        if ((lien.Depart == Noeuds[i] && lien.Arrivee == Noeuds[j]) || (lien.Depart == Noeuds[j] && lien.Arrivee == Noeuds[i]))
                        {
                            mat[i, j] = lien.Poids;
                        }
                    }
                }
            }

            string txt = "\t";

            foreach (Noeud<string> noeud in Noeuds)
            {
                txt += noeud.Nom + " ";
            }

            txt += "\n";

            for (int i = 0; i < taille; i++)
            {
                txt += Noeuds[i].Nom + "\t";
                for (int j = 0; j < taille; j++)
                {
                    txt += mat[i, j] + "  ";

                }
                txt += "\n";
            }
            return txt;
        }



        /// <summary>
        /// Rempli le graph à partir des fichiers dans le dossier "Graph".
        /// </summary>
        public void CreationGraph()
        {
            // Ajout des stations
            string[] texte = null;
            string Chemin = "Graph/ListeStations.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { Console.WriteLine("\nEchec lors de la lecture du fichier" + e.Message); return; }

            string[][] tab = new string[texte.Length][];
            for (int i = 0; i < texte.Length; i++)
            {
                tab[i] = texte[i].Split(",");
                for (int j = 0; j < tab[i].Length; j++)
                {
                    tab[i][j] = tab[i][j].Trim();
                }
            }


            foreach (string[] ligne in tab)
            {
                try
                {
                    AjouterNoeud(ligne[0], Convert.ToDouble(ligne[1].Replace(".", ",")), Convert.ToDouble(ligne[2].Replace(".", ",")));
                }
                catch (IndexOutOfRangeException e) { Console.WriteLine(e.Message); }

            }

            // Ajout des liaisons
            texte = null;
            Chemin = "Graph/ListeAdjacences.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { Console.WriteLine("\nEchec lors de la lecture du fichier" + e.Message); return; }

            tab = new string[texte.Length][];
            for (int i = 0; i < texte.Length; i++)
            {
                tab[i] = texte[i].Split(",");
                for (int j = 0; j < tab[i].Length; j++)
                {
                    tab[i][j] = tab[i][j].Trim();
                }
            }
            foreach (string[] ligne in tab)
            {
                try
                {
                    AjouterLien(ligne[0], ligne[1], Convert.ToDouble(ligne[2]));
                    AjouterLien(ligne[1], ligne[0], Convert.ToDouble(ligne[2]));
                }
                catch (IndexOutOfRangeException e) { Console.WriteLine("\nEchec lors de l'ajout du lien" + e.Message); }
            }
        }

        /// <summary>
        /// Renvoie une liste les noeuds avec lesquels un noeud pris en paramètre à des liens
        /// </summary>
        /// <param name="noeud"></param>
        /// <returns></returns>
        private List<Noeud<string>> listeVoisin(Noeud<string> noeud)
        {
            List<Noeud<string>> voisin = new List<Noeud<string>>();
            foreach (Lien<double> lien in Liens)
            {
                if (lien.Depart == noeud)
                    voisin.Add(lien.Arrivee);
                else if (lien.Arrivee == noeud)
                    voisin.Add(lien.Depart);
            }
            return voisin;
        }

        #region Algo de graphe propre au rendu 1 non réutilisé
        /// <summary>
        /// Parcours en largeur le graph
        /// </summary>
        /// <param name="NomDebut"></param>
        /// <returns></returns>
        public string ParcoursEnLargeur(string NomDebut)
        {
            string resultat = "";

            Noeud<string> debut = null;
            foreach (Noeud<string> noeud in Noeuds)
            {
                if (noeud.Nom == NomDebut)
                {
                    debut = noeud;
                }
            }
            if (debut == null)
            {
                Console.WriteLine("Le noeud n'a pas pu être trouvé.");
                return null;
            }


            Queue<Noeud<string>> file = new Queue<Noeud<string>>();
            List<Noeud<string>> dejaVisite = new List<Noeud<string>>();

            file.Enqueue(debut);
            dejaVisite.Add(debut);
            bool tempo = false;
            while (file.Count > 0)
            {
                Noeud<string> enCours = file.Dequeue();
                resultat += enCours.Nom + ", ";

                foreach (Noeud<string> voisin in listeVoisin(enCours))
                {
                    tempo = false;
                    foreach (Noeud<string> noeud in dejaVisite)
                    {
                        if (noeud == voisin)
                        {
                            tempo = true;
                        }
                    }

                    if (!tempo)
                    {
                        file.Enqueue(voisin);
                        dejaVisite.Add(voisin);
                    }
                }
            }

            return resultat;
        }

        /// <summary>
        /// Parcour en profondeurs le graph de façon récusive en appelant ParcoursEnProfondeurRecursif
        /// </summary>
        /// <param name="NomDebut"></param>
        /// <returns></returns>
        public string ParcoursEnProfondeur(string NomDebut)
        {
            Noeud<string> debut = null;
            foreach (Noeud<string> noeud in Noeuds)
            {
                if (noeud.Nom == NomDebut)
                {
                    debut = noeud;
                }
            }
            if (debut == null)
            {
                Console.WriteLine("Le noeud n'a pas pu être trouvé.");
                return null;
            }

            List<Noeud<string>> dejaVisite = new List<Noeud<string>>();
            return ParcoursEnProfondeurRecursif(debut, dejaVisite);
        }

        /// <summary>
        /// Pars d'un noeud et fais le parcours en profondeur de ses enfants
        /// </summary>
        /// <param name="enCours"></param>
        /// <param name="dejaVisite"></param>
        /// <returns></returns>
        private string ParcoursEnProfondeurRecursif(Noeud<string> enCours, List<Noeud<string>> dejaVisite)
        {
            string resultat = null;
            bool tempo = false;
            foreach (Noeud<string> noeud in dejaVisite)
            {
                if (noeud == enCours)
                {
                    tempo = true;
                }
            }

            if (!tempo)
            {
                resultat = enCours.Nom + ", ";

                dejaVisite.Add(enCours);

                foreach (Noeud<string> voisin in listeVoisin(enCours))
                {
                    resultat += ParcoursEnProfondeurRecursif(voisin, dejaVisite);
                }
            }
            return resultat;
        }

        /// <summary>
        /// renvoie true si le graph est connexe false sinon
        /// </summary>
        /// <returns></returns>
        public bool Connexe()
        {
            if (Noeuds.Count == 0) return true;

            List<Noeud<string>> dejaVisite = new List<Noeud<string>>();
            ParcoursEnProfondeurRecursif(Noeuds[0], dejaVisite);

            return dejaVisite.Count == Noeuds.Count;
        }

        /// <summary>
        /// renvoie un cycle du graph en débutant la recherche par le Noeud enCours pris en paramètre
        /// </summary>
        /// <param name="enCours"></param>
        /// <param name="parent"></param>
        /// <param name="dejaVisite"></param>
        /// <returns></returns>
        public string Cycle(Noeud<string> enCours, Noeud<string> parent = null, List<Noeud<string>> dejaVisite = null)
        {
            if (dejaVisite == null)
            {
                dejaVisite = new List<Noeud<string>>();
            }
            dejaVisite.Add(enCours);

            foreach (Noeud<string> voisin in listeVoisin(enCours))
            {
                if (voisin != parent)
                {
                    if (dejaVisite.Contains(voisin))
                    {
                        int i = dejaVisite.IndexOf(voisin);
                        string resultat = "";
                        for (int j = i; j < dejaVisite.Count; j++)
                        {
                            resultat += dejaVisite[j].Nom + " ";
                        }
                        resultat += dejaVisite[i].Nom;
                        return resultat;
                    }
                    else
                    {
                        if (Cycle(voisin, enCours, new List<Noeud<string>>(dejaVisite)) != null)
                        {
                            return Cycle(voisin, enCours, new List<Noeud<string>>(dejaVisite));
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        /// <summary>
        /// Calcule la distance Haversine entre deux stations en utilisant leurs coordonnées x et y.
        /// </summary>
        /// <param name="Nom1">Nom du premier noeud</param>
        /// <param name="Nom2">Nom du deuxième noeud</param>
        /// <returns></returns>
        public double Haversine(string Nom1, string Nom2)
        {
            Noeud<string> noeud1 = null;
            Noeud<string> noeud2 = null;
            foreach (Noeud<string> noeud in Noeuds)
            {
                if (noeud.Nom == Nom1) noeud1 = noeud;
                if (noeud.Nom == Nom2) noeud2 = noeud;
            }
            if (noeud1 == null || noeud2 == null)
            {
                Console.WriteLine("Erreur : Un ou les deux nœuds sont introuvables.");
                return -1;
            }

            double lat1 = noeud1.Y_Coord * Math.PI / 180;
            double lon1 = noeud1.X_Coord * Math.PI / 180;
            double lat2 = noeud2.Y_Coord * Math.PI / 180;
            double lon2 = noeud2.X_Coord * Math.PI / 180;

            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);


            double distance = 6371 * 2 * Math.Asin(Math.Sqrt(a));
            return distance;
        }

        /// <summary>
        /// Algorithme de Dijkstra pour trouver le plus court chemin entre deux noeuds.
        /// </summary>
        /// <param name="NomDepart">Nom du noeud de départ</param>
        /// <param name="NomArrivee">Nom du noeud d'arrivée</param>
        /// <returns>Liste des Noms des noeuds du chemin du départ à l'arrivée</returns>
        public List<string> Dijkstra(string NomDepart, string NomArrivee)
        {
            Noeud<string> Depart = null;
            Noeud<string> Arrivee = null;
            foreach (Noeud<string> noeud in Noeuds)
            {
                if (noeud.Nom == NomDepart) Depart = noeud;
                if (noeud.Nom == NomArrivee) Arrivee = noeud;
            }
            if (Depart == null || Arrivee == null)
            {
                return null;
            }

            Dictionary<Noeud<string>, double> distances = new Dictionary<Noeud<string>, double>();
            Dictionary<Noeud<string>, Noeud<string>> predecesseurs = new Dictionary<Noeud<string>, Noeud<string>>();
            List<Noeud<string>> nonVisites = new List<Noeud<string>>();

            foreach (Noeud<string> noeud in Noeuds)
            {
                distances[noeud] = -1;
                predecesseurs[noeud] = null;
                nonVisites.Add(noeud);
            }
            distances[Depart] = 0;

            while (nonVisites.Count > 0)
            {
                Noeud<string> courant = null;
                double minDistance = double.MaxValue;
                foreach (Noeud<string> noeud in nonVisites)
                {
                    double dist = distances[noeud];
                    if (dist >= 0 && dist < minDistance)
                    {
                        minDistance = dist;
                        courant = noeud;
                    }
                }

                if (courant == null) break;
                nonVisites.Remove(courant);

                if (courant == Arrivee) break;

                List<Noeud<string>> voisins = listeVoisin(courant);
                foreach (Noeud<string> voisin in voisins)
                {
                    double poidsLien = 0;
                    foreach (Lien<double> lien in Liens)
                    {
                        if ((lien.Depart == courant && lien.Arrivee == voisin) || (lien.Depart == voisin && lien.Arrivee == courant))
                        {
                            poidsLien = lien.Poids;
                            break;
                        }
                    }

                    double distanceAlternative = distances[courant] + poidsLien;
                    double distVoisin = distances[voisin];
                    if (distVoisin == -1 || distanceAlternative < distVoisin)
                    {
                        distances[voisin] = distanceAlternative;
                        predecesseurs[voisin] = courant;
                    }
                }
            }

            if (distances[Arrivee] == -1)
            {
                Console.WriteLine("Aucun chemin trouvé entre " + NomDepart + " et " + NomArrivee + ".");
                return null;
            }

            List<string> chemin = new List<string>();
            Noeud<string> noeudActuel = Arrivee;
            while (noeudActuel != null)
            {
                chemin.Insert(0, noeudActuel.Nom);
                noeudActuel = predecesseurs[noeudActuel];
            }

            return chemin;
        }

        /// <summary>
        /// Algorithme de Bellman-Ford pour trouver le plus court chemin entre deux noeuds.
        /// </summary>
        /// <param name="NomDepart">Nom du noeud de départ</param>
        /// <param name="NomArrivee">Nom du noeud d'arrivée</param>
        /// <returns>Liste des Noms des noeuds du chemin du départ à l'arrivée</returns>
        public List<string> BellmanFord(string NomDepart, string NomArrivee)
        {
            Noeud<string> Depart = Noeuds.FirstOrDefault(n => n.Nom == NomDepart);
            Noeud<string> Arrivee = Noeuds.FirstOrDefault(n => n.Nom == NomArrivee);

            if (Depart == null || Arrivee == null)
            {
                Console.WriteLine("Erreur : Noeud de départ ou d'arrivée introuvable.");
                return null;
            }

            Dictionary<Noeud<string>, double> distances = Noeuds.ToDictionary(n => n, n => double.PositiveInfinity);
            Dictionary<Noeud<string>, Noeud<string>> predecesseurs = new Dictionary<Noeud<string>, Noeud<string>>();
            distances[Depart] = 0;

            for (int i = 0; i < Noeuds.Count - 1; i++)
            {
                foreach (Lien<double> lien in Liens)
                {
                    Noeud<string> u = lien.Depart;
                    Noeud<string> v = lien.Arrivee;
                    double poids = lien.Poids;

                    if (distances[u] + poids < distances[v])
                    {
                        distances[v] = distances[u] + poids;
                        predecesseurs[v] = u;
                    }
                }
            }

            // Vérification des cycles négatifs
            foreach (Lien<double> lien in Liens)
            {
                Noeud<string> u = lien.Depart;
                Noeud<string> v = lien.Arrivee;
                double poids = lien.Poids;

                if (distances[u] + poids < distances[v])
                {
                    Console.WriteLine("Erreur : Cycle négatif détecté dans le graphe.");
                    return null;
                }
            }

            if (double.IsPositiveInfinity(distances[Arrivee]))
            {
                Console.WriteLine($"Aucun chemin trouvé entre {NomDepart} et {NomArrivee}.");
                return null;
            }

            // Reconstruction du chemin
            List<string> chemin = new List<string>();
            for (Noeud<string> noeudActuel = Arrivee; noeudActuel != null; noeudActuel = predecesseurs.GetValueOrDefault(noeudActuel))
            {
                chemin.Insert(0, noeudActuel.Nom);
            }

            return chemin;
        }


        /// <summary>
        /// Algorithme de Floyd-Warshall pour trouver le plus court chemin entre deux noeuds.
        /// </summary>
        /// <param name="NomDepart">Nom du noeud de départ</param>
        /// <param name="NomArrivee">Nom du noeud d'arrivée</param>
        /// <returns>Liste des Noms des noeuds du chemin du départ à l'arrivée</returns>
        public List<string> FloydWarshall(string NomDepart, string NomArrivee)
        {
            // Trouver les noeuds de départ et d'arrivée
            Noeud<string> Depart = null;
            Noeud<string> Arrivee = null;
            foreach (Noeud<string> noeud in Noeuds)
            {
                if (noeud.Nom == NomDepart) Depart = noeud;
                if (noeud.Nom == NomArrivee) Arrivee = noeud;
            }
            if (Depart == null || Arrivee == null)
            {
                Console.WriteLine("Erreur : Noeud de départ ou d'arrivée introuvable.");
                return null;
            }


            if (Depart == Arrivee)
            {
                return new List<string> { Depart.Nom };
            }

            List<Noeud<string>> noeuds = Noeuds.ToList();
            int N = noeuds.Count;

            var noeud_to_index = noeuds.Select((n, idx) => new { n, idx }).ToDictionary(x => x.n, x => x.idx);

            double[,] distances = new double[N, N];
            Noeud<string>[,] predecessors = new Noeud<string>[N, N];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (i == j)
                    {
                        distances[i, j] = 0;
                        predecessors[i, j] = null;
                    }
                    else
                    {
                        double weight = 0;

                        bool ok = false;
                        foreach (Lien<double> lien in Liens)
                        {
                            if ((lien.Depart == noeuds[i] && lien.Arrivee == noeuds[j]) || (lien.Depart == noeuds[j] && lien.Arrivee == noeuds[i]))
                            {
                                weight = lien.Poids;
                                ok = true;
                            }
                        }
                        if (!ok) { weight = double.PositiveInfinity; }

                        distances[i, j] = weight;
                        if (weight != double.PositiveInfinity)
                        {
                            predecessors[i, j] = noeuds[i]; // Prédécesseur initial pour arêtes directes
                        }
                        else
                        {
                            predecessors[i, j] = null;
                        }
                    }
                }
            }

            for (int k = 0; k < N; k++)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        if (distances[i, k] != double.PositiveInfinity && distances[k, j] != double.PositiveInfinity)
                        {
                            double newDistance = distances[i, k] + distances[k, j];
                            if (newDistance < distances[i, j])
                            {
                                distances[i, j] = newDistance;
                                predecessors[i, j] = noeuds[k];
                            }
                        }
                    }
                }
            }

            // Vérifier les cycles négatifs
            for (int i = 0; i < N; i++)
            {
                if (distances[i, i] < 0)
                {
                    Console.WriteLine("Erreur : Cycle négatif détecté dans le graphe.");
                    return null;
                }
            }

            int iDepart = noeud_to_index[Depart];
            int iArrivee = noeud_to_index[Arrivee];

            if (distances[iDepart, iArrivee] == double.PositiveInfinity)
            {
                Console.WriteLine("Aucun chemin trouvé entre " + Depart.Nom + " et " + Arrivee.Nom + ".");
                return null;
            }

            // Reconstruction du chemin
            List<Noeud<string>> chemin = new List<Noeud<string>>();
            Noeud<string> EnCours = Arrivee;
            while (EnCours != null && EnCours != Depart)
            {
                chemin.Add(EnCours);
                int idx_EnCours = noeud_to_index[EnCours];
                Noeud<string> prev = predecessors[iDepart, idx_EnCours];
                if (prev == null)
                {
                    Console.WriteLine("Erreur dans la reconstruction du chemin.");
                    return null;
                }
                EnCours = prev;
            }

            if (EnCours == Depart)
            {
                chemin.Add(Depart);
            }
            else
            {
                Console.WriteLine("Erreur : Chemin non valide.");
                return null;
            }

            chemin.Reverse();
            return chemin.Select(n => n.Nom).ToList();
        }

        /// <summary>
        /// Prend en paramètre la liste des sommets parcourus à la suite dans le chemin et renvoie le poids du chemin
        /// Renvoie null si le chemin n'existe pas
        /// </summary>
        /// <param name="chemin"></param>
        /// <returns></returns>
        public double? Poids(List<string> chemin)
        {
            if (chemin == null) return null;
            bool ok;

            double poids = 0;
            for (int i = 0; i < chemin.Count - 1; i++)
            {
                ok = false;
                foreach (Lien<double> lien in Liens)
                {
                    if (lien.Depart.Nom == chemin[i] && lien.Arrivee.Nom == chemin[i + 1])
                    {
                        poids += lien.Poids;
                        ok = true;
                    }
                }
                if (!ok) return null;
            }
            return poids;
        }

        #region Affichage du graphe
        /// <summary>
        /// Génère et ouvre un image png imageant le graph
        /// </summary>
        /// <summary>
        /// Génère et ouvre une image PNG représentant le graphe avec les noeuds aux coordonnées réelles
        /// </summary>
        public void DessinerGraphe()
        {
            string filechemin = "graphe.png";

            // Calculer les dimensions de la toile en fonction des coordonnées max/min
            double minX = Noeuds.Min(n => n.X_Coord);
            double maxX = Noeuds.Max(n => n.X_Coord);
            double minY = Noeuds.Min(n => n.Y_Coord);
            double maxY = Noeuds.Max(n => n.Y_Coord);

            // Ajouter une marge (par exemple, 100 pixels de chaque côté)
            int margin = 150;
            int width = 7000;
            int height = 3200;

            using (SKBitmap bitmap = new SKBitmap(width, height))
            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                // Fond blanc
                canvas.Clear(SKColors.White);

                // Styles pour les noeuds, arêtes et texte
                SKPaint paintnoeud = new SKPaint
                {
                    Color = SKColors.Black,
                    Style = SKPaintStyle.Fill
                };

                SKPaint paintnoeud2 = new SKPaint
                {
                    Color = SKColors.White,
                    Style = SKPaintStyle.Fill
                };

                SKPaint paintEdge = new SKPaint
                {
                    Color = SKColors.Black,
                    StrokeWidth = 2
                };

                SKPaint paintText = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 30,
                    IsAntialias = true,
                    TextAlign = SKTextAlign.Center
                };

                // Dictionnaire pour stocker les positions des noeuds
                Dictionary<string, SKPoint> positions = new Dictionary<string, SKPoint>();

                // Placer les noeuds aux coordonnées réelles, ajustées avec la marge
                foreach (Noeud<string> noeud in Noeuds)
                {
                    double x = (noeud.X_Coord - minX) * 35000 + margin; // Ajuster par rapport au minimum + marge
                    double y = (noeud.Y_Coord - minY) * 35000 + margin; // Ajuster par rapport au minimum + marge
                    positions[noeud.Nom.Split(" - ")[0]] = new SKPoint(Convert.ToSingle(x), Convert.ToSingle(y));
                }

                // Dessiner les arêtes (liens) entre les noeuds
                foreach (Lien<double> lien in Liens)
                {
                    SKPoint p1 = positions[lien.Depart.Nom.Split(" - ")[0]];
                    SKPoint p2 = positions[lien.Arrivee.Nom.Split(" - ")[0]];
                    canvas.DrawLine(p1, p2, paintEdge);
                }

                // Dessiner les noeuds et leurs Noms
                foreach (var kvp in positions)
                {
                    canvas.DrawCircle(kvp.Value, 17, paintnoeud); // Cercle pour le noeud
                    canvas.DrawCircle(kvp.Value, 15, paintnoeud2); // Cercle pour le noeud
                    canvas.DrawText(kvp.Key, kvp.Value.X, kvp.Value.Y - 7, paintText); // Nom au centre
                }

                // Sauvegarder l'image en PNG
                using (SKImage image = SKImage.FromBitmap(bitmap))
                using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    System.IO.File.WriteAllBytes(filechemin, data.ToArray());
                }

                // Ouvre l'image générée
                Process.Start(new ProcessStartInfo
                {
                    FileName = filechemin,
                    UseShellExecute = true
                });
            }
        }

        /// <summary>
        /// Génère et ouvre un image png imageant le graph en mettant le chemin pris en paramètre en valeur
        /// </summary>
        /// <summary>
        /// Génère et ouvre une image PNG représentant le graphe avec les noeuds aux coordonnées réelles
        /// </summary>
        public void DessinerCheminGraphe(List<string> chemin)
        {
            string filechemin = "grapheChemin.png";

            // Calculer les dimensions de la toile en fonction des coordonnées max/min
            double minX = Noeuds.Min(n => n.X_Coord);
            double maxX = Noeuds.Max(n => n.X_Coord);
            double minY = Noeuds.Min(n => n.Y_Coord);
            double maxY = Noeuds.Max(n => n.Y_Coord);

            // Ajouter une marge (par exemple, 100 pixels de chaque côté)
            int margin = 150;
            int width = 7000;
            int height = 3200;

            using (SKBitmap bitmap = new SKBitmap(width, height))
            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                // Fond blanc
                canvas.Clear(SKColors.White);

                // Styles pour les noeuds, arêtes et texte
                SKPaint paintnoeud = new SKPaint
                {
                    Color = SKColors.Black,
                    Style = SKPaintStyle.Fill
                };

                SKPaint paintnoeud2 = new SKPaint
                {
                    Color = SKColors.White,
                    Style = SKPaintStyle.Fill
                };

                SKPaint paintEdge = new SKPaint
                {
                    Color = SKColors.Black,
                    StrokeWidth = 2
                };
                SKPaint paintEdgeChemin = new SKPaint
                {
                    Color = SKColors.Red,
                    StrokeWidth = 8
                };

                SKPaint paintText = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 30,
                    IsAntialias = true,
                    TextAlign = SKTextAlign.Center
                };

                // Dictionnaire pour stocker les positions des noeuds
                Dictionary<string, SKPoint> positions = new Dictionary<string, SKPoint>();

                // Placer les noeuds aux coordonnées réelles, ajustées avec la marge
                foreach (Noeud<string> noeud in Noeuds)
                {
                    double x = (noeud.X_Coord - minX) * 35000 + margin; // Ajuster par rapport au minimum + marge
                    double y = (noeud.Y_Coord - minY) * 35000 + margin; // Ajuster par rapport au minimum + 
                    positions[noeud.Nom.Split(" - ")[0]] = new SKPoint(Convert.ToSingle(x), Convert.ToSingle(y));
                }

                // Dessiner les arêtes (liens) entre les noeuds
                foreach (Lien<double> lien in Liens)
                {
                    SKPoint p1 = positions[lien.Depart.Nom.Split(" - ")[0]];
                    SKPoint p2 = positions[lien.Arrivee.Nom.Split(" - ")[0]];

                    bool DuChemin = false;
                    for (int i = 0; i < chemin.Count - 1; i++)
                    {
                        if (chemin[i] == lien.Depart.Nom && chemin[i + 1] == lien.Arrivee.Nom)
                        {
                            DuChemin = true;
                        }

                    }
                    if (DuChemin)
                    {
                        canvas.DrawLine(p1, p2, paintEdgeChemin);
                    }
                    else { canvas.DrawLine(p1, p2, paintEdge); }



                }

                // Dessiner les noeuds et leurs Noms
                foreach (var kvp in positions)
                {
                    canvas.DrawCircle(kvp.Value, 17, paintnoeud); // Cercle pour le noeud
                    canvas.DrawCircle(kvp.Value, 15, paintnoeud2); // Cercle pour le noeud
                    canvas.DrawText(kvp.Key, kvp.Value.X, kvp.Value.Y - 7, paintText); // Nom au centre
                }

                // Sauvegarder l'image en PNG
                using (SKImage image = SKImage.FromBitmap(bitmap))
                using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    System.IO.File.WriteAllBytes(filechemin, data.ToArray());
                }

                // Ouvre l'image générée
                Process.Start(new ProcessStartInfo
                {
                    FileName = filechemin,
                    UseShellExecute = true
                });
            }
        }
        #endregion
    }
}
