using System;


namespace Live_In_Paris
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graphe graphe = new Graphe();
            graphe.AjouterNoeud("Paris");
            graphe.AjouterNoeud("Lyon");
            graphe.AjouterLien("Paris", "Lyon", 450);
        }
    }
}