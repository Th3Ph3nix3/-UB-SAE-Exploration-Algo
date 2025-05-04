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
    private int nombreSommets;        // Nombre total de sommets
    private const int TEMPS_LIMITE_MS = 5000; // Temps limite d'exécution pour chaque amélioration
    private List<int> chemin = new List<int>(); // Chemin trouvé
    #endregion

    #region Propriétés
    /// <summary>
    /// Renvoie le chemin trouvé après l'exécution de l'algorithme.
    /// </summary>
    public List<int> Chemin => chemin;
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

        this.chemin = OptimiserRecursivement(cheminInitial, 2);
    }

    /// <summary>
    /// Lance la recherche VNS à partir d'une solution initiale donnée.
    /// </summary>
    /// <param name="solutionInitiale">Liste représentant un chemin initial (peut être ouvert ou fermé).</param>
    public void TrouverCycleVNS(List<int> solutionInitiale)
    {
        if (solutionInitiale[0] != solutionInitiale[^1])
            solutionInitiale.Add(solutionInitiale[0]);

        this.chemin = OptimiserRecursivement(new List<int>(solutionInitiale), 2);
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
    /// Chaque appel est limité à un temps d'exécution maximal pour chaque amélioration.
    /// </summary>
    /// <param name="chemin">Chemin actuel à optimiser.</param>
    /// <param name="niveauOpt">Taille du voisinage utilisée pour le swap.</param>
    /// <returns>Le meilleur chemin trouvé après optimisation locale.</returns>
    private List<int> OptimiserRecursivement(List<int> chemin, int niveauOpt)
    {
        int coutActuel = CalculerCout(chemin);
        Stopwatch chrono = Stopwatch.StartNew(); // Chrono pour mesurer chaque tentative d'amélioration
        bool improvementFound = true;
        // Tant qu'il y a des améliorations à faire et que le temps n'est pas écoulé
        while (improvementFound && chrono.ElapsedMilliseconds <= TEMPS_LIMITE_MS)
        {
            improvementFound = false; // Par défaut, pas d'amélioration trouvée
            for (int i = 1; i < chemin.Count - niveauOpt; i++)
            {
                int j = i + niveauOpt - 1;
                if (j >= chemin.Count - 1) break;
                // Vérifie le temps limite pour chaque tentative d'amélioration
                if (chrono.ElapsedMilliseconds > TEMPS_LIMITE_MS)
                    return chemin;
                // Essayer un swap de voisinage
                List<int> nouveauChemin = OptSwap(chemin, i, j);
                int nouveauCout = CalculerCout(nouveauChemin);
                // Si on a trouvé une meilleure solution, on l'applique et on réinitialise le chrono
                if (nouveauCout < coutActuel)
                {
                    chemin = nouveauChemin;
                    coutActuel = nouveauCout;
                    improvementFound = true; // On a trouvé une amélioration
                    chrono.Restart(); // Réinitialisation du chrono après chaque amélioration
                    break; // On quitte le for pour réessayer une amélioration avec le nouveau chemin
                }
            }
            // Si on n'a pas trouvé d'amélioration dans la boucle, on augmente la taille du voisinage
            if (!improvementFound && niveauOpt < chemin.Count - 2)
            {
                chemin = OptimiserRecursivement(chemin, niveauOpt + 1); // Essayer un voisinage plus large
            }
        }
        return chemin; // Retourner le meilleur chemin trouvé
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
