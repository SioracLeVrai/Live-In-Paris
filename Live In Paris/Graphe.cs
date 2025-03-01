using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using SkiaSharp;
using System.Diagnostics;
using System.ComponentModel;

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

        public void AjouterNoeud(string valeur)
        {
            foreach (Noeud noeud in Noeuds)
            {
                if (noeud.valeur == valeur)
                    return; // Le noeud existe deja
            }

            Noeuds.Add(new Noeud(valeur));
        }

        /// <summary>
        /// Affiche tout les liens existant dans le graph
        /// </summary>
        public void AffichageLiens()
        {
            foreach(Lien liens in Liens)
            {
                Console.WriteLine("Lien entre " + liens.Noeud1.valeur + " et " + liens.Noeud2.valeur + "ayant le poid " + liens.poids + ".");
            }
        }

        /// <summary>
        /// Affiche la matrice d'adjacence du graph
        /// </summary>
        public void MatriceAdjacence()
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
                        if ((lien.Noeud1 == Noeuds[i] && lien.Noeud2 == Noeuds[j]) || (lien.Noeud1 == Noeuds[j] && lien.Noeud2 == Noeuds[i]))
                        {
                            mat[i, j] = lien.poids;
                        }
                    }
                }
            }

            Console.Write("\t");
            foreach (Noeud noeud in Noeuds)
            {
                Console.Write(noeud.valeur + " ");
                if (Convert.ToInt32(noeud.valeur) < 10)
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();

            for (int i = 0; i < taille; i++)
            {
                Console.Write(Noeuds[i].valeur + "\t");
                for (int j = 0; j < taille; j++)
                {
                    Console.Write(mat[i, j] + "  ");

                }
                Console.WriteLine();
            }
        }
 

        /// <summary>
        /// Ajoute un lien entre deux noeuds avec un poid pris en paramètre
        /// </summary>
        /// <param name="valeur1"></param>
        /// <param name="valeur2"></param>
        /// <param name="poids"></param>
        public void AjouterLien(string valeur1, string valeur2, double poids)
            {
                Noeud noeud1 = null;
                Noeud noeud2 = null;

                foreach (Noeud noeud in Noeuds)
                {
                    if (noeud.valeur == valeur1)
                        noeud1 = noeud;
                    if (noeud.valeur == valeur2)
                        noeud2 = noeud;
                }

                if (noeud1 == null || noeud2 == null)
                {
                    Console.WriteLine("Erreur : Un ou les deux nœuds n'existent pas.");
                    return;
                }

                Lien lien = new Lien(noeud1, noeud2, poids);
                Liens.Add(lien);

            }

        /// <summary>
        /// Rempli le graph à partir du fichier "soc-karate.txt"
        /// </summary>
        public void CreationGraph()
        {
            string[] texte = null;
            string Chemin = "soc-karate.txt";
            try
            {
                texte = File.ReadAllText(Chemin).Split('\n');
            }
            catch (FileNotFoundException e) { Console.WriteLine("echec lors de la lecture du fichier" + e.Message); }

            string[][] mat = new string[texte.Length][];
            string[] tempo = null;
            for (int i = 0; i < texte.Length; i++)
            {
                mat[i] = new string[3];
                texte[i] = texte[i].Substring(3, texte[i].Length - 3);
                tempo = texte[i].Split(",");
                mat[i][0] = tempo[0].Trim();
                mat[i][1] = tempo[1].Split(")")[0].Trim();
                mat[i][2] = tempo[1].Split(")")[1].Trim();
            }
            for (int i = 0; i < mat.Length; i++)
            {
                AjouterNoeud(mat[i][0]);
                AjouterNoeud(mat[i][1]);
                AjouterLien(mat[i][0], mat[i][1], Convert.ToDouble(mat[i][2].Replace('.', ',')));
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
                if (lien.Noeud1 == noeud)
                    voisin.Add(lien.Noeud2);
                else if (lien.Noeud2 == noeud)
                    voisin.Add(lien.Noeud1);
            }
            return voisin;
        }

        /// <summary>
        /// Parcours en largeur le graph
        /// </summary>
        /// <param name="valeurDebut"></param>
        /// <returns></returns>
        public string ParcoursEnLargeur(string valeurDebut)
        {
            string resultat = "";

            Noeud debut = null;
            foreach (Noeud noeud in Noeuds)
            {
                if (noeud.valeur == valeurDebut)
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
                resultat += enCours.valeur + " ";

                foreach (Noeud voisin in listeVoisin(enCours))
                {
                    tempo = false;
                    foreach(Noeud noeud in dejaVisite)
                    {
                        if(noeud == voisin)
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
        /// <param name="valeurDebut"></param>
        /// <returns></returns>
        public string ParcoursEnProfondeur(string valeurDebut)
        {
            Noeud debut = null;
            foreach (Noeud noeud in Noeuds)
            {
                if (noeud.valeur == valeurDebut)
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
                resultat = enCours.valeur + " ";

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
                        for(int j = i; j < dejaVisite.Count; j++)
                        {
                            resultat += dejaVisite[j].valeur + " ";
                        }
                        resultat += dejaVisite[i].valeur;
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
        public void DessinerGraphe()
        {
            string filePath = "graphe.png";
            int width = 2000, height = 2000;
            using (SKBitmap bitmap = new SKBitmap(width, height))
            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.White);

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
                    Color = SKColors.White,
                    TextSize = 20,
                    IsAntialias = true,
                    TextAlign = SKTextAlign.Center
                };

                Dictionary<string, SKPoint> positions = new Dictionary<string, SKPoint>();
                int centerX = width / 2;
                int centerY = height / 2;
                int radius = Math.Min(width, height) / 3;
                int count = Noeuds.Count;

                for (int i = 0; i < count; i++)
                {
                    double angle = 2 * Math.PI * i / count;
                    float x = centerX + (float)(radius * Math.Cos(angle));
                    float y = centerY + (float)(radius * Math.Sin(angle));
                    positions[Noeuds[i].valeur] = new SKPoint(x, y);
                }

                foreach (Lien lien in Liens)
                {
                    SKPoint p1 = positions[lien.Noeud1.valeur];
                    SKPoint p2 = positions[lien.Noeud2.valeur];
                    canvas.DrawLine(p1, p2, paintEdge);
                }

                foreach (var kvp in positions)
                {
                    canvas.DrawCircle(kvp.Value, 20, paintNode);
                    canvas.DrawText(kvp.Key, kvp.Value.X, kvp.Value.Y + 7, paintText);
                }

                using (SKImage image = SKImage.FromBitmap(bitmap))
                using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    System.IO.File.WriteAllBytes(filePath, data.ToArray());
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
        }
    }
}
