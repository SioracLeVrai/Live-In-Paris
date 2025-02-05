using System;
using System.Collections.Generic;

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
            Noeud noeud = new Noeud(valeur);
            Noeuds.Add(noeud);
        }

        public void AjouterLien(string valeur1, string valeur2, int poids)
        {
            Noeud noeud1 = null;
            Noeud noeud2 = null;

            foreach (Noeud noeud in Noeuds)
            {
                if (noeud.valeur.Equals(valeur1))
                    noeud1 = noeud;
                if (noeud.valeur.Equals(valeur2))
                    noeud2 = noeud;
            }

            if (noeud1 == null || noeud2 == null)
            {
                Console.WriteLine("Erreur : Un ou les deux nœuds n'existent pas.");
                return;
            }

            Lien lien = new Lien(noeud1, noeud2, poids);
            Liens.Add(lien);
            noeud1.Liens.Add(lien);
            noeud2.Liens.Add(lien);
        }
    }
}