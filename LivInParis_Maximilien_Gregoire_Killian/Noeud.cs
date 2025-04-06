using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivInParis_Maximilien_Gregoire_Killian
{
    internal class Noeud<T>
    {
        public T Nom { get; }
        public double X_Coord { get; }
        public double Y_Coord { get; }

        public Noeud(T nom, double x_coord, double y_coord)
        {
            this.Nom = nom;
            this.X_Coord = x_coord;
            this.Y_Coord = y_coord;
        }
    }
}
