using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_In_Paris
{
    internal class Noeud
    {
        public string nom;
        public int x_coord;
        public int y_coord;

        public Noeud(string nom, int x_coord, int y_coord)
        {
            this.nom = nom;
            this.x_coord = x_coord;
            this.y_coord = y_coord;
        }
    }
}