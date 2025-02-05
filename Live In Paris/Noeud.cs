using System;
using System.Collections.Generic;

namespace Live_In_Paris
{
    internal class Noeud
    {
        public string valeur;
        public List<Lien> Liens;

        public Noeud(string valeur)
        {
            this.valeur = valeur;
            this.Liens = new List<Lien>();
        }
    }
}