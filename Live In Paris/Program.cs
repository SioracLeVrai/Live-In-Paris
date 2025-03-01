using SkiaSharp;
using System;


namespace Live_In_Paris
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graphe graphe = new Graphe();
            graphe.CreationGraph();
            Console.WriteLine("Les liens caractérisant le graph sont : ");
            graphe.AffichageLiens();

            Console.WriteLine("\nLa matrice du graph est : ");
            graphe.MatriceAdjacence();

            Console.WriteLine("\nParcour en largeur : " + graphe.ParcoursEnLargeur("0"));
            Console.WriteLine("\nParcour en profondeur : " + graphe.ParcoursEnProfondeur("0"));
            Console.WriteLine("\nLe graphe est connexe : " + graphe.Connexe());
            Console.WriteLine("\nUn cycle est : " + graphe.Cycle(graphe.Noeuds[0]) + ". Il existe donc un cycle.");


            graphe.DessinerGraphe();
        }
    }
}