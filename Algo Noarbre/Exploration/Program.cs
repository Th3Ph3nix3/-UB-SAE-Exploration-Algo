using System;

internal class Program
{
    private static void Main()
    {
        Instance test = new Instance(10);
        int[,] matrice = test.Lecture();
        test.affiche();
        VNS vns = new VNS(matrice);
        vns.TrouverCycleVNS(0);
        test.ecriture(vns.Chemin, vns.ObtenirCout(vns.Chemin), "VNS");

    }
}