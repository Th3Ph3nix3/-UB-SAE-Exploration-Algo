using System;
using System.Collections.Generic;
using System.Diagnostics;

class HeldKarpTSP
{
    // Méthode principale pour résoudre le TSP avec l'algorithme Held-Karp
    public static int HeldKarp(int[,] distances)
    {
        int n = distances.GetLength(0);
        // Utilisation d'un dictionnaire pour stocker les coûts minimaux
        Dictionary<(int, int), int> memo = new Dictionary<(int, int), int>();

        // Fonction récursive pour calculer le coût minimum
        int TSP(int mask, int pos)
        {
            // Si tous les sommets ont été visités, retourner le coût pour revenir au sommet de départ
            if (mask == (1 << n) - 1)
            {
                return distances[pos, 0];
            }

            // Si le coût est déjà calculé, le retourner
            if (memo.ContainsKey((mask, pos)))
            {
                return memo[(mask, pos)];
            }

            // Initialiser le coût minimum à l'infini
            int minCost = int.MaxValue;

            // Parcourir tous les sommets
            for (int city = 0; city < n; city++)
            {
                // Si le sommet n'a pas été visité
                if ((mask & (1 << city)) == 0)
                {
                    // Calculer le coût pour visiter ce sommet
                    int newCost = distances[pos, city] + TSP(mask | (1 << city), city);
                    // Mettre à jour le coût minimum
                    minCost = Math.Min(minCost, newCost);
                }
            }

            // Mémoriser le coût minimum pour cet état
            memo[(mask, pos)] = minCost;
            return minCost;
        }

        // Appel initial de la fonction récursive
        return TSP(1, 0);
    }

    static void Main(string[] args)
    {
        // Créer une instance de Stopwatch
        Stopwatch stopwatch1 = new Stopwatch();

        // Démarrer le chronomètre
        stopwatch1.Start();

        // Créer une instance de la classe Instance avec un nombre de villes spécifié
        Instance instance = new Instance(30);

        // Arrêter le chronomètre
        stopwatch1.Stop();

        // Afficher le temps écoulé
        Console.WriteLine("Temps d'exécution : " + stopwatch1.ElapsedMilliseconds + " ms");

        // Afficher les points et la matrice de distances
        //instance.affiche();

        // Créer une instance de Stopwatch
        Stopwatch stopwatch = new Stopwatch();

        // Démarrer le chronomètre
        stopwatch.Start();

        // Récupérer la matrice de distances générée
        int[,] distances = instance.Mat_dis;

        // Arrêter le chronomètre
        stopwatch.Stop();

        // Résolution du TSP avec la matrice de distances générée
        int minCost = HeldKarp(distances);
        Console.WriteLine("Le coût minimum du circuit est : " + minCost);

        // Afficher le temps écoulé
        Console.WriteLine("Temps d'exécution : " + stopwatch.ElapsedMilliseconds + " ms");
    }
}
