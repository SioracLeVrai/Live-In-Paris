using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using SkiaSharp;
using System.ComponentModel;
using MySql.Data.MySqlClient;

namespace Live_In_Paris
{
    internal class Graphe
    {
        public List<Noeud> Noeuds;
        public List<Lien> Liens;

        public Graphe()
        {
            Noeuds = new List<Noeud>();
            Liens = new List<Lien>();
        }

        public void AjouterNoeud(string nom, int x_coord, int y_coord)
        {
            foreach (Noeud noeud in Noeuds)
            {
                if (noeud.nom == nom)
                    return; // Le noeud existe deja
            }

            Noeuds.Add(new Noeud(nom, x_coord, y_coord));
        }

        /// <summary>
        /// Ajoute un lien entre deux noeuds avec un poid pris en paramètre
        /// </summary>
        /// <param name="nom1"></param>
        /// <param name="nom2"></param>
        /// <param name="poids"></param>
        public void AjouterLien(string NomDepart, string NomArrivee, double poids)
        {
            Noeud Depart = null;
            Noeud Arrivee = null;

            foreach (Noeud noeud in Noeuds)
            {
                if (noeud.nom == NomDepart)
                    Depart = noeud;
                if (noeud.nom == NomArrivee)
                    Arrivee = noeud;
            }

            if (Depart == null || Arrivee == null)
            {
                Console.WriteLine("Erreur : Un ou les deux nœuds n'existent pas.");
                return;
            }

            if (poids == null) { poids = 0; }

            Lien lien = new Lien(Depart, Arrivee, poids);
            Liens.Add(lien);

        }

        /// <summary>
        /// Affiche tout les liens existant dans le graph
        /// </summary>
        public string AffichageLiens()
        {
            string txt = "";
            foreach (Lien liens in Liens)
            {
                txt += "\nLien entre " + liens.Depart.nom + " et " + liens.Arrivee.nom + " ayant le poid " + liens.poids + ".";
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
                    foreach (Lien lien in Liens)
                    {
                        if ((lien.Depart == Noeuds[i] && lien.Arrivee == Noeuds[j]) || (lien.Depart == Noeuds[j] && lien.Arrivee == Noeuds[i]))
                        {
                            mat[i, j] = lien.poids;
                        }
                    }
                }
            }

            string txt = "\t";

            foreach (Noeud noeud in Noeuds)
            {
                txt += noeud.nom + " ";
            }

            txt += "\n";

            for (int i = 0; i < taille; i++)
            {
                txt += Noeuds[i].nom + "\t";
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
                AjouterNoeud(ligne[0], Convert.ToInt32(ligne[1]), Convert.ToInt32(ligne[2]));
            }

