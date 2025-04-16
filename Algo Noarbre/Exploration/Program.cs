using System;

internal class Program
{
    private static void Main()
    {
        Instance test = new Instance(10);
        int[,] matrice = test.Lecture();
        test.affiche();
    }
}