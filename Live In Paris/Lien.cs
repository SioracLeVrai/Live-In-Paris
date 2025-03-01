using System;
using System.Collections.Generic;

namespace Live_In_Paris
{
    internal class Lien
    {
        public Noeud Noeud1;
        public Noeud Noeud2;
        public double poids;

        public Lien(Noeud Noeud1, Noeud Noeud2, double poids)
        {
            this.Noeud1 = Noeud1;
            this.Noeud2 = Noeud2;
            this.poids = poids;
        }
    }
}