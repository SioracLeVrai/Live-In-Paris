using Live_In_Paris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_In_Paris
{
    internal class Lien
    {
        public Noeud Depart;
        public Noeud Arrivee;
        public double poids;

        public Lien(Noeud Depart, Noeud Arrivee, double poids)
        {
            this.Depart = Depart;
            this.Arrivee = Arrivee;
            this.poids = poids;
        }
    }
}