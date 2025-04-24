

using Exploration;
using System.Diagnostics.Tracing;
using System.Security.Cryptography.X509Certificates;
using static Exploration.Class;

internal class Program
{
    private static List<arretes> TrouverChemin(int[,] tab)
    {
        graphe graphe = creerGraphe(tab);

       

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
                    if (a.depart == f.position && a.arrivee == graphe.noeuds[0])
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

        return Chemin_le_plus_court;

    }

    public int Calculer_taille(List<arretes> chemin)
    {
        int taille = 0;
        foreach (arretes a in chemin)
        {
            taille += a.count;
        }
        return taille;
    }

    public static graphe creerGraphe(int[,] tab)
    {
        graphe graphe = new graphe();
        List<noeud> noeuds = new List<noeud>();
        List<arretes> arretes = new List<arretes>();
        for (int i = 0; i < tab.GetLength(0); i++)
        {
            noeuds.Add(new noeud(i));
        }
        for (int i = 0; i < tab.GetLength(0); i++)
        {
            for (int j = 0; j < tab.GetLength(1); j++)
            {
                if (tab[i, j] != 0)
                {
                    arretes.Add(new arretes(noeuds[i], noeuds[j], tab[i, j]));
                }
            }
        }
        graphe.noeuds = noeuds;
        graphe.arretes = arretes;
        return graphe;
    }

}
