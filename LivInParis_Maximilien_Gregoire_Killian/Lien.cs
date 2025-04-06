using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivInParis_Maximilien_Gregoire_Killian
{
    internal class Lien<T>
    {
        public Noeud<string> Depart { get; }
        public Noeud<string> Arrivee { get; }
        public T Poids { get; }

        public Lien(Noeud<string> depart, Noeud<string> arrivee, T poids)
        {
            this.Depart = depart;
            this.Arrivee = arrivee;
            this.Poids = poids;
        }
    }
}
