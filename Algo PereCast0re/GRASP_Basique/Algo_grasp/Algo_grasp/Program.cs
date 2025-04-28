using System.Text.RegularExpressions;
using Algo_grasp;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Bonjour voici l'algo du GRASP !");

        Instance test = new Instance(10);
        int[,] matrice = test.Lecture();
        test.affiche();

        Console.WriteLine("\n\n");
        Console.WriteLine("Algo GRASP");

        grasp algo = new grasp(matrice);
        algo.Parcour(matrice);
        algo.ToString();

        test.ecriture(algo.CheminGrasp, 10, "GRASP_Classique");
    }
}
