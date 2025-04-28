using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class VNS
{
    private int[,] matriceDistances;
    private int nombreSommets;
    private const int TEMPS_LIMITE_MS = 5000;
    private List<int> chemin = new List<int>();
    public List<int> Chemin => chemin;

    public VNS(int[,] mat_dis)
    {
        matriceDistances = mat_dis;
        nombreSommets = mat_dis.GetLength(0);
    }

    public void TrouverCycleVNS(int depart)
    {
        List<int> cheminInitial = new List<int> { depart };
        for (int i = 0; i < nombreSommets; i++)
        {
            if (i != depart)
                cheminInitial.Add(i);
        }
        cheminInitial.Add(depart);
        Stopwatch chrono = Stopwatch.StartNew();
        this.chemin = OptimiserRecursivement(cheminInitial, 2, chrono);
    }

    public void TrouverCycleVNS(List<int> solutionInitiale)
    {
        if (solutionInitiale[0] != solutionInitiale[^1])
            solutionInitiale.Add(solutionInitiale[0]);
        Stopwatch chrono = Stopwatch.StartNew();
        this.chemin = OptimiserRecursivement(new List<int>(solutionInitiale), 2, chrono);
    }

    private List<int> OptimiserRecursivement(List<int> chemin, int niveauOpt, Stopwatch chrono)
    {
        if (chrono.ElapsedMilliseconds > TEMPS_LIMITE_MS)
            return chemin;
        int coutActuel = CalculerCout(chemin);
        for (int i = 1; i < chemin.Count - niveauOpt; i++)
        {
            int j = i + niveauOpt - 1;
            if (j >= chemin.Count - 1) break;
            List<int> nouveauChemin = OptSwap(chemin, i, j);
            int nouveauCout = CalculerCout(nouveauChemin);
            if (nouveauCout < coutActuel)
            {
                return OptimiserRecursivement(nouveauChemin, 2, chrono);
            }
            if (chrono.ElapsedMilliseconds > TEMPS_LIMITE_MS)
                return chemin;
        }
        if (niveauOpt < chemin.Count - 2)
            return OptimiserRecursivement(chemin, niveauOpt + 1, chrono);
        return chemin;
    }

    private List<int> OptSwap(List<int> chemin, int i, int j)
    {
        List<int> nouveauChemin = new List<int>();
        for (int k = 0; k < i; k++)
            nouveauChemin.Add(chemin[k]);
        for (int k = j; k >= i; k--)
            nouveauChemin.Add(chemin[k]);
        for (int k = j + 1; k < chemin.Count; k++)
            nouveauChemin.Add(chemin[k]);
        return nouveauChemin;
    }

    private int CalculerCout(List<int> chemin)
    {
        int cout = 0;
        for (int i = 0; i < chemin.Count - 1; i++)
        {
            cout += matriceDistances[chemin[i], chemin[i + 1]];
        }
        return cout;
    }

    public int ObtenirCout(List<int> chemin)
    {
        return CalculerCout(chemin);
    }
}
