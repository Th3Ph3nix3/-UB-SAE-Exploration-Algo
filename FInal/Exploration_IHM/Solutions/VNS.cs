using System;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// Classe implémentant l'algorithme VNS (Variable Neighborhood Search) 
/// pour optimiser un cycle (problème du voyageur de commerce).
/// </summary>
public class VNS
{
    #region Attributs
    private int[,] matriceDistances;  // Matrice des distances entre les sommets
    private int nombreSommets;         // Nombre total de sommets
    private const int TEMPS_LIMITE_MS = 5000; // Temps limite d'exécution en millisecondes
    private List<int> chemin = new List<int>(); // Chemin trouvé
    #endregion

    #region Propriétés
    public List<int> Chemin => chemin;          // Propriété pour accéder au chemin
    #endregion

    #region Constructeur
    /// <summary>
    /// Constructeur de la classe VNS, initialise la matrice de distances.
    /// </summary>
    /// <param name="mat_dis">Matrice carrée des distances entre les sommets.</param>
    public VNS(int[,] mat_dis)
    {
        matriceDistances = mat_dis;
        nombreSommets = mat_dis.GetLength(0);
    }
    #endregion

    #region Méthodes publiques
    /// <summary>
    /// Lance la recherche VNS à partir d'un sommet de départ donné.
    /// </summary>
    /// <param name="depart">Indice du sommet de départ.</param>
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

    /// <summary>
    /// Lance la recherche VNS à partir d'une solution initiale donnée.
    /// </summary>
    /// <param name="solutionInitiale">Liste représentant un chemin initial (peut être ouvert ou fermé).</param>
    public void TrouverCycleVNS(List<int> solutionInitiale)
    {
        if (solutionInitiale[0] != solutionInitiale[^1])
            solutionInitiale.Add(solutionInitiale[0]);
        Stopwatch chrono = Stopwatch.StartNew();
        this.chemin = OptimiserRecursivement(new List<int>(solutionInitiale), 2, chrono);
    }

    /// <summary>
    /// Renvoie le coût total d'un chemin.
    /// </summary>
    /// <param name="chemin">Chemin pour lequel le coût est demandé.</param>
    /// <returns>Coût total du chemin.</returns>
    public int ObtenirCout(List<int> chemin)
    {
        return CalculerCout(chemin);
    }
    #endregion

    #region Méthodes privées
    /// <summary>
    /// Optimise récursivement un chemin en testant différentes tailles de voisinage.
    /// </summary>
    /// <param name="chemin">Chemin actuel à optimiser.</param>
    /// <param name="niveauOpt">Taille du voisinage utilisée pour le swap.</param>
    /// <param name="chrono">Chronomètre utilisé pour respecter la limite de temps.</param>
    /// <returns>Le meilleur chemin trouvé après optimisation.</returns>
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

    /// <summary>
    /// Effectue une inversion du sous-chemin entre les indices i et j.
    /// </summary>
    /// <param name="chemin">Chemin original.</param>
    /// <param name="i">Indice de début du sous-chemin à inverser.</param>
    /// <param name="j">Indice de fin du sous-chemin à inverser.</param>
    /// <returns>Un nouveau chemin après inversion.</returns>
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

    /// <summary>
    /// Calcule le coût total d'un chemin (somme des distances entre sommets consécutifs).
    /// </summary>
    /// <param name="chemin">Chemin dont on veut calculer le coût.</param>
    /// <returns>Coût total du chemin.</returns>
    private int CalculerCout(List<int> chemin)
    {
        int cout = 0;
        for (int i = 0; i < chemin.Count - 1; i++)
        {
            cout += matriceDistances[chemin[i], chemin[i + 1]];
        }
        return cout;
    }
    #endregion
}
