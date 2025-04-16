

using Exploration;
using System.Diagnostics.Tracing;
using static Exploration.Class;

internal class Program
{
    private static void Main(string[] args)
    {
        graphe graphe = new graphe();
        // Création des noeuds
        noeud n0 = new noeud(0);
        noeud n1 = new noeud(1);
        noeud n2 = new noeud(2);
        noeud n3 = new noeud(3);

        graphe.noeuds = new List<noeud> { n0, n1, n2, n3 };

        // Création des arêtes
        graphe.arretes = new List<arretes>
        {
            new arretes(n0, n1, 5),
            new arretes(n1, n2, 10),
            new arretes(n2, n3, 15),
            new arretes(n3, n0, 10),
            new arretes(n0, n2, 12),
            new arretes(n1, n3, 8),
            new arretes(n1, n0, 5),
            new arretes(n2, n1, 10),
            new arretes(n3, n2, 15),
            new arretes(n0, n3, 10),
            new arretes(n2, n0, 12),
            new arretes(n3, n1, 8)
        };

        List<fourmi> fourmis = new List<Class.fourmi>();
        List<arretes> Chemin_le_plus_court = new List<arretes>();
        uint Count_Chemin_Le_Plus_court = unchecked((uint)-1);
        for (int i = 0; i < 10; i++)
        {
            fourmis.Add(new fourmi(graphe.noeuds[0])); 
        }


        for (int t = 0; t < 10; t++)
        {
            foreach (fourmi f in fourmis)
            {
                for (int i = 0; i < graphe.noeuds.Count; i++)
                {
                    arretes a = f.deplacement(graphe.arretes);
                    if (a != null)
                    {
                        f.position = a.arrivee;
                        f.visite.Add(a.arrivee);
                        f.chemin.Add(a);
                    }
                }
                arretes retour = null;
                foreach (arretes a in graphe.arretes)
                {
                    if (a.depart == f.position && a.arrivee == n0)
                    {
                        retour = a;
                        break;
                    }
                }
                if (retour != null)
                {
                    f.chemin.Add(retour);
                    f.position = retour.arrivee;
                }
                f.pheromones();
                int count = 0;
                foreach (arretes a in f.chemin)
                {
                    count += a.count;
                }
                if (count < Count_Chemin_Le_Plus_court)
                {
                    Count_Chemin_Le_Plus_court = (uint)count;
                    Chemin_le_plus_court = f.chemin;
                }
            }
            foreach (arretes a in graphe.arretes)
            {
                a.evaporation();
            }
        }

        // afficher le chemin le plus court
        Console.WriteLine("Le chemin le plus court est :");
        foreach (arretes a in Chemin_le_plus_court)
        {
            Console.WriteLine("de " + a.depart.numero + " à " + a.arrivee.numero);
        }

    }

}
