using System;
using Exploration;

internal class Program
{
    private static void Main()
    {
        Instance test = new Instance(10);
        int[,] matrice = test.Lecture();
        test.affiche();
        VLS vls = new VLS(matrice);
        vls.TrouverCycleVLS(0);
        test.ecriture(vls.Chemin, vls.ObtenirCout(vls.Chemin), "VLS");

    }
}