            // Ajout des liaisons
            texte = null;
            Chemin = "Graph/ListeAdjacencesLignes.txt";
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
                }
                catch (FileNotFoundException e) { Console.WriteLine("\nEchec lors de l'ajout du lien" + e.Message); }
            }

            texte = null;
            Chemin = "Graph/ListeAdjacencesChangements.txt";
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
                }
                catch (IndexOutOfRangeException e) { Console.WriteLine("\nEchec lors de l'ajout du lien" + e.Message); }

            }

        }

        /// <summary>
        /// Renvoie une liste les noeuds avec lesquels un noeud pris en paramètre à des liens
        /// </summary>
        /// <param name="noeud"></param>
        /// <returns></returns>
        private List<Noeud> listeVoisin(Noeud noeud)
        {
            List<Noeud> voisin = new List<Noeud>();
            foreach (Lien lien in Liens)
            {
                if (lien.Depart == noeud)
                    voisin.Add(lien.Arrivee);
                else if (lien.Arrivee == noeud)
                    voisin.Add(lien.Depart);
            }
            return voisin;
        }

        /// <summary>
        /// Parcours en largeur le graph
        /// </summary>
        /// <param name="nomDebut"></param>
        /// <returns></returns>
        public string ParcoursEnLargeur(string nomDebut)
        {
            string resultat = "";

            Noeud debut = null;
            foreach (Noeud noeud in Noeuds)
            {
                if (noeud.nom == nomDebut)
                {
                    debut = noeud;
                }
            }
            if (debut == null)
            {
                Console.WriteLine("Le noeud n'a pas pu être trouvé.");
                return null;
            }


            Queue<Noeud> file = new Queue<Noeud>();
            List<Noeud> dejaVisite = new List<Noeud>();

            file.Enqueue(debut);
            dejaVisite.Add(debut);
            bool tempo = false;
            while (file.Count > 0)
            {
                Noeud enCours = file.Dequeue();
                resultat += enCours.nom + ", ";

                foreach (Noeud voisin in listeVoisin(enCours))
                {
                    tempo = false;
                    foreach (Noeud noeud in dejaVisite)
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
        /// <param name="nomDebut"></param>
        /// <returns></returns>
        public string ParcoursEnProfondeur(string nomDebut)
        {
            Noeud debut = null;
            foreach (Noeud noeud in Noeuds)
            {
                if (noeud.nom == nomDebut)
                {
                    debut = noeud;
                }
            }
            if (debut == null)
            {
                Console.WriteLine("Le noeud n'a pas pu être trouvé.");
                return null;
            }

            List<Noeud> dejaVisite = new List<Noeud>();
            return ParcoursEnProfondeurRecursif(debut, dejaVisite);
        }

        /// <summary>
        /// Pars d'un noeud et fais le parcours en profondeur de ses enfants
        /// </summary>
        /// <param name="enCours"></param>
        /// <param name="dejaVisite"></param>
        /// <returns></returns>
        private string ParcoursEnProfondeurRecursif(Noeud enCours, List<Noeud> dejaVisite)
        {
            string resultat = null;
            bool tempo = false;
            foreach (Noeud noeud in dejaVisite)
            {
                if (noeud == enCours)
                {
                    tempo = true;
                }
            }

            if (!tempo)
            {
                resultat = enCours.nom + ", ";

                dejaVisite.Add(enCours);

                foreach (Noeud voisin in listeVoisin(enCours))
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

            List<Noeud> dejaVisite = new List<Noeud>();
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
        public string Cycle(Noeud enCours, Noeud parent = null, List<Noeud> dejaVisite = null)
        {
            if (dejaVisite == null)
            {
                dejaVisite = new List<Noeud>();
            }
            dejaVisite.Add(enCours);

            foreach (Noeud voisin in listeVoisin(enCours))
            {
                if (voisin != parent)
                {
                    if (dejaVisite.Contains(voisin))
                    {
                        int i = dejaVisite.IndexOf(voisin);
                        string resultat = "";
                        for (int j = i; j < dejaVisite.Count; j++)
                        {
                            resultat += dejaVisite[j].nom + " ";
                        }
                        resultat += dejaVisite[i].nom;
                        return resultat;
                    }
                    else
                    {
                        if (Cycle(voisin, enCours, new List<Noeud>(dejaVisite)) != null)
                        {
                            return Cycle(voisin, enCours, new List<Noeud>(dejaVisite));
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Génère et ouvre un image png imageant le graph
        /// </summary>
        /// <summary>
        /// Génère et ouvre une image PNG représentant le graphe avec les noeuds aux coordonnées réelles
        /// </summary>
        public void DessinerGraphe()
        {
            string filePath = "graphe.png";

            // Calculer les dimensions de la toile en fonction des coordonnées max/min
            int minX = Noeuds.Min(n => n.x_coord);
            int maxX = Noeuds.Max(n => n.x_coord);
            int minY = Noeuds.Min(n => n.y_coord);
            int maxY = Noeuds.Max(n => n.y_coord);

            // Ajouter une marge (par exemple, 100 pixels de chaque côté)
            int margin = 150;
            int width = 5000;
            int height = 6000;

            using (SKBitmap bitmap = new SKBitmap(width, height))
            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                // Fond blanc
                canvas.Clear(SKColors.White);

                // Styles pour les noeuds, arêtes et texte
                SKPaint paintNode = new SKPaint
                {
                    Color = SKColors.Blue,
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
                foreach (Noeud noeud in Noeuds)
                {
                    float x = (noeud.x_coord - minX) * 10 + margin; // Ajuster par rapport au minimum + marge
                    float y = (noeud.y_coord - minY) * 10 + margin; // Ajuster par rapport au minimum + marge
                    positions[noeud.nom.Split(" - ")[0]] = new SKPoint(x, y);
                }

                // Dessiner les arêtes (liens) entre les noeuds
                foreach (Lien lien in Liens)
                {
                    SKPoint p1 = positions[lien.Depart.nom.Split(" - ")[0]];
                    SKPoint p2 = positions[lien.Arrivee.nom.Split(" - ")[0]];
                    canvas.DrawLine(p1, p2, paintEdge);
                }

                // Dessiner les noeuds et leurs noms
                foreach (var kvp in positions)
                {
                    canvas.DrawCircle(kvp.Value, 40, paintNode); // Cercle pour le noeud
                    canvas.DrawText(kvp.Key, kvp.Value.X, kvp.Value.Y + 7, paintText); // Nom au centre
                }

                // Sauvegarder l'image en PNG
                using (SKImage image = SKImage.FromBitmap(bitmap))
                using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    System.IO.File.WriteAllBytes(filePath, data.ToArray());
                }

                // Ouvre l'image générée
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
        }
    }
}